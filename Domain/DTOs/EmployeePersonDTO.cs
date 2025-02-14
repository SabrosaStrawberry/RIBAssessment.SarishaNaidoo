using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.DTOs
{
    public class EmployeePersonDTO
    {
        public int? EmployeeId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTimeOffset BirthDate { get; set; }
        public string EmployeeNumber { get; set; }
        public DateTimeOffset EmployedDate { get; set; }
        public DateTimeOffset? TerminatedDate { get; set; }
    }
}
