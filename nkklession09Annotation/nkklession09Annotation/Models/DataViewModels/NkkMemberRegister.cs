using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace nkklession09Annotation.Models.DataViewModels
{
    public class NkkMemberRegister
    {
        public int NkkMemberId { get; set; }
        [DisplayName("Tên đăng nhập")]
        [Required(ErrorMessage ="Tên đăng nhập không thể trống")]
        [StringLength(20, MinimumLength = 2, ErrorMessage = "Tên đăng nhập có độ dài trong khoảng 2-20 ký tự")]
        public string TvcUserName { get; set; }
        [DisplayName("Mật khẩu")]
        [Required(ErrorMessage = "Tên đăng nhập không thể trống")]
        [DataType(DataType.Password)]
        public string NkkPassword { get; set; }
        public string NkkEmail { get; set; }
        public string NkkPhoneNumber { get; set; }
        public string NkkFullName { get; set; }
        public DateTime NkkBirthday { get; set; }
    }
}
