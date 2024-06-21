using PGHub.Domain.Entities;

namespace PGHub.DataPersistance.Repositories
{
    public interface IUsersRepository
    {
        User? Find(Guid id);
    }
}