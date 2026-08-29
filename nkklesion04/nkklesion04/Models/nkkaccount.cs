namespace nkklesion04.Models
{
    public class nkkaccount
    {
        public int id { get; set; }
        public string name { get; set; } = string.Empty;
        public string email { get; set; } = string.Empty;
        public string phone { get; set; } = string.Empty;
        public string avartar { get; set; } = string.Empty;
        public string address { get; set; } = string.Empty;
        public string bio { get; set; } = string.Empty;
        public int gender { get; set; }
        public DateTime birthday { get; set; }
    }
}