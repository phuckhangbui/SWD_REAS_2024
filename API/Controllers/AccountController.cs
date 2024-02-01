using API.DTOs;
using API.Entity;
using API.Errors;
using API.Interfaces;
using AutoMapper;
using Google.Apis.Auth;
using Microsoft.AspNetCore.Mvc;
using System.Security.Cryptography;
using System.Text;

namespace API.Controllers
{
    public class AccountController : BaseApiController
    {
        private IAccountRepository _accountRepository;
        private ITokenService _tokenService;
        private IMapper _mapper;
        public AccountController(ITokenService tokenService, IMapper mapper, IAccountRepository accountRepository)
        {
            _tokenService = tokenService;
            _mapper = mapper;
            _accountRepository = accountRepository;
        }

        [HttpPost("login/google")]
        public async Task<ActionResult<UserDto>> LoginGoogle(string idTokenString)
        {
            try
            {
                // validate token
                GoogleJsonWebSignature.Payload payload = await GoogleJsonWebSignature.ValidateAsync(idTokenString);

                string userEmail = payload.Email;

                if (await _accountRepository.isEmailExisted(userEmail))
                {
                    // login

                }
                else
                {
                    // register 

                }

                return new UserDto();
            }
            catch (Exception ex)
            {
                return BadRequest(new ApiResponse(400));

            }

        }


        [HttpPost("register")]
        public async Task<ActionResult<UserDto>> Register(RegisterDto registerDto)
        {

            if (await _accountRepository.isUserNameExisted(registerDto.Username))
            {
                return BadRequest(new ApiResponse(400, "Username already exist"));
            }
            if (await _accountRepository.isEmailExisted(registerDto.AccountEmail))
            {
                return BadRequest(new ApiResponse(400, "Email already exist"));
            }

            var account = _mapper.Map<Account>(registerDto);

            using var hmac = new HMACSHA512();

            account.AccountEmail = registerDto.AccountEmail.ToLower();
            account.Username = registerDto.Username.ToLower();
            account.AccountName = registerDto.AccountName;
            account.PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(registerDto.Password));
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
                    Unauthorized(new ApiResponse(401));
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
