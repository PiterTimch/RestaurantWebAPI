using Core.Models.Delivery;
using FluentValidation;

namespace Core.Validators.Order;

public class OrderCreateValidator : AbstractValidator<DeliveryInfoCreateModel>
{
    public OrderCreateValidator()
    {
        RuleFor(x => x.CityId)
            .GreaterThan(0).WithMessage("Виберіть місто");
        RuleFor(x => x.PostDepartmentId)
            .GreaterThan(0).WithMessage("Виберіть відділення");
        RuleFor(x => x.PaymentTypeId)
            .GreaterThan(0).WithMessage("Виберіть тип оплати");
        RuleFor(x => x.PhoneNumber)
            .NotEmpty().WithMessage("Номер телефону є обов'язковим")
            .Matches(@"^\+380\d{9}$").WithMessage("Номер телефону має бути у форматі +380XXXXXXXXX");
        RuleFor(x => x.RecipientName)
            .NotEmpty().WithMessage("Ім'я отримувача є обов'язковим")
            .MaximumLength(100).WithMessage("Ім'я отримувача не може перевищувати 100 символів");
    }
}
