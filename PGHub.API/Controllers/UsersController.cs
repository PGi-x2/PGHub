using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using PGHub.Application.Services;
using PGHub.Common.DTOs.User;
using PGHub.Common.Responses;
using PGHub.DataPersistance;
using PGHub.DataPersistance.Repositories;
using PGHub.Domain.Entities;


namespace PGHub.Common.Controllers
{
    /// <summary>
    /// Controller for managing users.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly DataContext _dataContext;
        private readonly IUsersRepository _usersRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<UsersController> _logger;
        private readonly IUsersService _usersService;

        /// <summary>
        /// Initializes a new instance of the <see cref="UsersController"/> class.
        /// </summary>
        /// <param name="dataContext">The data context.</param>
        /// <param name="usersRepository">The users repository.</param>
        /// <param name="mapper">The mapper.</param>
        /// <param name="logger">The logger.</param>
        /// <param name="usersService">The users service.</param>
        public UsersController(DataContext dataContext, IUsersRepository usersRepository, IMapper mapper, ILogger<UsersController> logger, IUsersService usersService)
        {
            _dataContext = dataContext;
            _usersRepository = usersRepository;
            _mapper = mapper;
            _logger = logger;
            _usersService = usersService;
        }

        /// <summary>
        /// Gets a user by its ID, asynchronously.
        /// </summary>
        /// <param name="id">The ID of the user.</param>
        /// <returns>Async Task of which result contains the user with the specified ID.</returns>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var serviceUserDTO = await _usersService.GetByIdAsync(id);

            // TODO:To add validators to check if the user(guid) / email exists in the DB
            if (serviceUserDTO == null)
            {
                return NotFound();
            }

            return Ok(serviceUserDTO);
        }

        /// <summary>
        /// Gets all users, asynchronously.
        /// </summary>
        /// <returns>Async Task of which result contains all users.</returns>
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var serviceUsersDTO = await _usersService.GetAllAsync();

            // is it better to assign the value of the mapping to a variable or to return it directly?
            // better to assign the value of the mapping to a variable for better readability, debugging and testing and to perform necessary validation or manipulation before returning it
            // IEnumerable<UserDTO> usersDTO = _mapper.Map<IEnumerable<UserDTO>>(users);
            var usersDTO = _mapper.Map<IEnumerable<UserDTO>>(serviceUsersDTO);

            return Ok(usersDTO);
        }

        /// <summary>Creates a user, asynchronously.</summary>
        /// <param name="createUserDTO">Represents a DTO that contains the necessary properties for creating user.</param>
        /// <returns>An <see cref="IActionResult"/> that contains the result of the create operation.</returns>
        [HttpPost]
        public async Task<IActionResult> Create(CreateUserDTO createUserDTO)
        {
            // How can I also retrieve the id of the created user from DB without mapping it from the DTO?
            var serviceUser = await _usersService.CreateAsync(createUserDTO);

            if (serviceUser == null)
            {
                return StatusCode(500, "An error occured while creating the user.");
            }
            return CreatedAtAction(nameof(GetById), new { id = serviceUser.Id }, serviceUser);
            // CreatedAtAction is a method provided by ControllerBase 
            // CreatedAtAction returns a 201 status code with the location of the created resource
            // nameof operator is used to get the name of the GetById method as a string that will be used to genereate the URL for Location header        
        }

        /// <summary>Updates an existing user, asynchronously.</summary>
        /// <param name="id">The ID of the user to update.</param>
        /// <param name="updateUserDTO">The DTO containing the updated user information.</param>
        /// <returns>An <see cref="IActionResult"/> that contains the result of the update operation.</returns>
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, UpdateUserDTO updateUserDTO)
        {
            // Validate the input data
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            // Mapping from UpdateUserDTO to User
            //var userEntity = _mapper.Map<User>(updateUserDTO);
            try
            {
                // Update the user in the DB via the repository
                var updatedUser = await _usersService.UpdateAsync(id, updateUserDTO);

                // TODO: need to cath the null guid when is the case
                if (updatedUser == null)
                {
                    return NotFound();
                }

                return Ok(updatedUser);
            }
            catch (Exception ex)
            {

                _logger.LogError(ex, "An error occurred while updating the user with the ID: {UserId}", id);
                return StatusCode(500, "An error occurred while updating the user.");
            }
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
