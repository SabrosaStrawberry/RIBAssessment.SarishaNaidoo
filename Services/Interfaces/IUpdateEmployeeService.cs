using Domain.DTOs;

namespace Services.Interfaces
{
    public interface IUpdateEmployeeService
    {
        Task<EmployeePersonDTO> UpdateEmployeeAsync(int id, EmployeePersonDTO employeePersonDTO);
    }
}
