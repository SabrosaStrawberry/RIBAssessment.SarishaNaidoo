using AutoMapper;
using Database;
using Domain.DTOs;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{

    public class UpdateEmployee : IUpdateEmployee
    {
        private readonly DatabaseContext _dbcontext;
        private readonly IMapper _mapper;
        //TODO: Add logger
        public UpdateEmployee(DatabaseContext dbContext,
            IMapper mapper)
        {
            _dbcontext = dbContext;
            _mapper = mapper;

        }

        public async Task<EmployeePersonDTO> UpdateEmployeeAsync(int id, EmployeePersonDTO employeePersonDTO)
        {
            var existingEmployee = await _dbcontext.Employees.Include(e => e.Person).FirstOrDefaultAsync(e => e.EmployeeId == id);

            if (existingEmployee == null)
            {
                throw new Exception($"Employee with ID {id} not found.");
            }

            // Update Employee fields
            existingEmployee.EmployeeNumber = employeePersonDTO.EmployeeNumber ?? existingEmployee.EmployeeNumber;
            existingEmployee.EmployedDate = string.IsNullOrEmpty(employeePersonDTO.EmployedDate.ToString())
            ? existingEmployee.EmployedDate
            :employeePersonDTO.EmployedDate;
            existingEmployee.TerminatedDate = string.IsNullOrEmpty(employeePersonDTO.TerminatedDate.ToString())
            ? existingEmployee.TerminatedDate
            : employeePersonDTO.TerminatedDate;


            // Update associated Person fields
            existingEmployee.Person.FirstName = employeePersonDTO.FirstName ?? existingEmployee.Person.FirstName;
            existingEmployee.Person.LastName = employeePersonDTO.LastName ?? existingEmployee.Person.LastName;
            existingEmployee.Person.BirthDate = string.IsNullOrEmpty(employeePersonDTO.BirthDate.ToString())
            ? existingEmployee.Person.BirthDate
            : employeePersonDTO.BirthDate;

           


            await _dbcontext.SaveChangesAsync();
            return _mapper.Map<EmployeePersonDTO>(existingEmployee);


        }
    }
}
