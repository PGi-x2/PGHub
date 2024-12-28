using PGHub.Application.DTOs.Post;

namespace PGHub.Application.Services
{
    public interface IPostsService
    {
        Task<PostDTO> GetByIdAsync(Guid id);
        Task<IReadOnlyCollection<PostDTO>> GetAllAsync(int pageNumber, int pageSize);
        Task<PostDTO> CreateAsync(CreatePostDTO postDto);
        Task<PostDTO> UpdateAsync(Guid id, UpdatePostDTO postDto);
        Task<bool> DeleteAsync(Guid id);
    }
}