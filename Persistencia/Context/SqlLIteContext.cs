
using Dominio.Models;
using Microsoft.EntityFrameworkCore;
using Persistencia.FluentConfig;

namespace Persistencia.Context
{
    public class SqlLIteContext: DbContext
    {
        public SqlLIteContext(DbContextOptions<SqlLIteContext>options):base (options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            new ClientConfig(modelBuilder.Entity<Client>());
            
        }

        public DbSet<Client> Client { get; set; }
    }
}
