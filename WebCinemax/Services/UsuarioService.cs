using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using WebCinemax.Models;

namespace WebCinemax.Services
{
    public class UsuarioService
    {
        private readonly CineDbContext _context;
        private readonly PasswordHasher<Usuario> _hasher;

        public UsuarioService(CineDbContext context)
        {
            _context = context;
            _hasher = new PasswordHasher<Usuario>();
        }

        public async Task<Usuario?> GetByUsernameAsync(string username) =>
            await _context.Usuarios.FirstOrDefaultAsync(u => u.Username == username);

        public async Task<Usuario?> GetByIdAsync(int id) =>
            await _context.Usuarios.FindAsync(id);

        public async Task<List<Usuario>> GetAllAsync() =>
            await _context.Usuarios.ToListAsync();

        // Crear usuario con hash de contraseña
        public async Task<bool> CreateAsync(Usuario usuario, string plainPassword)
        {
            if (await _context.Usuarios.AnyAsync(u => u.Username == usuario.Username))
                return false; // username repetido

            usuario.PasswordHash = _hasher.HashPassword(usuario, plainPassword);
            _context.Usuarios.Add(usuario);
            await _context.SaveChangesAsync();
            return true;
        }

        // Autenticar usuario comparando hash
        public async Task<Usuario?> AuthenticateAsync(string username, string password)
        {
            var user = await GetByUsernameAsync(username);
            if (user == null) return null;

            var result = _hasher.VerifyHashedPassword(user, user.PasswordHash, password);
            return result == PasswordVerificationResult.Success ? user : null;
        }

        public async Task UpdateAsync(Usuario usuario)
        {
            _context.Usuarios.Update(usuario);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var u = await _context.Usuarios.FindAsync(id);
            if (u != null)
            {
                _context.Usuarios.Remove(u);
                await _context.SaveChangesAsync();
            }
        }
    }
}
