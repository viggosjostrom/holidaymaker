using Npgsql;

string dbUri = "Host=localhost;Port=5455;Username=postgres;Password=postgres;Database=holidaymaker"; //Inloggning till databasen port, password osv

await using var db = NpgsqlDataSource.Create(dbUri);


await using var cmd = db.CreateCommand(@"

    

CREATE TABLE IF NOT EXISTS resort(
	id SERIAL PRIMARY KEY,
	name VARCHAR (255),
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
	id VARCHAR(50) PRIMARY KEY,
	resort_id INT,
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
	

INSERT INTO public.resort(
name, city, dist_beach, dist_centrum, pool, night_entertainment, child_club, resturant, stars)
	VALUES
	('Elite Hotels', 'Malmö', 250, 1000, '1', '1', '0', '1', 4),
	('Hilton Hotels', 'Malmö', 500, 750, '1', '1', '1', '1', 5),
	('Scandic Hotels', 'Helsingborg', 100, 400, '0', '0', '1', '1', 3),
	('First Hotels', 'Åhus', 100, 1250, '0', '0', '0', '1', 3),
	('Bjärnum Motel', 'Bjärnum', 50000, 0, '0', '0', '0', '1', 1);



");
{
    await cmd.ExecuteNonQueryAsync();
}
