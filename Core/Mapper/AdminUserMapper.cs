using AutoMapper;
using Core.Models.AdminUser;
using Domain.Entities.Identity;

namespace Core.Mapper;

public class AdminUserMapper : Profile
{
    public AdminUserMapper() 
    {
        CreateMap<UserEntity, AdminUserItemModel>()
            .ForMember(dest => dest.IsGoogleLogin, opt => opt.MapFrom(src => src.UserLogins!.Any(l => l.LoginProvider == "Google")))
            .ForMember(dest => dest.IsPasswordLogin, opt => opt.MapFrom(src => !string.IsNullOrEmpty(src.PasswordHash)))
            .ForMember(dest => dest.FullName, opt => opt.MapFrom(src => $"{src.FirstName} {src.LastName}"))
            .ForMember(dest => dest.LoginTypes, opt => opt.Ignore())
            .ForMember(dest => dest.Roles, opt => opt.MapFrom(src => src.UserRoles!.Select(ur => ur.Role.Name).ToList()));
        CreateMap<AdminUserItemModel, UserEntity>()
            .ForMember(dest => dest.Image, opt => opt.Ignore());

        CreateMap<AdminUserEditModel, UserEntity>()
            .ForMember(dest => dest.Image, opt => opt.Ignore());
    }
}
