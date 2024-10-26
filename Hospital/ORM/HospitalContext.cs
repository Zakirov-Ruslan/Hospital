using Microsoft.EntityFrameworkCore;
using System;
using System.Diagnostics;

namespace Hospital.ORM
{
    public class HospitalContext : DbContext
    {
        public HospitalContext(DbContextOptions<HospitalContext> options) : base(options) { }

        public DbSet<Area> Areas { get; set; }
        public DbSet<Specialization> Specializations { get; set; }
        public DbSet<Cabinet> Cabinets { get; set; }
        public DbSet<Patient> Patients { get; set; }
        public DbSet<Doctor> Doctors { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // One to many
            modelBuilder.Entity<Patient>()
                .HasOne(p => p.Area)
                .WithMany(u => u.Patients)
                .HasForeignKey(p => p.AreaId);

            modelBuilder.Entity<Doctor>()
                .HasOne(v => v.Cabinet)
                .WithMany(k => k.Doctors)
                .HasForeignKey(v => v.CabinetId);

            modelBuilder.Entity<Doctor>()
                .HasOne(v => v.Specialization)
                .WithMany(s => s.Doctors)
                .HasForeignKey(v => v.SpecializationId);

            modelBuilder.Entity<Doctor>()
                .HasOne(v => v.Area)
                .WithMany(u => u.Doctors)
                .HasForeignKey(v => v.AreaId);
        }
    }
}
