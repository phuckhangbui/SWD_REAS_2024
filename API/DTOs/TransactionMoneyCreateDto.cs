using API.Entity;

namespace API.DTOs
{
    public class TransactionMoneyCreateDto
    {
        public int IdReas { get; set; }
        public int TransactionStatus { get; set; }
        public string Money { get; set; }
        public string MoneyPaid { get; set; }
    }
}
