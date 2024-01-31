using API.Data;
using API.DTOs;
using API.Entity;
using API.Errors;
using API.Interfaces;
using AutoMapper;
using Google.Apis.Auth;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text;

namespace API.Controllers
{
    public class AccountController : BaseApiController
    {
        private DataContext _context;
        private ITokenService _tokenService;
        private IMapper _mapper;
        public AccountController(DataContext context, ITokenService tokenService, IMapper mapper)
        {
            _context = context;
            _tokenService = tokenService;
            _mapper = mapper;
        }

        [HttpPost("login-google")]
        public async Task<ActionResult<UserDto>> LoginGoogle(string idTokenString)
        {
            try
            {
                // validate token
                GoogleJsonWebSignature.Payload payload = await GoogleJsonWebSignature.ValidateAsync(idTokenString);

                string userEmail = payload.Email;

                if (await UserEmailExists(userEmail))
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

            if (await UserEmailExists(registerDto.AccountEmail))
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
            account.RoleId = 3;
            account.MajorId = 1;
            account.Date_Created = DateTime.UtcNow;
            account.Date_End = DateTime.MaxValue;

            _context.Account.Add(account);
            await _context.SaveChangesAsync();

            return new UserDto
            {
                Email = account.AccountEmail,
                Token = _tokenService.CreateToken(account),
                AccountName = account.AccountName,
                Username = account.Username
            };
        }

        [HttpPost("login-admin")]
        public async Task<ActionResult<UserDto>> LoginAdmin(LoginDto loginDto)
        {
            var account = await _context.Account
                .SingleOrDefaultAsync(x => x.Username == loginDto.Username);
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

        private async Task<bool> UserEmailExists(string email)
        {
            return await _context.Account.AnyAsync(x => x.AccountEmail.ToLower() == email.ToLower());
        }

        private async Task<bool> UserExists(string email)
        {
            return await _context.Account.AnyAsync(x => x.AccountEmail.ToLower() == email.ToLower());
        }
    }
}
