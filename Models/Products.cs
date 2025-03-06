using System.ComponentModel.DataAnnotations;

namespace Pos.Models
{
    public class products
    {
        public int productID { get; set; }
        [Required(ErrorMessage = "Please Upload a Photo.")]
        public byte[] photo { get; set; }
        public string name { get; set; } = "";
        public double _price { get; set; }
        public string code { get; set; } = "";
        public string status { get; set; } = "";
        public int qty { get; set; } = 1;
        public string price
        {
            get { return _price.ToString("0.00"); }
            set
            {
                if (string.IsNullOrEmpty(value))
                {
                    _price = 0;
                }
                else
                {
                    _price = double.Parse(value);
                }

            }
        }
    }
}
