namespace API.DTOs
{
    public class SearchRealEstateDto
    {
        public string ReasName { get; set; }
        public string ReasPriceFrom { get; set; }
        public string ReasPriceTo { get; set;}
        public int ReasStatus { get; set; }
    }
}
