using AutoMapper;
using WakaWaka.API.DataAccess.DTO;
using WakaWaka.API.Domain.Models.restaurant;
using WakaWaka.API.Domain.Models.User;
using WakaWaka.API.Domain.Models.User.UserDTO;
using WakaWaka.API.Models.Hotel;
using WakaWaka.API.Models.Restaurant;
using WakaWaka.API.Models.Resturant;

namespace WakaWaka.API.DataAccess.Mapping_Profiles
{
    public class EntityMappingProfiles : Profile
    {
        public EntityMappingProfiles()
        {
            CreateMap<RestaurantCreateDTO, Restaurant>().ReverseMap();
            CreateMap<Restaurant, RestaurantGetDTO>().ReverseMap();
            CreateMap<HotelCreateDTO, Hotel>().ReverseMap();
            CreateMap<Hotel, HotelGetDTO>().ReverseMap();
            CreateMap<RestaurantReviewCreateDTO, RestaurantReview>().ReverseMap();
            CreateMap<RestaurantReview, RestaurantReviewGetDTO>().ReverseMap();
            CreateMap<HotelReviewCreateDTO, HotelReview>().ReverseMap();
            CreateMap<HotelReview, HotelReviewGetDTO>().ReverseMap();
            CreateMap<UserCreateDTO, User>().ReverseMap();
            CreateMap<User, UserGetDTO>().ReverseMap();
            CreateMap<UserUpdateDTO, User>().ReverseMap();  
        }
    }
}
