namespace API.Entity;

public class MoneyTransaction
{
    public int TransactionId { get; set; }
    public MoneyTransactionType Type { get; set; }
    public int TypeId { get; set; }
    public Account AccountSend { get; set; }
    public int AccountSendId { get; set; }
    public int TransactionStatus { get; set; }
    public double Money { get; set; }
    public DateTime DateExecution { get; set; }
    
    public MoneyTransactionDetail MoneyTransactionDetail { get; set; }
}