using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace WebCinemax.Models;

[Index("IdSala", "Numero", "Fila", Name = "UQ_Butaca", IsUnique = true)]
public partial class Butaca
{
    [Key]
    [Column("idButaca")]
    public int IdButaca { get; set; }

    public int Numero { get; set; }

    [StringLength(5)]
    public string Fila { get; set; } = null!;

    public bool EsVip { get; set; }

    public bool EsAccesible { get; set; }

    [Column("idSala")]
    public int IdSala { get; set; }

    [ForeignKey("IdSala")]
    [InverseProperty("Butacas")]
    public virtual Sala IdSalaNavigation { get; set; } = null!;

    [InverseProperty("IdButacaNavigation")]
    public virtual ICollection<ReservaButaca> ReservaButacas { get; set; } = new List<ReservaButaca>();
}
