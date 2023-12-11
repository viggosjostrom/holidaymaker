using holidaymaker;
using Npgsql;

string dbUri = "Host=localhost;Port=5455;Username=postgres;Password=postgres;Database=holidaymaker"; //Inloggning till databasen port, password osv

await using var db = NpgsqlDataSource.Create(dbUri);

await SetupDB.NewDB();