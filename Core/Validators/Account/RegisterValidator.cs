using FluentValidation;
using Microsoft.AspNetCore.Identity;
using Domain.Entities.Identity;
using Core.Models.Account;

namespace Core.Validators.Account;

public class RegisterValidator : AbstractValidator<RegisterModel>
{
    public RegisterValidator(UserManager<UserEntity> userManager)
    {
        RuleFor(x => x.FirstName)
            .NotEmpty().WithMessage("Ім'я є обов'язковим")
            .MaximumLength(100).WithMessage("Ім'я не може бути довше 100 символів");

        RuleFor(x => x.LastName)
            .NotEmpty().WithMessage("Прізвище є обов'язковим")
            .MaximumLength(100).WithMessage("Прізвище не може бути довше 100 символів");

        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Електронна пошта є обов’язковою")
            .EmailAddress().WithMessage("Некоректний формат електронної пошти");

        RuleFor(x => x.Email)
            .MustAsync(async (email, cancellation) =>
            {
                var user = await userManager.FindByEmailAsync(email);
                return user == null;
            })
            .WithMessage("Користувач з такою поштою вже зареєстрований")
            .When(x => !string.IsNullOrWhiteSpace(x.Email));

        RuleFor(x => x.Password)
            .NotEmpty().WithMessage("Пароль є обов'язковим")
            .MinimumLength(6).WithMessage("Пароль повинен містити щонайменше 6 символів")
            .Matches("[A-Z]").WithMessage("Пароль повинен містити хоча б одну латинську велику літеру")
            .Matches("[a-z]").WithMessage("Пароль повинен містити хоча б одну латинську малу літеру")
            .Matches("[0-9]").WithMessage("Пароль повинен містити хоча б одну цифру")
            .Matches("[^a-zA-Z0-9]").WithMessage("Пароль повинен містити хоча б один спеціальний символ");

    }
}
