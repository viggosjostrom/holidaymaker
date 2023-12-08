using Npgsql;

string dbUri= "Host=localhost;Port=5455;Username=postgres;Password=postgres;Database=holidaymaker"; //Inloggning till databasen port, password osv

await using var db = NpgsqlDataSource.Create(dbUri);

await using var cmd = db.CreateCommand(@"CREATE TABLE IF NOT EXISTS resort(
	id SERIAL PRIMARY KEY,
	name VARCHAR (255),
	room_id INT,
	city VARCHAR(255),
	dist_beach INT,
	dist_centrum INT,
	pool BOOL,
	night_entertainment BOOL,
	child_club BOOL,
	resturant BOOL,
	stars INT
	);");
