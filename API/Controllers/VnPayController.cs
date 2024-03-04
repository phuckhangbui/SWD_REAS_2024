using API.Helper.VnPay;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace API.Controllers
{
    [Route("/test")]
    [ApiController]
    public class VnPayController : ControllerBase
    {

        private readonly VnPayProperties _vnPayProperties;


        public VnPayController(IOptions<VnPayProperties> vnPayProperties)
        {
            _vnPayProperties = vnPayProperties.Value;
        }

        [HttpGet]
        public IActionResult ProcessIPN([FromQuery] Dictionary<string, string> vnpayData)
        {
            // Assuming you have the vnp_HashSecret defined somewhere accessible.
            string vnp_HashSecret = _vnPayProperties.HashSecret;

            // Call the ProcessIPN function from your service
            string response = IPN.ProcessIPN(vnpayData, vnp_HashSecret);

            // Return the response
            return Ok(response);
        }
    }
}
