using AutoMapper;
using Database;
using Microsoft.EntityFrameworkCore;
using Services.Interfaces;

namespace Services
{
    public class DeleteEmployee : IDeleteEmployee
    {

        private readonly DatabaseContext _dbcontext;
        private readonly IMapper _mapper;
        //TODO: Add logger
        public DeleteEmployee(DatabaseContext dbContext,
            IMapper mapper)
        {
            _dbcontext = dbContext;
            _mapper = mapper;

        }

        public async Task<bool> DeleteEmployeeAsync(int Id)
        {
            var employee = await _dbcontext.Employees.Where(e=>e.EmployeeId == Id).FirstOrDefaultAsync();

            _dbcontext.Employees.Remove(employee);
            await _dbcontext.SaveChangesAsync();

            return true;
        }

    }
}
