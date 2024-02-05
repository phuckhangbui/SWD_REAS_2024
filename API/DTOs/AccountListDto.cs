using API.Entity;

namespace API.DTOs
{
    public class AccountListDto
    {
        public int AccountId { get; set; }
        public string Username { get; set; }
        public string AccountName { get; set; }
        public string PhoneNumber { get; set; }
        public string AccountEmail { get; set; }
        public string Role { get; set; }
        public int Account_Status { get; set; }
        public DateTime Date_Created { get; set; }
    }
}
