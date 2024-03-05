using API.Helper;
namespace API.Param
{
    public class MoneyTransactionParam : PaginationParams
    {
        public int TransactionStatus { get; set; } = -1;
        public int TypeId { get; set; } = 0;
        public DateTime DateExecutionFrom { get; set; }
        public DateTime DateExecutionTo { get; set; }

    }
}
