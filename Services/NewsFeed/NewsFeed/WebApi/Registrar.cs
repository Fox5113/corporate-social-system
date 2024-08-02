using BusinessLogic.Abstractions;
using BusinessLogic.Services;
using BusinessLogic.Services.Abstractions;
using DataAccess.EntityFramework;
using DataAccess.Repositories;
using DataAccess.Repositories.Abstractions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using WebApi.Settings;

namespace WebApi
{
    /// <summary>
    /// Регистратор сервиса
    /// </summary>
    public static class Registrar
    {
        public static IServiceCollection AddServices(this IServiceCollection services, IConfiguration configuration)
        {
            var applicationSettings = configuration.Get<ApplicationSettings>();
            services.AddSingleton(applicationSettings);
            return services.AddSingleton((IConfigurationRoot)configuration)
                .InstallServices()
                .ConfigureContext(applicationSettings.ConnectionString)
                .InstallRepositories();
        }

        private static IServiceCollection InstallServices(this IServiceCollection serviceCollection)
        {
            serviceCollection
            .AddTransient<INewsService, NewsService>()
            .AddTransient<INewsCommentService, NewsCommentService>()
            .AddTransient<IHashtagNewsService, HashtagNewsService>()
            .AddTransient<IHashtagService, HashtagService>()
            .AddTransient<IEmployeeService, EmployeeService>()
            .AddTransient<IBaseService, BaseService>();
            return serviceCollection;
        }

        private static IServiceCollection InstallRepositories(this IServiceCollection serviceCollection)
        {
            serviceCollection
                .AddTransient<INewsRepository, NewsRepository>()
                .AddTransient<INewsCommentRepository, NewsCommentRepository>()
                .AddTransient<IHashtagNewsRepository, HashtagNewsRepository>()
                .AddTransient<IHashtagRepository, HashtagRepository>()
                .AddTransient<IEmployeeRepository, EmployeeRepository>()
                .AddTransient<IBaseRepository, BaseRepository>();
            return serviceCollection;
        }
    }
}
