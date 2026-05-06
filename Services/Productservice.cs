using dotnet8_hero.Data;
using dotnet8_hero.Entities;
using dotnet8_hero.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace dotnet8_hero.Services
{
    public class ProductService : IProductService
    {   
        public DatabaseContext DatabaseContext { get; }

        public ProductService(DatabaseContext databaseContext)
        {
            this.DatabaseContext = databaseContext;
        }

        public async Task<IEnumerable<Product>> FindAll()
        {
            return await this.DatabaseContext.Products.Include(p => p.Category).ToListAsync();
        }

        public Task<Product> FindById(int id)
        {
            return this.DatabaseContext.Products.Include(p => p.Category).Where(p => p.ProductId == id).FirstOrDefaultAsync();
        }

        public Task Create(Product product)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<Product>> Search(string name)
        {
            return await DatabaseContext.Products.Include(p => p.Category).Where(p => p.Name.Contains(name)).ToListAsync();
        }

        public Task Update(Product product)
        {
            throw new NotImplementedException();
        }
    }
}