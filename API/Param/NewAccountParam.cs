namespace API.Param
{
    public class NewAccountParam
    {
        public string Username { get; set; }
        public string AccountName { get; set; }
        public string PasswordHash { get; set; }
        public string AccountEmail { get; set; }
        public string PhoneNumber { get; set; }
        public string Citizen_identification { get; set; }
        public string Address { get; set; }
        public int RoleId { get; set; }
        public int Account_Status { get; set; }
        public DateTime Date_Created { get; set; }
        public DateTime Date_End { get; set; }
    }
}
