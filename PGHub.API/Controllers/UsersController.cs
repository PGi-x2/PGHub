using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using PGHub.Application.Services;
using PGHub.Common.DTOs.User;
using PGHub.DataPersistance;
using PGHub.DataPersistance.Repositories;
using PGHub.Domain.Entities;


namespace PGHub.Common.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly DataContext _dataContext;
        private readonly IUsersRepository _usersRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<UsersController> _logger;
        private readonly UsersService _usersService;

        public UsersController(DataContext dataContext, IUsersRepository usersRepository, IMapper mapper, ILogger<UsersController> logger, UsersService usersService)
        {
            _dataContext = dataContext;
            _usersRepository = usersRepository;
            _mapper = mapper;
            _logger = logger;
            _usersService = usersService;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetByIdAsync(Guid id)
        {
            var userDTO = await _usersService.GetByIdAsync(id);

            //var user = await _usersRepository.Find(id);

            // TODO:To add validators to check if the user(guid) exists in the DB
            if (userDTO == null)
            {
                return NotFound();
            }

            //UserDTO userDTO = _mapper.Map<UserDTO>(user);
            //var userDTOBack = _mapper.Map<UserDTO>(userDTO);

            return Ok(userDTO);
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            var users = _usersRepository.GetAll();

            // is it better to assign the value of the mapping to a variable or to return it directly?
            // better to assign the value of the mapping to a variable for better readability, debugging and testing and to perform necessary validation or manipulation before returning it
            // IEnumerable<UserDTO> usersDTO = _mapper.Map<IEnumerable<UserDTO>>(users);
            var usersDTO = _mapper.Map<IEnumerable<UserDTO>>(users);

            return Ok(usersDTO);
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
            return CreatedAtAction(nameof(GetByIdAsync), new { id = createdUser.Id }, userDTO);
        }

        [HttpPut("{id}")]
        public IActionResult UpdateUser(Guid id, UpdateUserDTO updateUserDTO)
        {
            // Mapping from UpdateUserDTO to User
            var userEntity = _mapper.Map<User>(updateUserDTO);

            // Update the user in the DB via the repository
            var updatedUser = _usersRepository.Update(userEntity);

            // TODO: need to cath the null guid when is the case

            // Map back from User entity to UpdateUserDto to return it in the response
            var userDTO = _mapper.Map<UpdateUserDTO>(updatedUser);

            return Ok(userDTO);
        }

        //Hard Delete
        [HttpDelete("{id}")]
        public IActionResult DeleteUser(Guid id)
        {
            bool isDeleted;

            try
            {
                // assign the value of the user repository to this user variable
                isDeleted = _usersRepository.Delete(id);

                if (isDeleted == false)
                {
                    return NotFound();
                }
                else
                {
                    // Return a 204 status code to indicate that the user was deleted successfully
                    return NoContent();
                }
            }
            catch (Exception ex)
            {
                // Log the exception
                _logger.LogError(ex, "An error occured while deleting the user with the ID: {UserId}", id + ".");

                // Return a 500 status code to be more specific
                return StatusCode(500, "An error occured while deleting the user.");
            }
        }
    }
}
