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

        public virtual DbSet<CarCleaning> CarCleaning { get; set; }
        public virtual DbSet<CarRetire> CarRetire { get; set; }
        public virtual DbSet<CarService> CarService { get; set; }
        public virtual DbSet<Cars> Cars { get; set; }
        public virtual DbSet<Customers> Customers { get; set; }
        public virtual DbSet<Events> Events { get; set; }
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

            modelBuilder.Entity<CarCleaning>(entity =>
            {
                entity.Property(e => e.CleaningDoneDate).HasColumnType("date");

                entity.Property(e => e.FlaggedForCleaningDate).HasColumnType("date");

                entity.HasOne(d => d.Car)
                    .WithMany(p => p.CarCleaning)
                    .HasForeignKey(d => d.CarId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__CarCleani__CarId__7F2BE32F");
            });

            modelBuilder.Entity<CarRetire>(entity =>
            {
                entity.Property(e => e.FlaggedForRetiringDate).HasColumnType("date");

                entity.Property(e => e.RetiredDate).HasColumnType("date");

                entity.HasOne(d => d.Car)
                    .WithMany(p => p.CarRetire)
                    .HasForeignKey(d => d.CarId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__CarRetire__CarId__797309D9");
            });

            modelBuilder.Entity<CarService>(entity =>
            {
                entity.Property(e => e.FlaggedForServiceDate).HasColumnType("date");

                entity.Property(e => e.ServiceDoneDate).HasColumnType("date");

                entity.HasOne(d => d.Car)
                    .WithMany(p => p.CarService)
                    .HasForeignKey(d => d.CarId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__CarServic__CarId__7C4F7684");
            });

            modelBuilder.Entity<Cars>(entity =>
            {
                entity.HasIndex(e => e.Registrationnumber)
                    .HasName("UQ__Cars__B7F512B974C2FB03")
                    .IsUnique();

                entity.Property(e => e.CarType)
                    .IsRequired()
                    .HasMaxLength(450);

                entity.Property(e => e.Registrationnumber)
                    .IsRequired()
                    .HasMaxLength(6);
            });

            modelBuilder.Entity<Customers>(entity =>
            {
                entity.HasIndex(e => e.PersonNumber)
                    .HasName("UQ__Customer__85D6AE8F44FEF51D")
                    .IsUnique();

                entity.Property(e => e.FirstName).IsRequired();

                entity.Property(e => e.LastName).IsRequired();

                entity.Property(e => e.PersonNumber)
                    .IsRequired()
                    .HasMaxLength(11);
            });

            modelBuilder.Entity<Events>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.BookingId).HasColumnName("BookingID");

                entity.Property(e => e.CarId).HasColumnName("CarID");

                entity.Property(e => e.CustomerId).HasColumnName("CustomerID");

                entity.Property(e => e.Date).HasColumnType("datetime");

                entity.HasOne(d => d.Booking)
                    .WithMany(p => p.Events)
                    .HasForeignKey(d => d.BookingId)
                    .HasConstraintName("FK__Events__BookingI__6442E2C9");

                entity.HasOne(d => d.Car)
                    .WithMany(p => p.Events)
                    .HasForeignKey(d => d.CarId)
                    .HasConstraintName("FK__Events__CarID__625A9A57");

                entity.HasOne(d => d.Customer)
                    .WithMany(p => p.Events)
                    .HasForeignKey(d => d.CustomerId)
                    .HasConstraintName("FK__Events__Customer__634EBE90");
            });

            modelBuilder.Entity<Orders>(entity =>
            {
                entity.Property(e => e.CarId).HasColumnName("CarID");

                entity.Property(e => e.CustomerId).HasColumnName("CustomerID");

                entity.Property(e => e.RentalDate).HasColumnType("date");

                entity.Property(e => e.ReturnDate).HasColumnType("date");

                entity.HasOne(d => d.Car)
                    .WithMany(p => p.Orders)
                    .HasForeignKey(d => d.CarId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Orders__CarID__531856C7");

                entity.HasOne(d => d.Customer)
                    .WithMany(p => p.Orders)
                    .HasForeignKey(d => d.CustomerId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Orders__Customer__540C7B00");
            });
        }
    }
}
