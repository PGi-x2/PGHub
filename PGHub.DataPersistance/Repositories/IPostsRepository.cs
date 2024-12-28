using PGHub.Domain.Entities;

namespace PGHub.DataPersistance.Repositories
{
    public interface IPostsRepository
    {
        Task<Post?> GetByIdAsync(Guid id);
        Task<List<Post>> GetAllAsync(int pageNumber, int pageSize);
        Task<Post> CreateAsync(Post post);
        Task<Post?> UpdateAsync(Guid id, Post post);
        Task<bool> DeleteAsync(Guid id);
        
        
        
    }
}