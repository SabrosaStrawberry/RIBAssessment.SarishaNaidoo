using AutoMapper;
using Database;
using Domain.DTOs;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Services.Interfaces;

namespace Services
{
    /// <summary>
    /// Service to fetch employee data while following SRP (Single Responsibility Principle).
    /// </summary>
    public class GetEmployeeService : IGetEmployeeService
    {
        private readonly DatabaseContext _dbcontext;
        private readonly IMapper _mapper;
        private readonly ILogger<GetEmployeeService> _logger;

        public GetEmployeeService(DatabaseContext dbContext, IMapper mapper, ILogger<GetEmployeeService> logger)
        {
            _dbcontext = dbContext;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<List<EmployeePersonDTO>> GetEmployeeList(string? searchTerm)
        {
            _logger.LogInformation("Fetching employee list. Search term: {searchTerm}", searchTerm);

            var query = _dbcontext.Employees
                .Include(e => e.Person)
                .AsNoTracking(); 

            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                query = query.Where(e =>
                    e.Person.FirstName.Contains(searchTerm) ||
                    e.Person.LastName.Contains(searchTerm) ||
                    e.EmployeeNumber.Contains(searchTerm) ||
                    EF.Functions.Like(e.Person.BirthDate.ToString(), $"%{searchTerm}%") ||
                    EF.Functions.Like(e.EmployedDate.ToString(), $"%{searchTerm}%")
                );
            }

            var employeeList = await query.ToListAsync();

            _logger.LogInformation("Retrieved {count} employees", employeeList.Count);

            return _mapper.Map<List<EmployeePersonDTO>>(employeeList);
        }
    }
}
