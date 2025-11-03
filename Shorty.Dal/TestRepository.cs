using Microsoft.Extensions.Logging;
using Npgsql;

namespace Shorty.Dal
{
    public class TestRepository(ILogger<TestRepository> logger)
    {
        public async Task TestAsync(int i)
        {
            string connectionString = "Host=ep-patient-morning-agkih675-pooler.c-2.eu-central-1.aws.neon.tech;" +
                          "Username=neondb_owner;" +
                          "Password=npg_5jmasY6vLIRC;" +
                          "Database=mherdb;" +
                          "SSL Mode=Disable"; //+
                          //"Trust Server Certificate=true;";

            string createTableSql = @$"
            CREATE TABLE IF NOT EXISTS users_{i} (
                id SERIAL PRIMARY KEY,
                name VARCHAR(100) NOT NULL,
                email VARCHAR(150) UNIQUE NOT NULL,
                created_at TIMESTAMP DEFAULT NOW()
            );";

            try
            {
                using var connection = new NpgsqlConnection(connectionString);
                await connection.OpenAsync();

                using (var command = new NpgsqlCommand(createTableSql, connection))
                {
                    await command.ExecuteNonQueryAsync();
                    Console.WriteLine("Table checked/created successfully.");
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred while creating the table.");
            }
        }
    }
}
