using System.ComponentModel.DataAnnotations;

namespace nkklesion03.Models
{
    public class nkkproduct
    {
        [Key]
        public string nkkproductid {  get; set; }
        public string nkkproductname { get; set; }
        public int nkkyearrelease {  get; set; }
        public decimal nkkprice { get; set; }
    }
}
