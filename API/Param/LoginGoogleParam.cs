using System.ComponentModel.DataAnnotations;

namespace API.Param
{
    public class LoginGoogleParam
    {
        [Required]
        public string idTokenString { get; set; }
    }
}
