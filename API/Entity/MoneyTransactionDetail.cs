namespace API.Entity;

public class MoneyTransactionDetail
{
    public int MoneyTransactionDetailId { get; set; }
    public MoneyTransaction MoneyTransaction { get; set; }
    public int MoneyTransactionId { get; set; }
    public Account AccountReceive { get; set; }
    public int? AccountReceiveId { get; set; }
    public RealEstate RealEstate { get; set; }
    public int ReasId { get; set; }
    public Auction Auction { get; set; }
    public int? AuctionId { get; set; }
    public double TotalAmmount { get; set; }
    public double PaidAmount { get; set; }
    public double RemainingAmount { get; set; }
    public DateTime DateExecution { get; set; }
}