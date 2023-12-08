using Npgsql;

string dbUri = "Host=localhost;Port=5455;Username=postgres;Password=postgres;Database=holidaymaker"; //Inloggning till databasen port, password osv

await using var db = NpgsqlDataSource.Create(dbUri);




await using var cmd = db.CreateCommand(
    @"CREATE TABLE IF NOT EXISTS resort(
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
	);

	CREATE TABLE IF NOT EXISTS customer(
	id SERIAL PRIMARY KEY,
	firstname VARCHAR (255),
	lastname VARCHAR (255),
	email VARCHAR (255),
	phone VARCHAR (50),
	date_of_birth DATE
	);

	CREATE TABLE IF NOT EXISTS room(
	id SERIAL PRIMARY KEY,
	sqm INT,
	price INT
	);

	CREATE TABLE IF NOT EXISTS booking(
	id SERIAL PRIMARY KEY,
	room_id INT,
	customer_id INT,
	in_date DATE,
	out_date DATE,
	extra_bed BOOL,
	all_inclusive BOOL,
	half_pension BOOL
	);
	




");
{
    await cmd.ExecuteNonQueryAsync();
}

