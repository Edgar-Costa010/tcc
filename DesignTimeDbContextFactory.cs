using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace VidaPlus.Server.Data
{
    public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<VidaPlusDbContext>
    {
        public VidaPlusDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<VidaPlusDbContext>();

            // 🔹 Ajuste o caminho do banco conforme sua configuração
            optionsBuilder.UseSqlite("Data Source=vidaPlus.Sqlite");

            return new VidaPlusDbContext(optionsBuilder.Options);
        }
    }
}