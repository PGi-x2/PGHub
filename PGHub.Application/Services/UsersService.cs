using AutoMapper;
using PGHub.DataPersistance.Repositories;
using PGHub.Application.DTOs.User;
using PGHub.Domain.Entities;
using Microsoft.Extensions.Logging;
using PGHub.Common.Responses;

namespace PGHub.Application.Services;

public class UsersService : IUsersService
{
    private readonly IUsersRepository _usersRepository;
    private readonly IMapper _mapper;
    private readonly ILogger<UsersService> _logger;

    public UsersService(IUsersRepository usersRepository, IMapper mapper, ILogger<UsersService> logger)
    {
        _usersRepository = usersRepository;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<UserDTO> GetByIdAsync(Guid id)
    {
        _logger.LogInformation("Starting GetByIdAsync for user ID {UserId}", id + ".");
        try
        {
            var user = await _usersRepository.GetByIdAsync(id);

            if (user == null)
            {
                // TODO: return message by using APIResponse class
                _logger.LogWarning("User with ID {UserId} not found", id + ".");
                return null;
            }

            var userDto = _mapper.Map<UserDTO>(user);
            _logger.LogInformation("Successfully retrieved user with ID {UserId}", id);
            return userDto;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while retrieving the user with the ID: {UserId}", id + ".");
            throw;
        }
        
    }

    public async Task<IReadOnlyCollection<UserDTO>> GetAllAsync()
    {
        _logger.LogInformation("Starting GetAllAsync to retrieve all users.");
        try
        {
            var users = await _usersRepository.GetAllAsync();

            if (users == null)
            {
                _logger.LogWarning("No users found.");
                return null;
            }

            var userDtos = _mapper.Map<IReadOnlyCollection<UserDTO>>(users);
            _logger.LogInformation("Successfully retrieved users.");
            return userDtos;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while retrieving all users.");
            throw;
        }
        
    }

    public async Task<UserDTO> CreateAsync(CreateUserDTO createUserDTO)
    {
        _logger.LogInformation("Starting CreateAsync for user with email {UserEmail}", createUserDTO.Email + ".");
        try
        {
            // Mapping from CreateUserDTO to User entity
            var user = _mapper.Map<User>(createUserDTO);

            var createdUser = await _usersRepository.CreateAsync(user);
            _logger.LogInformation("Successfully created user with ID {UserId} and email {UserEmail}.", createdUser.Id, createdUser.Email);

            return await GetByIdAsync(createdUser.Id);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while creating a user with email {UserEmail}.", createUserDTO.Email);
            throw;
        }
        
    }

    public async Task<UserDTO> UpdateAsync(Guid id, UpdateUserDTO updateUserDTO)
    {
        _logger.LogInformation("Starting UpdateAsync for user with ID {UserId}.", id);
        try
        {
            // Mapping from UpdateUserDTO to User entity
            var user = _mapper.Map<User>(updateUserDTO);
            user.Id = id;

            var updatedUser = await _usersRepository.UpdateAsync(user);
            _logger.LogInformation("Successfully updated user with ID {UserId}.", updatedUser.Id);

            return await GetByIdAsync(updatedUser.Id);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while updating a user with ID {UserId}.", id);
            throw;
        }
       
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        _logger.LogInformation("Starting DeleteAsync for user with ID {UserId}.", id);
        try
        {
            var result = await _usersRepository.DeleteAsync(id);
            if (result)
            {
                _logger.LogInformation("Successfully deleted user with ID {UserId}.", id);
            }
            else
            {
                _logger.LogWarning("Failed to delete user with ID {UserId}. User may not exist.", id);
            }
            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while deleting the user with ID {UserId}.", id);
            throw;
        }
    }
}
