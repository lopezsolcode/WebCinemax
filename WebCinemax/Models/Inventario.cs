using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace WebCinemax.Models;

[Index("IdSnack", Name = "UQ__Inventar__F684702BBC950EB0", IsUnique = true)]
public partial class Inventario
{
    [Key]
    [Column("idInventario")]
    public int IdInventario { get; set; }

    public int CantidadDisponible { get; set; }

    [Column("idSnack")]
    public int IdSnack { get; set; }

    [ForeignKey("IdSnack")]
    [InverseProperty("Inventario")]
    public virtual Snack IdSnackNavigation { get; set; } = null!;
}
