using API.Data;
using API.DTOs;
using API.Enums;
using API.Interfaces;
using API.Repository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BaseApiController : ControllerBase
    {


        protected int? GetLoginAccountId()
        {
            try
            {
                return int.Parse(this.User.Claims.First(i => i.Type == "AccountId").Value);
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        protected int? GetIdAdmin(IAccountRepository accountRepository)
        {
            try
            {
                int idAdmin = 1;//int.Parse(this.User.Claims.First(i => i.Type == "AccountId").Value);
                if (accountRepository.GetAllAsync().Result.Where(x => x.AccountId == idAdmin).Select(x => x.RoleId).FirstOrDefault().Equals((int)RoleEnum.Admin))
                {
                    return idAdmin;
                }
                else
                {
                    return null;
                }
            }catch (Exception ex)
            {
                return null;
            }
        }

        protected UserInformationDto getDetailAccount(int id, IAccountRepository _accountRepository, DataContext _context)
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

            return account;
        }
    }
}
