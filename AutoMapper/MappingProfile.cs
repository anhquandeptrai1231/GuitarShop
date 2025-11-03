using AutoMapper;
using GuitarShop.DTOs;
using GuitarShop.Models;

namespace GuitarShop.AutoMapper
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Product, ProductDTO>()
                .ForMember(dest => dest.CategoryName, opt => opt.MapFrom(src => src.Category.Name));
            CreateMap<Product, CreateProductDTO>();
              
               // PK tự tăng
        }
    }
}