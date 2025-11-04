using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace WebCinemax.Models;

public partial class Reserva
{
    [Key]
    [Column("idReserva")]
    public int IdReserva { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime FechaReserva { get; set; }

    public bool Confirmada { get; set; }

    [Column("idFuncion")]
    public int IdFuncion { get; set; }

    [ForeignKey("IdFuncion")]
    [InverseProperty("Reservas")]
    public virtual Funcion IdFuncionNavigation { get; set; } = null!;

    [InverseProperty("IdReservaNavigation")]
    public virtual ICollection<ReservaButaca> ReservaButacas { get; set; } = new List<ReservaButaca>();

    [InverseProperty("IdReservaNavigation")]
    public virtual ICollection<SnackReserva> SnackReservas { get; set; } = new List<SnackReserva>();
}
