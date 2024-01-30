namespace API.Entity;

public class MoneyTransactionDetail
{
    public int MoneyTransactionDetailId { get; set; }
    public MoneyTransaction MoneyTransaction { get; set; }
    public int MoneyTransactionId { get; set; }
    public Account AccountReceive { get; set; }
    public int AccountReceiveId { get; set; }
    public RealEstate RealEstate { get; set; }
    public int ReasId { get; set; }
    public Auction Auction { get; set; }
    public int AuctionId { get; set; }
    public string TotalAmmount { get; set; }
    public string PaidAmount { get; set; }
    public string RemainingAmount { get; set; }
    public DateTime DateExecution { get; set; }
}