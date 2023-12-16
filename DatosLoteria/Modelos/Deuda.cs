using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace DatosLoteria.Modelos;

[Keyless]
public partial class Deuda
{
    [Column("ID_Socio")]
    public int IdSocio { get; set; }

    [StringLength(77)]
    public string? NombreCompleto { get; set; }

    [Column("ID_Sorteo")]
    public int IdSorteo { get; set; }

    public DateOnly Fecha { get; set; }

    [Column(TypeName = "money")]
    public decimal Cantidad { get; set; }

    public int? Papeletas { get; set; }

    [Column(TypeName = "money")]
    public decimal? Compra { get; set; }

    [Column("Deuda", TypeName = "money")]
    public decimal? Deuda1 { get; set; }
}
