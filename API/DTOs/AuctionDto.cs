namespace API.DTOs;

public class AuctionDto
{
    public int AuctionId { get; set; }
    public int ReasId { get; set; }
    public string? ReasName { get; set; }
    public DateTime DateStart { get; set; }
    public DateTime DateEnd { get; set; }
    public string AccountCreateName { get; set; }
    public string? OwnerName { get; set; }
    public int Status { get; set; }
}