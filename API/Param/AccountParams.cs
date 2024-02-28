using API.Helper;

namespace API.Param;

public class AccountParams : PaginationParams
{
    public string AccountEmail { get; set; }
    public int RoleId { get; set; }

}