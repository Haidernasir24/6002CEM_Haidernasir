namespace ChatApp.Models;

public class ChatMessagesModel
{
	public string messageId { get; set; }
	public string userId { get; set; }
	public string text { get; set; }
	public Boolean wasSent { get; set; }
	public long timeStamp { get; set; }
}
