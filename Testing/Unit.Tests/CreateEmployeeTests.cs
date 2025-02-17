using System;
using System.Threading.Tasks;
using AutoMapper;
using Database;
using Domain.DTOs;
using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Services;
using Services.Interfaces;
using Xunit;

namespace Services.Tests
{
    // Define a test mapping profile for AutoMapper
    public class TestMappingProfile : Profile
    {
        public TestMappingProfile()
        {
            // Adjust the mappings to match your actual classes
            CreateMap<EmployeePersonDTO, Person>()
                .ForMember(dest => dest.FirstName, opt => opt.MapFrom(src => src.FirstName))
                .ForMember(dest => dest.LastName, opt => opt.MapFrom(src => src.LastName));
            CreateMap<EmployeePersonDTO, Employee>()
                .ForMember(dest => dest.EmployeeNumber, opt => opt.MapFrom(src => src.EmployeeNumber))
                .ForMember(dest => dest.EmployedDate, opt => opt.MapFrom(src => src.EmployedDate))
                .ForMember(dest => dest.TerminatedDate, opt => opt.MapFrom(src => src.TerminatedDate));
            CreateMap<Employee, EmployeePersonDTO>()
                .ForMember(dest => dest.FirstName, opt => opt.MapFrom(src => src.Person.FirstName))
                .ForMember(dest => dest.LastName, opt => opt.MapFrom(src => src.Person.LastName))
                .ForMember(dest => dest.EmployeeNumber, opt => opt.MapFrom(src => src.EmployeeNumber))
                .ForMember(dest => dest.EmployedDate, opt => opt.MapFrom(src => src.EmployedDate))
                .ForMember(dest => dest.TerminatedDate, opt => opt.MapFrom(src => src.TerminatedDate));
        }
    }

    public class CreateEmployeeServiceTests : IDisposable
    {
        private readonly DatabaseContext _context;
        private readonly IMapper _mapper;
        private readonly IConfiguration _configuration;
        private readonly ICreateEmployeeService _employeeService;

        public CreateEmployeeServiceTests()
        {
            // Set up in-memory database
            var options = new DbContextOptionsBuilder<DatabaseContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;
            _context = new DatabaseContext(options);

            // Set up in-memory configuration for JWT settings if needed.
            var inMemorySettings = new System.Collections.Generic.Dictionary<string, string>
            {
                { "JwtSettings:SecretKey", "TestSecretKeyForMappingPurposesOnly12345" },
                { "JwtSettings:Issuer", "TestIssuer" },
                { "JwtSettings:Audience", "TestAudience" }
            };
            _configuration = new ConfigurationBuilder()
                .AddInMemoryCollection(inMemorySettings)
                .Build();

            // Configure AutoMapper with test mapping profile
            var mapperConfig = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new TestMappingProfile());
            });
            _mapper = mapperConfig.CreateMapper();

            // Instantiate the CreateEmployeeService
            _employeeService = new CreateEmployeeService(_context, _mapper);
        }

        public void Dispose()
        {
            _context.Dispose();
        }

        [Fact]
        public async Task CreateEmployee_ShouldAddPersonAndEmployeeToDatabase()
        {
            // Arrange: Create a sample EmployeePersonDTO with test data.
            var dto = new EmployeePersonDTO
            {
                FirstName = "John",
                LastName = "Doe",
                EmployeeNumber = "EMP1234567890123", // 16 characters if required
                EmployedDate = DateTimeOffset.UtcNow,
                TerminatedDate = null
            };

            // Act: Call CreateEmployee to add a new employee
            var result = await _employeeService.CreateEmployee(dto);

            // Assert: Verify the returned DTO is not null and has expected values.
            Assert.NotNull(result);
            Assert.Equal(dto.FirstName, result.FirstName);
            Assert.Equal(dto.LastName, result.LastName);
            Assert.Equal(dto.EmployeeNumber, result.EmployeeNumber);

            // Assert: Verify that the context contains one Person and one Employee.
            var personCount = await _context.Persons.CountAsync();
            var employeeCount = await _context.Employees.CountAsync();
            Assert.Equal(1, personCount);
            Assert.Equal(1, employeeCount);
        }

        // Additional tests (e.g., handling invalid input) can be added here.
    }
}
