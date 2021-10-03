using System.Data;

namespace DapperExample
{
    public interface IRepositoryHelper
    {
        IDbConnection GetConnection();
    }
}