using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace WebCinemax.Models;

[Table("ReservaButaca")]
[Index("IdReserva", "IdButaca", Name = "UQ_ReservaButaca", IsUnique = true)]
public partial class ReservaButaca
{
    [Key]
    [Column("idReservaButaca")]
    public int IdReservaButaca { get; set; }

    [Column("idReserva")]
    public int IdReserva { get; set; }

    [Column("idButaca")]
    public int IdButaca { get; set; }

    [ForeignKey("IdButaca")]
    [InverseProperty("ReservaButacas")]
    public virtual Butaca IdButacaNavigation { get; set; } = null!;

    [ForeignKey("IdReserva")]
    [InverseProperty("ReservaButacas")]
    public virtual Reserva IdReservaNavigation { get; set; } = null!;
}
