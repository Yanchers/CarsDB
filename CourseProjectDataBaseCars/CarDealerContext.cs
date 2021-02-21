using System;
using System.IO;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.Extensions.Configuration;

#nullable disable

namespace CourseProjectDataBaseCars
{
    public partial class CarDealerContext : DbContext
    {
        public CarDealerContext()
        {
        }

        public CarDealerContext(DbContextOptions<CarDealerContext> options)
            : base(options)
        {
        }

        public IQueryable<Car> GetCarsByFactory(int id) => FromExpression(() => GetCarsByFactory(id));
        public IQueryable<Car> GetCarsByName(string name) => FromExpression(() => GetCarsByName(name));
        public IQueryable<Car> GetCarsByPrice(float upper, float down) => FromExpression(() => GetCarsByPrice(upper, down));
        public virtual DbSet<Bank> Banks { get; set; }
        public virtual DbSet<Car> Cars { get; set; }
        public virtual DbSet<CarsCredit> CarsCredits { get; set; }
        public virtual DbSet<CarsFactory> CarsFactories { get; set; }
        public virtual DbSet<Credit> Credits { get; set; }
        public virtual DbSet<Factory> Factories { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var builder = new ConfigurationBuilder();
            builder.SetBasePath(Directory.GetCurrentDirectory());
            builder.AddJsonFile("appsettings.json");

            optionsBuilder.UseSqlServer(builder.Build().GetConnectionString("DefaultConnection"));
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "Cyrillic_General_CI_AS");

            modelBuilder.Entity<Bank>(entity =>
            {
                entity.ToTable("Banks", "Dealer");

                entity.Property(e => e.Id).HasColumnName("id").UseIdentityColumn();

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasColumnName("name");
            });

            modelBuilder.Entity<Car>(entity =>
            {
                entity.ToTable("Cars", "Dealer");

                entity.Property(e => e.Id).HasColumnName("id").UseIdentityColumn();

                entity.Property(e => e.Cost)
                    .HasColumnType("money")
                    .HasColumnName("cost");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(30)
                    .HasColumnName("name");
            });

            modelBuilder.Entity<CarsCredit>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("CarsCredits", "Dealer");

                entity.Property(e => e.CarId).HasColumnName("carId");

                entity.Property(e => e.CreditId).HasColumnName("creditId");

                entity.HasOne(d => d.Car)
                    .WithMany()
                    .HasForeignKey(d => d.CarId)
                    .HasConstraintName("FK__CarsCredi__carId__571DF1D5");

                entity.HasOne(d => d.Credit)
                    .WithMany()
                    .HasForeignKey(d => d.CreditId)
                    .HasConstraintName("FK__CarsCredi__credi__5812160E");
            });

            modelBuilder.Entity<CarsFactory>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("CarsFactories", "Dealer");

                entity.Property(e => e.CarId).HasColumnName("carId");

                entity.Property(e => e.FactoryId).HasColumnName("factoryId");

                entity.HasOne(d => d.Car)
                    .WithMany()
                    .HasForeignKey(d => d.CarId)
                    .HasConstraintName("FK__CarsFacto__carId__4F7CD00D");

                entity.HasOne(d => d.Factory)
                    .WithMany()
                    .HasForeignKey(d => d.FactoryId)
                    .HasConstraintName("FK__CarsFacto__facto__5070F446");
            });

            modelBuilder.Entity<Credit>(entity =>
            {
                entity.ToTable("Credits", "Dealer");

                entity.Property(e => e.Id).HasColumnName("id").UseIdentityColumn();

                entity.Property(e => e.BankId).HasColumnName("bankId");

                entity.Property(e => e.Expiration).HasColumnName("expiration");

                entity.Property(e => e.Rate).HasColumnName("rate");

                entity.HasOne(d => d.Bank)
                    .WithMany(p => p.Credits)
                    .HasForeignKey(d => d.BankId)
                    .HasConstraintName("FK__Credits__bankId__5535A963");
            });

            modelBuilder.Entity<Factory>(entity =>
            {
                entity.ToTable("Factories", "Dealer");

                entity.Property(e => e.Id).HasColumnName("id").UseIdentityColumn();

                entity.Property(e => e.City)
                    .HasMaxLength(50)
                    .HasColumnName("city");

                entity.Property(e => e.Country)
                    .HasMaxLength(50)
                    .HasColumnName("country");

                entity.Property(e => e.DeliveryTime).HasColumnName("deliveryTime");

                entity.Property(e => e.TranspCost)
                    .HasColumnType("money")
                    .HasColumnName("transpCost");

                entity.Property(e => e.Type).HasColumnName("type");
            });

            modelBuilder.HasDbFunction(() => GetCarsByFactory(default)).HasSchema("Dealer");
            modelBuilder.HasDbFunction(() => GetCarsByName(default)).HasSchema("Dealer");
            modelBuilder.HasDbFunction(() => GetCarsByPrice(default, default)).HasSchema("Dealer");

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
