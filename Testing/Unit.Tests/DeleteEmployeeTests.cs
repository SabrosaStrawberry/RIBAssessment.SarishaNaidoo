using System;
using System.Threading.Tasks;
using AutoMapper;
using Database;
using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Services;
using Services.Interfaces;
using Xunit;

namespace Services.Tests
{
    public class DeleteEmployeeServiceTests : IDisposable
    {
        private readonly DatabaseContext _context;
        private readonly IMapper _mapper;
        private readonly IDeleteEmployeeService _deleteEmployeeService;

        public DeleteEmployeeServiceTests()
        {
            // Set up in-memory database with a unique name for isolation
            var options = new DbContextOptionsBuilder<DatabaseContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;
            _context = new DatabaseContext(options);

            // Set up a dummy AutoMapper configuration (if mapping is needed later)
            var config = new MapperConfiguration(cfg =>
            {
                // Define mappings if needed
            });
            _mapper = config.CreateMapper();

            // Instantiate the DeleteEmployeeService
            _deleteEmployeeService = new DeleteEmployeeService(_context, _mapper);
        }

        public void Dispose()
        {
            _context.Dispose();
        }

        [Fact]
        public async Task DeleteEmployeeAsync_ReturnsFalse_WhenEmployeeDoesNotExist()
        {
            // Arrange
            int nonExistentId = 999;

            // Act
            var result = await _deleteEmployeeService.DeleteEmployeeAsync(nonExistentId);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public async Task DeleteEmployeeAsync_ReturnsTrue_And_RemovesEmployee_WhenEmployeeExists()
        {
            // Arrange: Add an employee record to the in-memory database.
            var employee = new Employee
            {
                EmployeeId = 1,
                PersonId = 1,
                EmployeeNumber = "EMP0000000000001",
                EmployedDate = DateTimeOffset.UtcNow,
                TerminatedDate = null
            };

            _context.Employees.Add(employee);
            await _context.SaveChangesAsync();

            // Act: Attempt to delete the existing employee.
            var result = await _deleteEmployeeService.DeleteEmployeeAsync(employee.EmployeeId);

            // Assert: The method should return true and the employee should be removed.
            Assert.True(result);
            var deletedEmployee = await _context.Employees.FirstOrDefaultAsync(e => e.EmployeeId == employee.EmployeeId);
            Assert.Null(deletedEmployee);
        }
    }
}
