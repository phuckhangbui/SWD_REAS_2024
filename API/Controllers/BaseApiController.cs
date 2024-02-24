using API.DTOs;
using API.Enums;
using API.Extension;
using API.Helper;
using API.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BaseApiController : ControllerBase
    {


        protected int GetLoginAccountId()
        {
            try
            {
                return int.Parse(this.User.Claims.First(i => i.Type == "AccountId").Value);
            }
            catch (Exception ex)
            {
                return 0;
            }
        }
        protected int GetIdAdmin(IAccountRepository accountRepository)
        {
            try
            {
                int idAdmin = int.Parse(this.User.Claims.First(i => i.Type == "AccountId").Value);
                if (accountRepository.GetAllAsync().Result.Where(x => x.AccountId == idAdmin).Select(x => x.RoleId).FirstOrDefault().Equals((int)RoleEnum.Admin))
                {
                    return idAdmin;
                }
                else
                {
                    return 0;
                }
            }
            catch (Exception ex)
            {
                return 0;
            }
        }

        protected int GetIdMember(IAccountRepository accountRepository)
        {
            try
            {
                int idAdmin = int.Parse(this.User.Claims.First(i => i.Type == "AccountId").Value);
                if (accountRepository.GetAllAsync().Result.Where(x => x.AccountId == idAdmin).Select(x => x.RoleId).FirstOrDefault().Equals((int)RoleEnum.Member))
                {
                    return idAdmin;
                }
                else
                {
                    return 0;
                }
            }
            catch (Exception ex)
            {
                return 0;
            }
        }

        protected int GetIdStaff(IAccountRepository accountRepository)
        {
            try
            {
                int idStaff = int.Parse(this.User.Claims.First(i => i.Type == "AccountId").Value);
                if (accountRepository.GetAllAsync().Result.Where(x => x.AccountId == idStaff).Select(x => x.RoleId).FirstOrDefault().Equals((int)RoleEnum.Staff))
                {
                    return idStaff;
                }
                else
                {
                    return 0;
                }
            }
            catch (Exception ex)
            {
                return 0;
            }
        }
    }
}
