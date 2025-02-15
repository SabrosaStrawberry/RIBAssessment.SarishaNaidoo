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
    internal class PersonConfiguration : IEntityTypeConfiguration<Person>
    {
        public void Configure(EntityTypeBuilder<Person> builder)
        {
            builder.HasData(
                  new Person
                  {
                      PersonId = 1,
                      FirstName = "Jane",
                      LastName = "Doe",
                      BirthDate = new DateTime(1994, 05, 16),
                  },
                  new Person
                  {
                      PersonId = 2,
                      FirstName = "John",
                      LastName = "Wick",
                      BirthDate = new DateTime(1993, 04, 22),
                  }, new Person
                  {
                      PersonId = 3,
                      FirstName = "Judy",
                      LastName = "Doe",
                      BirthDate = new DateTime(1998, 01, 03),
                  }
                  );
        }
    }
}
