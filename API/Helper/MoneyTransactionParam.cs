namespace API.Helper
{
    public class MoneyTransactionParam : PaginationParams
    {
        public int TransactionStatus { get; set; } = 0;
        public int TypeId { get; set; } = 0;
    }
}
