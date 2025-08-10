using System.Collections.Concurrent;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats.Webp;
using System.Text.RegularExpressions;
using Steria.Core.Interfaces;

namespace Steria.Data.Services;

public class FileService: IFileService
{
    private readonly Cloudinary cloudinary;
    private readonly string cloudName;
    private const int MaxParallelUploads = 10; // Max treads for uploading
    private const long MaxFileSizeBytes = 10 * 1024 * 1024; // 10 Mb
    private const int WebpQuality = 75; // Quality in %

    public FileService(IConfiguration configuration)
    {
        var cloudinarySettings = configuration.GetSection("Cloudinary");
        cloudName = cloudinarySettings["CloudName"]!;
        var account = new Account(
            cloudinarySettings["CloudName"],
            cloudinarySettings["ApiKey"],
            cloudinarySettings["ApiSecret"]);
        cloudinary = new Cloudinary(account);
    }
    
    private string ExtractPublicIdFromUrl(string url)
    {
        if (string.IsNullOrEmpty(url))
            throw new ArgumentException("URL cannot be empty.");
        
        var regex = new Regex($@"https://res\.cloudinary\.com/{cloudName}/image/upload/v\d+/(.+)\.webp");
        var match = regex.Match(url);

        if (!match.Success)
            throw new ArgumentException("Invalid Cloudinary URL format.");

        return match.Groups[1].Value;
    }
    
    public async Task<string> UploadImageAsync(IFormFile file)
    {
        if (file == null || file.Length == 0)
            throw new ArgumentException("No file provided.");

        if (file.Length > MaxFileSizeBytes)
            throw new ArgumentException($"File size exceeds {MaxFileSizeBytes / (1024 * 1024)} MB.");

        if (!file.ContentType.StartsWith("image/"))
            throw new ArgumentException("Only image files are allowed.");
        
        using var memoryStream = new MemoryStream();
        using var image = await Image.LoadAsync(file.OpenReadStream());
        await image.SaveAsync(memoryStream, new WebpEncoder { Quality = WebpQuality });
        memoryStream.Position = 0;
        
        var publicId = $"images/{Guid.NewGuid().ToString()}";
        var uploadParams = new ImageUploadParams
        {
            File = new FileDescription(file.FileName, memoryStream),
            PublicId = publicId,
            Format = "webp",
            Transformation = new Transformation().Quality("auto").FetchFormat("webp")
        };

        var uploadResult = await cloudinary.UploadAsync(uploadParams);

        if (uploadResult.StatusCode != System.Net.HttpStatusCode.OK)
            throw new Exception($"Failed to upload image: {uploadResult.Error.Message}");

        return uploadResult.SecureUrl.ToString();
    }

    public async Task<List<string>> UploadImagesAsync(IList<IFormFile> files)
    {
        if (files == null || files.Count == 0)
            throw new ArgumentException("No files provided.");

        if (files.Count > 500)
            throw new ArgumentException("Too many files. Maximum allowed is 500.");

        var urls = new ConcurrentBag<string>();
        var semaphore = new SemaphoreSlim(MaxParallelUploads);

        var uploadTasks = files.Select(async file =>
        {
            await semaphore.WaitAsync();
            try
            {
                var url = await UploadImageAsync(file);
                urls.Add(url);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to upload {file.FileName}: {ex.Message}");
            }
            finally
            {
                semaphore.Release();
            }
        });

        await Task.WhenAll(uploadTasks);

        return urls.ToList();
    }

    public async Task<bool> DeleteImageAsync(string publicId)
    {
        if (string.IsNullOrEmpty(publicId))
            throw new ArgumentException("PublicId cannot be empty.");

        var deletionParams = new DeletionParams(publicId);
        var deletionResult = await cloudinary.DestroyAsync(deletionParams);

        return deletionResult.Result == "ok";
    }

    public async Task<bool> DeleteImagesAsync(IList<string> publicIds)
    {
        if (publicIds == null || publicIds.Count == 0)
            throw new ArgumentException("No PublicIds provided.");

        var deletionResult = await cloudinary.DeleteResourcesAsync(publicIds.ToArray());
        bool allSuccessful = deletionResult.Deleted.All(kvp => kvp.Value == "deleted");

        foreach (var kvp in deletionResult.Deleted)
        {
            if (kvp.Value != "deleted")
            {
                Console.WriteLine($"Failed to delete image with PublicId: {kvp.Key}");
            }
        }

        return allSuccessful;
    }

    public async Task<bool> DeleteImageByUrlAsync(string url)
    {   
        var publicId = ExtractPublicIdFromUrl(url);
        return await DeleteImageAsync(publicId);
    }

    public async Task<bool> DeleteImagesByUrlsAsync(IList<string> urls)
    {
        if (urls == null || urls.Count == 0)
            throw new ArgumentException("No URLs provided.");

        var publicIds = new List<string>();
        foreach (var url in urls)
        {
            try
            {
                var publicId = ExtractPublicIdFromUrl(url);
                publicIds.Add(publicId);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Invalid URL: {url}, Error: {ex.Message}");
            }
        }

        if (publicIds.Count == 0)
            return false;

        return await DeleteImagesAsync(publicIds);
    }
}
