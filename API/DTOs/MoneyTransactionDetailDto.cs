namespace API.DTOs
{
    public class MoneyTransactionDetailDto : MoneyTransactionDto
    {
        public int? AccountSendId { get; set; }
        public int? AccountReceiveId { get; set; }
        public int? ReasId { get; set; }
        public int? DepositId { get; set; }
        public string? TxnRef { get; set; }
    }
}
