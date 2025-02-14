using Domain.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Interfaces
{
  
    public interface IGetEmployees
    {
        Task<List<EmployeePersonDTO>> GetEmployeeList(string searchTerm);
    }
}
