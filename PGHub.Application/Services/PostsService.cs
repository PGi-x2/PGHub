using AutoMapper;
using Microsoft.Extensions.Logging;
using PGHub.Application.DTOs.Post;
using PGHub.DataPersistance.Repositories;
using PGHub.Domain.Entities;

namespace PGHub.Application.Services
{
    public class PostsService : IPostsService
    {
        private readonly IPostsRepository _postsRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<UsersService> _logger;

        public PostsService(IPostsRepository postsRepository, IMapper mapper, ILogger<UsersService> logger)
        {
            _postsRepository = postsRepository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<PostDTO> GetByIdAsync(Guid id)
        {
            //_logger.LogInformation("Starting GetByIdAsync for post ID {PostId}", id + ".");
            try
            {
                var post = await _postsRepository.GetById(id);
                if (post == null)
                {
                    //_logger.LogWarning("Post with ID {PostId} not found", id + ".");
                    return null;
                }
                var postDto = _mapper.Map<PostDTO>(post);
                //_logger.LogInformation("Successfully retrieved post with ID {PostId}", id);
                return postDto;
            }
            catch (Exception ex)
            {
                //_logger.LogError(ex, "An error occurred while retrieving the post with the ID: {PostId}", id + ".");
                throw new Exception("An error occurred while retrieving the post with the ID: {PostId}" + id + ".", ex);
            }
        }

        public async Task<IReadOnlyCollection<PostDTO>> GetAllAsync(int pageNumber, int pageSize)
        {
            //_logger.LogInformation("Starting GetAllAsync to retrieve all posts.");
            try
            {
                var posts = await _postsRepository.GetAllAsync(pageNumber, pageSize);
                var postsDto = _mapper.Map<IReadOnlyCollection<PostDTO>>(posts);
                //_logger.LogInformation("Successfully retrieved all posts.");
                return postsDto;
            }
            catch (Exception ex)
            {
                //_logger.LogError(ex, "An error occurred while retrieving all posts.");
                throw new Exception("An error occurred while retrieving all posts.", ex);
            }
        }

        public async Task<PostDTO> CreateAsync(PostDTO postDto)
        {
            //_logger.LogInformation("Starting CreateAsync for post with title {PostTitle}", postDto.Title + ".");
            try
            {
                var post = _mapper.Map<Post>(postDto);
                var createdPost = await _postsRepository.CreateAsync(post);

                //_logger.LogInformation("Successfully created post with title {PostTitle}", postDto.Title);
                return await GetByIdAsync(createdPost.Id);
            }
            catch (Exception ex)
            {
                //_logger.LogError(ex, "An error occurred while creating the post with the title: {PostTitle}", postDto.Title + ".");
                throw new Exception("An error occurred while creating the post with the title: {PostTitle}" + postDto.Title + ".", ex);
            }
        }

        public async Task<PostDTO> UpdateAsync(PostDTO postDto)
        {
            //_logger.LogInformation("Starting UpdateAsync for post with ID {PostId}", postDto.Id + ".");
            try
            {
                var post = _mapper.Map<Post>(postDto);
                var updatedPost = await _postsRepository.UpdateAsync(post);

                if (updatedPost == null)
                {
                    //_logger.LogWarning("Post with ID {PostId} not found for update", postDto.Id + ".");
                    return null;
                }

                //_logger.LogInformation("Successfully updated post with ID {PostId}", postDto.Id);
                return await GetByIdAsync(updatedPost.Id);
            }
            catch (Exception ex)
            {
                //_logger.LogError(ex, "An error occurred while updating the post with the ID: {PostId}", postDto.Id + ".");
                throw new Exception("An error occurred while updating the post with the ID: {PostId}" + postDto.Id + ".", ex);
            }
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            //_logger.LogInformation("Starting DeleteAsync for post with ID {PostId}", id + ".");
            try
            {
                var result = await _postsRepository.DeleteAsync(id);
                //_logger.LogInformation("Successfully deleted post with ID {PostId}", id);
                return result;
            }
            catch (Exception ex)
            {
                //_logger.LogError(ex, "An error occurred while deleting the post with the ID: {PostId}", id + ".");
                throw new Exception("An error occurred while deleting the post with the ID: {PostId}" + id + ".", ex);
            }
        }
    }
}
