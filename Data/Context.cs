using Data.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Data
{
    public class Context : IdentityDbContext<Usuario>
    {

        private readonly string ConnectionString;

        public Context()
        {
            ConnectionString = "Server=localhost; Port=52000; Database=FH_Bank; User Id=root; Password=teste123; Pooling=false;";
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);

            optionsBuilder.UseMySql(
                    ConnectionString,
                    ServerVersion.AutoDetect(ConnectionString),
                    options => options.EnableRetryOnFailure()
                );
        }

        DbSet<Conta> Conta { get; set; }
        DbSet<Extrato> Extrato { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.ApplyConfigurationsFromAssembly(typeof(Context).Assembly);
        }
    }
}
