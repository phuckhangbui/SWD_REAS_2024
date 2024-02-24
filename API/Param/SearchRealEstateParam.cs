using API.Helper;

namespace API.Param
{
    public class SearchRealEstateParam : PaginationParams
    {
        public string ReasName { get; set; }
        public string ReasPriceFrom { get; set; }
        public string ReasPriceTo { get; set; }
        public int ReasStatus { get; set; }
    }
}
