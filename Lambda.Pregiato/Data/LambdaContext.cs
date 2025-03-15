using Lambda.Pregiato.Models;
using Microsoft.EntityFrameworkCore;

namespace Lambda.Pregiato.Data
{
    public class LambdaContext : DbContext 
    {
        public LambdaContext (DbContextOptions<LambdaContext> options) : base(options) { }
        public DbSet<Contract> Contracts { get; set; }  
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Contract>(entity =>
            {
                entity.ToTable("Contracts");
                entity.HasKey(c => c.ContractId);
                entity.Property(c => c.CodProposta)
                    .IsRequired()
                    .ValueGeneratedOnAdd()
                    .HasDefaultValue(110);

                entity.Property(c => c.ContractFilePath).IsRequired(false);
                entity.Property(c => c.Content).HasColumnType("bytea");

            });
        }
    }
}
