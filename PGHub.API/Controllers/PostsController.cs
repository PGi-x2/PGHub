using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using PGHub.Application.DTOs.Post;
using PGHub.Application.Services;
using PGHub.Common.Responses;
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
        private readonly IPostsService _postsService;

        /// <summary> </summary>
        /// <param name="dataContext"></param>
        /// <param name="postsRepository"></param>
        /// <param name="mapper"></param>
        /// <param name="logger"></param>
        /// <param name="postsService"></param>
        public PostsController(DataContext dataContext, IPostsRepository postsRepository, IMapper mapper, ILogger<PostsRepository> logger, IPostsService postsService)
        {
            _dataContext = dataContext;
            _postsRepository = postsRepository;
            _mapper = mapper;
            _logger = logger;
            _postsService = postsService;
        }

        /// <summary>Gets a post by its ID, asynchronously.</summary>
        /// <param name="id">The ID of the post.</param>
        /// <returns>An <see cref="Task{IActionResult}"/> async of which result contains the post with the specified ID.</returns>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            try
            {
                var postServiceDTO = await _postsService.GetByIdAsync(id);

                // TODO:To add validators to check if the post(guid) exists in the DB
                if (postServiceDTO == null)
                {
                    return NotFound(APIResponse<PostDTO>.NotFound("Post not found.", null));
                }

                var response = APIResponse<PostDTO>.SuccesResult("Post retrieved successfully.", postServiceDTO);

                return Ok(response);
            }
            catch (Exception)
            {
                _logger.LogError("An error occurred while retrieving the post with the ID: {PostId}", id + ".");
                var response = APIResponse<PostDTO>.InternalServerError("An error occurred while retrieving the post.", null);
                return StatusCode(500, "An error occurred while retrieving the post.");
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetAll(int pageNumber, int pageSize)
        {
            var postsServiceDTO = await _postsService.GetAllAsync(pageNumber, pageSize);

            return Ok(postsServiceDTO);
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
