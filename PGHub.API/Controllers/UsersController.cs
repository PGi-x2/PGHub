using AutoMapper;
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
        private readonly IMapper _mapper;

        public UsersController(DataContext dataContext, IUsersRepository usersRepository, IMapper mapper)
        {
            _dataContext = dataContext;
            _usersRepository = usersRepository;
            _mapper = mapper;
        }

        [HttpGet("{id}")]
        public IActionResult GetById(Guid id)
        {
            var user = _usersRepository.Find(id);

            if (user == null)
            {
                return NotFound();
            }

            UserDTO userDTO = _mapper.Map<UserDTO>(user);

            return Ok(userDTO);
        }

        [HttpPost]
        public IActionResult CreateUser(CreateUserDTO createUserDTO)
        {
            // Mapping from CreateUserDTO to User
            var userEntity = _mapper.Map<User>(createUserDTO);

            // Create the user in the database / repository
            // How can I also retrieve the id of the created user from DB without mapping it from the DTO?
            var createdUser = _usersRepository.Create(userEntity);

            // Mapping from User entity back to CreateUserDTO to return it in the response
            var userDTO = _mapper.Map<CreateUserDTO>(createdUser);

            // CreatedAtAction is a method provided by ControllerBase 
            // CreatedAtAction returns a 201 status code with the location of the created resource
            // nameof operator is used to get the name of the GetById method as a string that will be used to genereate the URL for Location header
            return CreatedAtAction(nameof(GetById), new { id = createdUser.Id }, userDTO);
        }

        [HttpPut("{id}")]
        public IActionResult UpdateUser(Guid id, UpdateUserDTO updateUserDTO)
        {
            //var user = _dataContext.Users.Find(id);

            //if (user == null)
            //{
            //    return NotFound();
            //}

            // Mapping from UpdateUserDTO to User
            var userEntity = _mapper.Map<User>(updateUserDTO);

            // Update the user in the DB via the repository
            var updatedUser = _usersRepository.Update(userEntity);

            // need to cath the null guid when is the case -> TODO

            // Map back from User entity to UpdateUserDto to return it in the response
            var userDTO = _mapper.Map<UpdateUserDTO>(updatedUser);

            return Ok(userDTO);
        }

        //Hard Delete
        [HttpDelete("{id}")]
        public IActionResult DeleteUser(Guid id)
        {
            bool user;

            try
            {
                user = _usersRepository.Delete(id);

                if (user == false)
                {
                    return NotFound();
                }
                else
                {
                    return NoContent();
                }
            }
            catch (Exception)
            {
                throw;

            }
        }
    }
}
