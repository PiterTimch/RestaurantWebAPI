using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Domain;
using Core.Models.Category;

namespace Core.Validators.Category
{
    public class CategoryDeleteValidator : AbstractValidator<CategoryDeleteModel>
    {
        public CategoryDeleteValidator(AppDbRestaurantContext db)
        {
            RuleFor(x => x.Id)
                .GreaterThan(0).WithMessage("Id повинен бути більше 0")
                .MustAsync(async (id, cancellation) =>
                    await db.Categories.AnyAsync(c => c.Id == id, cancellation))
                .WithMessage("Категорію з таким Id не знайдено");
        }
    }
}
