using Microsoft.EntityFrameworkCore;
using WebCinemax.Models;

namespace WebCinemax.Services
{
    public class SalaService
    {
        private readonly IDbContextFactory<CineDbContext> _dbFactory;

        public SalaService(IDbContextFactory<CineDbContext> dbFactory)
        {
            _dbFactory = dbFactory;
        }

        // Obtener todas las salas
        public async Task<List<Sala>> GetAllAsync()
        {
            using var context = _dbFactory.CreateDbContext();
            return await context.Salas.AsNoTracking().ToListAsync();
        }

        // Obtener una sala por Id
        public async Task<Sala?> GetByIdAsync(int id)
        {
            using var context = _dbFactory.CreateDbContext();
            return await context.Salas.AsNoTracking()
                .FirstOrDefaultAsync(s => s.IdSala == id);
        }

        // Crear una nueva sala
        public async Task AddAsync(Sala sala)
        {
            using var context = _dbFactory.CreateDbContext();
            context.Salas.Add(sala);
            await context.SaveChangesAsync();
        }

        // Actualizar una sala existente
        public async Task UpdateAsync(Sala sala)
        {
            using var context = _dbFactory.CreateDbContext();
            context.Salas.Update(sala);
            await context.SaveChangesAsync();
        }

        // Eliminar una sala por Id
        public async Task DeleteAsync(int id)
        {
            using var context = _dbFactory.CreateDbContext();
            var sala = await context.Salas.FindAsync(id);
            if (sala != null)
            {
                context.Salas.Remove(sala);
                await context.SaveChangesAsync();
            }
        }
    }
}