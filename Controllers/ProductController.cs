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
using dotnet8_hero.Services;
//using dotnet8_hero.Models;

namespace dotnet8_hero.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class ProductController : ControllerBase

    {
        public IProductService ProductService { get; }

        public ProductController(IProductService productService)
        {
            ProductService = productService;
        }


        [HttpGet]
        // non i action result to custom tpye to parse.
        public async Task<ActionResult<IEnumerable<ProductResponse>>> GetProductsAsync()
        {
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

        [HttpGet("search")]
        public async Task<ActionResult<IEnumerable<ProductResponse>>> Search([FromQuery] string name)
        {
            var result = (await this.ProductService.Search(name)).Select(ProductResponse.FromProduct).ToList();
            return result;
        }

        [HttpPost]
        public async Task<IActionResult> AddProductAsync([FromForm] ProductRequest productRequest)
        {
            string finalImageName = "";
            if(productRequest.FromFiles != null)
            {
                (string errorMessage, string imageName) = await ProductService.UploadImage(productRequest.FromFiles);
                if (!string.IsNullOrEmpty(errorMessage))
                {
                    return BadRequest(errorMessage);
                }
                finalImageName = imageName;
            }
            
            var product = productRequest.Adapt<Product>();
            product.Image = finalImageName;
            await this.ProductService.Create(product);
            return StatusCode((int)HttpStatusCode.Created, product);
        }
        
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateProductAsync(int id, [FromForm] ProductRequest productRequest)
        {
            if (id != productRequest.ProductId)
            {
                return BadRequest();
            }

            var product = await this.ProductService.FindById(id);
            if (product == null)
            {
                return NotFound();
            }

            // (string errorMessage, string imageName) = await ProductService.UploadImage(productRequest.FromFiles);
            // if (!string.IsNullOrEmpty(errorMessage))
            // {
            //     return BadRequest(errorMessage);
            // }
            // if (!string.IsNullOrEmpty(imageName))
            // {
            //     product.Image = imageName;
            // }

            productRequest.Adapt(product);
            await ProductService.Update(product);
            return Ok(ProductResponse.FromProduct(product));
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProductAsync(int id)
        {
            var product = await this.ProductService.FindById(id);
            if (product == null)
            {
                return NotFound();
            }

            await ProductService.Delete(product);
            return NoContent();
        }
    }
}