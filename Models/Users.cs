namespace Pos.Models
{
    public class users
    {
        public int userID { get; set; }
        public string name { get; set; } = "";
        public string username { get; set; } = "";
        public string password { get; set; } = "";
        public string role { get; set; } = "";
        public string token { get; set; } = "";
    }
}
