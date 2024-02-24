using API.Helper;

namespace API.Param
{
    public class SearchNewsParam : PaginationParams
    {
        public string? KeyWork { get; set; }
    }
}
