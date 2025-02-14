using AutoMapper;
using Database;
using Domain.DTOs;
using Microsoft.EntityFrameworkCore;
using Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    /// <summary>
    /// All methods that focus on getting employee data, following single responsibility principle
    /// </summary>
    public class GetEmployees: IGetEmployees
    {
        private readonly DatabaseContext _dbcontext;
        private readonly IMapper _mapper;
        //TODO: Add logger
        public GetEmployees(DatabaseContext dbContext,
            IMapper mapper) {
            _dbcontext = dbContext;
            _mapper = mapper;

        }

        public string? SearchTerm { get; set; }

        public async Task<List<EmployeePersonDTO>> GetEmployeeList(string searchTerm)
        {

            var query =  _dbcontext.Employees.Include(e=>e.Person).AsQueryable();

            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                query = query.Where(e =>
                    e.Person.FirstName.Contains(searchTerm) 
                    || e.Person.LastName.Contains(searchTerm) 
                    || e.Person.BirthDate.ToString().Contains(searchTerm)
                    || e.EmployeeNumber.Contains(searchTerm)
                    || e.EmployedDate.ToString().Contains(searchTerm)
                );
            }

            var employeeList = await query.ToListAsync();

            return _mapper.Map<List<EmployeePersonDTO>>(employeeList);

        }

       
            
    }
}
