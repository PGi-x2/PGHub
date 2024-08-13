using PGHub.Application.DTOs.User;
using PGHub.Common.Responses;

namespace PGHub.Application.Services
{
    public interface IUsersService
    {
        Task<APIResponse<UserDTO>> GetByIdAsync(Guid id);
        Task<IReadOnlyCollection<UserDTO>> GetAllAsync();
        Task<APIResponse<UserDTO>> CreateAsync(CreateUserDTO createUserDTO);
        Task<APIResponse<UserDTO>> UpdateAsync(Guid id, UpdateUserDTO updateUserDTO);
        Task<bool> DeleteAsync(Guid id);
    }
}