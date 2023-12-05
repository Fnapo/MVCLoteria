using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace DatosLoteria.Modelos;

[Keyless]
public partial class VistaPago
{
    [Column(TypeName = "money")]
    public decimal? Pagado { get; set; }

    [StringLength(77)]
    public string? NombreCompleto { get; set; }

    public DateOnly Fecha { get; set; }

    [Column("ID_Socio")]
    public int IdSocio { get; set; }

    [Column("ID_Sorteo")]
    public int IdSorteo { get; set; }
}
