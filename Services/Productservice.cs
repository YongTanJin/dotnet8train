using dotnet8_hero.Data;
using dotnet8_hero.Entities;
using dotnet8_hero.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace dotnet8_hero.Services
{
    public class ProductService : IProductService
    {   
        public DatabaseContext DatabaseContext { get; }
        public IUploadFileService UploadFileService { get; set; }

        public ProductService(DatabaseContext databaseContext, IUploadFileService UploadFileService)
        {
            this.DatabaseContext = databaseContext;
            this.UploadFileService = UploadFileService;
        }

        public async Task<IEnumerable<Product>> FindAll()
        {
            return await this.DatabaseContext.Products.Include(p => p.Category).ToListAsync();
        }

        public Task<Product> FindById(int id)
        {
            return this.DatabaseContext.Products.Include(p => p.Category).Where(p => p.ProductId == id).FirstOrDefaultAsync();
        }

        public async Task Create(Product product)
        {
            DatabaseContext.Products.Add(product);
            await DatabaseContext.SaveChangesAsync();
        }

        public async Task Delete(Product product)
        {
            DatabaseContext.Products.Remove(product);
            await DatabaseContext.SaveChangesAsync();
        }

        public async Task Update(Product product)
        {
            DatabaseContext.Products.Update(product);
            await DatabaseContext.SaveChangesAsync();
        }

        public async Task<IEnumerable<Product>> Search(string name)
        {
            return await DatabaseContext.Products.Include(p => p.Category).Where(p => p.Name.Contains(name)).ToListAsync();
        }

        public async Task<(string ErrorMessage, string imageName)> UploadImage(List<IFormFile> formFiles)
        {
            string errorMesage = String.Empty;
            string imageName = String.Empty;
            if (UploadFileService.IsUpload(formFiles))
            {
                errorMesage = UploadFileService.Validation(formFiles);
                if (string.IsNullOrEmpty(errorMesage))
                {
                    imageName = (await UploadFileService.UploadImages(formFiles))[0];
                }
            }
            return (errorMesage, imageName);
        }
    }
}