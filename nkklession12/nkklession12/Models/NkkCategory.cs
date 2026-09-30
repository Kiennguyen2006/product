using System.ComponentModel.DataAnnotations;

namespace NetCoreLAB6_EF.Models
{
    public class NkkCategory
    {
        [Key]
        public int Id { get; set; }

        // Thêm = string.Empty;
        public string Name { get; set; } = string.Empty;

        public byte Status { get; set; }

        public DateTime CreatedDate { get; set; }

        // Navigation property gán nullable ?
        public virtual ICollection<NkkProduct>? Products { get; set; }
    }
}