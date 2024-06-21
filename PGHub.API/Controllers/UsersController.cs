using Microsoft.AspNetCore.Mvc;
using PGHub.API.DTOs.User;
using PGHub.DataPersistance;
using PGHub.DataPersistance.Repositories;
using PGHub.Domain.Entities;

namespace PGHub.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly DataContext _dataContext;
        private readonly IUsersRepository _usersRepository;

        public UsersController(DataContext dataContext, IUsersRepository usersRepository)
        {
            _dataContext = dataContext;
            _usersRepository = usersRepository;
        }

        [HttpGet("{id}")]
        public IActionResult GetById(Guid id)
        {
            var user = _usersRepository.Find(id);

           //var user = _dataContext.Users.Find(id);

            if (id == null)
            {
                return NotFound();
            }

            var userDTO = new UserDTO
            {
                Id = id,
                Email = user.Email,
                FirstName = user.FirstName,
                LastName = user.LastName
            };

            return Ok(userDTO);
        }

        [HttpPost]
        public IActionResult CreateUser(CreateUserDTO createUserDTO)
        {
            var user = new User();
            user.FirstName = createUserDTO.FirstName;
            user.LastName = createUserDTO.LastName;
            user.Email = createUserDTO.Email;

            _dataContext.Users.Add(user);
            _dataContext.SaveChanges();

            return Ok();
        }

        [HttpPut("{id}")]
        public IActionResult UpdateUser(Guid id, UpdateUserDTO updateUserDTO)
        {

            var user = _dataContext.Users.Find(id);

            if (id == null)
            {
                return NotFound();
            }

            user.FirstName = updateUserDTO.FirstName;
            user.LastName = updateUserDTO.LastName;
            user.Email = updateUserDTO.Email;

            _dataContext.SaveChanges();

            return Ok(user);
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteUser(Guid id)
        {
            // need repository to check if the user exists in the DB
            // Delete should have its own DTO?
            var user = _dataContext.Users.Find(id);

            if (id == null)
            {
                return NotFound();
            }

            _dataContext.Users.Remove(user);
            _dataContext.SaveChanges();

            return NoContent();
        }
    }
}
