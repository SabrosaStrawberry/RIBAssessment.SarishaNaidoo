using Domain.DTOs;
using Microsoft.AspNetCore.Mvc;
using Services.Interfaces;

namespace SarishaNaidooRIBAssessment.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EmployeeController : ControllerBase
    {

        private readonly ILogger<EmployeeController> _logger;
        private readonly IGetEmployees _getEmployees;
        private readonly ICreateOrEditEmployee _createOrEditEmployee;
        private readonly IUpdateEmployee _updateEmployee;
        private readonly IDeleteEmployee _deleteEmployee;
        public EmployeeController(ILogger<EmployeeController> logger,
            IGetEmployees getEmployees,
            ICreateOrEditEmployee createOrEditEmployee,
            IUpdateEmployee updateEmployee,
            IDeleteEmployee deleteEmployee)
        {
            _logger = logger;
            _getEmployees = getEmployees;
            _createOrEditEmployee = createOrEditEmployee;
            _updateEmployee = updateEmployee;
            _deleteEmployee = deleteEmployee;
        }

        //Get employees, also allows for search
        [HttpGet(Name = "GetEmployees")]
        public async Task<ActionResult<List<EmployeePersonDTO>>> Get([FromQuery] string? search)
        {
            return await _getEmployees.GetEmployeeList(search);
        }

        // 🔹 GET: api/Employee/5 → Get Employee by ID
        //[HttpGet("{id}")]
        //public async Task<IActionResult> Get([FromQuery] string? search)
        //{
        //   throw new NotImplementedException();
        //}


        // 🔹 POST: api/Employee → Create New Employee
        [HttpPost]
        public async Task<ActionResult<EmployeePersonDTO>> CreateEmployee([FromBody] EmployeePersonDTO employee)
        {
            var result = await _createOrEditEmployee.CreateEmployee(employee); // ✅ Await the task
            return Ok(result);
        }


        // 🔹 PUT: api/Employees/5 → Update Employee
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateEmployee(int id, EmployeePersonDTO employeePersonDTO)
        {

                var result = await _updateEmployee.UpdateEmployeeAsync(id, employeePersonDTO);
                return Ok(result);


        }

        // 🔹 DELETE: api/Employees/5 → Delete Employee
        [HttpDelete("{id}")]
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

