using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Domain;
using Core.Models.Category;

namespace Core.Validators.Category;

public class CategoryUpdateValidator : AbstractValidator<CategoryEditModel>
{
    public CategoryUpdateValidator(AppDbRestaurantContext db)
    {
        RuleFor(x => x.Id)
            .MustAsync(async (id, cancellation) =>
                await db.Categories.AnyAsync(c => c.Id == id, cancellation))
            .WithMessage("Категорію не знайдено.")
            .WithName("Id");

        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Назва є обов'язковою")
            .MaximumLength(250).WithMessage("Назва повинна містити не більше 250 символів")
            .Must(name => !string.IsNullOrWhiteSpace(name)).WithMessage("Назва не може бути порожньою або null")
            .MustAsync(async (model, name, cancellation) =>
                !await db.Categories
                    .AnyAsync(c => c.Name.ToLower().Trim() == name.ToLower().Trim() && c.Id != model.Id, cancellation))
            .WithMessage("Інша категорія з таким іменем вже існує");

        RuleFor(x => x.Slug)
            .NotEmpty().WithMessage("Слаг є обов'язковим")
            .MaximumLength(250).WithMessage("Слаг повинен містити не більше 250 символів")
            .MustAsync(async (model, slug, cancellation) =>
            {
                var normalized = slug.Trim().ToLower().Replace(" ", "-");
                return !await db.Categories
                    .AnyAsync(c => c.Slug == normalized && c.Id != model.Id, cancellation);
            })
            .WithMessage("Інша категорія з таким слагом вже існує");
    }
}
