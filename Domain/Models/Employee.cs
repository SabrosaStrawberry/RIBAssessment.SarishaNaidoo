using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace Domain.Models
{
    [Index(nameof(EmployeeNumber), IsUnique = true)]
    public class Employee
    {
        public int EmployeeId { get; set; }
        public int PersonId { get; set; }
        [NotNull]
        [MaxLength(16)]
        public string EmployeeNumber { get; set; }
        public DateTimeOffset EmployedDate { get; set; }
        public DateTimeOffset? TerminatedDate { get; set; }
        public Person Person { get; set; }
    }
}
