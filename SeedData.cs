using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using VidaPlus.Server.Models;

namespace VidaPlus.Server.Data
{
    public class SeedData
    {
        public async Task InitializeAsync(IServiceProvider serviceProvider)
        {
            var ctx = serviceProvider.GetRequiredService<VidaPlusDbContext>();

            // Admin
            if (!await ctx.Usuarios.AnyAsync(u => u.Email == "admin@vidaplus.com"))
            {
                ctx.Usuarios.Add(new UsuarioBase
                {
                    Nome = "Administrador",
                    Email = "admin@vidaplus.com",
                    SenhaHash = BCrypt.Net.BCrypt.HashPassword("admin123"),
                    Perfil = "Administrador"
                });

                await ctx.SaveChangesAsync();
            }
        }
    }
}