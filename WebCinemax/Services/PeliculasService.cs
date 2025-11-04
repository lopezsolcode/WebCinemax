using Microsoft.EntityFrameworkCore;
using WebCinemax.Models;

namespace WebCinemax.Services
{
    public class PeliculaService
    {
        private readonly CineDbContext _context;

        public PeliculaService(CineDbContext context)
        {
            _context = context;
        }

        public async Task<List<Pelicula>> GetAllAsync()
        {
            return await _context.Peliculas.ToListAsync();
        }

        public async Task<Pelicula?> GetByIdAsync(int id)
        {
            return await _context.Peliculas.FindAsync(id);
        }

        public async Task CreateAsync(Pelicula pelicula)
        {
            _context.Peliculas.Add(pelicula);
            await _context.SaveChangesAsync();
        }

        /*
        public async Task UpdateAsync(Pelicula pelicula)
        {
            _context.Peliculas.Update(pelicula);
            await _context.SaveChangesAsync();
        }
        */
        public async Task<bool> UpdateAsync(Pelicula pelicula)
        {
            _context.Peliculas.Update(pelicula);
            try
            {
                await _context.SaveChangesAsync();
                return true;
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await _context.Peliculas.AnyAsync(p => p.IdPelicula == pelicula.IdPelicula))
                    return false;

                throw;
            }
        }

        public async Task DeleteAsync(int id)
        {
            var pelicula = await _context.Peliculas.FindAsync(id);
            if (pelicula != null)
            {
                _context.Peliculas.Remove(pelicula);
                await _context.SaveChangesAsync();
            }
        }
    }
}