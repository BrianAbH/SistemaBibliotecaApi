using System;
using System.Collections.Generic;

namespace DataBaseFirst.Models;

public partial class t_personal
{
    public int id_personal { get; set; }

    public string correo { get; set; } = null!;

    public string password_hash { get; set; } = null!;
}
