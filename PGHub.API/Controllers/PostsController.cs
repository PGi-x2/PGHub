using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using PGHub.Application.DTOs.Post;
using PGHub.DataPersistance;
using PGHub.DataPersistance.Repositories;
using PGHub.Domain.Entities;

namespace PGHub.API.Controllers
{
    /// <summary>Controller for managing posts.</summary>
    [Route("api/[controller]")]
    [ApiController]
    public class PostsController : ControllerBase
    {
        private readonly DataContext _dataContext;
        private readonly IPostsRepository _postsRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<PostsRepository> _logger;

        /// <summary> </summary>
        /// <param name="dataContext"></param>
        /// <param name="postsRepository"></param>
        /// <param name="mapper"></param>
        /// <param name="logger"></param>
        public PostsController(DataContext dataContext, IPostsRepository postsRepository, IMapper mapper, ILogger<PostsRepository> logger)
        {
            _dataContext = dataContext;
            _postsRepository = postsRepository;
            _mapper = mapper;
            _logger = logger;
        }

        /// <summary>Gets a post by its ID, asynchronously.</summary>
        /// <param name="id">The ID of the post.</param>
        /// <returns>An <see cref="Task{IActionResult}"/> async of which result contains the post with the specified ID.</returns>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var post = await _dataContext.Posts.FindAsync(id);

            // TODO:To add validators to check if the post(guid) exists in the DB
            if (post == null)
            {
                return NotFound();
            }

            // This maps the properties of the post object to a new instance of the PostDTO class.
            // Maps the properties from domain entity to the DTO object that can be displayed to the client
            var postDTO = _mapper.Map<PostDTO>(post);

            return Ok(postDTO);
        }

        [HttpGet]
        public async Task<IActionResult> GetAll(int pageNumber, int pageSize)
        {
            var posts = await _postsRepository.GetAllAsync(pageNumber, pageSize);

            var postDTO = _mapper.Map<IEnumerable<PostDTO>>(posts);

            return Ok(postDTO);
        }


        [HttpPost]
        public async Task<IActionResult> Create(CreatePostDTO createPostDTO)
        {
            // Maps the properties from the DTO object to the domain entity
            var post = _mapper.Map<Post>(createPostDTO);

            // Creates the post in the database / repository
            var createdPost = await _postsRepository.CreateAsync(post);

            // TODO: Need to return this via the GetById Method that will have the mapping from domain entity to DTO
            // Maps the properties from the domain entity back to the DTO object to return it in the response
            var postDTO = _mapper.Map<PostDTO>(createdPost);

            return CreatedAtAction(nameof(GetById), new { id = createdPost.Id }, postDTO);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, UpdatePostDTO updatePostDTO)
        {
            // Retrieve the existing post from the repository
            var existingPost = await _postsRepository.GetById(id);
            if (existingPost == null)
            {
                return NotFound();
            }

            _mapper.Map(updatePostDTO, existingPost);

            // Clear the existing attachments and add the new ones
            existingPost.Attachments.Clear();
            foreach (var attachmentDTO in updatePostDTO.Attachments)
            {
                existingPost.Attachments.Add(new Attachment
                {
                    FileName = attachmentDTO.FileName,
                    //Id = attachmentDTO.Id,
                });
            }

            // Update the post in the DB via the repository
            var updatedPost = await _postsRepository.UpdateAsync(existingPost);

            // Map back from Post entity to UpdatePostDto to return it in the response / client
            var postDTO = _mapper.Map<UpdatePostDTO>(updatedPost);

            return Ok(postDTO);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            bool isDeleted;

            try
            {
                isDeleted = await _postsRepository.DeleteAsync(id);

                if (!isDeleted)
                {
                    return NotFound();
                }
                else
                {
                    return NoContent();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occured while deleting the post with the ID: {PostId}", id + ".");

                return StatusCode(500, "An error occured while deleting the post.");
            }
        }

    }
}
