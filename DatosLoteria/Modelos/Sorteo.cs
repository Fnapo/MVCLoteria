using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace DatosLoteria.Modelos;

[Table("Sorteo")]
public partial class Sorteo
{
    [Key]
    [Column("ID_Sorteo")]
    public int IdSorteo { get; set; }

    public DateOnly Fecha { get; set; }

    [Column(TypeName = "money")]
    public decimal Precio { get; set; }

    public int Numero { get; set; }

    [InverseProperty("FkSorteoNavigation")]
    public virtual ICollection<Pago> Pagos { get; set; } = new List<Pago>();

    [InverseProperty("FkSorteoNavigation")]
    public virtual ICollection<Serie> Series { get; set; } = new List<Serie>();
}
