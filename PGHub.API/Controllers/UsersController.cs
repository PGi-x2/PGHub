using Microsoft.AspNetCore.Mvc;
using PGHub.API.DTOs.User;
using PGHub.DataPersistance;
using PGHub.Domain.Entities;

namespace PGHub.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly DataContext _dataContext;

        public UsersController(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        [HttpGet("{id}")]
        public IActionResult GetById(Guid id)
        {
            var users = new User
            {
                Id = id
            };

            return Ok();
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
            var user = new User
            {
                Id = id,
                FirstName = updateUserDTO.FirstName,
                LastName = updateUserDTO.LastName,
                Email = updateUserDTO.Email,
            };

            return Ok();
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteUser(Guid id)
        {
            // need repository to check if the user exists in the DB
            // Delete should have its own DTO?
            var user = new User
            {
                Id = id
            };

            return NoContent();
        }
    }
}
