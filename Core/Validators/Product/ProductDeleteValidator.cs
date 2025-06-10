using Core.Models.Product;
using Domain;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace Core.Validators.Product
{
    public class ProductDeleteValidator : AbstractValidator<ProductDeleteModel>
    {
        public ProductDeleteValidator(AppDbRestaurantContext db)
        {
            RuleFor(x => x.Id)
                .GreaterThan(0).WithMessage("Id повинен бути більше 0")
                .MustAsync(async (id, cancellation) =>
                    await db.Products.AnyAsync(c => c.Id == id, cancellation))
                .WithMessage("Продукт з таким Id не знайдено");
        }
    }
}
