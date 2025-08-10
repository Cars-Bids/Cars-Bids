using Microsoft.AspNetCore.Http;

namespace Steria.Core.Interfaces;

public interface IFileService
{
    Task<string> UploadImageAsync(IFormFile file);
    Task<List<string>> UploadImagesAsync(IList<IFormFile> files);
    Task<bool> DeleteImageAsync(string publicId);
    Task<bool> DeleteImagesAsync(IList<string> publicIds);
    Task<bool> DeleteImageByUrlAsync(string url);
    Task<bool> DeleteImagesByUrlsAsync(IList<string> urls);
}
