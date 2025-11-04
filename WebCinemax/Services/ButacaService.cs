using Microsoft.EntityFrameworkCore;
using WebCinemax.Models;

namespace WebCinemax.Services
{
    public class ButacaService
    {
        private readonly IDbContextFactory<CineDbContext> _dbFactory;

        public ButacaService(IDbContextFactory<CineDbContext> dbFactory)
        {
            _dbFactory = dbFactory;
        }

        // ✅ Obtener todas las butacas (con su sala)
        public async Task<List<Butaca>> GetAllAsync()
        {
            using var context = _dbFactory.CreateDbContext();
            return await context.Butacas
                .Include(b => b.IdSalaNavigation)
                .ToListAsync();
        }

        // ✅ Obtener una butaca por ID
        public async Task<Butaca?> GetByIdAsync(int id)
        {
            using var context = _dbFactory.CreateDbContext();
            return await context.Butacas
                .Include(b => b.IdSalaNavigation)
                .FirstOrDefaultAsync(b => b.IdButaca == id);
        }

        // ✅ Crear nueva butaca
        public async Task AddAsync(Butaca butaca)
        {
            using var context = _dbFactory.CreateDbContext();
            context.Butacas.Add(butaca);
            await context.SaveChangesAsync();
        }

        // ✅ Actualizar butaca existente
        public async Task UpdateAsync(Butaca butaca)
        {
            using var context = _dbFactory.CreateDbContext();
            context.Butacas.Update(butaca);
            await context.SaveChangesAsync();
        }

        // ✅ Eliminar butaca por ID
        public async Task DeleteAsync(int id)
        {
            using var context = _dbFactory.CreateDbContext();
            var butaca = await context.Butacas.FindAsync(id);
            if (butaca != null)
            {
                context.Butacas.Remove(butaca);
                await context.SaveChangesAsync();
            }
        }

        // ✅ Comprobar existencia
        public async Task<bool> ExistsAsync(int id)
        {
            using var context = _dbFactory.CreateDbContext();
            return await context.Butacas.AnyAsync(b => b.IdButaca == id);
        }
    }
}