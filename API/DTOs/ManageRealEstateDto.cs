namespace API.DTOs
{
    public class ListRealEstateDto
    {
        public int ReasID { get; set; }
        public string ReasName { get; set;}
        public string ReasPrice { get; set;}
        public int ReasStatus { get; set;}
        public DateTime ReasDateStart { get; set;}
        public DateTime ReasDateEnd { get; set;}

    }
}
