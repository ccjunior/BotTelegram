namespace BotTelegram.Model
{
    public class User
    {
        public int id { get; set; }
        public bool is_bot { get; set; }
        public string first_name { get; set; } = string.Empty;
        public string username { get; set; } = string.Empty;
    }
}
