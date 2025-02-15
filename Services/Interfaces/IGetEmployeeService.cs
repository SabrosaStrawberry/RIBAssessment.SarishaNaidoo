using Domain.DTOs;

namespace Services.Interfaces
{

    public interface IGetEmployeeService
    {
        Task<List<EmployeePersonDTO>> GetEmployeeList(string searchTerm);
    }
}
