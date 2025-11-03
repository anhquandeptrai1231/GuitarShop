using GuitarShop.DTOs;

namespace GuitarShop.Services.Interfaces
{
    public interface IProductServices
    {
        Task<IEnumerable<ProductDTO>> GetAllProductsAsync();
        Task<ProductDTO?> GetProductByIdAsync(int id);
        Task AddProductAsync(CreateProductDTO dto);
        Task UpdateProductAsync(int id, ProductDTO dto);
        Task DeleteProductAsync(int id);
    }
}
