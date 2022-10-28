namespace BotTelegram.Model
{
    public class Chat
    {
        public int id { get; set; }
        public string first_name { get; set; } = string.Empty;
        public string last_name { get; set; } = string.Empty;
        public string username { get; set; } = string.Empty;
        public string type { get; set; } = string.Empty;
    }
}
