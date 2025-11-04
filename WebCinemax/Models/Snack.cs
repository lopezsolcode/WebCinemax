using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace WebCinemax.Models;

public partial class Snack
{
    [Key]
    [Column("idSnack")]
    public int IdSnack { get; set; }

    [StringLength(100)]
    public string Nombre { get; set; } = null!;

    [Column(TypeName = "decimal(10, 2)")]
    public decimal Precio { get; set; }

    [StringLength(500)]
    public string Imagen { get; set; } = null!;

    [InverseProperty("IdSnackNavigation")]
    public virtual Inventario? Inventario { get; set; }

    [InverseProperty("IdSnackNavigation")]
    public virtual ICollection<SnackReserva> SnackReservas { get; set; } = new List<SnackReserva>();
}
