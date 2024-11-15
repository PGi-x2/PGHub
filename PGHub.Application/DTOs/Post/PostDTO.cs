﻿using PGHub.Application.DTOs.Attachment;

namespace PGHub.Application.DTOs.Post
{
    public class PostDTO
    {
        public PostDTO()
        {
            Attachments = new List<AttachmentDTO>();
            Title = string.Empty;
            Body = string.Empty;
        }
        public Guid Id { get; set; }
        public Guid AuthorId { get; set; }
        public string Title { get; set; }
        public string Body { get; set; }
        public bool IsPined { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime DeletionDate { get; set; }
        public List<AttachmentDTO> Attachments { get; set; }
    }
}
