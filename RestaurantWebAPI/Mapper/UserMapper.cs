using AutoMapper;
using RestaurantWebAPI.Data.Entities.Identity;
using RestaurantWebAPI.Models.Account;
using RestaurantWebAPI.Models.Seeder;

namespace RestaurantWebAPI.Mapper
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
        }
    }
}
