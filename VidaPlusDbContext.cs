using Microsoft.EntityFrameworkCore;
using VidaPlus.Server.Controllers;
using VidaPlus.Server.Models;

namespace VidaPlus.Server.Data
{
    public class VidaPlusDbContext : DbContext
    {
        public VidaPlusDbContext(DbContextOptions<VidaPlusDbContext> options) : 
            base(options)
        { 
        }

        public DbSet<UsuarioBase> Usuarios { get; set; }
        public DbSet<Paciente> Pacientes { get; set; }
        public DbSet<Profissional> Profissionais { get; set; }
        public DbSet<Consulta> Consultas { get; set; }
        public DbSet<Leito> Leitos { get; set; }
        public DbSet<Prescricao> Prescricoes { get; set; }
        public DbSet<Internacao> Internacoes { get; set; }
        public DbSet<AuditLog> AuditLogs { get; set; }
        public DbSet<Movimentacao> Movimentacoes { get; set; }
        public DbSet<Financeiro> Financeiro { get; set; }
        public DbSet<Leito> Leito { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UsuarioBase>().HasIndex(u => u.Email).IsUnique();
            modelBuilder.Entity<Paciente>().HasIndex(p => p.CPF).IsUnique();
            modelBuilder.Entity<Profissional>().HasIndex(p => p.CRM).IsUnique();

            base.OnModelCreating(modelBuilder);
        }
    }
}