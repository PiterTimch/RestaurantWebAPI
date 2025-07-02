using Core.Models.Search.Params;
using Domain;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace Core.Validators.Search;

public class CategorySearchValidator : AbstractValidator<CategorySearchModel>
{
    public CategorySearchValidator(AppDbRestaurantContext db)
    {
        RuleFor(x => x.Id)
            .GreaterThan(0).WithMessage("Id повинен бути більше 0")
            .MustAsync(async (id, cancellation) =>
                await db.Categories.AnyAsync(c => c.Id == id, cancellation))
            .WithMessage("Категорію з таким Id не знайдено")
            .When(x => x.Id.HasValue);

        RuleFor(x => x.ItemsPerPage)
            .NotNull().WithMessage("Кількість елементів на сторінці обов'язкова")
            .GreaterThan(0).WithMessage("Кількість елементів на сторінці повинна бути більше 0")
            .LessThanOrEqualTo(50).WithMessage("Кількість елементів на сторінці не може перевищувати 50");

        RuleFor(x => x.PageNumber)
            .NotNull().WithMessage("Номер сторінки обов'язковий")
            .GreaterThan(0).WithMessage("Номер сторінки повинен бути більше 0")
            .MustAsync(async (model, page, cancellation) =>
            {
                if (model.ItemsPerPage <= 0) return true;

                var totalItems = await db.Categories.CountAsync(cancellation);
                var totalPages = (int)Math.Ceiling((double)totalItems / model.ItemsPerPage);
                return page <= totalPages;
            })
            .WithMessage("Номер сторінки перевищує кількість доступних сторінок.");
    }
}
