using API.Data;
using API.DTOs;
using API.Entity;
using API.Enums;
using API.Errors;
using API.Interfaces;
using API.MessageResponse;
using API.Repository;
using AutoMapper;
using Google.Apis.Auth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text;
using API.Services;

namespace API.Controllers
{
    public class AccountController : BaseApiController
    {
        private DataContext _context;
        private ITokenService _tokenService;
        private IMapper _mapper;
        private IAccountRepository _accountRepository;
        private IMajorRepository _majorRepository;
        private IRoleRepository _roleRepository;
        public AccountController(DataContext context, ITokenService tokenService, IMapper mapper, IAccountRepository accountRepository, IMajorRepository majorRepository, IRoleRepository roleRepository)
        {
            _context = context;
            _tokenService = tokenService;
            _mapper = mapper;
            _accountRepository = accountRepository;
            _majorRepository = majorRepository;
            _roleRepository = roleRepository;
        }

        [HttpGet("testAuth")]
        [Authorize]
        public async Task<ActionResult<String>> TestAuth()
        {
            return ((int)RoleEnum.Member).ToString();

        }

        [HttpGet("testAuthMember")]
        [Authorize(policy: "Member")]
        public async Task<ActionResult<String>> TestAuthMem()
        {
            return "You are good member";
        }

        [HttpGet("testAuthAdmin")]
        [Authorize(policy: "Admin")]
        public async Task<ActionResult<String>> TestAuthAd()
        {
            return "You are good admin";
        }

        [HttpGet("testAuthStaff")]
        [Authorize(policy: "Staff")]
        public async Task<ActionResult<String>> TestAuthStaff()
        {
            return "You are good staff";
        }

        [HttpGet("testAuthAdminStaff")]
        [Authorize(policy: "AdminAndStaff")]
        public async Task<ActionResult<String>> TestAuthAdStaff()
        {
            return "You are good admin and staff";
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

            if (await UserExists(registerDto.Username))
            {
                return BadRequest(new ApiResponse(400, "Username already exist"));
            }
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
            account.RoleId = 1;
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
            return await _context.Account.AnyAsync(x => x.Username.ToLower() == email.ToLower());
        }

        [HttpGet("/admin/accounts")]
        public async Task<ActionResult<List<AccountListDto>>> GetAllAccounts()
        {
            var list_account = _accountRepository.GetAll().Where(x => new[] {(int)RoleEnum.Member, (int)RoleEnum.Staff}.Contains(x.RoleId)).OrderByDescending(x => x.AccountId).Select(x => new AccountListDto
            {
                AccountId = x.AccountId,
                Username = x.Username,
                AccountName = x.AccountName,
                AccountEmail = x.AccountEmail,
                PhoneNumber = x.PhoneNumber,
                Role = _roleRepository.GetAll().Where(y => y.RoleId == x.RoleId).Select(x => x.RoleName).FirstOrDefault(),
                Account_Status = x.Account_Status,
                Date_Created = x.Date_Created
            }).ToList();
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            return Ok(list_account);
        }

        [HttpGet("/admin/accounts/detail/{id}")]
        public async Task<ActionResult<UserInformationDto>> GetAccountDetail(int id)
        {
            var account = _accountRepository.GetAll().Where(x => x.AccountId == id).Select(x => new UserInformationDto
            {
                AccountId = x.AccountId,
                AccountName = x.AccountName,
                AccountEmail = x.AccountEmail,
                Address = x.Address,
                Citizen_identification = x.Citizen_identification,
                PhoneNumber = x.PhoneNumber,
                Username = x.Username,
                Date_Created= x.Date_Created,
                Date_End = x.Date_End,
                Major = _majorRepository.GetAll().Where(y => y.MajorId == x.MajorId).Select(x => x.MajorName).FirstOrDefault(),
                Role = _roleRepository.GetAll().Where(y => y.RoleId == x.RoleId).Select(x => x.RoleName).FirstOrDefault(),
                Account_Status = x.Account_Status
            }).FirstOrDefault();
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            return Ok(account);
        }

        [HttpPost("searchAccount")]
        public async Task<ActionResult<List<AccountListDto>>> GetAllAccountsBýearch(SearchAccountDto searchAccountDto)
        {
            var list_account = _accountRepository.GetAll().Where(x => ((new[] { (int)RoleEnum.Member, (int)RoleEnum.Staff }.Contains(x.RoleId) && searchAccountDto.RoleId == 0) || x.RoleId == searchAccountDto.RoleId)
            && (searchAccountDto.AccountName == null || x.AccountName.Contains(searchAccountDto.AccountName)) 
            && (searchAccountDto.AccountEmail == null || x.AccountEmail.Contains(searchAccountDto.AccountEmail))
            && (searchAccountDto.Username == null || x.Username.Contains(searchAccountDto.Username))
            && (searchAccountDto.PhoneNumber == null || x.AccountEmail.Contains(searchAccountDto.PhoneNumber))
            ).OrderByDescending(x => x.AccountId).Select(x => new AccountListDto
            {
                AccountId = x.AccountId,
                Username = x.Username,
                AccountName = x.AccountName,
                AccountEmail = x.AccountEmail,
                PhoneNumber = x.PhoneNumber,
                Role = _roleRepository.GetAll().Where(y => y.RoleId == x.RoleId).Select(x => x.RoleName).FirstOrDefault(),
                Account_Status = x.Account_Status,
                Date_Created = x.Date_Created
            }).ToList();
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            return Ok(list_account);
        }

        [HttpPost("changeStatusAccount")]
        public async Task<ActionResult<ApiResponseMessage>> ChangeStatusAccount(ChangeStatusAccountDto changeStatusAccountDto)
        {
            var newaccount = new Account();
            var account = _accountRepository.GetAll().Where(x => x.AccountId == changeStatusAccountDto.AccountId).FirstOrDefault();
            if (account != null)
            {
                _context.Entry(account).State = EntityState.Detached;
            }
            newaccount.AccountId = account.AccountId;
            newaccount.AccountEmail = account.AccountEmail;
            newaccount.Username = account.Username;
            newaccount.AccountName = account.AccountName;
            newaccount.PhoneNumber = account.PhoneNumber;
            newaccount.Address = account.Address;
            newaccount.Citizen_identification = account.Citizen_identification;
            newaccount.PasswordHash = account.PasswordHash;
            newaccount.PasswordSalt = account.PasswordSalt;
            newaccount.MajorId = account.MajorId;
            newaccount.RoleId = account.RoleId;
            newaccount.Date_Created = account.Date_Created;
            newaccount.Account_Status = changeStatusAccountDto.AccountStatus;
            if (changeStatusAccountDto.AccountStatus == 0)
            {
                newaccount.Date_End = DateTime.UtcNow;
            }
            else
            {
                newaccount.Date_End = DateTime.MaxValue;
            }
            _accountRepository.Update(newaccount);
            return new ApiResponseMessage("MSG17");
        }

        [HttpPost("AddAccount")] 
        public async Task<ActionResult<ApiResponseMessage>> CreateNewAccountForStaff(NewAccountDto account)
        {
            if (await UserExists(account.Username))
            {
                return BadRequest(new ApiResponse(400, "Username already exist"));
            }
            if (await UserEmailExists(account.AccountEmail))
            {
                return BadRequest(new ApiResponse(400, "Email already exist"));
            }
            var newaccount = new Account();
            using var hmac = new HMACSHA512();
            newaccount.Username = account.Username;
            newaccount.AccountEmail = account.AccountEmail;
            newaccount.Address = account.Address;
            newaccount.AccountName = account.AccountName;
            newaccount.Citizen_identification = account.Citizen_identification;
            newaccount.PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(account.PasswordHash));
            newaccount.PasswordSalt = hmac.Key;
            newaccount.PhoneNumber = account.PhoneNumber;
            newaccount.RoleId = 2;
            newaccount.Date_Created = DateTime.UtcNow;
            newaccount.Date_End = DateTime.MaxValue;
            newaccount.Account_Status = 1;
            _accountRepository.Create(newaccount);
            SendMailNewStaff.SendEmailWhenCreateNewStaff(account.AccountEmail, account.Username, account.PasswordHash, account.AccountName);
            return new ApiResponseMessage("MSG04");
        }
        
    }
}
