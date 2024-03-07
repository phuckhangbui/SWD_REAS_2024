using System.ComponentModel.DataAnnotations;

namespace API.DTOs
{
    public class AuctionDetailDto
    {
        [Required]
        public int AuctionId { get; set; }

        [Required]
        public int AccountWinId { get; set; }

        [Required]
        public float WinAmount { get; set; }
    }
}
