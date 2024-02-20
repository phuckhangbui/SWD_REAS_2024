using API.Data;
using API.DTOs;
using API.Entity;
using API.Enums;
using API.Errors;
using API.Interfaces;
using API.MessageResponse;
using API.Services;
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
        private ITokenService _tokenService;
        private IMapper _mapper;
        private IAccountRepository _accountRepository;
        private DataContext _context;
        public AccountController(DataContext context, ITokenService tokenService, IMapper mapper, IAccountRepository accountRepository)
        {
            _tokenService = tokenService;
            _mapper = mapper;
            _context = context;
            _accountRepository = accountRepository;
        }

        [HttpPost("login/google")]
        public async Task<ActionResult<UserDto>> LoginGoogle([FromBody] LoginGoogleDto loginGoogleDto)
        {
            var test = 1;
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


        [HttpPost("register")]
        public async Task<ActionResult<UserDto>> Register(RegisterDto registerDto)
        {

            if (await _accountRepository.isUserNameExisted(registerDto.Username))
            {
                return BadRequest(new ApiException(400, "Username already exist"));
            }
            if (await _accountRepository.isEmailExisted(registerDto.AccountEmail))
            {
                return BadRequest(new ApiException(400, "Email already exist"));
            }

            var account = _mapper.Map<Account>(registerDto);

            using var hmac = new HMACSHA512();

            account.AccountEmail = registerDto.AccountEmail.ToLower();
            account.Username = registerDto.Username.ToLower();
            account.AccountName = registerDto.AccountName;
            account.PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(registerDto.Password));
            account.PasswordSalt = hmac.Key;
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


        [HttpGet("/admin/accounts")]
        public async Task<ActionResult<List<AccountListDto>>> GetAllAccounts()
        {
            var adminAccount = GetIdAdmin(_accountRepository);
            if(adminAccount != null)
            {
                var list_account = _accountRepository.GetAllAsync().Result.Where(x => new[] { (int)RoleEnum.Member, (int)RoleEnum.Staff }.Contains(x.RoleId)).OrderByDescending(x => x.AccountId).Select(x => new AccountListDto
                {
                    AccountId = x.AccountId,
                    Username = x.Username,
                    AccountName = x.AccountName,
                    AccountEmail = x.AccountEmail,
                    PhoneNumber = x.PhoneNumber,
                    Role = _context.Role.Where(y => y.RoleId == x.RoleId).Select(x => x.RoleName).FirstOrDefault(),
                    Account_Status = x.Account_Status,
                    Date_Created = x.Date_Created
                }).ToList();
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);
                return Ok(list_account);
            }
            else
            {
                return BadRequest(new ApiResponse(401));
            }
        }

        [HttpGet("/admin/accounts/detail/{id}")]
        public async Task<ActionResult<UserInformationDto>> GetAccountDetail(int id)
        {
            var adminAccount = GetIdAdmin(_accountRepository);
            if(adminAccount != null)
            {
                var account = _accountRepository.GetAllAsync().Result.Where(x => x.AccountId == id).Select(x => new UserInformationDto
                {
                    AccountId = x.AccountId,
                    AccountName = x.AccountName,
                    AccountEmail = x.AccountEmail,
                    Address = x.Address,
                    Citizen_identification = x.Citizen_identification,
                    PhoneNumber = x.PhoneNumber,
                    Username = x.Username,
                    Date_Created = x.Date_Created,
                    Date_End = x.Date_End,
                    Major = _context.Major.Where(y => y.MajorId == x.MajorId).Select(x => x.MajorName).FirstOrDefault(),
                    Role = _context.Role.Where(y => y.RoleId == x.RoleId).Select(x => x.RoleName).FirstOrDefault(),
                    Account_Status = x.Account_Status
                }).FirstOrDefault();
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);
                return Ok(account);
            }  
            else
            {
                return BadRequest(new ApiResponse(401));
            }
        }

        [HttpPost("searchAccount")]
        public async Task<ActionResult<List<AccountListDto>>> GetAllAccountsBýearch(SearchAccountDto searchAccountDto)
        {
            var accountAdmin = GetIdAdmin(_accountRepository);
            if(accountAdmin != null)
            {
                var list_account = _accountRepository.GetAllAsync().Result.Where(x => ((new[] { (int)RoleEnum.Member, (int)RoleEnum.Staff }.Contains(x.RoleId) && searchAccountDto.RoleId == 0) || x.RoleId == searchAccountDto.RoleId)
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
                Role = _context.Role.Where(y => y.RoleId == x.RoleId).Select(x => x.RoleName).FirstOrDefault(),
                Account_Status = x.Account_Status,
                Date_Created = x.Date_Created
            }).ToList();
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);
                return Ok(list_account);
            }
            else
            {
                return BadRequest(new ApiResponse(401));
            }
        }

        [HttpPost("changeStatusAccount")]
        public async Task<ActionResult<ApiResponseMessage>> ChangeStatusAccount(ChangeStatusAccountDto changeStatusAccountDto)
        {
            var accountAdmin = GetIdAdmin(_accountRepository);
            if(accountAdmin != null)
            {
                var newaccount = new Account();
                var account = _accountRepository.GetAllAsync().Result.Where(x => x.AccountId == changeStatusAccountDto.AccountId).FirstOrDefault();
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
                _accountRepository.UpdateAsync(newaccount);
                return new ApiResponseMessage("MSG17");
            }
            else
            {
                return BadRequest(new ApiResponse(401));
            }
        }

        [HttpPost("AddAccount")]
        public async Task<ActionResult<ApiResponseMessage>> CreateNewAccountForStaff(NewAccountDto account)
        {
            var accountAdmin = GetIdAdmin(_accountRepository);
            if(accountAdmin != null)
            {
                if (await _accountRepository.isUserNameExisted(account.Username))
                {
                    return BadRequest(new ApiResponse(400, "Username already exist"));
                }
                if (await _accountRepository.isEmailExisted(account.AccountEmail))
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
                _accountRepository.CreateAsync(newaccount);
                SendMailNewStaff.SendEmailWhenCreateNewStaff(account.AccountEmail, account.Username, account.PasswordHash, account.AccountName);
                return new ApiResponseMessage("MSG04");
            }
            else
            {
                return BadRequest(new ApiResponse(401));
            }
        }

    }
}
