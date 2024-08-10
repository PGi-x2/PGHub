using PGHub.Application.DTOs.User;

namespace PGHub.Application.Services
{
    public interface IUsersService
    {
        Task<IReadOnlyCollection<UserDTO>> GetAllAsync();
        Task<UserDTO> GetByIdAsync(Guid id);
        Task<UserDTO> CreateAsync(CreateUserDTO createUserDTO);
        Task<UserDTO> UpdateAsync(Guid id, UpdateUserDTO updateUserDTO);
        Task<bool> DeleteAsync(Guid id);
    }
}