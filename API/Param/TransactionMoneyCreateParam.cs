using API.Entity;

namespace API.Param
{
    public class TransactionMoneyCreateParam
    {
        public int IdReas { get; set; }
        public int TransactionStatus { get; set; }
        public string Money { get; set; }
        public string MoneyPaid { get; set; }
    }
}
