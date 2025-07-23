using Core.Models.AdminUser;
using Core.Models.Category;
using Domain;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace Core.Validators.User;

public class AdminUserEditValidator : AbstractValidator<AdminUserEditModel>
{
    public AdminUserEditValidator(AppDbRestaurantContext db)
    {
        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email є обов’язковим.")
            .EmailAddress().WithMessage("Некоректний формат email.");

        RuleFor(x => x.FirstName)
            .NotEmpty().WithMessage("Ім'я є обов’язковим.")
            .MaximumLength(100).WithMessage("Ім'я не може бути довшим за 100 символів.")
            .Must(name => !string.IsNullOrWhiteSpace(name)).WithMessage("Ім'я не може бути порожнім.");

        RuleFor(x => x.LastName)
            .NotEmpty().WithMessage("Прізвище є обов’язковим.")
            .MaximumLength(100).WithMessage("Прізвище не може бути довшим за 100 символів.")
            .Must(name => !string.IsNullOrWhiteSpace(name)).WithMessage("Прізвище не може бути порожнім.");

        RuleFor(x => x.Roles)
            .MustAsync(async (roles, cancellation) =>
            {
                if (roles == null || roles.Count == 0)
                    return true;

                var existingRoles = await db.Roles.Select(r => r.Name).ToListAsync(cancellation);
                return roles.All(r => existingRoles.Contains(r));
            })
            .WithMessage("Деякі ролі не існують у системі.");
    }
}
