using API.Helper;

namespace API.DTOs
{
    public class SearchNewsParam : PaginationParams
    {
        public string? KeyWork {  get; set; }
    }
}
