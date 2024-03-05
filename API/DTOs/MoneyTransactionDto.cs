namespace API.DTOs
{
    public class MoneyTransactionDto
    {
        public int TransactionId { get; set; }
        public int TransactionStatus { get; set; }
        public string? TransactionNo { get; set; }
        public int TypeId { get; set; }
        public double Money { get; set; }
        public DateTime DateExecution { get; set; }
    }
}
