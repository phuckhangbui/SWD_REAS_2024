using API.Data;
using API.Entity;
using API.Interface.Repository;
using API.Param;
using API.Validate;

namespace API.Repository
{
    public class MoneyTransactionDetailRepository : BaseRepository<MoneyTransactionDetail>, IMoneyTransactionDetailRepository
    {
        private readonly DataContext _dataContext;
        private readonly IAccountRepository _accountRepository;
        private ParseValidate parseValidate = new ParseValidate();
        public MoneyTransactionDetailRepository(DataContext context, IAccountRepository accountRepository) : base(context)
        {
            _dataContext = context;
            _accountRepository = accountRepository;
        }

        public async Task<bool> CreateNewMoneyTransaction(TransactionMoneyCreateParam transactionMoneyCreateDto, int idTransaction)
        {
            MoneyTransactionDetail moneyTransactionDetail = new MoneyTransactionDetail();
            moneyTransactionDetail.MoneyTransactionId = idTransaction;
            moneyTransactionDetail.AccountReceiveId = await _accountRepository.GetIdAccountToReceiveMoney();
            moneyTransactionDetail.DateExecution = DateTime.UtcNow;
            moneyTransactionDetail.ReasId = transactionMoneyCreateDto.IdReas;
            moneyTransactionDetail.TotalAmmount = transactionMoneyCreateDto.Money;
            moneyTransactionDetail.PaidAmount = transactionMoneyCreateDto.MoneyPaid;
            int kq = (int)(parseValidate.ParseStringToInt(transactionMoneyCreateDto.Money) - parseValidate.ParseStringToInt(transactionMoneyCreateDto.MoneyPaid));
            moneyTransactionDetail.RemainingAmount = kq.ToString();
            try
            {
                bool check = await CreateAsync(moneyTransactionDetail);
                if (check) { return true; }
                else return false;
            }catch (Exception ex)
            {
                return false;
            }
        }
    }
}
