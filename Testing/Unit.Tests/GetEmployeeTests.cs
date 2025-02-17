using AutoMapper;
using Database;
using Domain.DTOs;
using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using Services.Interfaces;
using Xunit;

namespace Services.Tests
{
    public class GetEmployeeServiceTests : IDisposable
    {
        private readonly DatabaseContext _context;
        private readonly IMapper _mapper;
        private readonly ILogger<GetEmployeeService> _logger;
        private readonly IGetEmployeeService _getEmployeeService;

        public GetEmployeeServiceTests()
        {
            // Setup in-memory database with a unique name for isolation
            var options = new DbContextOptionsBuilder<DatabaseContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;
            _context = new DatabaseContext(options);

            // Setup AutoMapper configuration with a test mapping profile
            var mapperConfig = new MapperConfiguration(cfg =>
            {
                // Adjust mappings as needed. Here we assume EmployeePersonDTO has properties for
                // FirstName, LastName, EmployeeNumber, EmployedDate, and TerminatedDate.
                cfg.CreateMap<Employee, EmployeePersonDTO>()
                    .ForMember(dest => dest.FirstName, opt => opt.MapFrom(src => src.Person.FirstName))
                    .ForMember(dest => dest.LastName, opt => opt.MapFrom(src => src.Person.LastName))
                    .ForMember(dest => dest.EmployeeNumber, opt => opt.MapFrom(src => src.EmployeeNumber))
                    .ForMember(dest => dest.EmployedDate, opt => opt.MapFrom(src => src.EmployedDate))
                    .ForMember(dest => dest.TerminatedDate, opt => opt.MapFrom(src => src.TerminatedDate));
            });
            _mapper = mapperConfig.CreateMapper();

            // Setup a mock logger
            var mockLogger = new Mock<ILogger<GetEmployeeService>>();
            _logger = mockLogger.Object;

            // Instantiate the GetEmployeeService
            _getEmployeeService = new GetEmployeeService(_context, _mapper, _logger);
        }

        public void Dispose()
        {
            _context.Dispose();
        }

        [Fact]
        public async Task GetEmployeeList_NoSearchTerm_ReturnsAllEmployees()
        {
            // Arrange
            await SeedEmployeesAsync();

            // Act
            var result = await _getEmployeeService.GetEmployeeList(null);

            // Assert
            Assert.NotNull(result);
            // Assuming we seeded 3 employees
            Assert.Equal(3, result.Count);
        }

        [Theory]
        [InlineData("John", 1)]
        [InlineData("Doe", 2)]
        [InlineData("EMP000000000000", 3)]
        [InlineData("NonExistent", 0)]
        public async Task GetEmployeeList_WithSearchTerm_ReturnsFilteredEmployees(string searchTerm, int expectedCount)
        {
            // Arrange
            await SeedEmployeesAsync();

            // Act
            var result = await _getEmployeeService.GetEmployeeList(searchTerm);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(expectedCount, result.Count);
        }

        private async Task SeedEmployeesAsync()
        {
            // Clear existing data
            _context.Employees.RemoveRange(_context.Employees);
            _context.Persons.RemoveRange(_context.Persons);
            await _context.SaveChangesAsync();

            // Create sample employees with Persons
            var employees = new List<Employee>
            {
                new Employee
                {
                    EmployeeId = 1,
                    EmployeeNumber = "EMP0000000000001",
                    EmployedDate = DateTimeOffset.UtcNow,
                    TerminatedDate = null,
                    Person = new Person { FirstName = "John", LastName = "Doe", BirthDate = new DateTime(1990, 1, 1) }
                },
                new Employee
                {
                    EmployeeId = 2,
                    EmployeeNumber = "EMP0000000000002",
                    EmployedDate = DateTimeOffset.UtcNow,
                    TerminatedDate = null,
                    Person = new Person { FirstName = "Jane", LastName = "Doe", BirthDate = new DateTime(1992, 2, 2) }
                },
                new Employee
                {
                    EmployeeId = 3,
                    EmployeeNumber = "EMP0000000000003",
                    EmployedDate = DateTimeOffset.UtcNow,
                    TerminatedDate = null,
                    Person = new Person { FirstName = "Alice", LastName = "Smith", BirthDate = new DateTime(1988, 3, 3) }
                }
            };

            // Add Persons and Employees
            await _context.Persons.AddRangeAsync(new List<Person>
            {
                employees[0].Person,
                employees[1].Person,
                employees[2].Person
            });
            await _context.Employees.AddRangeAsync(employees);
            await _context.SaveChangesAsync();
        }
    }
}
