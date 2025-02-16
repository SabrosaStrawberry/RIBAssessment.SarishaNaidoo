using Microsoft.Extensions.DependencyInjection;
using Services.Interfaces;
using Services;

namespace RIBAssessment.SarishaNaidoo.ServiceExtensions
{
    public static class ServiceExtensions
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<IGetEmployeeService, GetEmployeeService>();
            services.AddScoped<ICreateEmployeeService, CreateEmployeeService>();
            services.AddScoped<IUpdateEmployeeService, UpdateEmployeeService>();
            services.AddScoped<IDeleteEmployeeService, DeleteEmployeeService>();

            return services;
        }
    }
}