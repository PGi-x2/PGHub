using AutoMapper;
using PGHub.Application.DTOs.Attachment;
using PGHub.Domain.Entities;

namespace PGHub.API.Profiles
{
    public class AttachmentProfile : Profile
    {
        public AttachmentProfile()
        {
            CreateMap<Attachment, AttachmentDTO>().ReverseMap();
        }
    }
}
