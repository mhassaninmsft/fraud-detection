using CreditCardSimulator.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Npgsql;

namespace CreditCardSimulator.Database
{
    public class Functions
    {
        internal const string AddTwoName = "add_two";

        private readonly MagicBankContext context;
        private readonly Config.Database dbConfig;
        private readonly NpgsqlDataSource dataSource;
        public Functions(MagicBankContext context, IOptions<Config.Database> dbConfig)
        {
            this.context = context;
            this.dbConfig = dbConfig.Value;
            dataSource = NpgsqlDataSource.Create(this.dbConfig.ConnectionString);
        }

        public async Task<int> AddTwo(int a, int b)
        {
            var result = await context.Database.ExecuteSqlRawAsync(@$"SELECT {AddTwoName}(3,4);");
            // Gets the number of rows affected
            Console.WriteLine($"db result is {result}");
            await using var command = dataSource.CreateCommand("SELECT * FROM pos_machine;");
            await using var reader = await command.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                Console.WriteLine(reader["id"] as Guid?);
                Console.WriteLine(reader["zip_code"] as string);
            }

            await AddTwo2(a, b);

            return a + b;
        }
        private async Task<int> AddTwo2(int a, int b)
        {

            await using var command = new NpgsqlCommand(@$"SELECT {AddTwoName}($1, $2)", dataSource.OpenConnection())
            {
                Parameters =
    {
        new() { Value = a },
        new() { Value = b }
    }
            };
            await using var reader = await command.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                // the stored procedure column return is the name of the stored procedure
                Console.WriteLine(reader[AddTwoName] as int?);
                //Console.WriteLine(reader["zip_code"] as string);
            }
            return 5;
        }
        //private static BoardGame ReadBoardGame(NpgsqlDataReader reader)
        //{
        //    int? id = reader["id"] as int?;
        //    string name = reader["name"] as string;
        //    short? minPlayers = reader["minplayers"] as Int16?;
        //    short? maxPlayers = reader["maxplayers"] as Int16?;
        //    short? averageDuration = reader["averageduration"] as Int16?;

        //    BoardGame game = new BoardGame
        //    {
        //        Id = id.Value,
        //        Name = name,
        //        MinPlayers = minPlayers.Value,
        //        MaxPlayers = maxPlayers.Value,
        //        AverageDuration = averageDuration.Value
        //    };
        //    return game;
        //}
    }
}
