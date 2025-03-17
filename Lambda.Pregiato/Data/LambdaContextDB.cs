using Lambda.Pregiato.Models;
using Microsoft.EntityFrameworkCore;

namespace Lambda.Pregiato.Data
{
    public class LambdaContextDB : DbContext 
    {
        public LambdaContextDB (DbContextOptions<LambdaContextDB> options) : base(options) { }
        public DbSet<Contract> Contracts { get; set; }     
        public DbSet<Model> Model { get; set; }


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

                entity.Property(c => c.City).IsRequired(false);
                entity.Property(c => c.Neighborhood).IsRequired(false);
                entity.Property(c => c.ContractFilePath).IsRequired(false);
                entity.Property(c => c.Content).HasColumnType("bytea");
                entity.Property(c => c.ValorContrato).IsRequired();
                entity.Property(c => c.FormaPagamento)
                    .HasColumnType("text")
                    .IsRequired();
                entity.Property(c => c.StatusPagamento)
                    .HasColumnType("text")
                    .IsRequired();

                entity.HasOne(c => c.Model)
                    .WithMany(m => m.Contracts)
                    .HasForeignKey(c => c.ModelId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

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

            modelBuilder.Entity<Model>(entity =>
            {
                entity.HasKey(e => e.IdModel);
                entity.Property(e => e.IdModel)
                    .ValueGeneratedOnAdd()
                    .HasDefaultValueSql("gen_random_uuid()");

                entity.Property(e => e.CPF).IsRequired().HasMaxLength(14);
                entity.Property(e => e.RG).IsRequired().HasMaxLength(20);
                entity.Property(e => e.Email).IsRequired().HasMaxLength(255);
                entity.Property(e => e.PostalCode).HasMaxLength(10);
                entity.Property(e => e.Address).HasMaxLength(255);
                entity.Property(e => e.BankAccount).HasMaxLength(30);
                entity.Property(e => e.Status).HasDefaultValue(true);
                entity.Property(e => e.CreatedAt).HasDefaultValueSql("NOW()");
                entity.Property(e => e.UpdatedAt).HasDefaultValueSql("NOW()");
                entity.Property(e => e.DNA)
                    .HasColumnType("jsonb")
                    .HasDefaultValueSql("'{}'::jsonb");
            });

        }
    }
}
