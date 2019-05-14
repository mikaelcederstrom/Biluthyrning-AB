using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Biluthyrning_AB.Models.Entities
{
    public partial class BiluthyrningContext : DbContext
    {
        public BiluthyrningContext()
        {
        }

        public BiluthyrningContext(DbContextOptions<BiluthyrningContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Cars> Cars { get; set; }
        public virtual DbSet<Orders> Orders { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
                optionsBuilder.UseSqlServer("Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=Biluthyrning;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("ProductVersion", "2.2.4-servicing-10062");

            modelBuilder.Entity<Cars>(entity =>
            {
                entity.HasIndex(e => e.Registrationnumber)
                    .HasName("UQ__Cars__B7F512B957C0CAD9")
                    .IsUnique();

                entity.Property(e => e.CarType)
                    .IsRequired()
                    .HasMaxLength(450);

                entity.Property(e => e.Registrationnumber)
                    .IsRequired()
                    .HasMaxLength(6);
            });

            modelBuilder.Entity<Orders>(entity =>
            {
                entity.Property(e => e.CarId).HasColumnName("CarID");

                entity.Property(e => e.Personnumber)
                    .IsRequired()
                    .HasMaxLength(10);

                entity.Property(e => e.RentalDate)
                    .IsRequired()
                    .HasMaxLength(1);

                entity.Property(e => e.ReturnDate).HasMaxLength(1);

                entity.HasOne(d => d.Car)
                    .WithMany(p => p.Orders)
                    .HasForeignKey(d => d.CarId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Orders__CarID__31EC6D26");
            });
        }
    }
}
