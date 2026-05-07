using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Razor.Hosting;

namespace dotnet8_hero.DTO.Product
{
    public class ProductRequest
    {
        public int ProductId { get; set; }
        
        [Required]
        [MaxLength(10, ErrorMessage = "Name maximum length is 10.")]
        public string Name { get; set; } = "";
        
        [Range(0, 10000, ErrorMessage = "Stock must be between 0 and 10000.")]
        public int Stock { get; set; }

        [Range(0, 10000, ErrorMessage = "Price must be between 0 and 10000.")]
        public decimal Price { get; set; }
        
        public int CategoryId { get; set; }

        public List<IFormFile>? FromFiles { get; set; }
    }
}