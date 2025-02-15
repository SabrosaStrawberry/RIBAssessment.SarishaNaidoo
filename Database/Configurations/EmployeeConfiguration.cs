using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Database.Configurations
{
    public class EmployeeConfiguration : IEntityTypeConfiguration<Employee>
    {
        public void Configure(EntityTypeBuilder<Employee> builder)
        {
            builder.HasData(
                new Employee
                {
                    EmployeeId = 1,
                    EmployeeNumber = "ABC123",
                    EmployedDate = new DateTime(2025, 2, 14),
                    PersonId = 1,
                },
                new Employee
                {
                    EmployeeId = 2,
                    EmployeeNumber = "ASD345",
                    EmployedDate = new DateTime(2025, 2, 14),
                    PersonId = 2,
                }, new Employee
                {
                    EmployeeId = 3,
                    EmployeeNumber = "QWE456",
                    EmployedDate = new DateTime(2025, 2, 14),
                    PersonId = 3,
                }
                );
        }
    }
}
