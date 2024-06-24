using PGHub.Domain.Entities;

namespace PGHub.DataPersistance.Repositories
{
    public class UsersRepository : IUsersRepository
    {
        private readonly DataContext _dataContext;

        public UsersRepository(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public User? Find(Guid id)
        {
            return _dataContext.Users.Find(id);
        }

        public User Create(User user)
        {
            _dataContext.Users.Add(user);
            _dataContext.SaveChanges();

            return user;
        }

        public User Update(User user)
        {
            _dataContext.SaveChanges();

            return user;
        }

        public bool Delete(Guid id)
        {

            var user = _dataContext.Users.Find(id);

            if (user != null) 
            { 
                _dataContext.Users.Remove(user);
                _dataContext.SaveChanges();
                return true;
            }

            return false;
        }
    }
}
