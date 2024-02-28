namespace API.DTOs
{
    public class TaskDto
    {
        public int TaskId { get; set; }
        public int AccountCreateId { get; set; }
        public string? AccountCreateName { get; set; }
        public int AccountAssignedId { get; set; }
        public string? AccountAssignedName { get; set; }
        public string TaskTitle { get; set; }
        public string TaskContent { get; set; }
        public int Status { get; set; }
        public string TaskNotes { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime DateUpdated { get; set; }
    }
}
