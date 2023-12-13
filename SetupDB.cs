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
	id SERIAL PRIMARY KEY,
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


INSERT INTO public.booking (resort_id, room_id, customer_id, in_date, out_date)
VALUES
(1, 12, 17, '2022-06-01', '2022-06-08'),
(4, 5, 9, '2022-06-06', '2022-06-13'),
(3, 18, 15, '2022-06-11', '2022-06-18'),
(2, 8, 5, '2022-06-16', '2022-06-23'),
(1, 25, 13, '2022-06-21', '2022-06-28'),
(5, 3, 19, '2022-06-26', '2022-07-03'),
(2, 7, 4, '2022-07-01', '2022-07-08'),
(3, 15, 1, '2022-07-06', '2022-07-13'),
(4, 27, 10, '2022-07-11', '2022-07-18'),
(1, 10, 18, '2022-07-16', '2022-07-23'),
(3, 20, 7, '2022-07-20', '2022-07-27'),
(5, 29, 14, '2022-06-01', '2022-06-08'),
(1, 2, 11, '2022-06-06', '2022-06-13'),
(2, 17, 16, '2022-06-11', '2022-06-18'),
(4, 6, 3, '2022-06-16', '2022-06-23'),
(1, 23, 8, '2022-06-21', '2022-06-28'),
(3, 14, 12, '2022-06-26', '2022-07-03'),
(2, 22, 2, '2022-07-01', '2022-07-08'),
(4, 11, 20, '2022-07-06', '2022-07-13'),
(5, 30, 6, '2022-07-11', '2022-07-18'),
(1, 8, 15, '2022-07-15', '2022-07-22'),
(2, 9, 17, '2022-07-20', '2022-07-27'),
(4, 16, 1, '2022-07-25', '2022-08-01'),
(5, 2, 19, '2022-06-01', '2022-06-08'),
(1, 19, 8, '2022-06-06', '2022-06-13'),
(3, 24, 14, '2022-06-11', '2022-06-18'),
(2, 30, 6, '2022-06-16', '2022-06-23'),
(4, 1, 9, '2022-06-21', '2022-06-28'),
(5, 13, 13, '2022-06-26', '2022-07-03'),
(1, 4, 5, '2022-07-01', '2022-07-08'),
(2, 5, 10, '2022-07-06', '2022-07-13'),
(4, 12, 6, '2022-07-11', '2022-07-18'),
(1, 21, 16, '2022-07-16', '2022-07-23'),
(3, 10, 8, '2022-07-21', '2022-07-28'),
(5, 28, 14, '2022-06-01', '2022-06-08'),
(3, 19, 9, '2022-06-06', '2022-06-13'),
(4, 14, 5, '2022-06-11', '2022-06-18'),
(1, 26, 10, '2022-06-16', '2022-06-23'),
(5, 7, 17, '2022-06-21', '2022-06-28'),
(2, 16, 8, '2022-06-26', '2022-07-03'),
(3, 1, 14, '2022-07-01', '2022-07-08'),
(1, 9, 6, '2022-07-06', '2022-07-13'),
(4, 26, 11, '2022-07-11', '2022-07-18'),
(5, 15, 18, '2022-07-15', '2022-07-22'),
(2, 3, 7, '2022-07-20', '2022-07-27'),
(3, 11, 2, '2022-06-01', '2022-06-08'),
(1, 18, 12, '2022-06-06', '2022-06-13'),
(4, 21, 5, '2022-06-11', '2022-06-18'),
(5, 8, 17, '2022-06-16', '2022-06-23'),
(2, 27, 7, '2022-06-21', '2022-06-28'),
(3, 4, 14, '2022-06-26', '2022-07-03'),
(1, 13, 5, '2022-07-01', '2022-07-08'),
(4, 24, 10, '2022-07-06', '2022-07-13'),
(5, 6, 17, '2022-07-11', '2022-07-18'),
(2, 15, 8, '2022-07-15', '2022-07-22'),
(3, 30, 14, '2022-07-20', '2022-07-27'),
(1, 17, 6, '2022-07-25', '2022-08-01'),
(4, 28, 11, '2022-08-02', '2022-08-09'),
(5, 4, 18, '2022-08-06', '2022-08-13'),
(2, 10, 8, '2022-08-11', '2022-08-18'),
(3, 22, 14, '2022-08-16', '2022-08-23'),
(1, 7, 6, '2022-08-21', '2022-08-28'),
(4, 17, 11, '2022-08-26', '2022-09-02'),
(5, 20, 18, '2022-08-31', '2022-09-07');



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
