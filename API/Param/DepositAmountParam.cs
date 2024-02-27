using API.Helper;

namespace API.Param
{
    public class DepositAmountParam : PaginationParams
    {
        public string? AmountFrom { get; set; }
        public string? AmountTo { get; set; }
        public DateTime DepositDateFrom { get; set; }
        public DateTime DepositDateTo { get; set; }
        public int Status { get; set; } = -1;
    }
}
