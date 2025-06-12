using Core.Models.Cart;
using Domain;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace Core.Validators.Cart;

public class CartItemQuantityEditValidator : AbstractValidator<CartItemQuantityEditModel>
{
    public CartItemQuantityEditValidator(AppDbRestaurantContext db)
    {
        RuleFor(x => x.CartItemId)
            .GreaterThan(0).WithMessage("CartItemId має бути більшим за 0")
            .MustAsync(async (id, _) => await db.CartItems.AnyAsync(ci => ci.Id == id && !ci.IsDeleted))
            .WithMessage("CartItem з таким ID не існує або вже видалений");

        RuleFor(x => x.NewQuantity)
            .GreaterThan(0).WithMessage("Кількість повинна бути більшою за 0");
    }
}