using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace WebCinemax.Models;

[Table("SnackReserva")]
[Index("IdReserva", "IdSnack", Name = "UQ_SnackReserva", IsUnique = true)]
public partial class SnackReserva
{
    [Key]
    [Column("idSnackReserva")]
    public int IdSnackReserva { get; set; }

    public int Cantidad { get; set; }

    [Column("idReserva")]
    public int IdReserva { get; set; }

    [Column("idSnack")]
    public int IdSnack { get; set; }

    [ForeignKey("IdReserva")]
    [InverseProperty("SnackReservas")]
    public virtual Reserva IdReservaNavigation { get; set; } = null!;

    [ForeignKey("IdSnack")]
    [InverseProperty("SnackReservas")]
    public virtual Snack IdSnackNavigation { get; set; } = null!;
}
