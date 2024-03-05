using API.Helper;

namespace API.Param
{
    public class DepositAmountParam : PaginationParams
    {
        public double AmountFrom { get; set; }
        public double AmountTo { get; set; }
        public DateTime DepositDateFrom { get; set; }
        public DateTime DepositDateTo { get; set; }
        public int Status { get; set; } = -1;
    }
}
