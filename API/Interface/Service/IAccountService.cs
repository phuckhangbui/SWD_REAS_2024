using API.DTOs;
using API.Entity;
using API.Interface.Repository;
using API.Param;

namespace API.Interface.Service
{
    public interface IAccountService : IBaseService<Account>
    {
        IAccountRepository AccountRepository { get; }
        Task<UserDto> LoginGoogleByMember(LoginGoogleParam loginGoogleDto);
        Task<UserDto> LoginByAdminOrStaff(LoginDto loginDto);
    }
}
