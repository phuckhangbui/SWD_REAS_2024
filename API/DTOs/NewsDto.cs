namespace API.DTOs
{
    public class NewsDto
    {
        public int NewsId { get; set; }
        public string NewsTitle { get; set; }
        public string NewsSumary { get; set; }
        public DateTime DateCreated { get; set; }
    }
}
