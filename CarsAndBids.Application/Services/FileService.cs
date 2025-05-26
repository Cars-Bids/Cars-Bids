using CarsAndBids.Core.Interfaces;

namespace CarsAndBids.Application.Services
{
    public class FileService(IWebHostEnvironment evn) : IFileService
    {
        const string folderName = "Files";
        public async Task<string> SaveImage(IFormFile file)
        {
            var root = evn.WebRootPath;
            var name = Guid.NewGuid().ToString();
            var ext = Path.GetExtension(file.FileName);

            var relativePath = Path.Combine(folderName, name + ext);
            var fullPath = Path.Combine(root, relativePath);

            using FileStream fs = new FileStream(fullPath, FileMode.Create);
            await file.CopyToAsync(fs);

            return Path.DirectorySeparatorChar + relativePath;
        }

        public async Task<IList<string>> SaveImages(List<IFormFile> files)
        {
            var root = evn.WebRootPath;
            var folderPath = Path.Combine(root, folderName);
            var savedFiles = new List<string>();

            foreach (var file in files)
            {
                var name = Guid.NewGuid().ToString();
                var ext = Path.GetExtension(file.FileName);

                var relativePath = Path.Combine(folderName, name + ext);
                var fullPath = Path.Combine(root, relativePath);

                using FileStream fs = new FileStream(fullPath, FileMode.Create);
                await file.CopyToAsync(fs);

                savedFiles.Add(Path.DirectorySeparatorChar + relativePath);
            }

            return savedFiles;
        }
        public void DeleteImage(string path)
        {
            if (File.Exists(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Images", path)))
            {
                File.Delete(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Images", path));
            }
        }
    }
}
