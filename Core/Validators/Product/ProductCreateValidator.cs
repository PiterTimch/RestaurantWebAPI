using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Core.Models.Product;
using Domain;

namespace Core.Validators.Product
{
    public class ProductCreateValidator : AbstractValidator<ProductCreateModel>
    {
        public ProductCreateValidator(AppDbRestaurantContext db)
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Назва є обов'язковою")
                .MaximumLength(250).WithMessage("Назва повинна містити не більше 250 символів")
                .Must(name => !string.IsNullOrWhiteSpace(name)).WithMessage("Назва не може бути порожньою або null");

            RuleFor(x => x.Slug)
                .NotEmpty().WithMessage("Слаг є обов'язковим")
                .MaximumLength(250).WithMessage("Слаг повинен містити не більше 250 символів")
                .Must(slug => !string.IsNullOrWhiteSpace(slug)).WithMessage("Слаг не може бути порожнім");

            RuleFor(x => x.CategoryId)
                .NotNull().WithMessage("Категорія є обов'язковою");
        }
    }
}
