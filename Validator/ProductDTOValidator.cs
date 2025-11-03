using FluentValidation;
using GuitarShop.DTOs;
using System.Runtime.InteropServices;

namespace GuitarShop.Validator
{
    public class ProductDTOValidator : AbstractValidator<CreateProductDTO>
    {
        public ProductDTOValidator() {
            RuleFor(x  => x.Name).NotEmpty()
                .WithMessage("Ten san pham khong duoc de trong")
                .MaximumLength(100)
                .WithMessage("Ten san pham khong vuot qua 100 ki tu");
            RuleFor(x => x.Description).MaximumLength(500)
                .WithMessage("Do dai khong qua 500 ki tu");
            RuleFor(x => x.Price).GreaterThan(0)
                .WithMessage("Gia phai lon hon 0");
            
        
        }
    }
}
