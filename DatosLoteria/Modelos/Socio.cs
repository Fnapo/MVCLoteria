using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace DatosLoteria.Modelos;

[Table("Socio")]
public partial class Socio
{
    [Key]
    [Column("ID_Socio")]
    public int IdSocio { get; set; }

    [StringLength(25)]
    public string Nombre { get; set; } = null!;

    [StringLength(50)]
    public string Apellidos { get; set; } = null!;

    [StringLength(100)]
    public string Domicilio { get; set; } = null!;

    [StringLength(25)]
    public string Poblacion { get; set; } = null!;

    public int CodigoPostal { get; set; }

    [StringLength(77)]
    public string? NombreCompleto { get; set; }

    [Column("DNI")]
    public int? Dni { get; set; }

    [StringLength(1)]
    [Unicode(false)]
    public string? Letra { get; set; }

    [InverseProperty("FkSocioNavigation")]
    public virtual ICollection<Pago> Pagos { get; set; } = new List<Pago>();

    [InverseProperty("FkSocioNavigation")]
    public virtual ICollection<Serie> Series { get; set; } = new List<Serie>();
}
