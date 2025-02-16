using Domain.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Services.Interfaces;

namespace SarishaNaidooRIBAssessment.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EmployeesController : ControllerBase
    {

        private readonly ILogger<EmployeesController> _logger;
        private readonly IGetEmployeeService _getEmployees;
        private readonly ICreateEmployeeService _createOrEditEmployee;
        private readonly IUpdateEmployeeService _updateEmployee;
        private readonly IDeleteEmployeeService _deleteEmployee;
        public EmployeesController(ILogger<EmployeesController> logger,
            IGetEmployeeService getEmployees,
            ICreateEmployeeService createOrEditEmployee,
            IUpdateEmployeeService updateEmployee,
            IDeleteEmployeeService deleteEmployee)
        {
            _logger = logger;
            _getEmployees = getEmployees;
            _createOrEditEmployee = createOrEditEmployee;
            _updateEmployee = updateEmployee;
            _deleteEmployee = deleteEmployee;
        }

        //Get employees, also allows for search
        [HttpGet]
        [Authorize]
        public async Task<ActionResult<List<EmployeePersonDTO>>> Get([FromQuery] string? search)
        {
            return await _getEmployees.GetEmployeeList(search ?? string.Empty);
        }

        // GET: api/Employee/5 → Get Employee by ID
        //[HttpGet("{id}")]
        //public async Task<ActionResult<List<EmployeePersonDTO>>> GetEmployeeById([FromQuery] string? search)
        //{
        //    return await _getEmployees.GetEmployeeList(search ?? string.Empty);
        //}


        // POST: api/Employee → Create New Employee
        [HttpPost]
        [Authorize]
        public async Task<ActionResult<EmployeePersonDTO>> CreateEmployee([FromBody] EmployeePersonDTO employee)
        {
            var result = await _createOrEditEmployee.CreateEmployee(employee);

            return Ok(result);
                //CreatedAtAction(nameof(Get), new { id = result.FirstName }, result);


        }


        // 🔹 PUT: api/Employees/5 → Update Employee
        [HttpPut("{id}")]
        [Authorize]
        public async Task<IActionResult> UpdateEmployee(int id, EmployeePersonDTO employeePersonDTO)
        {

            var result = await _updateEmployee.UpdateEmployeeAsync(id, employeePersonDTO);
            return Ok(result);


        }

        // 🔹 DELETE: api/Employees/5 → Delete Employee
        [HttpDelete("{id}")]
        [Authorize]
        public async Task<IActionResult> DeleteEmployee(int id)
        {
            var employeeFound = await _deleteEmployee.DeleteEmployeeAsync(id);

            if (!employeeFound)
            {
                return NotFound($"Employee with ID {id} not found.");
            }

            return NoContent();
        }
    }

}

