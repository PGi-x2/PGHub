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
            // Check if the guid exists in the DB
            var userDb = _dataContext.Users.Find(user.Id);

            if (userDb != null)
            {
                // Entry(userDb).CurretValues gets the current property values from the entity (basically from the client)
                // SetValues(user) will set the values of user with the values from userDb
                _dataContext.Entry(userDb).CurrentValues.SetValues(user);
                _dataContext.SaveChanges();
            }
            else
            {
                // need to improve this
                return null;
            }
            
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
