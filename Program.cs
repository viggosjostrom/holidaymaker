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
	
INSERT INTO customer (firstname, lastname, email, phone, date_of_birth) 
VALUES 
('Neron', 'Hrachovec', 'nhrachovec0@miitbeian.gov.cn', '+62 637 330 7995', '1993-06-03'),
('Aila', 'Willwood', 'awillwood1@plala.or.jp', '+51 544 970 1128', '2002-12-19'),
('Hadleigh', 'Torricina', 'htorricina2@wisc.edu', '+420 517 729 7176', '1985-07-15'),
('Roseann', 'Guswell', 'rguswell3@diigo.com', '+506 518 190 9718', '1990-05-24'),
('Octavius', 'Reade', 'oreade4@reference.com', '+86 661 234 9822', '1999-04-23'),
('Juliane', 'McIvor', 'jmcivor5@cloudflare.com', '+30 891 587 5176', '1994-09-20'),
('Gillie', 'Stihl', 'gstihl6@myspace.com', '+62 426 215 1345', '1990-08-28'),
('Pinchas', 'Cicero', 'pcicero7@unc.edu', '+880 573 592 5332', '1982-07-08'),
('Milt', 'Bilfoot', 'mbilfoot8@biblegateway.com', '+62 208 806 1256', '1986-01-30'),
('Patrick', 'Russ', 'pruss9@hud.gov', '+62 459 640 4100', '2002-05-09'),
('Kenneth', 'Warfield', 'kwarfielda@yale.edu', '+251 253 958 2376', '1981-12-17'),
('Jennette', 'Dundon', 'jdundonb@ning.com', '+351 543 107 9406', '1988-06-26'),
('Haydon', 'Laight', 'hlaightc@disqus.com', '+53 341 775 1404', '1982-12-22'),
('Charity', 'Kinnett', 'ckinnettd@g.co', '+62 153 697 1063', '1982-11-27'),
('Briggs', 'Bees', 'bbeese@biglobe.ne.jp', '+242 998 130 2896', '2003-07-02'),
('Rupert', 'Bancroft', 'rbancroftf@comcast.net', '+63 578 183 0290', '1993-11-12'),
('Vidovic', 'Gawkes', 'vgawkesg@addthis.com', '+7 299 837 5147', '1987-05-15'),
('Ariana', 'Klimaszewski', 'aklimaszewskih@cornell.edu', '+86 820 272 7993', '2002-12-24'),
('Kimbell', 'Yaxley', 'kyaxleyi@state.gov', '+373 793 238 0425', '1993-09-10'),
('Harwell', 'Ilchenko', 'hilchenkoj@goo.ne.jp', '+886 971 717 3898', '1988-10-03');


/*
INSERT INTO public.resort(
name, city, dist_beach, dist_centrum, pool, night_entertainment, child_club, resturant, stars)
	VALUES
	('Elite Hotels', 'Malmö', 250, 1000, '1', '1', '0', '1', 4),
	('Hilton Hotels', 'Malmö', 500, 750, '1', '1', '1', '1', 5),
	('Scandic Hotels', 'Helsingborg', 100, 400, '0', '0', '1', '1', 3),
	('First Hotels', 'Åhus', 100, 1250, '0', '0', '0', '1', 3),
	('Bjärnum Motel', 'Bjärnum', 50000, 0, '0', '0', '0', '1', 1);

*/

");
{
    await cmd.ExecuteNonQueryAsync();
}
