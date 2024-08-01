using AutoMapper;
using PGHub.DataPersistance.Repositories;
using PGHub.Common.DTOs.User;

namespace PGHub.Application.Services;

public class UsersService
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
}
