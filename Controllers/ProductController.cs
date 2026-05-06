using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using dotnet8_hero.Data;
using dotnet8_hero.DTO;
using dotnet8_hero.DTO.Product;
using dotnet8_hero.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
//using dotnet8_hero.Models;

namespace dotnet8_hero.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        public DatabaseContext DatabaseContext { get; set; }
        public ProductController(DatabaseContext databaseContext)
        {
            this.DatabaseContext = databaseContext;
        }
        

        [HttpGet]
        // non i action result to custom tpye to parse.
        public ActionResult<IEnumerable<ProductResponse>> GetProducts()
        {
            //search all products
            var products = this.DatabaseContext.Products.Include(p=>p.Category).Select(ProductResponse.FromProduct).ToList();
            return products;
        }

        [HttpGet("{id}")]
        public IActionResult GetProductById(int id)
        {
            // var product = this.DatabaseContext.Products.Find(id);
            // return Ok(product);

            //  var SelectedProduct = this.DatabaseContext.Products.Find(id);
            // // return Ok(ProductResponse.FromProduct(SelectedProduct));

            // if (SelectedProduct != null)
            // {
            //     var Product = ProductResponse.FromProduct(SelectedProduct);
            //     return Ok(Product);
            // }
            
            // return NotFound();

            // LINQ Technique
            var selectedProduct = this.DatabaseContext.Products.Include(p=>p.Category).Select(ProductResponse.FromProduct).Where(p=>p.ProductId==id).FirstOrDefault();
            return Ok(selectedProduct);
        }

        [HttpPost]
        public IActionResult AddProduct([FromForm] ProductRequest productRequest)
        {
            var product = new Product
            {
                Name = productRequest.Name,
                Stock = productRequest.Stock,
                Price = productRequest.Price,
                CategoryId = productRequest.CategoryId
            };
            product.Image = "";

            this.DatabaseContext.Products.Add(product);
            this.DatabaseContext.SaveChanges();
            return StatusCode((int)HttpStatusCode.Created, product);
        }
    }
}