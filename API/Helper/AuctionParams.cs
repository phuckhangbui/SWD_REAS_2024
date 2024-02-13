namespace API.Helper;

public class AuctionParams : PaginationParams
{
    public DateTime? DateStart { get; set; }
    public DateTime? DateEnd { get; set; }
    public int? Month { get; set; }
    public int? Year { get; set; }
}