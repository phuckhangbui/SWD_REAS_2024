using API.DTOs;
using API.Entity;
using API.Errors;
using API.Interfaces;
using Google.Apis.Auth;
using Microsoft.AspNetCore.Mvc;
using System.Security.Cryptography;
using System.Text;

namespace API.Controllers
{
    public class AccountController : BaseApiController
    {
        private ITokenService _tokenService;
        private IAccountRepository _accountRepository;
        public AccountController(ITokenService tokenService, IAccountRepository accountRepository)
        {
            _tokenService = tokenService;
            _accountRepository = accountRepository;
        }

        [HttpPost("login/google")]
        public async Task<ActionResult<UserDto>> LoginGoogle([FromBody] LoginGoogleDto loginGoogleDto)
        {
            try
            {
                // validate token
                GoogleJsonWebSignature.Payload payload = await GoogleJsonWebSignature.ValidateAsync(loginGoogleDto.idTokenString);

                string userEmail = payload.Email;

                if (await _accountRepository.isEmailExisted(userEmail))
                {
                    // login
                    Account account = await _accountRepository.GetAccountByEmailAsync(userEmail);
                    return new UserDto
                    {
                        Email = account.AccountEmail,
                        Token = _tokenService.CreateToken(account),
                        AccountName = account.AccountName,
                        Username = account.Username
                    };
                }
                else
                {
                    // register
                    using var hmac = new HMACSHA512();

                    Account account = new Account();
                    account.AccountEmail = userEmail;
                    account.Username = payload.Name;
                    account.AccountName = payload.Name;
                    account.PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes("FixThislater123@"));
                    account.PasswordSalt = hmac.Key;
                    account.RoleId = 1;
                    account.MajorId = 1;
                    account.Date_Created = DateTime.UtcNow;
                    account.Date_End = DateTime.MaxValue;

                    await _accountRepository.CreateAsync(account);

                    return new UserDto
                    {
                        Email = account.AccountEmail,
                        Token = _tokenService.CreateToken(account),
                        AccountName = account.AccountName,
                        Username = account.Username
                    };
                }
            }
            catch (Exception ex)
            {
                return BadRequest(new ApiException(400));

            }
        }

        [HttpPost("login/admin")]
        public async Task<ActionResult<UserDto>> LoginAdmin(LoginDto loginDto)
        {
            Account account = await _accountRepository.GetAccountByUsernameAsync(loginDto.Username);
            if (account == null) return Unauthorized(new ApiResponse(401));

            using var hmac = new HMACSHA512(account.PasswordSalt);
            var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(loginDto.Password));
            for (int i = 0; i < computedHash.Length; i++)
            {
                if (computedHash[i] != account.PasswordHash[i])
                {
                    Unauthorized(new ApiException(401));
                }
            }

            return new UserDto
            {
                Email = account.AccountEmail,
                Token = _tokenService.CreateToken(account),
                AccountName = account.AccountName,
                Username = account.Username
            };
        }
    }
}
