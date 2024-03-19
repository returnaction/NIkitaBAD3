using Microsoft.EntityFrameworkCore;
using NIkitaBAD3.Models;

namespace NIkitaBAD3.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Employee>().HasData(
                new Employee
                {
                    Id = new Guid("e310a6cb-6677-4aa6-93c7-2763956f7a97"),
                    Name = "Mark Miens",
                    Age = 26,
                    Position = "Software Developer"
                },
                new Employee
                {
                    Id = new Guid("398d10fe-4b8d-4606-8e9c-bd2c78d4e001"),
                    Name = "Anna Simmons",
                    Age = 29,
                    Position = "Software Developer"
                });
        }

        public DbSet<Employee> Employees { get; set; }
    }
}
