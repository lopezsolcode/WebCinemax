using Microsoft.EntityFrameworkCore;
using WebCinemax.Models;

namespace WebCinemax.Services
{
    public class FuncionService
    {
        private readonly IDbContextFactory<CineDbContext> _dbFactory;

        public FuncionService(IDbContextFactory<CineDbContext> dbFactory)
        {
            _dbFactory = dbFactory;
        }

        public async Task<List<Funcion>> GetAllAsync()
        {
            using var context = _dbFactory.CreateDbContext();
            return await context.Funciones
                .Include(f => f.IdPeliculaNavigation)
                .Include(f => f.IdSalaNavigation)
                .ToListAsync();
        }

        public async Task<Funcion?> GetByIdAsync(int id)
        {
            using var context = _dbFactory.CreateDbContext();
            return await context.Funciones
                .Include(f => f.IdPeliculaNavigation)
                .Include(f => f.IdSalaNavigation)
                .FirstOrDefaultAsync(f => f.IdFuncion == id);
        }

        public async Task AddAsync(Funcion funcion)
        {
            using var context = _dbFactory.CreateDbContext();
            context.Funciones.Add(funcion);
            await context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Funcion funcion)
        {
            using var context = _dbFactory.CreateDbContext();
            context.Funciones.Update(funcion);
            await context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            using var context = _dbFactory.CreateDbContext();
            var funcion = await context.Funciones.FindAsync(id);
            if (funcion != null)
            {
                context.Funciones.Remove(funcion);
                await context.SaveChangesAsync();
            }
        }
    }
}