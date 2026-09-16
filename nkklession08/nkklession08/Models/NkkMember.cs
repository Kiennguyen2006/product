using System.ComponentModel;
using System.Reflection.Metadata;

namespace nkklession08.Models
{
    public class NkkMember
    {
        public string NkkMemberId { get; set; }
        public string NkkUserName { get; set; }
        public string NkkPassword { get; set; }
        [DisplayName("Họ và tên")]
        public string NkkFullName { get; set; }
        public string NkkEmail { get; set; }
    }
}
