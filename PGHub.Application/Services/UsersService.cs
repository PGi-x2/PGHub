using AutoMapper;
using PGHub.DataPersistance.Repositories;
using PGHub.Common.DTOs.User;
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

    public async Task<UserDTO> GetByIdAsync(Guid id)
    {
        var user = await _usersRepository.FindAsync(id);

        return _mapper.Map<UserDTO>(user);
    }

    public async Task<List<UserDTO>> GetAllAsync()
    {
        var users = await _usersRepository.GetAllAsync();

        return _mapper.Map<List<UserDTO>>(users);
    }

    public async Task<IReadOnlyCollection<UserDTO>> GetAll2Async()
    {
        var users = await _usersRepository.GetAllAsync();

        return _mapper.Map<List<UserDTO>>(users);
    }


    public async Task<UserDTO> CreateAsync(CreateUserDTO createUserDTO)
    {
        // Mapping from CreateUserDTO to User entity
        var user = _mapper.Map<User>(createUserDTO);

        var createdUser = await _usersRepository.CreateAsync(user);

        // Mapping from User entity to UserDTO
        // return _mapper.Map<UserDTO>(createdUser);
        return await GetByIdAsync(createdUser.Id);
    }

    public async Task<UserDTO> UpdateAsync(Guid id, UpdateUserDTO updateUserDTO)
    {
        // Mapping from UpdateUserDTO to User entity
        var user = _mapper.Map<User>(updateUserDTO);
        user.Id = id;

        var updatedUser = await _usersRepository.UpdateAsync(user);

        // Mapping from User entity to UserDTO
        // return _mapper.Map<UserDTO>(updatedUser);
        return await GetByIdAsync(updatedUser.Id);
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        return await _usersRepository.DeleteAsync(id);
    }
}
