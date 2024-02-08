using API.Enums;
using Microsoft.AspNetCore.Mvc;

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
        protected int? GetIdAdmin()
        {
            try
            {
                int idAdmin = int.Parse(this.User.Claims.First(i => i.Type == "AccountId").Value);
                if (idAdmin.Equals(RoleEnum.Admin))
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
    }
}
