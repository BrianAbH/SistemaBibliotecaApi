using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace DataBaseFirst.Models;

public partial class prestamos
{
    public int id_prestamo { get; set; }

    public int id_usuario { get; set; }

    public int id_libro { get; set; }

    public DateOnly fechaPrestamo { get; set; }

    public DateOnly fechaDevolucion { get; set; }

    public string estado { get; set; }

    [JsonIgnore]
    public virtual libros? id_libroNavigation { get; set; } = null!;

    [JsonIgnore]
    public virtual usuarios? id_usuarioNavigation { get; set; } = null!;
}
