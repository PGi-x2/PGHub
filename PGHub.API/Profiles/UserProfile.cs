using AutoMapper;
using PGHub.API.DTOs.User;
using PGHub.Domain.Entities;

namespace PGHub.API.Profiles
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            // Define the mapping from User domain model to UserDTO
            CreateMap<User, UserDTO>();
            CreateMap<CreateUserDTO, User>();

            //For specific properties that have different names, use ForMember method as in the following example
                // CreateMap<User, UserDTO>()
                //    .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                //    .ForMember(dest => dest.Username, opt => opt.MapFrom(src => src.Username))
                //    .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email))
                //    .ForMember(dest => dest.CreationDate, opt => opt.MapFrom(src => src.CreationDate))
                //    .ForMember(dest => dest.DeletionDate, opt => opt.MapFrom(src => src.DeletionDate));
        }
    }
}
