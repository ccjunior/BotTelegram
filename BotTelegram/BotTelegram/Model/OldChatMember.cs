namespace BotTelegram.Model
{
    public class OldChatMember
    {
        public User user { get; set; }
        public string status { get; set; } = string.Empty;  
        public int until_date { get; set; }
    }
}
