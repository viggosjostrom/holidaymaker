using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace holidaymaker;

public class SetupDB()
{
	public static async Task NewDB()
	{
		string dbUri = "Host=localhost;Port=5455;Username=postgres;Password=postgres;Database=holidaymaker";
		await using var db = NpgsqlDataSource.Create(dbUri);
		await using var cmd = db.CreateCommand(@"

	CREATE TABLE IF NOT EXISTS resort(
	id SERIAL PRIMARY KEY,
	name TEXT NOT NULL,
	city TEXT NOT NULL,
	dist_beach INT NOT NULL,
	dist_centrum INT NOT NULL,
	stars INT NOT NULL
	);

	CREATE TABLE IF NOT EXISTS customer(
	id SERIAL PRIMARY KEY,
	firstname TEXT NOT NULL,
	lastname TEXT NOT NULL,
	email TEXT NOT NULL,
	phone TEXT NOT NULL,
	date_of_birth DATE NOT NULL
	);

	CREATE TABLE IF NOT EXISTS room(
	id SERIAL PRIMARY KEY,
	name TEXT NOT NULL,
	resort_id INT NOT NULL,
	sqm INT NOT NULL,
	price MONEY NOT NULL
	);

	CREATE TABLE IF NOT EXISTS booking(
	id SERIAL PRIMARY KEY,
	resort_id INT NOT NULL,
	room_id INT NOT NULL,
	in_date DATE NOT NULL,
	out_date DATE NOT NULL
	);

	CREATE TABLE IF NOT EXISTS amenities(
	id SERIAL PRIMARY KEY,
	name TEXT NOT NULL
	);

	CREATE TABLE IF NOT EXISTS extras(
	id SERIAL PRIMARY KEY,	
	name TEXT NOT NULL,
	price MONEY NOT NULL
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

	CREATE TABLE IF NOT EXISTS customer_x_booking(
	id SERIAL PRIMARY KEY,
	booking_id SERIAL REFERENCES booking(id) ON DELETE CASCADE,
	customer_id SERIAL REFERENCES customer(id)
	);

	INSERT INTO public.amenities(
	name)
	VALUES
	('pool'),
	('restaurant'),
	('childclub'),
	('night entertainment');

	INSERT INTO public.extras(
	name, price)
	VALUES 
	('Extra bed', 50),
	('All inclusive', 1000),
	('Half pension', 500);

	INSERT INTO public.resort(
	name, city, dist_beach, dist_centrum, stars)
	VALUES
	('Elite Hotels', 'Malmö', 250, 1000, 4),
	('Hilton Hotels', 'Malmö', 500, 750, 5),
	('Scandic Hotels', 'Helsingborg', 100, 400, 3),
	('First Hotels', 'Åhus', 100, 1250, 3),
	('Bjärnum Motel', 'Bjärnum', 50000, 0, 1);
	
	INSERT INTO public.room(
	name, resort_id, sqm, price)
	VALUES 
	('EH01', 1, 20, 2400),
	('HH01', 2, 30, 3000),
	('SH01', 3, 40, 1200),
	('FH01', 4, 20, 1500),
	('BM01', 5, 30, 1100),
	('HH02', 2, 40, 1300),
	('EH02', 1, 20, 2600),
	('SH02', 3, 30, 1600),
	('BM02', 5, 40, 1400),
	('FH02', 4, 20, 1300),
	('EH03', 1, 30, 1800),
	('HH03', 2, 40, 1700),
	('SH03', 3, 20, 1500),
	('FH03', 4, 30, 1400),
	('BM03', 5, 40, 1200),
	('HH06', 2, 20, 2500),
	('EH04', 1, 30, 2200),
	('SH04', 3, 40, 1600),
	('BM04', 5, 20, 1000),
	('FH04', 4, 30, 1600),
	('EH05', 1, 40, 2900),
	('HH04', 2, 20, 2200),
	('SH05', 3, 30, 1400),
	('BM05', 5, 40, 1500),
	('FH05', 4, 20, 1600),
	('EH06', 1, 30, 2600),
	('HH05', 2, 40, 1800),
	('SH06', 3, 20, 1600),
	('FH06', 4, 30, 1300),
	('BM06', 5, 40, 1700);

	INSERT INTO public.booking (resort_id, room_id, in_date, out_date)
	VALUES
	(1, 12, '2022-06-01', '2022-06-08'),
	(4, 5, '2022-06-06', '2022-06-13'),
	(3, 18, '2022-06-11', '2022-06-18'),
	(2, 8, '2022-06-16', '2022-06-23'),
	(1, 25, '2022-06-21', '2022-06-28'),
	(5, 3, '2022-06-26', '2022-07-03'),
	(2, 7, '2022-07-01', '2022-07-08'),
	(3, 15, '2022-07-06', '2022-07-13'),
	(4, 27, '2022-07-11', '2022-07-18'),
	(1, 10, '2022-07-16', '2022-07-23'),
	(3, 20, '2022-07-20', '2022-07-27'),
	(5, 29, '2022-07-25', '2022-08-01'),
	(1, 2, '2022-07-30', '2022-08-06'),
	(2, 17, '2022-08-04', '2022-08-11'),
	(4, 6, '2022-08-09', '2022-08-16'),
	(1, 23, '2022-08-14', '2022-08-21'),
	(3, 14, '2022-08-19', '2022-08-26'),
	(2, 22, '2022-08-24', '2022-08-31'),
	(4, 11, '2022-08-29', '2022-09-05'),
	(5, 30, '2022-09-03', '2022-09-10'),
	(1, 8, '2022-09-08', '2022-09-15'),
	(2, 9, '2022-09-13', '2022-09-20'),
	(4, 16, '2022-09-18', '2022-09-25'),
	(5, 2, '2022-06-01', '2022-06-08'),
	(1, 19, '2022-06-06', '2022-06-13'),
	(3, 24, '2022-06-11', '2022-06-18'),
	(2, 30, '2022-06-16', '2022-06-23'),
	(4, 1, '2022-06-21', '2022-06-28'),
	(5, 13, '2022-06-26', '2022-07-03'),
	(1, 4, '2022-07-01', '2022-07-08'),
	(2, 5, '2022-07-06', '2022-07-13'),
	(4, 12, '2022-07-11', '2022-07-18'),
	(1, 21, '2022-07-16', '2022-07-23'),
	(3, 10, '2022-07-21', '2022-07-28'),
	(5, 28, '2022-07-25', '2022-08-01'),
	(3, 19, '2022-08-03', '2022-08-10'),
	(4, 14, '2022-08-08', '2022-08-15'),
	(1, 26, '2022-08-13', '2022-08-20'),
	(5, 7, '2022-08-18', '2022-08-25'),
	(2, 16, '2022-08-23', '2022-08-30'),
	(3, 1, '2022-08-28', '2022-09-04'),
	(1, 9, '2022-09-02', '2022-09-09'),
	(4, 26, '2022-09-07', '2022-09-14'),
	(5, 15, '2022-09-12', '2022-09-19'),
	(2, 3, '2022-09-17', '2022-09-24'),
	(3, 11, '2022-06-01', '2022-06-08'),
	(1, 18, '2022-06-06', '2022-06-13'),
	(4, 21, '2022-06-11', '2022-06-18'),
	(5, 8, '2022-06-16', '2022-06-23'),
	(2, 27, '2022-06-21', '2022-06-28'),
	(3, 4, '2022-06-26', '2022-07-03'),
	(1, 13, '2022-07-01', '2022-07-08'),
	(4, 24, '2022-07-06', '2022-07-13'),
	(5, 6, '2022-07-11', '2022-07-18'),
	(2, 15, '2022-07-16', '2022-07-23'),
	(3, 30, '2022-07-21', '2022-07-28'),
	(1, 17, '2022-07-25', '2022-08-01'),
	(4, 28, '2022-08-02', '2022-08-09'),
	(5, 4, '2022-08-06', '2022-08-13'),
	(2, 10, '2022-08-11', '2022-08-18'),
	(3, 22, '2022-08-16', '2022-08-23'),
	(1, 7, '2022-08-21', '2022-08-28'),
	(4, 17, '2022-08-26', '2022-09-02'),
	(5, 20, '2022-08-31', '2022-09-07');

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

	INSERT INTO public.booking_x_extras(
	booking_id, extras_id)
	VALUES 
	(4, 2),
	(4, 1),
	(59, 3),
	(32, 2),
	(17, 1),
	(45, 2),
	(8, 1),
	(21, 3),
	(12, 2),
	(53, 3),
	(41, 2),
	(3, 1),
	(27, 3),
	(60, 2),
	(14, 1),
	(37, 2),
	(50, 2),
	(9, 3),
	(28, 1),
	(6, 1);

	INSERT INTO public.resort_x_amenities(
	resort_id, amenities_id)
	VALUES 
	(1, 1),
	(1, 2),
	(1, 4),
	(2, 1),
	(2, 2),
	(2, 3),
	(2, 4),
	(3, 2),
	(3, 4),
	(4, 2),
	(4, 3),
	(4, 1),
	(5, 2);

	INSERT INTO public.resort_x_extras(	
	resort_id, extras_id)
	VALUES 
	(1, 1),
	(1, 2),
	(1, 3),
	(2, 1),
	(2, 2),
	(2, 3),
	(3, 1),
	(3, 3),
	(4, 1),
	(4, 3),
	(5, 1);
	
	INSERT INTO public.customer_x_booking(
	booking_id, customer_id)
	VALUES 
	(17, 6),
	(13, 9),
	(8, 15),
	(16, 10),
	(2, 5),
	(20, 13),
	(9, 12),
	(10, 3),
	(6, 16),
	(12, 14),
	(18, 8),
	(11, 2),
	(19, 1),
	(5, 18),
	(7, 4),
	(14, 7),
	(3, 11),
	(15, 17),
	(4, 20),
	(1, 19),
	(21, 13),
	(26, 12),
	(27, 14),
	(15, 16),
	(20, 7),
	(19, 3),
	(17, 12),
	(18, 14),
	(16, 2),
	(22, 19),
	(29, 10),
	(3, 8),
	(25, 19),
	(18, 5),
	(12, 15),
	(5, 4),
	(11, 6),
	(23, 14),
	(32, 6),
	(21, 14),
	(13, 5),
	(28, 1),
	(14, 4),
	(35, 11),
	(38, 15),
	(9, 19),
	(15, 2),
	(22, 6),
	(6, 1),
	(16, 13),
	(34, 4),
	(5, 11),
	(25, 13),
	(29, 19),
	(33, 3),
	(30, 7),
	(24, 19),
	(11, 20),
	(27, 15),
	(39, 2),
	(31, 16),
	(29, 9),
	(36, 1),
	(35, 17),
	(16, 16),
	(27, 9),
	(38, 18),
	(41, 4),
	(50, 13),
	(43, 7),
	(14, 19),
	(45, 9),
	(30, 3),
	(42, 15),
	(10, 5),
	(26, 2),
	(28, 15),
	(46, 14),
	(11, 7),
	(34, 1),
	(28, 6),
	(10, 8),
	(18, 19),
	(33, 6),
	(29, 12),
	(20, 16),
	(14, 2),
	(47, 5),
	(48, 11),
	(49, 15),
	(50, 2),
	(51, 12),
	(52, 16),
	(53, 4),
	(54, 8),
	(55, 13),
	(56, 6),
	(57, 10),
	(58, 18),
	(59, 1),
	(60, 17),
	(61, 7),
	(62, 20),
	(63, 3),
	(64, 9);

	ALTER TABLE public.room ADD FOREIGN KEY (resort_id) references resort(id); 
	ALTER TABLE public.booking ADD FOREIGN KEY (room_id) references room(id);
		");
		{
		}
		await cmd.ExecuteNonQueryAsync();
	}
}