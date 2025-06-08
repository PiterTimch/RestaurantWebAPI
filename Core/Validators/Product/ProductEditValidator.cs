using Core.Models.Product;
using Domain;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
namespace Core.Validators.Product;

public class ProductEditValidator : AbstractValidator<ProductEditModel>
{
    public ProductEditValidator(AppDbRestaurantContext db)
    {
        RuleFor(x => x.Id)
            .GreaterThan(0).WithMessage("Невірний ідентифікатор продукту");

        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Назва є обов'язковою")
            .MaximumLength(250).WithMessage("Назва повинна містити не більше 250 символів")
            .Must(name => !string.IsNullOrWhiteSpace(name)).WithMessage("Назва не може складатися лише з пробілів");

        RuleFor(x => x.Slug)
            .NotEmpty().WithMessage("Слаг є обов'язковим")
            .MaximumLength(250).WithMessage("Слаг повинен містити не більше 250 символів")
            .Must(slug => !string.IsNullOrWhiteSpace(slug)).WithMessage("Слаг не може складатися лише з пробілів")
            .MustAsync(async (model, slug, cancellation) =>
            {
                return !await db.Products
                    .AnyAsync(p => p.Slug == slug && p.Id != model.Id, cancellationToken: cancellation);
            }).WithMessage("Продукт з таким слагом вже існує");

        RuleFor(x => x.CategoryId)
            .GreaterThan(0).WithMessage("Категорія є обов'язковою")
            .MustAsync(async (categoryId, cancellation) =>
            {
                return await db.Categories
                    .AnyAsync(c => c.Id == categoryId, cancellationToken: cancellation);
            }).WithMessage("Категорії з таким ID не існує");

        RuleForEach(x => x.IngredientIds!)
            .GreaterThan(0).WithMessage("Id інгредієнта має бути додатнім")
            .MustAsync(async (ingredientId, cancellation) =>
            {
                return await db.Ingredients
                    .AnyAsync(i => i.Id == ingredientId, cancellationToken: cancellation);
            }).WithMessage("Інгредієнта з таким ID не існує");
    }
}