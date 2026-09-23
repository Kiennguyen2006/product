using System.ComponentModel.DataAnnotations;

namespace nkkbai1.Models
{
    public class Nkkbai1Model
    {
        [Key]
        public int masp { get; set; }
        public string tensp { get; set; } = "";
        public decimal gia { get; set; }
        public int soluong { get; set; }
        public int loai { get; set; }
        public string anh { get; set; } = "";
    }
}
