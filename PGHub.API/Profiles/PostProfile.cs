using AutoMapper;
using PGHub.Common.DTOs.Attachment;
using PGHub.Common.DTOs.Post;
using PGHub.Domain.Entities;

namespace PGHub.Common.Profiles
{
    public class PostProfile : Profile
    {
        public PostProfile()
        {
            // Used for GetById
            CreateMap<Post, PostDTO>()
                .ForMember(dest => dest.Attachments, opt => opt.MapFrom(src => src.Attachments));
            CreateMap<PostDTO, Post>()
                .ForMember(dest => dest.Attachments, opt => opt.MapFrom(src => src.Attachments));

            // Map between Attachment and AttachmentDTO
            CreateMap<Attachment, AttachmentDTO>();
            CreateMap<AttachmentDTO, Attachment>();

            // Used for CreatePost
            CreateMap<CreatePostDTO, Post>();
            CreateMap<Post, CreatePostDTO>();

            // Used for UpdatePost
            CreateMap<UpdatePostDTO, Post>();
            CreateMap<Post, UpdatePostDTO>();

        }
    }
}
