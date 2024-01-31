namespace API.Entity;

public class Message
{
    public int MessageId { get; set; }
    public string MessageContent { get; set; }
    public Account AccountSerder { get; set; }
    public int AccountSerderId { get; set; }
    public string AccounSerdertName { get; set; }
    public Account AccountReceiver { get; set; }
    public int AccountReceiverId { get; set; }
    public DateTime DateSend { get; set; }
}