using FluentValidation;
using RestaurantWebAPI.Models.Category;

namespace RestaurantWebAPI.Validators.Category;

public class CategoryUpdateValidator : AbstractValidator<CategoryEditModel>
{
    public CategoryUpdateValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .WithMessage("Ідентифікатор обов'язковий");
        RuleFor(x => x.Name)
            .NotEmpty()
            .WithMessage("Ім'я є обов'язковим.")
            .Length(1, 250)
            .WithMessage("Ім'я не може бути довшим за 250 символів.");
        RuleFor(x => x.Slug)
            .NotEmpty()
            .WithMessage("Слаг є обов'язковим.")
            .Length(1, 250)
            .WithMessage("Слаг не може бути довшим за 250 символів.");
    }
}
