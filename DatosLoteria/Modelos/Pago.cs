using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace DatosLoteria.Modelos;

[PrimaryKey("FkSorteo", "FkSocio")]
[Table("Pago")]
public partial class Pago
{
    [Key]
    [Column("FK_Sorteo")]
    public int FkSorteo { get; set; }

    [Key]
    [Column("FK_Socio")]
    public int FkSocio { get; set; }

    [Column(TypeName = "money")]
    public decimal Cantidad { get; set; }

    [ForeignKey("FkSocio")]
    [InverseProperty("Pagos")]
    public virtual Socio FkSocioNavigation { get; set; } = null!;

    [ForeignKey("FkSorteo")]
    [InverseProperty("Pagos")]
    public virtual Sorteo FkSorteoNavigation { get; set; } = null!;
}
