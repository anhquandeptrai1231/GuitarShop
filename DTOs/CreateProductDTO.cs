namespace GuitarShop.DTOs
{
    public class CreateProductDTO
    {
        public string Name { get; set; } = null!;

        public string? Description { get; set; }

        public decimal Price { get; set; }

        public string? ImageUrl { get; set; }

        public int CategoryId { get; set; }
    }
}
