using Microsoft.EntityFrameworkCore;
using ProjetoEscola_API.Models;
using System.Diagnostics.CodeAnalysis;
namespace ProjetoEscola_API.Data
{
    public class PetShopContext : DbContext
    {
        protected readonly IConfiguration Configuration;
        public PetShopContext(IConfiguration configuration)
        {
            Configuration = configuration;
        }
        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            // connect to sql server with connection string from app settings
            options.UseSqlServer(Configuration.GetConnectionString("StringConexaoSQLServer"));
        }

        public DbSet<Perfil>? Perfil { get; set; }
        public DbSet<Agendamento>? Agendamento { get; set; }
  
    }
}