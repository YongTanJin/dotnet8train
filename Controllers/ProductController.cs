using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using dotnet8_hero.Data;
using dotnet8_hero.DTO;
using dotnet8_hero.DTO.Product;
using dotnet8_hero.Entities;
using Mapster;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using dotnet8_hero.Interfaces;
//using dotnet8_hero.Models;

namespace dotnet8_hero.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class ProductController : ControllerBase

    {
        public DatabaseContext DatabaseContext { get; set; }
        public IProductService ProductService { get; }

        public ProductController(DatabaseContext databaseContext, IProductService productService)
        {
            this.DatabaseContext = databaseContext;
            ProductService = productService;
        }


        [HttpGet]
        // non i action result to custom tpye to parse.
        public async Task<ActionResult<IEnumerable<ProductResponse>>> GetProducts()
        {
            //search all products
            // var products = this.DatabaseContext.Products.Include(p=>p.Category).Select(ProductResponse.FromProduct).ToList();
            // return products;

            return (await this.ProductService.FindAll()).Select(ProductResponse.FromProduct).ToList();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ProductResponse>> GetProductByIdAsync(int id)
        {
            var product = await this.ProductService.FindById(id);
            if (product == null)
            {
                return NotFound();
            }

            return ProductResponse.FromProduct(product);
        }

        [HttpPost]
        public IActionResult AddProduct([FromForm] ProductRequest productRequest)
        {
            var product = productRequest.Adapt<Product>();
            this.DatabaseContext.Products.Add(product);
            this.DatabaseContext.SaveChanges();
            return StatusCode((int)HttpStatusCode.Created, product);
        }

        // [HttpDelete("{id}")]
        // public IActionResult DeleteProduct(int id)
        // {
        //     var product = this.DatabaseContext.Products.Find(id);
        //     if (product == null)
        //     {
        //         return NotFound();
        //     }

        //     this.DatabaseContext.Products.Remove(product);
        //     this.DatabaseContext.SaveChangesAsync();
        //     return NoContent();         
        // }

        // this is async exammple
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProductAsync(int id)
        {
            var product = await this.DatabaseContext.Products.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }

            this.DatabaseContext.Products.Remove(product);
            await this.DatabaseContext.SaveChangesAsync();
            return NoContent();
        }

        [HttpGet("search")]
        public async Task<ActionResult<IEnumerable<ProductResponse>>> Search([FromQuery] string name)
        {
            var result = (await this.ProductService.Search(name)).Select(ProductResponse.FromProduct).ToList();
            return result;
        }
    }
}