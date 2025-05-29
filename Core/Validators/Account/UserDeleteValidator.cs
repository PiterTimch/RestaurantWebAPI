using FluentValidation;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Domain;
using Domain.Entities.Identity;
using Core.Models.Account;

namespace Core.Validators.Account;

public class UserDeleteValidator : AbstractValidator<DeleteUserModel>
{
    public UserDeleteValidator(AppDbRestaurantContext db)
    {
        RuleFor(x => x.Id)
                .GreaterThan(0).WithMessage("Id повинен бути більше 0")
                .MustAsync(async (id, cancellation) =>
                    await db.Users.AnyAsync(c => c.Id == id, cancellation))
                .WithMessage("Користувача з таким Id не знайдено");

    }
}
