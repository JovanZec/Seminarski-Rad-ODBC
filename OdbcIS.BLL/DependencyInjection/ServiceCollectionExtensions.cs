using Microsoft.Extensions.DependencyInjection;
using OdbcIS.BLL.Services;
using OdbcIS.DAL.Data;
using OdbcIS.DAL.Repositories;

namespace OdbcIS.BLL.DependencyInjection;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddOdbcApplication(this IServiceCollection services, string connectionString)
    {
        services.AddSingleton(new OdbcConnectionFactory(connectionString));
        services.AddScoped<IStudentRepository, StudentRepository>();
        services.AddScoped<IStudentService, StudentService>();
        return services;
    }
}
