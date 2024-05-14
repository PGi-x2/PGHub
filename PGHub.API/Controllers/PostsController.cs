using Microsoft.AspNetCore.Mvc;
using PGHub.API.DTOs.Post;
using PGHub.DataPersistance;
using PGHub.Domain.Entities;

namespace PGHub.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PostsController : ControllerBase
    {
        private readonly DataContext _dataContext;

        public PostsController(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        [HttpGet("{id}")]
        public IActionResult GetById(Guid id)
        {
            var post = new Post
            {
                Id = id
            };

            return Ok();
        }


        [HttpPost]
        public IActionResult CreatePost(CreatePostDTO createPostDTO)
        {
            var post = new Post
            {
                Id = createPostDTO.Id,
                AuthorId = createPostDTO.AuthorId,
                Title = createPostDTO.Title,
                Body = createPostDTO.Body,
                IsPined = createPostDTO.IsPined,
                CreationDate = createPostDTO.CreationDate,
                DeletionDate = createPostDTO.DeletionDate,
            };

            foreach (var attachmentDTO in createPostDTO.Attachments)
            {
                post.Attachments.Add(new Attachment
                {
                    FileName = attachmentDTO.FileName,
                    Id = attachmentDTO.Id,
                });
            }

            return Ok();
        }

        [HttpPut("{id}")]
        public IActionResult UpdatePost(Guid id, UpdatePostDTO updatePostDTO)
        {
            var post = new Post
            {
                Id = id,
                Title = updatePostDTO.Title,
                Body = updatePostDTO.Body,
                IsPined = updatePostDTO.IsPined,
                DeletionDate = updatePostDTO.DeletionDate,
                
            };

            foreach(var attachmentDTO in updatePostDTO.Attachments)
            {
                post.Attachments.Add(new Attachment
                {
                    FileName = attachmentDTO.FileName,
                    Id = attachmentDTO.Id,
                });
            }

            return Ok();
        }

        [HttpDelete("{id}")]
        public IActionResult DeletePost(Guid id)
        {
            // need repository to check if the post exists in the DB
            // Delete should have its own DTO?
            var post = new Post
            {
                Id = id
            };

            return NoContent();
        }

    }
}
