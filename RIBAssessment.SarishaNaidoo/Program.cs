using Microsoft.EntityFrameworkCore;
using Database;
using Services.Interfaces;
using Services;
using Domain.Profiles;
using System.Reflection;
using Domain.Models;
using Microsoft.AspNetCore.Identity;


var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<DatabaseContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));

});

//configure automapper
//builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
//builder.Services.AddAutoMapper(Assembly.GetExecutingAssembly());
builder.Services.AddAutoMapper(typeof(EmployeeProfile));

//TODO: Separate into independent file if application is large
builder.Services.AddScoped<IGetEmployees, GetEmployees>();
builder.Services.AddScoped<ICreateOrEditEmployee, CreateorEditEmployee>();
builder.Services.AddScoped<IUpdateEmployee, UpdateEmployee>();
builder.Services.AddScoped<IDeleteEmployee, DeleteEmployee>();
// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

////Configure Entity Providers for authentication
//builder.Services.AddIdentity<User, IdentityRole>()
//    .AddEntityFrameworkStores<DatabaseContext>();

var app = builder.Build();

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

