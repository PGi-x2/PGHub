using PGHub.Domain.Entities;

namespace PGHub.DataPersistance.Repositories
{
    public interface IUsersRepository
    {
        Task<User?> FindAsync(Guid id);
        Task<List<User>> GetAllAsync();
        User Create(User user);

        User Update(User user);
        bool Delete(Guid id);
    }
}