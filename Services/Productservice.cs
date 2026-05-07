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
    }
}