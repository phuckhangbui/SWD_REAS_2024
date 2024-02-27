using API.Helper;

namespace API.Param
{
    public class SearchDepositAmountParam : PaginationParams
    {
        public string AmountFrom { get; set; }
        public string AmountTo { get; set; }
        public DateTime DepositDateFrom { get; set; }
        public DateTime DepositDateTo { get; set; }
    }
}
