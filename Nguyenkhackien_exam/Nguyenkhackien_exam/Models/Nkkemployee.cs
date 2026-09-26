using System.ComponentModel.DataAnnotations;

namespace Nguyenkhackien_exam.Models;

public partial class Nkkemployee
{
    [Key]
    public int Id { get; set; }

    public string Nkkname { get; set; } = null!;

    public string? Nkkgender { get; set; }

    public DateOnly? NkkbirthDay { get; set; }

    public string? Nkkemail { get; set; }

    public string? Nkkphone { get; set; }

    public bool? Nkkactive { get; set; }
}