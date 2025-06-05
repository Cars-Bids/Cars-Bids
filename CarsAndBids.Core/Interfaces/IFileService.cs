using Microsoft.AspNetCore.Http;

namespace CarsAndBids.Core.Interfaces;

public interface IFileService
{
    Task<string> SaveImage(IFormFile file);
    void DeleteImage(string path);
    Task<IList<string>> SaveImages(List<IFormFile> files);
}
