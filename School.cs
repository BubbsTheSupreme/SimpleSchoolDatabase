using Microsoft.EntityFrameworkCore;

namespace School
{
    public class School : DbContext
    {
        // DbSet uses the properties as a table to tell EFCORE what data the table contains
        public DbSet<Student> Students { get; set; }
        public DbSet<Administrator> Administrators { get; set; }
        public DbSet<Applicant> Applicants { get; set; }
        public DbSet<ExpelledStudent> ExpelledStudents { get; set; }

        protected override void OnConfiguring(
            DbContextOptionsBuilder optionsBuilder)
        {   
            optionsBuilder.UseSqlite("Filename=School.db");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

        }

    }
}
