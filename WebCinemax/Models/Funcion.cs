using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace WebCinemax.Models;

public partial class Funcion
{
    [Key]
    [Column("idFuncion")]
    public int IdFuncion { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime FechaHora { get; set; }

    [Column(TypeName = "decimal(10, 2)")]
    public decimal PrecioEntrada { get; set; }

    [Column("idPelicula")]
    public int IdPelicula { get; set; }

    [Column("idSala")]
    public int IdSala { get; set; }

    [ForeignKey("IdPelicula")]
    [InverseProperty("Funciones")]
    public virtual Pelicula IdPeliculaNavigation { get; set; } = null!;

    [ForeignKey("IdSala")]
    [InverseProperty("Funciones")]
    public virtual Sala IdSalaNavigation { get; set; } = null!;

    [InverseProperty("IdFuncionNavigation")]
    public virtual ICollection<Reserva> Reservas { get; set; } = new List<Reserva>();
}
