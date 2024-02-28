using API.Param.Enums;

namespace API.Helper
{
    public class GetStatusName
    {
        public string GetRealEstateStatusName(int status)
        {
            RealEstateEnum realEstateStatus = (RealEstateEnum)status;
                string statusName = realEstateStatus.ToString();
                return statusName;
        }

        public string GetDepositAmountStatusName(int status)
        {
            int getStatus = 0;
            UserDepositEnum depositAmountStatus = (UserDepositEnum)getStatus;

            if (getStatus == status)
            {
                string statusName = depositAmountStatus.ToString();
                return statusName;
            }
            else
            {
                return null;
            }
        }
    }
}
