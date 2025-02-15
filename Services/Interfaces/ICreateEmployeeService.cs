using Domain.DTOs;

namespace Services.Interfaces
{
    public interface ICreateEmployeeService
    {
        Task<EmployeePersonDTO>CreateEmployee(EmployeePersonDTO employeePersonDTO);
    }
}
