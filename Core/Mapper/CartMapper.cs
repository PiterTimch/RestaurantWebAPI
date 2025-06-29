﻿using AutoMapper;
using Core.Models.Cart;
using Domain.Entities;
using Domain.Entities.Cart;

namespace Core.Mapper;

public class CartMapper : Profile
{
    public CartMapper()
    {
        CreateMap<CartItemEntity, CartItemModel>()
            .ForMember(x => x.CategoryName, opt => opt.MapFrom(x => x.Product.Category!.Name))
            .ForMember(x => x.Name, opt => opt.MapFrom(x => x.Product.Name))
            .ForMember(x => x.Price, opt => opt.MapFrom(x => x.Product.Price))
            .ForMember(x => x.ProductId, opt => opt.MapFrom(x => x.Product.Id))
            .ForMember(x => x.CategoryId, opt => opt.MapFrom(x => x.Product.Category!.Id))
            .ForMember(x => x.SizeName, opt => opt.MapFrom(x => x.Product!.ProductSize!.Name))
            .ForMember(x => x.ImageName, opt => opt.MapFrom(x =>
                x.Product.ProductImages != null && x.Product.ProductImages.Any()
                    ? x.Product.ProductImages.OrderBy(x => x.Priority).First().Name
                    : null));

        CreateMap<CartEntity, CartListModel>()
            .ForMember(x => x.Items, opt => opt.MapFrom(x => x.CartItems.Where(c => !c.IsDeleted)));

        
    }
}
