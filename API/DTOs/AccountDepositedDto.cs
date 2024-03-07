namespace API.DTOs
{
    public class AccountDepositedDto : AccountMemberDto
    {
        public string? PhoneNumber { get; set; }
        public int TestNumber {get; set; }
    }
}
