using AutoMapper;
using Domain.Entities.Identity;
using Core.Models.Account;
using Core.Models.Seeder;

namespace Core.Mapper
{
    public class UserMapper : Profile
    {
        public UserMapper() 
        {
            CreateMap<SeederUserModel, UserEntity>()
                .ForMember(x => x.UserName, opt=>opt.MapFrom(x => x.Email));

            CreateMap<RegisterModel, UserEntity>()
                .ForMember(x => x.UserName, opt => opt.MapFrom(x => x.Email));

            CreateMap<UserEntity, UserItemModel>();

            CreateMap<GoogleAccountModel, UserEntity>()
                .ForMember(x => x.UserName, opt => opt.MapFrom(x => x.Email));
        }
    }
}
