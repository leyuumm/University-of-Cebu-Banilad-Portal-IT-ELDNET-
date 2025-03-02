using Microsoft.EntityFrameworkCore;
using StudentPortal.Web.Models.Entities;

namespace StudentPortal.Web.Data
{
    public class ApplicationDB: DbContext
    {
        public ApplicationDB(DbContextOptions<ApplicationDB> options): base(options)
        { 
        }

        public DbSet<StudentEntry> StudentEntry { get; set; }

        public DbSet<SubjectEntry> SubjectEntry { get; set; }
        public DbSet<ScheduleEntry> ScheduleEntry { get; set; }
        public DbSet<StudentEnrollment> EnrollmentEntry { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<StudentEntry>()
                .HasKey(s => s.Id);

            modelBuilder.Entity<StudentEntry>()
                .HasIndex(s => s.Id)
                .IsUnique();

            base.OnModelCreating(modelBuilder);
        }
    }
}
