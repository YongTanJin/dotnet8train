using System.Runtime.InteropServices;
using dotnet8_hero.Entities;

namespace dotnet8_hero.Interfaces
{
    public interface IProductService
    {
        Task<IEnumerable<Product>> FindAll();
        Task<Product> FindById(int id);
        Task Create(Product product);
        Task Update(Product product);
        Task<IEnumerable<Product>> Search(string name);
        // Task<(string ErrorMessage, string imageName)> UploadImage(List<IFormFile> formFiles);
    }
}