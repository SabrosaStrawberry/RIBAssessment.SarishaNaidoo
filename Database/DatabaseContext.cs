using Database.Configurations;
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

        public DbSet<User> Users { get; set; }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<Person> Persons { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {


            modelBuilder.Entity<Employee>()
                .HasOne(e => e.Person)
                .WithOne(e => e.Employee)
                .HasForeignKey<Employee>(e => e.PersonId)
                .IsRequired();

            //TODO: Add indexes??

            modelBuilder.ApplyConfiguration(new PersonConfiguration());
            modelBuilder.ApplyConfiguration(new EmployeeConfiguration());

            base.OnModelCreating(modelBuilder);
        }
    }
}
