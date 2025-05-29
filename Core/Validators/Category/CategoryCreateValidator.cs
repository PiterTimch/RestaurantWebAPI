using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Domain;
using Core.Models.Category;

namespace Core.Validators.Category;

public class CategoryCreateValidator : AbstractValidator<CategoryCreateModel>
{
    public CategoryCreateValidator(AppDbRestaurantContext db)
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Назва є обов'язковою")
            .MaximumLength(250).WithMessage("Назва повинна містити не більше 250 символів")
            .Must(name => !string.IsNullOrWhiteSpace(name)).WithMessage("Назва не може бути порожньою або null")
            .MustAsync(async (name, cancellation) =>
                !await db.Categories.AnyAsync(c => c.Name.ToLower() == name.ToLower().Trim(), cancellation))
            .WithMessage("Категорія з такою назвою вже існує");

        RuleFor(x => x.Slug)
            .NotEmpty().WithMessage("Слаг є обов'язковим")
            .MaximumLength(250).WithMessage("Слаг повинен містити не більше 250 символів")
            .MustAsync(async (slug, cancellation) =>
            {
                var normalized = slug.Trim().ToLower().Replace(" ", "-");
                return !await db.Categories.AnyAsync(c => c.Slug == normalized, cancellation);
            })
            .WithMessage("Категорія з таким слагом вже існує");
    }
}
