using API.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    public class TestController : BaseApiController
    {
        public TestController(IAccountRepository accountRepository) : base(accountRepository)
        {
        }

        [HttpGet("auth")]
        public async Task<ActionResult<string>> TestAuth()
        {
            return "Get in good";
        }

        [HttpGet("auth/member")]
        [Authorize(policy: "Member")]
        public async Task<ActionResult<String>> TestAuthMem()
        {
            return "You are good member";
        }

        [HttpGet("authAdmin")]
        [Authorize(policy: "Admin")]
        public async Task<ActionResult<String>> TestAuthAd()
        {
            return "You are good admin";
        }

        [HttpGet("auth/staff")]
        [Authorize(policy: "Staff")]
        public async Task<ActionResult<String>> TestAuthStaff()
        {
            return "You are good staff";
        }

        [HttpGet("auth/adminstaff")]
        [Authorize(policy: "AdminAndStaff")]
        public async Task<ActionResult<String>> TestAuthAdStaff()
        {
            return "You are good admin and staff";
        }
    }
}
