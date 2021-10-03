using DapperExample.Borders.Repositories;
using DapperExample.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace DapperExample.Configurations
{
    public static class RepositoryConfig
    {
        public static void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<IRepositoryHelper, RepositoryHelper>();
            services.AddSingleton<ICustomerRepository, CustomerRepository>();
        }
    }
}