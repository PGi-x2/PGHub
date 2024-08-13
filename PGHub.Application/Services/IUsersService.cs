using PGHub.Application.DTOs.User;

namespace PGHub.Application.Services
{
    public interface IUsersService
    {
        Task<UserDTO> GetByIdAsync(Guid id);
        Task<IReadOnlyCollection<UserDTO>> GetAllAsync();
        Task<UserDTO> CreateAsync(CreateUserDTO createUserDTO);
        Task<UserDTO> UpdateAsync(Guid id, UpdateUserDTO updateUserDTO);
        Task<bool> DeleteAsync(Guid id);
    }
}