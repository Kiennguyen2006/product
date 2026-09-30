using System.ComponentModel.DataAnnotations;

namespace NetCoreLAB6_EF.Models
{
    public class NkkProduct
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Image { get; set; }
        public float Price { get; set; }
        public float? SalePrice { get; set; } // Kiểu nullable
        public byte Status { get; set; }
        public string? Descriptions { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.Now;

        public int CategoryId { get; set; }
        public virtual NkkCategory? Category { get; set; }
    }
}