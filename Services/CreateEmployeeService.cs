using AutoMapper;
using Database;
using Domain.DTOs;
using Domain.Models;
using Services.Interfaces;

namespace Services
{
    public class CreateEmployeeService : ICreateEmployeeService
    {
        private readonly DatabaseContext _dbcontext;
        private readonly IMapper _mapper;
        //TODO: Add logger
        public CreateEmployeeService(DatabaseContext dbContext,
            IMapper mapper)
        {
            _dbcontext = dbContext;
            _mapper = mapper;

        }

        public async Task<EmployeePersonDTO> CreateEmployee(EmployeePersonDTO employeePersonDTO)
        {
            var person = _mapper.Map<Person>(employeePersonDTO);
            var employee = _mapper.Map<Employee>(employeePersonDTO);

            await _dbcontext.Persons.AddAsync(person);
            employee.Person = person;
            await _dbcontext.Employees.AddAsync(employee);
            await _dbcontext.SaveChangesAsync();

            return _mapper.Map<EmployeePersonDTO>(employee);
        }
    
    }
}
