namespace API.Helper;

public class AccountParams : PaginationParams
{
    public int? Month { get; set; }
    public int? Year { get; set; }

    public int? RoleID { get; set; }

}