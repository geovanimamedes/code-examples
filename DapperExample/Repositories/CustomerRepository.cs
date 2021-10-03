using Dapper;
using DapperExample.Borders.Entities;
using DapperExample.Borders.Repositories;
using DapperExample.Repositories.SqlStatements;
using DapperExample.Shared.Configurations;
using System;
using System.Linq;
using System.Data;
using System.Threading.Tasks;
using System.Transactions;

namespace DapperExample.Repositories
{
    public class CustomerRepository : ICustomerRepository
    {
        private readonly IRepositoryHelper helper;
        private readonly ApplicationConfig config;

        public CustomerRepository(IRepositoryHelper helper, ApplicationConfig config)
        {
            this.helper = helper;
            this.config = config;
        }

        public async Task Create(Customer customer)
        {
            const string sql = CustomerStatements.CREATE_CUSTOMER;
            
            var param = new DynamicParameters();
            param.Add("@Id", customer.Id, DbType.Guid);
            param.Add("@Name", customer.Name, DbType.String);

            using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                using (var connection = helper.GetConnection())
                {
                    await connection.QueryAsync(sql, param);
                }

                scope.Complete();
            }
        }

        public Customer Get(Guid id)
        {
            var query = @"SELECT name
                          FROM customer
                          WHERE id = @id";
            
            using (var connection = helper.GetConnection())
            {
                return connection.QueryFirstOrDefault<Customer>(query, new { id });
            }
        }

        public decimal GetValue(int id)
        {

            using (var connection = helper.GetConnection())
            {
                return connection.Query<decimal>("Banco..GSISP_Procedure",
                new
                {
                    id
                },
                commandType: CommandType.StoredProcedure).First();
            }
        }
    }
}