using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace DataBaseFirst.Models;

public partial class libros
{
    public int id_libro { get; set; }

    public string titulo { get; set; } = null!;

    public string ISBN { get; set; } = null!;

    public string autor { get; set; } = null!;

    public int? id_categoria { get; set; }

    public DateOnly anioPublicacion { get; set; }

    public int ejemplares { get; set; }

    public bool estado { get; set; }

    public void ReducirEjemplares(int cantidad)
    {
        if (cantidad <= 0) throw new ArgumentException("La cantidad debe ser mayor a cero.");
        if (ejemplares - cantidad < 0) throw new InvalidOperationException("Stock insuficiente.");

        ejemplares -= cantidad;
    }

    public void devolverEjemplares(int cantidad)
    {
        if (cantidad <= 0) throw new ArgumentException("La cantidad debe ser mayor a cero.");

        ejemplares += cantidad;
    }

    [JsonIgnore]
    public virtual categorias? id_categoriaNavigation { get; set; }

    [JsonIgnore]
    public virtual ICollection<prestamos> prestamos { get; set; } = new List<prestamos>();
}
