using System.Diagnostics.CodeAnalysis;

namespace Domain.Models
{
    public class Employee
    {
        public int EmployeeId { get; set; }
        public int PersonId { get; set; }
        [NotNull]
        public string EmployeeNumber { get; set; }
        public DateTimeOffset EmployedDate { get; set; }
        public DateTimeOffset? TerminatedDate { get; set; }
        public Person Person { get; set; }
    }
}
