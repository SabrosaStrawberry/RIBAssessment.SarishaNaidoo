using System;
using System.Threading.Tasks;
using AutoMapper;
using Database;
using Domain.DTOs;
using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using Services;
using Services.Interfaces;
using Xunit;

namespace Services.Tests
{
    // A simple AutoMapper profile for testing purposes
    public class UpdateEmployeeTestProfile : Profile
    {
        public UpdateEmployeeTestProfile()
        {
            // Map from Employee to EmployeePersonDTO
            CreateMap<Employee, EmployeePersonDTO>()
                .ForMember(dest => dest.FirstName, opt => opt.MapFrom(src => src.Person.FirstName))
                .ForMember(dest => dest.LastName, opt => opt.MapFrom(src => src.Person.LastName))
                .ForMember(dest => dest.EmployeeNumber, opt => opt.MapFrom(src => src.EmployeeNumber))
                .ForMember(dest => dest.EmployedDate, opt => opt.MapFrom(src => src.EmployedDate))
                .ForMember(dest => dest.TerminatedDate, opt => opt.MapFrom(src => src.TerminatedDate))
                .ForMember(dest => dest.BirthDate, opt => opt.MapFrom(src => src.Person.BirthDate));
        }
    }

    public class UpdateEmployeeServiceTests : IDisposable
    {
        private readonly DatabaseContext _context;
        private readonly IMapper _mapper;
        private readonly IUpdateEmployeeService _updateEmployeeService;
        private readonly Mock<ILogger<UpdateEmployeeService>> _loggerMock;

        public UpdateEmployeeServiceTests()
        {
            // Set up an in-memory database
            var options = new DbContextOptionsBuilder<DatabaseContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;
            _context = new DatabaseContext(options);

            // Set up AutoMapper using our test profile
            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<UpdateEmployeeTestProfile>();
            });
            _mapper = config.CreateMapper();

            // Create a mock logger
            _loggerMock = new Mock<ILogger<UpdateEmployeeService>>();

            // Instantiate the service to test
            _updateEmployeeService = new UpdateEmployeeService(_context, _mapper);
        }

        public void Dispose()
        {
            _context.Dispose();
        }

        [Fact]
        public async Task UpdateEmployeeAsync_ShouldThrowException_WhenEmployeeNotFound()
        {
            // Arrange
            int nonExistentId = 999;
            var dto = new EmployeePersonDTO
            {
                FirstName = "Updated",
                LastName = "User",
                EmployeeNumber = "EMPUPDATED0000001",
                EmployedDate = DateTimeOffset.UtcNow,
                TerminatedDate = null,
                BirthDate = new DateTime(1990, 1, 1)
            };

            // Act & Assert
            var exception = await Assert.ThrowsAsync<Exception>(async () =>
                await _updateEmployeeService.UpdateEmployeeAsync(nonExistentId, dto));
            Assert.Contains($"Employee with ID {nonExistentId} not found", exception.Message);
        }

        [Fact]
        public async Task UpdateEmployeeAsync_ShouldUpdateEmployeeAndReturnUpdatedDTO()
        {
            // Arrange
            // Seed an employee into the in-memory database
            var person = new Person
            {
                PersonId = 1,
                FirstName = "John",
                LastName = "Doe",
                BirthDate = new DateTime(1980, 1, 1)
            };

            var employee = new Employee
            {
                EmployeeId = 1,
                PersonId = 1,
                EmployeeNumber = "EMP0000000000001",
                EmployedDate = DateTimeOffset.UtcNow.AddYears(-1),
                TerminatedDate = null,
                Person = person
            };

            await _context.Persons.AddAsync(person);
            await _context.Employees.AddAsync(employee);
            await _context.SaveChangesAsync();

            // Create a DTO with updated information
            var updateDto = new EmployeePersonDTO
            {
                FirstName = "Jane",           // update first name
                LastName = "Smith",           // update last name
                EmployeeNumber = "EMP0000000000002", // update employee number
                EmployedDate = DateTimeOffset.UtcNow.AddYears(-2), // update employed date
                TerminatedDate = DateTimeOffset.UtcNow,             // set terminated date
                BirthDate = new DateTime(1990, 2, 2)                // update birth date
            };

            // Act
            var result = await _updateEmployeeService.UpdateEmployeeAsync(employee.EmployeeId, updateDto);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Jane", result.FirstName);
            Assert.Equal("Smith", result.LastName);
            Assert.Equal("EMP0000000000002", result.EmployeeNumber);
            Assert.Equal(updateDto.EmployedDate, result.EmployedDate);
            Assert.Equal(updateDto.TerminatedDate, result.TerminatedDate);
            Assert.Equal(updateDto.BirthDate, result.BirthDate);

            // Also verify that the database has the updated values
            var updatedEmployee = await _context.Employees.Include(e => e.Person)
                .FirstOrDefaultAsync(e => e.EmployeeId == employee.EmployeeId);
            Assert.NotNull(updatedEmployee);
            Assert.Equal("Jane", updatedEmployee.Person.FirstName);
            Assert.Equal("Smith", updatedEmployee.Person.LastName);
            Assert.Equal("EMP0000000000002", updatedEmployee.EmployeeNumber);
        }
    }
}
