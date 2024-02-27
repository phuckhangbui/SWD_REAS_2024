using System.ComponentModel.DataAnnotations.Schema;

namespace API.Entity;

[Table("AuctionsAccounting")]
public class AuctionAccounting
{
    public int AuctionAccountingId { get; set; }
    public Auction? Auction { get; set; }
    public int AuctionId { get; set; }
    public RealEstate? RealEstate { get; set; }
    public int ReasId { get; set; }
    public float DepositAmount { get; set; }
    public float MaxAmount { get; set; }
    public float CommissionAmount { get; set; }
    public float AmountOwnerReceived { get; set; }
    public DateTime EstimatedPaymentDate { get; set; }
    public Account? AccountWin { get; set; }
    public int AccountWinId { get; set; }
    public string AccountWinName { get; set; }
    public Account? AccountOwner { get; set; }
    public int AccountOwnerId { get; set; }
    public string AccountOwnerName { get; set; }
}