using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using PGHub.Application.Services;
using PGHub.Application.DTOs.User;
using PGHub.Common.Responses;
using PGHub.DataPersistance;
using PGHub.DataPersistance.Repositories;


namespace PGHub.API.Controllers
{
    /// <summary>Controller for managing users.</summary>
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
        public UsersController(DataContext dataContext, IUsersRepository usersRepository, IMapper mapper, ILogger<UsersController> logger, IUsersService usersService)
        {
            _dataContext = dataContext;
            _usersRepository = usersRepository;
            _mapper = mapper;
            _logger = logger;
            _usersService = usersService;
        }

        /// <summary>Gets a user by its ID, asynchronously.</summary>
        /// <param name="id">The ID of the user.</param>
        /// <returns> An <see cref="Task{IActionResult}"/> async of which result contains the user with the specified ID.</returns>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            try
            {
                var serviceUserDTO = await _usersService.GetByIdAsync(id);

                // TODO:To add validators to check if the user(guid) / email exists in the DB
                if (serviceUserDTO == null)
                {
                    return NotFound(APIResponse<UserDTO>.NotFound("User not found.", null));
                }

                // TODO: Move all the messages to a resource / constants file
                var response = APIResponse<UserDTO>.SuccesResult("User retrieved successfully.", serviceUserDTO);

                return Ok(response);
            }
            catch (Exception ex)
            {
                // TODO: Still needed if we have the ExceptionHandlingMiddleware????
                _logger.LogError(ex, "An error occurred while retrieving the user with the ID: {UserId}", id + ".");
                var response = APIResponse<UserDTO>.InternalServerError("An error occurred while retrieving the user.", null);
                return StatusCode(500, "An error occurred while retrieving the user.");
            }
        }

        /// <summary>Gets all users, asynchronously.</summary>
        /// <returns>An <see cref="Task{IActionResult}"/> async of which result contains all users.</returns>
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var serviceUsersDTO = await _usersService.GetAllAsync();

            return Ok(serviceUsersDTO);
        }

        /// <summary>Creates a user, asynchronously.</summary>
        /// <param name="createUserDTO">Represents a DTO that contains the necessary properties for creating user.</param>
        /// <returns>An <see cref="Task{IActionResult}"/> that contains the result of the create operation.</returns>
        [HttpPost]
        public async Task<IActionResult> Create(CreateUserDTO createUserDTO)
        {
            // Validate the input data
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var serviceUser = await _usersService.CreateAsync(createUserDTO);

            if (serviceUser == null)
            {
                return StatusCode(500, "An error occured while creating the user.");
            }
            // CreatedAtAction is a method provided by ControllerBase 
            // CreatedAtAction returns a 201 status code with the location of the created resource
            // nameof operator is used to get the name of the GetById method as a string that will be used to genereate the URL for Location header 
            return CreatedAtAction(nameof(GetById), new { id = serviceUser.Id }, serviceUser);
        }

        /// <summary>Updates an existing user, asynchronously.</summary>
        /// <param name="id">The ID of the user to update.</param>
        /// <param name="updateUserDTO">The DTO containing the updated user information.</param>
        /// <returns>An <see cref="Task{IActionResult}"/> that contains the result of the update operation.</returns>
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, UpdateUserDTO updateUserDTO)
        {
            // Validate the input data
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var updatedUser = await _usersService.UpdateAsync(id, updateUserDTO);

                // TODO: need to catch the null guid when is the case
                if (updatedUser == null)
                {
                    return NotFound();
                }

                return Ok(updatedUser);
            }
            catch (Exception ex)
            {

                _logger.LogError(ex, "An error occurred while updating the user with the ID: {UserId}", id + ".");
                return StatusCode(500, "An error occurred while updating the user.");
            }
        }

        /// <summary>Deletes a user by its ID, asynchronously.</summary>
        /// <param name="id">The ID of the user to delete.</param>
        /// <returns>An <see cref="Task{IActionResult}"/> that contains the result of the delete operation (true or false).</returns>
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            bool isDeleted;

            try
            {
                isDeleted = await _usersService.DeleteAsync(id);

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
