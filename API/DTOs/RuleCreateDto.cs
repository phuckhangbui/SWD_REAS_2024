namespace API.DTOs
{
    public class RuleCreateDto
    {
        public string Content { get; set; }
        public string Title { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime DateUpdated { get; set; }
    }
}
