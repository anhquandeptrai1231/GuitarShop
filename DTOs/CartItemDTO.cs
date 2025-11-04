namespace GuitarShop.DTOs
{
    public class CartItemDTO
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public string ProductName { get; set; } = string.Empty;
        public string ProductImage { get; set; } = string.Empty;
        public double ProductPrice { get; set; }
        public int Quantity { get; set; }
    }
}
