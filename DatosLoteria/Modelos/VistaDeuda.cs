using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace DatosLoteria.Modelos;

[Keyless]
public partial class VistaDeuda
{
    public DateOnly Fecha { get; set; }

    [StringLength(77)]
    public string? NombreCompleto { get; set; }

    [Column(TypeName = "money")]
    public decimal? Deuda { get; set; }

    public int? Papeletas { get; set; }

    [Column("ID_Socio")]
    public int IdSocio { get; set; }

    [Column("ID_Sorteo")]
    public int IdSorteo { get; set; }

    [Column(TypeName = "money")]
    public decimal? Pagado { get; set; }
}
