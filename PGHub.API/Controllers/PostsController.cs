using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
            var post = _dataContext.Posts.Find(id);

            if (id == null)
            {
                return NotFound();
            }

            var postDTO = new PostDTO
            {
                Id = id,
                AuthorId = post.AuthorId,
                Title = post.Title,
                Body = post.Body,
                IsPined = post.IsPined,
                CreationDate = post.CreationDate,
                DeletionDate = post.DeletionDate
            };

            return Ok(post);
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

            _dataContext.Posts.Add(post);
            _dataContext.SaveChanges();

            return Ok();
        }

        [HttpPut("{id}")]
        public IActionResult UpdatePost(Guid id, UpdatePostDTO updatePostDTO)
        {
            var post = _dataContext.Posts.Find(id);

            if (post == null)
            {
                return NotFound();
            }

            //var post = new Post();

            post.Id = id;
            post.Title = updatePostDTO.Title;
            post.Body = updatePostDTO.Body;
            post.IsPined = updatePostDTO.IsPined;
            post.DeletionDate = updatePostDTO.DeletionDate;


            foreach (var attachmentDTO in updatePostDTO.Attachments)
            {
                post.Attachments.Add(new Attachment
                {
                    FileName = attachmentDTO.FileName,
                    Id = attachmentDTO.Id,
                });
            }

            _dataContext.SaveChanges();

            return Ok();
        }

        [HttpDelete("{id}")]
        public IActionResult DeletePost(Guid id)
        {
            // need repository to check if the post exists in the DB
            // Delete should have its own DTO?

            var post = _dataContext.Posts.Where(p => p.Id == id).Include(p => p.Attachments).FirstOrDefault();

            if (post == null)
            {
                return NotFound();
            }

            _dataContext.Posts.Remove(post);
            _dataContext.SaveChanges();

            return NoContent();
        }

    }
}
