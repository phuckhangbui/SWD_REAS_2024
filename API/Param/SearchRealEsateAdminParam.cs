namespace API.Param
{
    public class SearchRealEsateAdminParam
    {
        public string? reasName { get; set; }
        public int? reasPriceFrom { get; set; }
        public int? reasPriceTo { get; set; }
        public int reasStatus { get; set; }
    }
}
