using API.Enums;

namespace API.Helper
{
    public class GetStatusName
    {
        public string GetRealEstateStatusName(int status)
        {
            int getStatus = 0;
            RealEstateEnum realEstateStatus = (RealEstateEnum)getStatus;

            if (getStatus == status)
            {
                string statusName = realEstateStatus.ToString();
                return statusName;
            }
            else
            {
                return null;
            }
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
