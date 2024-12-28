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
                //_logger.LogError("An error occurred while retrieving the post with the ID: {PostId}", id + ".");
                var response = APIResponse<PostDTO>.InternalServerError("An error occurred while retrieving the post.", null);
                return StatusCode(500, response);
            }
        }

        /// <summary>Gets all posts, asynchronously.</summary>
        /// <param name="pageNumber"></param>
        /// <param name="pageSize"></param>
        /// <returns>An <see cref="Task{IActionResult}"/> async of which result contains all posts.</returns>
        [HttpGet]
        public async Task<IActionResult> GetAll(int pageNumber, int pageSize)
        {
            try
            {
                var postsService = await _postsService.GetAllAsync(pageNumber, pageSize);

                var response = APIResponse<IReadOnlyCollection<PostDTO>>.SuccesResult("Posts retrieved successfully.", postsService);

                return Ok(response);
            }
            catch (Exception)
            {
                var response = APIResponse<IReadOnlyCollection<PostDTO>>.InternalServerError("An error occurred while retrieving all posts.", null);
                return StatusCode(500, response);
            }
        }

        /// <summary>Creates a new post, asynchronously.</summary>
        /// <param name="createPostDTO">Represents a DTO that contains the necessary properties for creating a post.</param>
        /// <returns>An <see cref="Task{IActionResult}"/> that contains the result of the create operation.</returns>
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreatePostDTO createPostDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var postsService = await _postsService.CreateAsync(createPostDTO);

                if (postsService == null)
                {
                    return BadRequest("An error occurred while creating the post.");
                }

                return CreatedAtAction(nameof(GetById), new { id = postsService.Id }, postsService);
            }
            catch (Exception)
            {
                //_logger.LogError("An error occurred while creating the post with the title: {PostTitle}", createPostDTO.Title + ".");
                var response = APIResponse<PostDTO>.InternalServerError("An error occurred while creating the post.", null);
                return StatusCode(500, response);
            }
        }

        /// <summary>Updates an existing post, asynchronously.</summary>
        /// <param name="id">The ID of the post to update.</param>
        /// <param name="updatePostDTO">The DTO containing the updated post information.</param>
        /// <returns>An <see cref="Task{IActionResult}"/> that contains the result of the update operation.</returns>
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, UpdatePostDTO updatePostDTO)
        {
            // Validate the input data
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                // Retrieve the existing post from the repository
                var updatedPost = await _postsService.UpdateAsync(id, updatePostDTO);

                if (updatedPost == null)
                {
                    return NotFound();
                }

                return Ok(updatedPost);
            }
            catch (Exception)
            {
                var response = APIResponse<PostDTO>.InternalServerError("An error occurred while updating the post.", null);
                return StatusCode(500, response);
            }

        }

        /// <summary>Deletes a post by its ID, asynchronously.</summary>
        /// <param name="id">The ID of the post to delete.</param>
        /// <returns>An <see cref="Task{IActionResult}"/> that contains the result of the delete operation (true or false).</returns>
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
                //_logger.LogError(ex, "An error occured while deleting the post with the ID: {PostId}", id + ".");
                var response = APIResponse<PostDTO>.InternalServerError("An error occurred while deleting the post.", null);
                return StatusCode(500, response);
            }
        }
    }
}
