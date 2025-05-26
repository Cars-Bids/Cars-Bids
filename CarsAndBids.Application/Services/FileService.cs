using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using CarsAndBids.Core.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;

namespace CarsAndBids.Application.Services
{
    public class FileService : IFileService
    {
        private readonly IWebHostEnvironment _env;
        private readonly string _folderName;
        private readonly long _maxFileSize;
        private readonly string[] _allowedExtensions = { ".jpg", ".jpeg", ".png", ".gif" };

        public FileService(IWebHostEnvironment env, IConfiguration configuration)
        {
            _env = env;
            _folderName = configuration.GetValue<string>("FileStorage:FolderName") ?? "Files";
            _maxFileSize = configuration.GetValue<long>("FileStorage:MaxFileSize", 5 * 1024 * 1024);
        }

        public async Task<string> SaveImage(IFormFile file)
        {
            ValidateFile(file);

            var root = _env.WebRootPath;
            var folderPath = Path.Combine(root, _folderName);
            Directory.CreateDirectory(folderPath);

            var name = Guid.NewGuid().ToString();
            var ext = Path.GetExtension(file.FileName);
            var relativePath = Path.Combine(_folderName, name + ext);
            var fullPath = Path.Combine(root, relativePath);

            try
            {
                using var fs = new FileStream(fullPath, FileMode.Create);
                await file.CopyToAsync(fs);
                return Path.DirectorySeparatorChar + relativePath;
            }
            catch (Exception ex)
            {
                throw new Exception("Не вдалося зберегти файл.", ex);
            }
        }

        public async Task<IList<string>> SaveImages(List<IFormFile> files)
        {
            var savedFiles = new List<string>();
            var root = _env.WebRootPath;
            var folderPath = Path.Combine(root, _folderName);
            Directory.CreateDirectory(folderPath);

            try
            {
                foreach (var file in files)
                {
                    ValidateFile(file);

                    var name = Guid.NewGuid().ToString();
                    var ext = Path.GetExtension(file.FileName);
                    var relativePath = Path.Combine(_folderName, name + ext);
                    var fullPath = Path.Combine(root, relativePath);

                    using var fs = new FileStream(fullPath, FileMode.Create);
                    await file.CopyToAsync(fs);
                    savedFiles.Add(Path.DirectorySeparatorChar + relativePath);
                }
                return savedFiles;
            }
            catch (Exception ex)
            {
                foreach (var savedFile in savedFiles)
                {
                    DeleteImage(savedFile);
                }
                throw new Exception("Не вдалося зберегти файли.", ex);
            }
        }

        public void DeleteImage(string path)
        {
            if (string.IsNullOrEmpty(path))
                return;

            var fullPath = Path.Combine(_env.WebRootPath, path.TrimStart(Path.DirectorySeparatorChar));
            if (File.Exists(fullPath))
            {
                File.Delete(fullPath);
            }
        }
        private void ValidateFile(IFormFile file)
        {
            if (file == null || file.Length == 0)
                throw new ArgumentException("Файл не надано.");
            if (file.Length > _maxFileSize)
                throw new ArgumentException($"Розмір файлу перевищує ліміт ({_maxFileSize / 1024 / 1024} МБ).");
            var ext = Path.GetExtension(file.FileName).ToLowerInvariant();
            if (!_allowedExtensions.Contains(ext))
                throw new ArgumentException("Непідтримуваний формат файлу. Дозволені: " + string.Join(", ", _allowedExtensions));
        }
    }
}