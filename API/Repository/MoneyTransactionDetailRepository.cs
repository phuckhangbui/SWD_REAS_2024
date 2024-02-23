using API.Data;
using API.DTOs;
using API.Entity;
using API.Interfaces;
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

        public async Task<TransactionMoneyCreateDto> CreateNewMoneyTransaction(TransactionMoneyCreateDto transactionMoneyCreateDto, int idTransaction)
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
                await CreateAsync(moneyTransactionDetail);
                return transactionMoneyCreateDto;
            }catch (Exception ex)
            {
                return null;
            }
        }
    }
}
