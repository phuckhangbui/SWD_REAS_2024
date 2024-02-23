using API.Entity;

namespace API.DTOs
{
    public class NewRealEstateDto
    {
        public string ReasName { get; set; }
        public string ReasAddress { get; set; }
        public string ReasPrice { get; set; }
        public string ReasDescription { get; set; }
        public int ReasStatus { get; set; }
        public DateTime DateStart { get; set; }
        public DateTime DateEnd { get; set; }
        public int Type_Reas { get; set; }
        public string Message { get; set; }
        public int AccountOwnerId { get; set; }
        public string AccountOwnerName { get; set; }
        public DateTime DateCreated { get; set; }

        public List<PhotoFileDto> Photos { get; set; }

        public DetailFileReasDto Detail { get; set; }
    }
}
