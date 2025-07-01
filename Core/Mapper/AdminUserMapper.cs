using AutoMapper;
using Core.Models.AdminUser;
using Domain.Entities.Identity;

namespace Core.Mapper;

public class AdminUserMapper : Profile
{
    public AdminUserMapper() 
    {
        CreateMap<UserEntity, AdminUserItemModel>()
            .ForMember(dest => dest.FullName, opt => opt.MapFrom(src => $"{src.FirstName} {src.LastName}"))
            .ForMember(dest => dest.LoginTypes, opt => opt.Ignore());
    }
}
