namespace API.Entity;

public class Log
{
    public int LogId { get; set; }
    public Account AccountWriter { get; set; }
    public int AccountWriterId { get; set; }
    public string AccountWriterName { get; set; }
    public string Message { get; set; }
    public DateTime LogStartDate { get; set; }
    public DateTime LogEndDate { get; set; }
}