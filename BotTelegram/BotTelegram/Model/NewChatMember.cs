namespace BotTelegram.Model
{
    public class NewChatMember
    {
        public User user { get; set; }
        public string status { get; set; } = string.Empty;  
    }
}
