using AutoMapper;
using PGHub.Application.DTOs.Attachment;
using PGHub.Application.DTOs.Post;
using PGHub.Domain.Entities;

namespace PGHub.Application.Profiles
{
    public class PostProfile : Profile
    {
        public PostProfile()
        {
            // Define the mapping from Post domain model to PostDTO and vice versa
            CreateMap<Post, PostDTO>()
                .ForMember(dest => dest.Attachments, opt => opt.MapFrom(src => src.Attachments))
                .ReverseMap();

            CreateMap<Attachment, AttachmentDTO>().ReverseMap();

            CreateMap<CreatePostDTO, Post>().ReverseMap();

            CreateMap<UpdatePostDTO, Post>().ReverseMap();
        }
    }
}
