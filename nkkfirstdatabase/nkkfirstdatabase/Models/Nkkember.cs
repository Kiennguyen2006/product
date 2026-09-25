using System;
using System.Collections.Generic;

namespace nkkfirstdatabase.Models;

public partial class Nkkember
{
    public int NkkMemberId { get; set; }

    public string? Nkkuser { get; set; }

    public string? Nkkpassword { get; set; }

    public string? Nkkfullname { get; set; }

    public string? Nkkemail { get; set; }

    public string? Nkkphone { get; set; }
}
