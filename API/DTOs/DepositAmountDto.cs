namespace API.DTOs
{
    public class DepositAmountDto
    {
        public int DepositId { get; set; }
        public int RuleId { get; set; }
        public int AccountSignId { get; set; }
        public int ReasId { get; set; }
        public string Amount { get; set; }
        public DateTime DepositDate { get; set; }
        public string Status { get; set; }
    }
}
