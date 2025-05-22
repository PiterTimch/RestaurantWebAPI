using FluentValidation;
using Microsoft.EntityFrameworkCore;
using RestaurantWebAPI.Data;
using RestaurantWebAPI.Models.Category;

namespace RestaurantWebAPI.Validators.Category;

public class CategoryCreateValidator : AbstractValidator<CategoryCreateModel>
{
    public CategoryCreateValidator(AppDbRestaurantContext db)
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .WithMessage("Ім'я є обов'язковим.")
            .Length(1, 250)
            .WithMessage("Ім'я не може бути довшим за 250 символів.");
            //.MustAsync(async (name, cancellation) =>
            //!await db.Categories.AnyAsync(c => c.Name == name, cancellation))
            //.WithMessage("Категорія з такою назвою вже існує");
        RuleFor(x => x.Slug)
            .NotEmpty()
            .WithMessage("Слаг є обов'язковим.")
            .Length(1, 250)
            .WithMessage("Слаг не може бути довшим за 250 символів.");
    }
}
