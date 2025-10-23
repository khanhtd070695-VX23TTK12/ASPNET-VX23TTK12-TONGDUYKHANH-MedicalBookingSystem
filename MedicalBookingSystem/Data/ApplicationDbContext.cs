using Microsoft.EntityFrameworkCore;
using MedicalBookingSystem.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;

namespace MedicalBookingSystem.Data
{
    public class ApplicationDbContext : IdentityDbContext<IdentityUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Doctor> Doctors { get; set; } = null!;
        public DbSet<Specialty> Specialties { get; set; } = null!;
        public DbSet<Appointment> Appointments { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Doctor>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Name).IsRequired().HasMaxLength(100);
                entity.Property(e => e.MedicalLicense).IsRequired().HasMaxLength(50);
                entity.Property(e => e.ConsultationFee).HasColumnType("decimal(18,2)");

                entity.HasOne(d => d.Specialty)
                      .WithMany(s => s.Doctors)
                      .HasForeignKey(d => d.SpecialtyId)
                      .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<Appointment>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.PatientName).IsRequired().HasMaxLength(100);
                entity.Property(e => e.PatientEmail).IsRequired().HasMaxLength(100);
                entity.Property(e => e.PatientPhone).IsRequired().HasMaxLength(15);
                entity.Property(e => e.Status).HasMaxLength(50);

                entity.HasOne(a => a.Doctor)
                      .WithMany()
                      .HasForeignKey(a => a.DoctorId)
                      .OnDelete(DeleteBehavior.Restrict);
            });
        }
    }
}