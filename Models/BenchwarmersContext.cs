using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace BenchWarmerAPI.Models
{
    public partial class BenchwarmersContext : DbContext
    {
        public BenchwarmersContext()
        {
        }

        public BenchwarmersContext(DbContextOptions<BenchwarmersContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Characters> Characters { get; set; }
        public virtual DbSet<Classes> Classes { get; set; }
        public virtual DbSet<Users> Users { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
                optionsBuilder.UseSqlServer("Data Source=benchwarmersdb.crsp7d0nfbrs.us-east-2.rds.amazonaws.com,1521;Initial Catalog=Benchwarmers;User ID=admin;Password=gkTBC1grTnxWVI89GA2F;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Characters>(entity =>
            {
                entity.HasKey(e => e.CharacterId);

                entity.Property(e => e.CharacterId).HasColumnName("characterID");

                entity.Property(e => e.ClassIdFk).HasColumnName("classID_fk");

                entity.Property(e => e.FullName)
                    .HasColumnName("full_name")
                    .HasMaxLength(50);

                entity.Property(e => e.UserIdFk).HasColumnName("userID_fk");

                entity.HasOne(d => d.ClassIdFkNavigation)
                    .WithMany(p => p.Characters)
                    .HasForeignKey(d => d.ClassIdFk)
                    .HasConstraintName("FK__Character__class__3D5E1FD2");

                entity.HasOne(d => d.UserIdFkNavigation)
                    .WithMany(p => p.Characters)
                    .HasForeignKey(d => d.UserIdFk)
                    .HasConstraintName("FK__Character__userI__3C69FB99");
            });

            modelBuilder.Entity<Classes>(entity =>
            {
                entity.HasKey(e => e.ClassId);

                entity.Property(e => e.ClassId).HasColumnName("classID");

                entity.Property(e => e.ClassName)
                    .HasColumnName("className")
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<Users>(entity =>
            {
                entity.HasKey(e => e.UserId);

                entity.Property(e => e.UserId).HasColumnName("userID");

                entity.Property(e => e.Upassword)
                    .HasColumnName("upassword")
                    .HasMaxLength(10);

                entity.Property(e => e.Username)
                    .HasColumnName("username")
                    .HasMaxLength(15);
            });
        }
    }
}
