using FluentValidation;
using Microsoft.EntityFrameworkCore;
using RestaurantWebAPI.Data;
using RestaurantWebAPI.Models.Category;

namespace RestaurantWebAPI.Validators.Category;

public class CategoryUpdateValidator : AbstractValidator<CategoryEditModel>
{
    public CategoryUpdateValidator(AppDbRestaurantContext db)
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .WithMessage("Назва є обов'язковою")
            .Must(name => !string.IsNullOrEmpty(name))
            .WithMessage("Назва не може бути порожньою або null")
            .DependentRules(() =>
            {
                RuleFor(x => x.Name)
                    .MustAsync(async (name, cancellation) =>
                        !await db.Categories.AnyAsync(c => c.Name.ToLower() == name.ToLower().Trim(), cancellation))
                    .WithMessage("Категорія з такою назвою вже існує");
            })
            .MaximumLength(250)
            .WithMessage("Назва повинна містити не більше 250 символів");

        RuleFor(x => x.Slug)
            .NotEmpty()
            .WithMessage("Слаг є обов'язковим")
            .MaximumLength(250)
            .WithMessage("Слаг повинен містити не більше 250 символів");
    }
}
