using System.Diagnostics.CodeAnalysis;

namespace Domain.Models
{
    public class Person
    {
        public int PersonId { get; set; }
        [NotNull]
        public string LastName { get; set; } = string.Empty;
        [NotNull]
        public string FirstName { get; set; }= string.Empty;
        public DateTimeOffset BirthDate { get; set; }
        public Employee Employee { get; set; }
        public string FullName => FirstName + " " + LastName;

    }
}
