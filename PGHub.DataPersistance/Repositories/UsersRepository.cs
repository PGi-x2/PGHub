using Microsoft.EntityFrameworkCore;
using PGHub.Domain.Entities;

namespace PGHub.DataPersistance.Repositories
{
    public class UsersRepository : IUsersRepository
    {
        private readonly DataContext _dataContext;

        // The DataContext (instance of it) is injected into the repository through the constructor
        public UsersRepository(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        // ? is used to indicate that the return type can be null
        public async Task<User?> GetByIdAsync(Guid id)
        {
            return await _dataContext.Users.FindAsync(id);
        }

        public async Task<List<User>> GetAllAsync()
        {
            return await _dataContext.Users.ToListAsync();
        }

        public async Task<User> CreateAsync(User user)
        {
            using var transaction = await _dataContext.Database.BeginTransactionAsync();
            try
            {
                await _dataContext.Users.AddAsync(user);
                await _dataContext.SaveChangesAsync();
                await transaction.CommitAsync();

                return user;
            }
            catch (Exception)
            {
                await transaction.RollbackAsync();

                throw;
            }
        }

        public async Task<User?> UpdateAsync(User user)
        {
            using var transaction = await _dataContext.Database.BeginTransactionAsync();
            try
            {
                // Check if the guid exists in the DB
                var userDb = await _dataContext.Users.FindAsync(user.Id);

                if (userDb != null)
                {
                    // Entry(userDb).CurrentValues gets the current property values from the entity / db
                    // SetValues(user) will update the values of userDb with the values from user
                    _dataContext.Entry(userDb).CurrentValues.SetValues(user);
                    await _dataContext.SaveChangesAsync();
                    await transaction.CommitAsync();

                    return user;
                }
                return null;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            using var transaction = await _dataContext.Database.BeginTransactionAsync();
            try
            {
                var user = await _dataContext.Users.FindAsync(id);

                if (user != null)
                {
                    _dataContext.Users.Remove(user);
                    await _dataContext.SaveChangesAsync();
                    await transaction.CommitAsync();

                    return true;
                }

                return false;
            }
            catch (Exception)
            {
                await transaction.RollbackAsync();
                throw;
            }
        }
    }
}
