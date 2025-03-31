using Lambda.Pregiato.Models;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

namespace Lambda.Pregiato.Data
{
    public class LambdaContextDB : DbContext 
    {
        public LambdaContextDB (DbContextOptions<LambdaContextDB> options) : base(options) { }
        public DbSet<Contract> Contracts { get; set; }     
        public DbSet<Model> Models { get; set; }
        public DbSet<Producers> Producers { get; set; } 


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Contract>(entity =>
            {
                entity.ToTable("Contracts");
                entity.HasKey(e => e.ContractId);
                entity.Property(e => e.CodProposta)
                    .IsRequired()
                    .HasDefaultValue(400);
                entity.Property(e => e.ContractFilePath).IsRequired(true);
                entity.Property(e => e.Content).HasColumnType("bytea");
                entity.Property(e => e.ValorContrato).IsRequired();
                entity.Property(e => e.FormaPagamento)
                    .HasColumnType("text")
                    .IsRequired();
                entity.Property(e => e.StatusPagamento)
                    .HasColumnType("text")
                    .IsRequired();
                entity.Property(e => e.StatusContratc)
                    .HasConversion<string>();
                entity.Property(e => e.CreatedAt)
                    .HasDefaultValueSql("CURRENT_TIMESTAMP")
                    .HasColumnType("timestamp with time zone");
                entity.Property(e => e.UpdatedAt)
                    .HasDefaultValueSql("CURRENT_TIMESTAMP")
                    .HasColumnType("timestamp with time zone");
            });



            modelBuilder.Entity<Model>(entity =>
            {
                entity.ToTable("Models");
                entity.HasKey(e => e.IdModel);
                entity.Property(e => e.IdModel)
                    .HasColumnName("ModelId")
                    .HasDefaultValueSql("gen_random_uuid()");
                entity.Property(e => e.CPF).IsRequired().HasMaxLength(20);
                entity.Property(e => e.Name).IsRequired().HasMaxLength(100);
                entity.Property(e => e.RG).IsRequired().HasMaxLength(20);
                entity.Property(e => e.Email).IsRequired().HasMaxLength(255);
                entity.Property(e => e.DateOfBirth)
                    .HasDefaultValueSql("CURRENT_TIMESTAMP")
                    .HasColumnType("timestamp with time zone");
                entity.Property(e => e.Age).IsRequired(true);
                entity.Property(e => e.Complement).IsRequired(true);
                entity.Property(e => e.Status).IsRequired(true);
                entity.Property(e => e.Neighborhood).IsRequired(true);
                entity.Property(e => e.UF).IsRequired(true);
                entity.Property(e => e.TelefonePrincipal).IsRequired(true);
                entity.Property(e => e.PostalCode).IsRequired();
                entity.Property(e => e.Address).HasMaxLength(255);
                entity.Property(e => e.BankAccount).HasMaxLength(30);
                entity.Property(e => e.Status).HasDefaultValue(true);
                entity.Property(e => e.CreatedAt)
                    .HasDefaultValueSql("CURRENT_TIMESTAMP")
                    .HasColumnType("timestamp with time zone");
                entity.Property(e => e.UpdatedAt)
                    .HasDefaultValueSql("CURRENT_TIMESTAMP")
                    .HasColumnType("timestamp with time zone");
                entity.Property(e => e.DNA);
            });

            modelBuilder.Entity<Producers>(entity =>
            {
                entity.ToTable("Producers");
                entity.HasKey(e => e.CodProducers);
                entity.Property(e => e.NameProducer).IsRequired(true);
                entity.Property(e => e.CodProposal).IsRequired(true);
                entity.Property(e => e.AmountContract).IsRequired().IsRequired(true);
                entity.Property(e => e.StatusContratc)
                    .HasConversion<string>();
                entity.Property(e => e.TotalAgreements).HasDefaultValue(1);
                entity.Property(e => e.InfoModel)
                    .HasConversion(
                        v => JsonSerializer.Serialize(v, new JsonSerializerOptions()),
                        v => JsonSerializer.Deserialize<DetailsInfo>(v, new JsonSerializerOptions())!);
                entity.Property(e => e.CreatedAt)
                    .HasDefaultValueSql("CURRENT_TIMESTAMP")
                    .HasColumnType("timestamp with time zone");

                entity.Property(e => e.UpdatedAt)
                    .HasDefaultValueSql("CURRENT_TIMESTAMP")
                    .HasColumnType("timestamp with time zone");
            });

        }
    }
}
