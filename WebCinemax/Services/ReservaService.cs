using Microsoft.EntityFrameworkCore;
using WebCinemax.Models;

public class ReservaService
{
    private readonly IDbContextFactory<CineDbContext> _dbFactory;

    public ReservaService(IDbContextFactory<CineDbContext> dbFactory)
    {
        _dbFactory = dbFactory;
    }

    public async Task<List<Reserva>> GetAllAsync()
    {
        using var context = _dbFactory.CreateDbContext();
        return await context.Reservas
            .Include(r => r.IdFuncionNavigation)
                .ThenInclude(f => f.IdPeliculaNavigation)
            .Include(r => r.IdFuncionNavigation)
                .ThenInclude(f => f.IdSalaNavigation)
            .ToListAsync();
    }

    public async Task<List<Funcion>> ObtenerFuncionesAsync()
    {
        using var context = _dbFactory.CreateDbContext();
        return await context.Funciones
            .Include(f => f.IdPeliculaNavigation)
            .Include(f => f.IdSalaNavigation)
            .ToListAsync();
    }


    public async Task<List<Butaca>> GetButacasAsync()
    {
        using var context = _dbFactory.CreateDbContext();
        return await context.Butacas.Include(b => b.IdSalaNavigation).ToListAsync();
    }

    public async Task<List<Butaca>> ObtenerButacasDisponiblesPorFuncionAsync(int idFuncion)
    {
        using var context = _dbFactory.CreateDbContext();
        var funcion = await context.Funciones
            .Include(f => f.IdSalaNavigation)
            .FirstOrDefaultAsync(f => f.IdFuncion == idFuncion);

        if (funcion == null)
            return new List<Butaca>();

        var butacasSala = await context.Butacas
            .Where(b => b.IdSala == funcion.IdSalaNavigation.IdSala)
            .ToListAsync();

        var butacasOcupadas = await context.ReservaButacas
            .Where(rb => rb.IdReservaNavigation.IdFuncion == idFuncion)
            .Select(rb => rb.IdButaca)
            .ToListAsync();

        return butacasSala.Where(b => !butacasOcupadas.Contains(b.IdButaca)).ToList();
    }

    public async Task CreateReservaAsync(Reserva reserva, List<int> butacasSeleccionadas)
    {
        using var context = _dbFactory.CreateDbContext();

        context.Reservas.Add(reserva);
        await context.SaveChangesAsync();

        foreach (var idButaca in butacasSeleccionadas)
        {
            context.ReservaButacas.Add(new ReservaButaca
            {
                IdReserva = reserva.IdReserva,
                IdButaca = idButaca
            });
        }

        await context.SaveChangesAsync();
    }
}
