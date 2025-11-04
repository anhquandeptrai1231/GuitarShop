namespace GuitarShop.DTOs
{
    public class OrderRequest
    {
        public string OrderId { get; set; }        // Mã đơn hàng, có thể để trống để tự sinh
        public string OrderDescription { get; set; } // Mô tả đơn hàng
        public decimal Amount { get; set; }        // Số tiền (VND)
    }
}
