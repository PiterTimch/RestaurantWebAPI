using Core.Models.Cart;
using Domain;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace Core.Validators.Cart;

public class CartItemCreateValidator : AbstractValidator<CartItemCreateModel>
{
    public CartItemCreateValidator(AppDbRestaurantContext db)
    {
        RuleFor(x => x.ProductId)
            .GreaterThan(0).WithMessage("ProductId має бути більшим за 0")
            .MustAsync(async (id, _) => await db.Products.AnyAsync(p => p.Id == id))
            .WithMessage("Продукт із таким ID не знайдено");

        RuleFor(x => x.UserId)
            .GreaterThan(0).WithMessage("UserId має бути більшим за 0")
            .MustAsync(async (id, _) => await db.Users.AnyAsync(u => u.Id == id))
            .WithMessage("Користувача з таким ID не знайдено");
    }
}