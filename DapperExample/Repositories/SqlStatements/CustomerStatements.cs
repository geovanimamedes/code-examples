
namespace DapperExample.Repositories.SqlStatements
{
    public static class CustomerStatements
    {
		public const string CREATE_CUSTOMER = @"
			INSERT INTO customer (
				id,
				name
			)
			VALUES (
				@Id,
				@Name
			)
		";
	}
}