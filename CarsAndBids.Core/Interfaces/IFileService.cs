using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace CarsAndBids.Core.Interfaces
{
    public interface IFileService
    {
        Task<string> SaveImage(IFormFile file);
        void DeleteImage(string path);
        Task<IList<string>> SaveImages(List<IFormFile> files);
    }
}
