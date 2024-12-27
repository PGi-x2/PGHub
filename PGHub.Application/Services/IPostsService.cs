using PGHub.Application.DTOs.Post;

namespace PGHub.Application.Services
{
    public interface IPostsService
    {
        Task<PostDTO> CreateAsync(PostDTO postDto);
        Task<bool> DeleteAsync(Guid id);
        Task<IReadOnlyCollection<PostDTO>> GetAllAsync(int pageNumber, int pageSize);
        Task<PostDTO> GetByIdAsync(Guid id);
        Task<PostDTO> UpdateAsync(PostDTO postDto);
    }
}