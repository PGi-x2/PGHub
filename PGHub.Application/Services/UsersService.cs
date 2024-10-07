using AutoMapper;
using PGHub.DataPersistance.Repositories;
using PGHub.Application.DTOs.User;
using PGHub.Domain.Entities;

namespace PGHub.Application.Services;

public class UsersService : IUsersService
{
    private readonly IUsersRepository _usersRepository;
    private readonly IMapper _mapper;

    public UsersService(IUsersRepository usersRepository, IMapper mapper)
    {
        _usersRepository = usersRepository;
        _mapper = mapper;
    }

    /// <summary> Get a user by its ID, asynchronously. </summary>
    /// <param name="id">The ID of the user.</param>
    /// <returns>Async Task of which result contains the user with the specified ID.</returns>
    public async Task<UserDTO> GetByIdAsync(Guid id)
    {
        var user = await _usersRepository.GetByIdAsync(id);

        if (user == null)
        {
            // TODO: return message by using APIResponse class
            return null;
        }

        return _mapper.Map<UserDTO>(user);
    }

    /// <summary>Get all users, asynchronously.</summary>
    /// <returns>Async Task of which result contains all users.</returns>
    public async Task<IReadOnlyCollection<UserDTO>> GetAllAsync()
    {
        var users = await _usersRepository.GetAllAsync();

        if (users == null)
        {
            return null;
        }

        return _mapper.Map<IReadOnlyCollection<UserDTO>>(users);
    }

    /// <summary>Create user, asynchronously.</summary>
    /// <param name="createUserDTO">Represents a DTO that contains the necessary properties for creating user.</param>
    /// <returns>An <see cref="GetByIdAsync(Guid)"/> that contains the result of the created user.</returns>
    public async Task<UserDTO> CreateAsync(CreateUserDTO createUserDTO)
    {
        // Mapping from CreateUserDTO to User entity
        var user = _mapper.Map<User>(createUserDTO);

        var createdUser = await _usersRepository.CreateAsync(user);

        return await GetByIdAsync(createdUser.Id);
    }

    /// <summary>Updates an existing user, asynchronously.</summary>
    /// <param name="id">The ID of the user to update.</param>
    /// <param name="updateUserDTO">The DTO containing the updated user information.</param>
    /// <returns>An <see cref="GetByIdAsync(Guid)"/> that contains the result of the updated user.</returns>
    public async Task<UserDTO> UpdateAsync(Guid id, UpdateUserDTO updateUserDTO)
    {
        // Mapping from UpdateUserDTO to User entity
        var user = _mapper.Map<User>(updateUserDTO);
        user.Id = id;

        var updatedUser = await _usersRepository.UpdateAsync(user);

        return await GetByIdAsync(updatedUser.Id);
    }

    /// <summary>Deletes a user by its ID, asynchronously.</summary>
    /// <param name="id"> The ID of the user to delete.</param>
    /// <returns>An <see cref="DeleteAsync(Guid)"/> that represents the asynchronous operation. 
    /// The result contains a bool that indicates the success of the delete operation (true or false).</returns>
    public async Task<bool> DeleteAsync(Guid id)
    {
        return await _usersRepository.DeleteAsync(id);
    }
}
