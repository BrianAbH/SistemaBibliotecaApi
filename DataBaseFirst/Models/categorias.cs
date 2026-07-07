using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace DataBaseFirst.Models;

public partial class categorias
{
    public int id_categoria { get; set; }

    public string nombre_categoria { get; set; } = null!;

    public string descripcion { get; set; } = null!;

    [JsonIgnore]
    public virtual ICollection<libros> libros { get; set; } = new List<libros>();
}
