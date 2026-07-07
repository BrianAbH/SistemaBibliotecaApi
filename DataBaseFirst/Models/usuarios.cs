using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace DataBaseFirst.Models;

public partial class usuarios
{
    public int id_usuario { get; set; }

    public string nombre { get; set; } = null!;

    public string correo { get; set; } = null!;

    public string telefono { get; set; } = null!;

    public string direccion { get; set; } = null!;

    public bool estado { get; set; }

    [JsonIgnore]
    public virtual ICollection<prestamos> prestamos { get; set; } = new List<prestamos>();
}
