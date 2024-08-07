using AutoMapper;
using PGHub.Common.DTOs.User;
using PGHub.Domain.Entities;

namespace PGHub.Common.Profiles
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            // Define only the mapping from User domain model to UserDTO because it only needs to retrieve data from DB
            CreateMap<User, UserDTO>();

            // Define the mapping from CreateUserDTO to User domain model
            // Doesn't return back the values from the DB
            // Why are both needed for the Mapping? Is that correct?
            CreateMap<CreateUserDTO, User>()
                    .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email))
                    .ForMember(dest => dest.FirstName, opt => opt.MapFrom(src => src.FirstName))
                    .ForMember(dest => dest.LastName, opt => opt.MapFrom(src => src.LastName));

            // Define the mapping from User domain model to UserDTO
            // Return back the values from the DB
            CreateMap<User, CreateUserDTO>()
                    .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email))
                    .ForMember(dest => dest.FirstName, opt => opt.MapFrom(src => src.FirstName))
                    .ForMember(dest => dest.LastName, opt => opt.MapFrom(src => src.LastName));

            CreateMap<UpdateUserDTO, User>();
            CreateMap<User, UpdateUserDTO>();
        }
    }
}
