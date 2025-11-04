using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace WebCinemax.Models;

public partial class Pelicula
{
    [Key]
    [Column("idPelicula")]
    public int IdPelicula { get; set; }

    [StringLength(200)]
    public string Titulo { get; set; } = null!;

    [StringLength(100)]
    public string Genero { get; set; } = null!;

    [StringLength(10)]
    public string Clasificacion { get; set; } = null!;

    public int DuracionMinutos { get; set; }

    [StringLength(50)]
    public string Idioma { get; set; } = null!;

    [StringLength(500)]
    public string? PosterUrl { get; set; }

    [InverseProperty("IdPeliculaNavigation")]
    public virtual ICollection<Funcion> Funciones { get; set; } = new List<Funcion>();
}
