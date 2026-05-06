namespace dotnet8_hero.DTO.Product
{
    public class ProductRequest
    {
        public string Name { get; set; } = "";
        
        public int Stock { get; set; }

        public decimal Price { get; set; }
        
        public int CategoryId { get; set; }

        public List<IFormFile>? FromFiles { get; set; }
    }
}