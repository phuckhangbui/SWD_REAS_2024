using API.Helper;

namespace API.DTOs
{
    public class SearchRealEstateDto : PaginationParams
    {
        public string ReasName { get; set; }
        public string ReasPriceFrom { get; set; }
        public string ReasPriceTo { get; set;}
        public int ReasStatus { get; set; }
    }
}
