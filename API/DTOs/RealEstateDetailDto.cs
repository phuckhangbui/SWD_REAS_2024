using API.Entity;

namespace API.DTOs
{
    public class RealEstateDetailDto
    {
        public int ReasId { get; set; }
        public string ReasName { get; set; }
        public string ReasAddress { get; set; }
        public string ReasPrice { get; set; }
        public string ReasDescription { get; set; }
        public int ReasStatus { get; set; }
        public DateTime DateStart { get; set; }
        public DateTime DateEnd { get; set; }
        public int AccountOwnerId { get; set; }
        public string AccountOwnerName { get; set; }
        public DateTime DateCreated { get; set; }

        public List<ListPhotoRealEstateDto> Photos { get; set; }
        public RealEstatePaper Detail { get; set; }
    }
}
