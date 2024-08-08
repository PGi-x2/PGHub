using PGHub.Common.DTOs.User;

namespace PGHub.Application.Services
{
    public interface IUsersService
    {
        Task<List<UserDTO>> GetAllAsync();
        Task<IReadOnlyCollection<UserDTO>> GetAll2Async();
        Task<UserDTO> GetByIdAsync(Guid id);
        Task<UserDTO> CreateAsync(CreateUserDTO createUserDTO);
        Task<UserDTO> UpdateAsync(Guid id, UpdateUserDTO updateUserDTO);
        Task<bool> DeleteAsync(Guid id);
    }
}