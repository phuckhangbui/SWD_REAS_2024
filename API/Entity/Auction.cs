namespace API.Entity;

public class Auction
{
    public int AuctionId { get; set; }
    public RealEstate RealEstate { get; set; }
    public int ReasId { get; set; }
    public DateTime DateStart { get; set; }
    public DateTime DateEnd { get; set; }
    public Account AcountCreate { get; set; }
    public int AccountCreateId { get; set; }
    public string AccountCreateName { get; set; }
    public int Status { get; set; }
    public AuctionAccounting AuctionAccounting { get; set; }
}