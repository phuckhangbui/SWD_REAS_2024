using API.DTOs;
using API.Entity;
using API.Helper;
using API.Interface.Repository;
using API.MessageResponse;
using API.Param;
using Microsoft.AspNetCore.Mvc;

namespace API.Interface.Service
{
    public interface IMemberRealEstateService : IBaseService<RealEstate>
    {
        IAccountRepository AccountRepository { get; }
        Task<PageList<RealEstateDto>> GetOnwerRealEstate(int userMember);
        Task<PageList<RealEstateDto>> SearchOwnerRealEstateForMember(SearchRealEstateParam searchRealEstateParam, int userMember);
        Task<IEnumerable<CreateNewRealEstatePage>> ViewCreateNewRealEstatePage();
        Task<bool> CreateNewRealEstate(NewRealEstateParam newRealEstateParam, int userMember);
        Task<RealEstateDetailDto> ViewOwnerRealEstateDetail(int id);
        Task<bool> PaymentAmountToUpRealEstaeAfterApprove(TransactionMoneyCreateParam transactionMoneyCreateParam, int userMember);
    }
}
