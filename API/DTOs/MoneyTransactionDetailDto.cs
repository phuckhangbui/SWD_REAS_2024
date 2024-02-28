namespace API.DTOs
{
    public class MoneyTransactionDetailDto
    {
        public int MoneyTransactionDetailId { get; set; }
        public int MoneyTransactionId { get; set; }
        public int AccountReceiveId { get; set; }
        public int ReasId { get; set; }
        public int AuctionId { get; set; }
        public string TotalAmmount { get; set; }
        public string PaidAmount { get; set; }
        public string RemainingAmount { get; set; }
        public DateTime DateExecution { get; set; }
    }
}
