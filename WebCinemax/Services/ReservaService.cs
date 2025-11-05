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
        .Include(r => r.ReservaButacas) // 👈 carga la relación intermedia
            .ThenInclude(rb => rb.IdButacaNavigation) // 👈 carga las butacas asociadas
        .ToListAsync();
    }
    public async Task<Reserva?> GetByIdAsync(int idReserva)
    {
        using var _context = _dbFactory.CreateDbContext();
        return await _context.Reservas
            .Include(r => r.IdFuncionNavigation)
                .ThenInclude(f => f.IdPeliculaNavigation)
            .Include(r => r.IdFuncionNavigation)
                .ThenInclude(f => f.IdSalaNavigation)
            .Include(r => r.ReservaButacas)
                .ThenInclude(rb => rb.IdButacaNavigation)
            .FirstOrDefaultAsync(r => r.IdReserva == idReserva);
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
    public async Task UpdateReservaAsync(Reserva reserva, List<int> nuevasButacas)
    {
        using var _context = _dbFactory.CreateDbContext();
        var reservaExistente = await _context.Reservas
            .Include(r => r.ReservaButacas)
            .FirstOrDefaultAsync(r => r.IdReserva == reserva.IdReserva);

        if (reservaExistente == null)
            throw new Exception("Reserva no encontrada.");

        // Actualizar propiedades básicas
        reservaExistente.Confirmada = reserva.Confirmada;
        reservaExistente.IdFuncion = reserva.IdFuncion;
        reservaExistente.FechaReserva = reserva.FechaReserva;

        // Actualizar butacas
        _context.ReservaButacas.RemoveRange(reservaExistente.ReservaButacas);
        foreach (var idButaca in nuevasButacas)
        {
            _context.ReservaButacas.Add(new ReservaButaca
            {
                IdReserva = reservaExistente.IdReserva,
                IdButaca = idButaca
            });
        }

        await _context.SaveChangesAsync();
    }
    public async Task<bool> DeleteAsync(int idReserva)
    {
        using var _context = _dbFactory.CreateDbContext();
        var reserva = await _context.Reservas
            .Include(r => r.ReservaButacas)
            .FirstOrDefaultAsync(r => r.IdReserva == idReserva);

        if (reserva == null)
            return false;

        // Eliminar las butacas asociadas primero (si tu modelo no las elimina en cascada)
        if (reserva.ReservaButacas != null && reserva.ReservaButacas.Any())
        {
            _context.ReservaButacas.RemoveRange(reserva.ReservaButacas);
        }

        _context.Reservas.Remove(reserva);
        await _context.SaveChangesAsync();
        return true;
    }
}
