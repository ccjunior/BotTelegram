namespace BotTelegram.Model
{
    public class From
    {
        public int id { get; set; }
        public bool is_bot { get; set; }
        public string first_name { get; set; } = string.Empty;  
        public string last_name { get; set; } = string.Empty;
        public string username { get; set; } = string.Empty;
        public string language_code { get; set; } = string.Empty;
    }
}
