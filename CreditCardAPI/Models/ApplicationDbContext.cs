using Microsoft.EntityFrameworkCore;

namespace CreditCardAPI.Models
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        // DbSet para las entidades
        public DbSet<CreditCardHolder> CreditCardHolders { get; set; }
        public DbSet<Transaction> Transactions { get; set; }

        // Configuración del modelo en caso de ser necesario
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configuraciones específicas para las tablas
            modelBuilder.Entity<CreditCardHolder>(entity =>
            {
                entity.HasKey(e => e.CardHolderID);
                entity.Property(e => e.Name).IsRequired().HasMaxLength(100);
                entity.Property(e => e.CardNumber).IsRequired().HasMaxLength(16);
                entity.Property(e => e.CreditLimit).HasColumnType("decimal(18, 2)");
                entity.Property(e => e.CurrentBalance).HasColumnType("decimal(18, 2)");
            });

            modelBuilder.Entity<Transaction>(entity =>
            {
                entity.HasKey(e => e.TransactionID);
                entity.Property(e => e.Description).HasMaxLength(255);
                entity.Property(e => e.TransactionType).IsRequired().HasMaxLength(10);
                entity.Property(e => e.Amount).HasColumnType("decimal(18, 2)");

                entity.HasOne(e => e.CardHolder)
                      .WithMany(c => c.Transactions)
                      .HasForeignKey(e => e.CardHolderID)
                      .OnDelete(DeleteBehavior.Cascade);
            });
        }
    }
}
