namespace API.DTOs
{
    public class AuctionDto
    {
        public int AuctionId { get; set; }
        public string ReasName { get; set; }
        public string ReasPrice { get; set; }
        public int ReasId { get; set; }
        public DateTime DateStart { get; set; }
        public DateTime DateEnd { get; set; }
        //public Account AcountCreate { get; set; }
        public int AccountCreateId { get; set; }
        public string AccountCreateName { get; set; }
        public int Status { get; set; }
        public int AuctionAccountingId { get; set; }
    }
}
