
namespace DapperExample.Shared.Configurations
{
    public class ApplicationConfig
    {
        public DatabaseConfig Database { get; set; }

        public ApplicationConfig()
        {
            Database = new DatabaseConfig();
        }
    }

    public class DatabaseConfig
    {
        public string ConnectionString { get; set; }
        public string AssemblyName { get; set; }
        public string DbFactoryName { get; set; }
    }
}