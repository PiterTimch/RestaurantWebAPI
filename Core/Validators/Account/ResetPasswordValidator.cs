using Core.Models.Account;
using Domain.Entities.Identity;
using FluentValidation;
using Microsoft.AspNetCore.Identity;

namespace Core.Validators.Account;

public class ResetPasswordValidator : AbstractValidator<ResetPasswordModel>
{
    public ResetPasswordValidator()
    {
        RuleFor(x => x.NewPassword)
            .NotEmpty().WithMessage("Пароль є обов'язковим")
            .MinimumLength(6).WithMessage("Пароль повинен містити щонайменше 6 символів")
            .Matches("[A-Z]").WithMessage("Пароль повинен містити хоча б одну латинську велику літеру")
            .Matches("[a-z]").WithMessage("Пароль повинен містити хоча б одну латинську малу літеру")
            .Matches("[0-9]").WithMessage("Пароль повинен містити хоча б одну цифру")
            .Matches("[^a-zA-Z0-9]").WithMessage("Пароль повинен містити хоча б один спеціальний символ");
    }
}