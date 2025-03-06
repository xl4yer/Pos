namespace Pos.Models
{
    public class purchase
    {
        public int purchaseID { get; set; }
        public DateTime date { get; set; }
        public string code { get; set; } = "";
        public string name { get; set; } = "";
        public int quantity { get; set; }
        public double price { get; set; }
        public double total { get; set; }
        public int month { get; set; }
        public double total_sales { get; set; }
    }
}
