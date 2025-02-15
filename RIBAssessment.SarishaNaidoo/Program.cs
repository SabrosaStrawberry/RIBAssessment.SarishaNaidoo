using Microsoft.EntityFrameworkCore;
using Database;
using Services.Interfaces;
using Services;
using Domain.Profiles;
using System.Reflection;
using Domain.Models;
using Microsoft.AspNetCore.Identity;
using NLog.Web;
using NLog;
using RIBAssessment.SarishaNaidoo.CustomExtenstionMiddleware;
using FluentValidation.AspNetCore;
using Domain.Validators;
using FluentValidation;
using RIBAssessment.SarishaNaidoo.ServiceExtensions;


var logger = NLog.LogManager.Setup().LoadConfigurationFromFile(string.Concat(Directory.GetCurrentDirectory(), "/nlog.config"));

var builder = WebApplication.CreateBuilder(args);

builder.Logging.ClearProviders();
builder.Host.UseNLog();
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();

builder.Services.AddDbContext<DatabaseContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));

});


//configure automapper
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());


//DI for services
builder.Services.AddApplicationServices();


builder.Services.AddControllers();

// Register validators from the assembly containing EmployeePersonDTOValidator
builder.Services.AddFluentValidationAutoValidation(); 
builder.Services.AddValidatorsFromAssemblyContaining<EmployeePersonDTOValidator>();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

////Configure Entity Providers for authentication
//builder.Services.AddIdentity<User, IdentityRole>()
//    .AddEntityFrameworkStores<DatabaseContext>();

var app = builder.Build();

//Custom Exception Handling - I chose this method as it is flexible and allows us to define comprehensive error handling strategies
app.UseExceptionHandler(opt => { });

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

//To allow requests from frontend
app.UseCors(policy =>
    policy.AllowAnyOrigin()
          .AllowAnyMethod()
          .AllowAnyHeader());



app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
