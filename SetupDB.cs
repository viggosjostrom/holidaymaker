using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace holidaymaker;

public class SetupDB
{
    public static async Task NewDB()
    {



        string dbUri = "Host=localhost;Port=5455;Username=postgres;Password=postgres;Database=holidaymaker"; //Inloggning till databasen port, password osv

        await using var db = NpgsqlDataSource.Create(dbUri);


        await using var cmd = db.CreateCommand(@"

    

	CREATE TABLE IF NOT EXISTS resort(
	id SERIAL PRIMARY KEY,
	name TEXT,
	city TEXT,
	dist_beach INT,
	dist_centrum INT,
	stars INT
	);

	CREATE TABLE IF NOT EXISTS customer(
	id SERIAL PRIMARY KEY,
	firstname TEXT,
	lastname TEXT,
	email TEXT,
	phone TEXT,
	date_of_birth DATE
	);

	CREATE TABLE IF NOT EXISTS room(
	id INT PRIMARY KEY,
	name TEXT NOT NULL,
	resort_id INT,
	sqm INT,
	price MONEY
	);

	CREATE TABLE IF NOT EXISTS booking(
	id SERIAL PRIMARY KEY,
	resort_id INT,
	room_id INT,
	customer_id INT,
	in_date DATE,
	out_date DATE
	);


	CREATE TABLE IF NOT EXISTS amenities(
	id SERIAL PRIMARY KEY,
	name TEXT
	);

	CREATE TABLE IF NOT EXISTS extras(
	id SERIAL PRIMARY KEY,	
	name TEXT,
	price MONEY
	);

	CREATE TABLE IF NOT EXISTS resort_x_amenities(
	id SERIAL PRIMARY KEY,
	resort_id SERIAL REFERENCES resort(id),
	amenities_id SERIAL REFERENCES amenities(id)
	);

	CREATE TABLE IF NOT EXISTS resort_x_extras(
	id SERIAL PRIMARY KEY,
	resort_id SERIAL REFERENCES resort(id),
	extras_id SERIAL REFERENCES extras(id)	
	);

	CREATE TABLE IF NOT EXISTS booking_x_extras(
	id SERIAL PRIMARY KEY,
	booking_id SERIAL REFERENCES booking(id),
	extras_id SERIAL REFERENCES extras(id)	
	);


/*

------------------------------ta bort enkel citat-tecken. och lägga till customerID
INSERT INTO public.booking (resort_id, room_id, customer_id, in_date, out_date)
VALUES
(5, 5, 1, '2022-06-05', '2022-06-06'),
(3, 5, 2, '2022-06-10', '2022-06-13'),
(2, 3, 3, '2022-06-15', '2022-06-18'),
(3, 2, 4, '2022-06-20', '2022-06-22'),
(1, 4, 5, '2022-06-25', '2022-06-27'),
(1, 1, 6, '2022-06-30', '2022-07-02'),
(4, 2, 7, '2022-07-05', '2022-07-08'),
(3, 6, 8, '2022-07-10', '2022-07-13'),
(1, 2, 9, '2022-07-15', '2022-07-17'),
(1, 3, 10, '2022-07-20', '2022-07-24'),
(5, 2, 11, '2022-06-01', '2022-06-02'),
(5, 3, 12, '2022-06-06', '2022-06-08'),
(1, 5, 13, '2022-06-11', '2022-06-14'),
(3, 1, 14, '2022-06-16', '2022-06-19'),
(5, 6, 15, '2022-06-21', '2022-06-23'),
(4, 4, 16, '2022-06-26', '2022-06-29'),
(1, 6, 17, '2022-07-01', '2022-07-04'),
(2, 4, 18, '2022-07-06', '2022-07-09'),
(3, 4, 19, '2022-07-11', '2022-07-14'),
(4, 3, 20, '2022-07-16', '2022-07-19'),
(5, 4, 5, '2022-06-02', '2022-06-03'),
(5, 1, 8, '2022-06-07', '2022-06-09'),
(2, 6, 7, '2022-06-12', '2022-06-15'),
(2, 2, 1, '2022-06-17', '2022-06-20'),
(2, 1, 12, '2022-06-22', '2022-06-25'),
(4, 1, 7, '2022-06-27', '2022-06-30'),
(4, 6, 19, '2022-07-02', '2022-07-05'),
(1, 4, 14, '2022-07-07', '2022-07-10'),
(3, 3, 16, '2022-07-12', '2022-07-15'),
(3, 1, 2, '2022-07-17', '2022-07-20'),
(2, 3, 3, '2022-07-25', '2022-07-28'),
(3, 4, 9, '2022-07-30', '2022-08-01'),
(4, 5, 17, '2022-07-05', '2022-07-08'),
(1, 3, 11, '2022-06-05', '2022-06-06'),
(5, 6, 12, '2022-06-10', '2022-06-13'),
(1, 1, 10, '2022-06-15', '2022-06-18'),
(1, 5, 5, '2022-06-20', '2022-06-22'),
(5, 3, 15, '2022-06-25', '2022-06-27'),
(4, 4, 17, '2022-06-30', '2022-07-02'),
(2, 6, 20, '2022-07-05', '2022-07-08'),
(3, 6, 19, '2022-07-10', '2022-07-13'),
(5, 2, 17, '2022-07-15', '2022-07-17'),
(4, 5, 15, '2022-07-20', '2022-07-24'),
(1, 2, 13, '2022-06-01', '2022-06-02'),
(1, 6, 11, '2022-06-06', '2022-06-08'),
(2, 1, 10, '2022-06-11', '2022-06-14'),
(5, 4, 9, '2022-06-16', '2022-06-19'),
(5, 5, 7, '2022-06-21', '2022-06-23'),
(1, 2, 5, '2022-06-26', '2022-06-29'),
(4, 2, 3, '2022-07-01', '2022-07-04'),
(2, 6, 1, '2022-07-06', '2022-07-09'),
(2, 4, 18, '2022-07-11', '2022-07-14'),
(5, 2, 16, '2022-07-16', '2022-07-19'),
(1, 6, 14, '2022-06-03', '2022-06-04'),
(3, 2, 12, '2022-06-08', '2022-06-11'),
(5, 5, 10, '2022-06-13', '2022-06-16'),
(1, 1, 8, '2022-06-18', '2022-06-21'),
(1, 5, 6, '2022-06-24', '2022-06-27'),
(4, 6, 4, '2022-06-28', '2022-07-01'),
(5, 3, 2, '2022-07-03', '2022-07-06'),
(1, 1, 1, '2022-07-08', '2022-07-11'),
(4, 4, 12, '2022-07-13', '2022-07-16'),
(3, 1, 15, '2022-07-18', '2022-07-21'),
(4, 6, 5, '2022-06-04', '2022-06-05'),
(5, 6, 2, '2022-06-09', '2022-06-12'),
(1, 4, 20, '2022-06-14', '2022-06-17'),
(3, 3, 19, '2022-06-19', '2022-06-22'),
(3, 4, 18, '2022-06-24', '2022-06-27'),
(4, 6, 13, '2022-06-29', '2022-07-02'),
(4, 4, 16, '2022-07-04', '2022-07-07'),
(1, 1, 1, '2022-07-09', '2022-07-12'),
(3, 5, 11, '2022-07-15', '2022-07-17'),
(3, 2, 8, '2022-07-20', '2022-07-23'),
(4, 2, 17, '2022-07-25', '2022-07-28'),
(2, 6, 2, '2022-07-30', '2022-08-01'),
(3, 5, 1, '2022-07-05', '2022-07-08'),
(2, 3, 14, '2022-08-05', '2022-08-08'),
(3, 4, 12, '2022-08-10', '2022-08-13'),
(4, 5, 3, '2022-08-15', '2022-08-18'),
(1, 3, 4, '2022-08-20', '2022-08-22'),
(2, 1, 9, '2022-08-25', '2022-08-27'),
(5, 6, 20, '2022-08-30', '2022-09-02'),
(4, 2, 2, '2022-09-05', '2022-09-08'),
(1, 5, 7, '2022-09-10', '2022-09-13'),
(5, 2, 18, '2022-09-15', '2022-09-18'),
(3, 6, 15, '2022-09-20', '2022-09-22');


INSERT INTO public.room(
	id, resort_id, sqm, price)
	VALUES 
(1, 1, 20, 2400),
(1, 2, 30, 3000),
(1, 3, 40, 1200),
(1, 4, 20, 1500),
(1, 5, 30, 1100),
(2, 2, 40, 1300),
(2, 1, 20, 2600),
(2, 3, 30, 1600),
(2, 5, 40, 1400),
(2, 4, 20, 1300),
(3, 1, 30, 1800),
(3, 2, 40, 1700),
(3, 3, 20, 1500),
(3, 4, 30, 1400),
(3, 5, 40, 1200),
(6, 2, 20, 2500),
(4, 1, 30, 2200),
(4, 3, 40, 1600),
(4, 5, 20, 1000),
(4, 4, 30, 1600),
(5, 1, 40, 2900),
(4, 2, 20, 2200),
(5, 3, 30, 1400),
(5, 5, 40, 1500),
(5, 4, 20, 1600),
(6, 1, 30, 2600),
(5, 2, 40, 1800),
(6, 3, 20, 1600),
(6, 4, 30, 1300),
(6, 5, 40, 1700);

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
('Haydon', 'Laight', 'hlaightc@disqus.com', '+53 341 775 14', '1982-12-22'),
('Charity', 'Kinnett', 'ckinnettd@g.co', '+62 153 697 1063', '1982-11-27'),
('Briggs', 'Bees', 'bbeese@biglobe.ne.jp', '+242 998 130 2896', '2003-07-02'),
('Rupert', 'Bancroft', 'rbancroftf@comcast.net', '+63 578 183 0290', '1993-11-12'),
('Vidovic', 'Gawkes', 'vgawkesg@addthis.com', '+7 299 837 5147', '1987-05-15'),
('Ariana', 'Klimaszewski', 'aklimaszewskih@cornell.edu', '+86 820 272 7993', '2002-12-24'),
('Kimbell', 'Yaxley', 'kyaxleyi@state.gov', '+373 793 238 0425', '1993-09-10'),
('Harwell', 'Ilchenko', 'hilchenkoj@goo.ne.jp', '+886 971 717 3898', '1988-10-03');




INSERT INTO public.resort(
name, city, dist_beach, dist_centrum, stars)
	VALUES
	('Elite Hotels', 'Malmö', 250, 1000, 4),
	('Hilton Hotels', 'Malmö', 500, 750, 5),
	('Scandic Hotels', 'Helsingborg', 100, 400, 3),
	('First Hotels', 'Åhus', 100, 1250, 3),
	('Bjärnum Motel', 'Bjärnum', 50000, 1);

ALTER TABLE public.room ADD FOREIGN KEY (resort_id) references resort(id);
ALTER TABLE public.booking ADD FOREIGN KEY (room_id) references room(id);
ALTER TABLE public.booking ADD FOREIGN KEY (customer_id) references customer(id);
*/

");
        {
        }
        await cmd.ExecuteNonQueryAsync();
    }


}
