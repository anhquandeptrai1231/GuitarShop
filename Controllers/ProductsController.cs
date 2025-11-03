using GuitarShop.DTOs;
using GuitarShop.Response;
using GuitarShop.Services;
using GuitarShop.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;

namespace GuitarShop.Controllers
{
    
        [Route("api/[controller]")]
        [ApiController]
        public class ProductController : ControllerBase
        {
            private readonly IProductServices _productService;

            public ProductController(IProductServices productService)
            {
                _productService = productService;
            }

            [HttpGet]
            public async Task<IActionResult> GetAll()
            {
                var products = await _productService.GetAllProductsAsync();
                if (products != null) {
                    return Ok(ApiResponse<IEnumerable<ProductDTO>>.SuccessResponse(products, "Lay danh sach san pham thanh cong"));
                }
                return BadRequest(ApiResponse<string>.FailResponse("Khong tim thay san pham"));
            }

            [HttpGet("{id}")]
            public async Task<IActionResult> GetById(int id)
            {
                var product = await _productService.GetProductByIdAsync(id);
                if (product == null)
                {
                    return BadRequest(ApiResponse<string>.FailResponse("Khong tim thay san pham "));
                }
                return Ok(ApiResponse<ProductDTO>.SuccessResponse(product, "Lay san pham thanh cong."));
                
            }

            [HttpPost]
            public async Task<IActionResult> Create(CreateProductDTO dto)
            {
                await _productService.AddProductAsync(dto);
                return Ok(ApiResponse<string>.SuccessResponse("Tao san pham thanh cong"));
            }

            [HttpPut("{id}")]
            public async Task<IActionResult> Update(int id, ProductDTO dto)
            {
                await _productService.UpdateProductAsync(id, dto);
                return Ok("Updated successfully");
            }

            [HttpDelete("{id}")]
            public async Task<IActionResult> Delete(int id)
            {
                await _productService.DeleteProductAsync(id);
                return Ok("Deleted successfully");
            }
        [HttpGet("getbycategoryid")]
        public async Task<IActionResult> GetProductsByCategory([FromQuery] int categoryId)
        {
            var products = await _productService.GetProductsByCategoryAsync(categoryId);
            return Ok(products);
        }
    }


    }

