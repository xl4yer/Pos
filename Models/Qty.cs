namespace Pos.Models
{
    public class Qty
    {
        public int qtyId { get; set; }
        public DateTime date { get; set; }
        public string code { get; set; } = string.Empty;
        public string name { get; set; } = string.Empty;
        public int qty { get; set; }
    }
}
