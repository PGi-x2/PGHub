using PGHub.Domain.Entities;

namespace PGHub.DataPersistance.Repositories
{
    public interface IUsersRepository
    {
        Task<User?> FindAsync(Guid id);
        Task<List<User>> GetAllAsync();
        Task<User> CreateAsync(User user);

        Task<User> UpdateAsync(User user);
        Task<bool> DeleteAsync(Guid id);
    }
}