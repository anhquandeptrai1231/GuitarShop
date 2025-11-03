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
            CreateMap<Product, CreateProductDTO>()
               .ForMember(dest => dest.CategoryName, opt => opt.MapFrom(src => src.Category.Name));
            CreateMap<CreateProductDTO, Product>()
    .ForMember(dest => dest.CategoryId, opt => opt.Ignore()) // gán thủ công
    .ForMember(dest => dest.Id, opt => opt.Ignore());        // PK tự tăng
        }
    }
}