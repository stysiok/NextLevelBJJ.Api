using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace NextLevelBJJ.DataService.Models
{
    public partial class NextLevelContext : DbContext
    {
        public virtual DbSet<Attendance> Attendances { get; set; }
        public virtual DbSet<Pass> Passes { get; set; }
        public virtual DbSet<PassType> PassTypes { get; set; }
        public virtual DbSet<Student> Students { get; set; }

        public NextLevelContext()
        {

        }
       
        public NextLevelContext(DbContextOptions<NextLevelContext> options) : base(options)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Attendance>(entity =>
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

            modelBuilder.Entity<Pass>(entity =>
            {
                entity.HasIndex(e => e.StudentId)
                    .HasName("IX_Student_Id");

                entity.HasIndex(e => e.TypeId)
                    .HasName("IX_Type_Id");

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

            modelBuilder.Entity<Student>(entity =>
            {
                entity.Property(e => e.Gender).HasDefaultValueSql("((0))");
            });
        }
    }
}
