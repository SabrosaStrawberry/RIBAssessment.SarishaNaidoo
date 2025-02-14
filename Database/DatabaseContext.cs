using Domain.Models;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Metadata;

namespace Database
{
    public class DatabaseContext : DbContext
    {


        public DatabaseContext(DbContextOptions options) : base(options)
        {

        }

        public DbSet<Employee> Employees { get; set; }
        public DbSet<Person> Persons { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Employee>()
                .HasOne(e => e.Person)
                .WithOne(e => e.Employee)
                .HasForeignKey<Employee>(e => e.PersonId)
                .IsRequired();


            modelBuilder.ApplyConfiguration(new PersonConfiguration());
            modelBuilder.ApplyConfiguration(new EmployeeConfiguration());
        }
    }
}
