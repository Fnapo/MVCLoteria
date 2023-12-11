using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace DatosLoteria.Modelos;

[PrimaryKey("FkSorteo", "Inicio")]
public partial class Series
{
    [Key]
    [DisplayName("Sorteo")]
    [Column("FK_Sorteo")]
    public int FkSorteo { get; set; }

    [DisplayName("Socio")]
    [Column("FK_Socio")]
    public int FkSocio { get; set; }

    [Key]
    [Range(1, 10000, ErrorMessage = "El campo {0} ha de ser mayor o igual que {1}")]
    public int Inicio { get; set; }

    [Range(1, 10000, ErrorMessage = "El campo {0} ha de ser mayor o igual que {1}")]
    public int Fin { get; set; }

    [ForeignKey("FkSocio")]
    [InverseProperty("Series")]
    public virtual Socio? FkSocioNavigation { get; set; } = null;

    [ForeignKey("FkSorteo")]
    [InverseProperty("Series")]
    public virtual Sorteo? FkSorteoNavigation { get; set; } = null;
}
