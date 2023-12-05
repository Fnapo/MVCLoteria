using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace DatosLoteria.Modelos;

public partial class Sorteo
{
    [Key]
    [Column("ID_Sorteo")]
    public int IdSorteo { get; set; }

    public DateOnly Fecha { get; set; }

    [StringLength(5)]
    public string Numero { get; set; } = null!;

    [Column(TypeName = "money")]
    public decimal Precio { get; set; }

    [InverseProperty("FkSorteoNavigation")]
    public virtual ICollection<Pago> Pagos { get; set; } = new List<Pago>();

    [InverseProperty("FkSorteoNavigation")]
    public virtual ICollection<Series> Series { get; set; } = new List<Series>();
}
