using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace NextLevelBJJ.DataService.Models
{
    public partial class AttendanceTrackingContext : DbContext
    {
        public virtual DbSet<Attendances> Attendances { get; set; }
        public virtual DbSet<MigrationHistory> MigrationHistory { get; set; }
        public virtual DbSet<Passes> Passes { get; set; }
        public virtual DbSet<PassTypes> PassTypes { get; set; }
        public virtual DbSet<Settings> Settings { get; set; }
        public virtual DbSet<Students> Students { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
                optionsBuilder.UseSqlServer(@"Server=DESKTOP-LOMG36A;Database=AttendanceTracking;Trusted_Connection=True;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Attendances>(entity =>
            {
                entity.HasIndex(e => e.PassId)
                    .HasName("IX_Pass_Id");

                entity.HasIndex(e => e.StudentId)
                    .HasName("IX_Student_Id");

                entity.Property(e => e.PassId).HasColumnName("Pass_Id");

                entity.Property(e => e.StudentId).HasColumnName("Student_Id");

                entity.HasOne(d => d.Pass)
                    .WithMany(p => p.Attendances)
                    .HasForeignKey(d => d.PassId)
                    .HasConstraintName("FK_dbo.Attendances_dbo.Passes_Pass_Id");

                entity.HasOne(d => d.Student)
                    .WithMany(p => p.Attendances)
                    .HasForeignKey(d => d.StudentId)
                    .HasConstraintName("FK_dbo.Attendances_dbo.Students_Student_Id");
            });

            modelBuilder.Entity<MigrationHistory>(entity =>
            {
                entity.HasKey(e => new { e.MigrationId, e.ContextKey });

                entity.ToTable("__MigrationHistory");

                entity.Property(e => e.MigrationId).HasMaxLength(150);

                entity.Property(e => e.ContextKey).HasMaxLength(300);

                entity.Property(e => e.Model).IsRequired();

                entity.Property(e => e.ProductVersion)
                    .IsRequired()
                    .HasMaxLength(32);
            });

            modelBuilder.Entity<Passes>(entity =>
            {
                entity.HasIndex(e => e.StudentId)
                    .HasName("IX_Student_Id");

                entity.HasIndex(e => e.TypeId)
                    .HasName("IX_Type_Id");

                entity.Property(e => e.Discount).HasDefaultValueSql("((0))");

                entity.Property(e => e.Price).HasDefaultValueSql("((0))");

                entity.Property(e => e.StudentId).HasColumnName("Student_Id");

                entity.Property(e => e.TypeId).HasColumnName("Type_Id");

                entity.HasOne(d => d.Student)
                    .WithMany(p => p.Passes)
                    .HasForeignKey(d => d.StudentId)
                    .HasConstraintName("FK_dbo.Passes_dbo.Students_Student_Id");

                entity.HasOne(d => d.Type)
                    .WithMany(p => p.Passes)
                    .HasForeignKey(d => d.TypeId)
                    .HasConstraintName("FK_dbo.Passes_dbo.PassTypes_Type_Id");
            });

            modelBuilder.Entity<Students>(entity =>
            {
                entity.Property(e => e.Gender).HasDefaultValueSql("((0))");
            });
        }
    }
}
