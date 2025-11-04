using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace WebCinemax.Models;

public partial class Sala
{
    [Key]
    [Column("idSala")]
    public int IdSala { get; set; }

    [StringLength(100)]
    public string Nombre { get; set; } = null!;

    public int Capacidad { get; set; }

    [InverseProperty("IdSalaNavigation")]
    public virtual ICollection<Butaca> Butacas { get; set; } = new List<Butaca>();

    [InverseProperty("IdSalaNavigation")]
    public virtual ICollection<Funcion> Funciones { get; set; } = new List<Funcion>();
}
