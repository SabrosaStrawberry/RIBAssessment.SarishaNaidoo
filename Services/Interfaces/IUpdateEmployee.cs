using Domain.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Interfaces
{
    public interface IUpdateEmployee
    {
        Task<EmployeePersonDTO> UpdateEmployeeAsync(int id, EmployeePersonDTO employeePersonDTO);
    }
}
