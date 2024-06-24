using PGHub.Domain.Entities;

namespace PGHub.DataPersistance.Repositories
{
    public interface IUsersRepository
    {
        User? Find(Guid id);
        User Create(User user);

        User Update(User user);
        bool Delete(Guid id);
    }
}