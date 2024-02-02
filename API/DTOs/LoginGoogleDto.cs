using System.ComponentModel.DataAnnotations;

namespace API.DTOs
{
    public class LoginGoogleDto
    {
        [Required]
        public string idTokenString { get; set; }
    }
}
