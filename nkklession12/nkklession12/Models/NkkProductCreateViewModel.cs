using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace NetCoreLAB6_EF.Models
{
    public class NkkProductCreateViewModel
    {
        [Required(ErrorMessage = "Tên sản phẩm không được trống")]
        public string Name { get; set; }

        public IFormFile? ImageFile { get; set; }

        [Required(ErrorMessage = "Giá không được trống")]
        public float Price { get; set; }

        public float SalePrice { get; set; }

        public byte Status { get; set; } = 1;

        public string? Descriptions { get; set; }

        [Required(ErrorMessage = "Vui lòng chọn danh mục")]
        public int CategoryId { get; set; }
    }
}