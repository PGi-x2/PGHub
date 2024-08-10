using AutoMapper;
using PGHub.Application.DTOs.User;
using PGHub.Domain.Entities;

namespace PGHub.Common.Profiles
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            // Define the mapping from CreateUserDTO to User domain model and vice versa
            CreateMap<User, UserDTO>().ReverseMap();
            
            CreateMap<CreateUserDTO, User>()
                    .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email))
                    .ForMember(dest => dest.FirstName, opt => opt.MapFrom(src => src.FirstName))
                    .ForMember(dest => dest.LastName, opt => opt.MapFrom(src => src.LastName))
                    .ReverseMap();

            CreateMap<UpdateUserDTO, User>().ReverseMap();
        }
    }
}
