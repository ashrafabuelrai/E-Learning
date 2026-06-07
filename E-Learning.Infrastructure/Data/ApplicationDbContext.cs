using E_Learning.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace E_Learning.Infrastructure.Data
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<Course> Courses { get; set; }
        public DbSet<Learner> Learners { get; set; }
        public DbSet<Enrollment> Enrollments { get; set; }
        public DbSet<AuditLog> AuditLogs { get; set; }
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Course
            modelBuilder.Entity<Course>(e =>
            {
                e.HasKey(c => c.Id);
                e.Property(c => c.Title).IsRequired().HasMaxLength(200);
                e.Property(c => c.Description).HasMaxLength(2000);
            });

            // Learner
            modelBuilder.Entity<Learner>(e =>
            {
                e.HasKey(l => l.Id);
                e.Property(l => l.FullName).IsRequired().HasMaxLength(200);
                e.Property(l => l.Email).IsRequired().HasMaxLength(200);
                e.Property(l => l.NationalId).IsRequired().HasMaxLength(50);
                e.HasIndex(l => l.NationalId).IsUnique();
                e.HasIndex(l => l.Email).IsUnique();
            });

            // Enrollment
            modelBuilder.Entity<Enrollment>(e =>
            {
                e.HasKey(en => en.Id);
                e.Property(en => en.Status).HasConversion<string>();

                e.HasOne(en => en.Learner)
                 .WithMany(l => l.Enrollments)
                 .HasForeignKey(en => en.LearnerId)
                 .OnDelete(DeleteBehavior.Restrict);

                e.HasOne(en => en.Course)
                 .WithMany(c => c.Enrollments)
                 .HasForeignKey(en => en.CourseId)
                 .OnDelete(DeleteBehavior.Restrict);

                // Prevent duplicate enrollment
                e.HasIndex(en => new { en.LearnerId, en.CourseId }).IsUnique();
            });

        }
    }
}
