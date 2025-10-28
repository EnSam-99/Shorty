using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;
using Npgsql;

namespace Shorty.Dal
{
	public class TestRepository
	{
		private readonly string _connectionString;
		private readonly ILogger<TestRepository> _logger;

		public TestRepository(ILogger<TestRepository> logger, IConfiguration configuration)
		{
			if (configuration == null) throw new ArgumentNullException(nameof(configuration));
			if (logger == null) throw new ArgumentNullException(nameof(logger));

			_connectionString = configuration.GetConnectionString("DefaultConnection")
								?? throw new InvalidOperationException("Connection string 'DefaultConnection' is not configured.");
			_logger = logger;
		}

		public async Task TestAsync(int i)
		{
			string createTableSql = @$"
                CREATE TABLE IF NOT EXISTS users_{i} (
                    id SERIAL PRIMARY KEY,
                    name VARCHAR(100) NOT NULL,
                    email VARCHAR(150) UNIQUE NOT NULL,
                    createdAt TIMESTAMP DEFAULT NOW()
                );";

			try
			{
				using var connection = new NpgsqlConnection(_connectionString);
				await connection.OpenAsync();

				using (var command = new NpgsqlCommand(createTableSql, connection))
				{
					await command.ExecuteNonQueryAsync();
					Console.WriteLine("Table checked/created successfully.");
				}
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "An error occurred while creating the table.");
			}
		}
	}
}
