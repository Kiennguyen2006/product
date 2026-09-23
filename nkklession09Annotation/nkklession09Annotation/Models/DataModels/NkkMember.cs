using System.ComponentModel.DataAnnotations;

namespace nkklession09Annotation.Models.DataModels
{
    public class NkkMember
    {
        [Key]
        public int NkkMemberId { get; set; }
        public string TvcUserName { get; set; }
        public string NkkPassword { get; set; }
        public string NkkEmail { get; set; }
        public string NkkPhoneNumber { get; set; }
        public string NkkFullName { get; set; }
        public DateTime NkkBirthday { get; set; }
    }
}
