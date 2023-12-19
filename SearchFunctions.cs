using Npgsql;
using NpgsqlTypes;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace holidaymaker;

public class SearchFunctions(NpgsqlDataSource db)
{
    public async Task AvaliableRooms()
    {
        bool loop = true;
        while (loop)
        {
            string beachDistance = string.Empty;
            string centrumDistance = string.Empty;
            string roomSqm = string.Empty;
            string Amenity = string.Empty;
            string orderByResult = string.Empty;
            string cityChoice = string.Empty;

            Console.WriteLine("SEARCH AVALIABLE ROOMS");
            Console.WriteLine("Enter desired check in date (YYYY-MM-DD): ");
            if (DateOnly.TryParse(Console.ReadLine(), out DateOnly in_date))
            {
            }
            else
            {
                Console.Clear();
                Console.WriteLine("Wrong input, try again! ");
                continue;
            }
            Console.Clear();
            Console.WriteLine("SEARCH AVALIABLE ROOMS");
            Console.WriteLine($"Choosen check in date: {in_date}");
            Console.WriteLine("");
            Console.WriteLine("Enter desired check out date (YYYY-MM-DD): ");
            if (DateOnly.TryParse(Console.ReadLine(), out DateOnly out_date))
            {
                if (out_date < in_date)
                {
                    Console.Clear();
                    await Console.Out.WriteLineAsync("Sorry wrong date input.");
                    await Console.Out.WriteLineAsync("Indate is after outdate!");
                }
                Console.Clear();
            }
            else
            {
                Console.Clear();
                Console.WriteLine("Wrong input, try again! ");
                continue;
            }

            bool searchmenu = true;
            while (searchmenu)
            {
                await Console.Out.WriteLineAsync("Pick an option.\n\n1. Filter\n\n2. Order by\n\n3. Search room.\n\n4. Start booking\n\n5. Return to menu");
                string? searchInput = Console.ReadLine();
                switch (searchInput)
                {
                    case "1":
                        Console.Clear();
                        await Console.Out.WriteLineAsync("Pick an option.\n1. City\n2. Sqm.\n3. Amenities.\n4. Dist to Beach.\n5. Dist to Centrum.\n6. Return.");
                        string? caseOne = Console.ReadLine();
                        Console.Clear();

                        switch (caseOne) //alla extra sökval som amenities and distance/sqm/city
                        {


                            case "1": //city
                                Console.Clear();
                                Console.WriteLine("Choose a city: ");
                                Console.WriteLine("1: Malmö");
                                Console.WriteLine("2: Helsingborg");
                                Console.WriteLine("3: Åhus");
                                Console.WriteLine("4: Bjärnum");

                                if (int.TryParse(Console.ReadLine(), out int inputCity))
                                {
                                    switch (inputCity)
                                    {
                                        case 1:
                                            cityChoice = $"AND rs.city LIKE '%Malmö%'";
                                            break;

                                        case 2:
                                            cityChoice = $"AND rs.city LIKE '%Helsingborg%'";
                                            break;
                                        case 3:
                                            cityChoice = $"AND rs.city LIKE '%Åhus%'";
                                            break;
                                        case 4:
                                            cityChoice = $"AND rs.city LIKE '%Bjärnum%'";
                                            break;
                                    }
                                }
                                break;


                            case "2": //Sqm
                                Console.Clear();
                                Console.WriteLine("Choose minimun desired room-size: ");
                                string? inputSqm = Console.ReadLine();
                                if (Int32.TryParse(inputSqm, out int sqmInt))
                                {
                                    roomSqm = $"AND r.sqm >= {sqmInt}";
                                    Console.Clear();
                                    break;
                                }
                                else
                                {
                                    Console.Clear();
                                    Console.WriteLine("Wrong input, try again! ");
                                    break;
                                }

                            case "3": //Amenities
                                Console.Clear();
                                Console.WriteLine("Choose desired amenity. Enter keywords eg. pool, childclub, night entertainment or restaurant"); //skriv ut fler keywords
                                Console.WriteLine("1: Pool");
                                Console.WriteLine("2: Childclub");
                                Console.WriteLine("3: Night entertainment");
                                Console.WriteLine("4: Restaurant");
                                if (int.TryParse(Console.ReadLine(), out int inputAmenities))
                                {
                                    switch (inputAmenities)
                                    {
                                        case 1:
                                            Amenity += $"AND LOWER(a.name) LIKE '%pool%' ";
                                            break;

                                        case 2:
                                            Amenity += $"AND LOWER(a.name) LIKE '%childclub%' ";
                                            break;
                                        case 3:
                                            Amenity += $"AND LOWER(a.name) LIKE '%night entertainment%' ";
                                            break;
                                        case 4:
                                            Amenity += $"AND LOWER(a.name) LIKE '%restaurant%' ";
                                            break;
                                    }
                                }
                                break;

                            case "4": //distance to beach
                                Console.Clear();
                                Console.WriteLine("Enter desired MAX distance to beach in meters: ");
                                string? inputBeach = Console.ReadLine();
                                if (Int32.TryParse(inputBeach, out int dist_beach))
                                {
                                    beachDistance = $"AND rs.dist_beach <={dist_beach}";
                                    break;
                                }
                                else
                                {
                                    Console.Clear();
                                    Console.WriteLine("Wrong input, try again! ");
                                    break;
                                }

                            case "5": //distance to centrum
                                Console.Clear();
                                Console.WriteLine("Enter desired MAX distance to centrum in meters: ");
                                string? inputCentrum = Console.ReadLine();
                                if (Int32.TryParse(inputCentrum, out int dist_centrum))
                                {
                                    centrumDistance = $"AND rs.dist_centrum <={dist_centrum}";
                                    break;
                                }
                                else
                                {
                                    Console.Clear();
                                    Console.WriteLine("Wrong input, try again! ");
                                    break;
                                }

                            case "6": //return option

                                Console.Clear();
                                break;



                            default:
                                Console.WriteLine("Invalid option.\nPress any key to return to main menu");
                                Console.ReadKey();
                                Console.Clear();
                                continue;
                        }
                        break;

                    case "2":
                        Console.Clear();
                        Console.WriteLine("What do you want to order by?");
                        Console.WriteLine("1: Distance beach");
                        Console.WriteLine("2: Distance to centrum");
                        Console.WriteLine("3: Price");
                        Console.WriteLine("4: Stars");
                        Console.WriteLine("");

                        if (int.TryParse(Console.ReadLine(), out int input))
                        {
                            switch (input)
                            {
                                case 1:

                                    orderByResult = $"ORDER BY rs.dist_beach ASC";
                                    Console.Clear();
                                    break;

                                case 2:
                                    orderByResult = $"ORDER BY rs.dist_centrum ASC";
                                    Console.Clear();
                                    break;


                                case 3:
                                    orderByResult = "ORDER BY r.price ASC";
                                    Console.Clear();
                                    break;

                                case 4:
                                    orderByResult = "ORDER BY rs.stars DESC";
                                    Console.Clear();
                                    break;


                                default:
                                    Console.WriteLine("Sorry this was not a valid choice.");
                                    break;
                            }


                        }

                        else
                        {
                            Console.WriteLine("Wrong input, please try again");
                            Console.ReadKey();
                            Console.Clear();
                        }


                        continue;


                    case "3":

                        await using (var cmd = db.CreateCommand($@"SELECT
                        r.id AS room_id,
                        rs.id AS resort_id,
                        r.sqm,
                        r.price,
                        rs.name AS resort_name,
                        rs.city,
                        rs.dist_beach,
                        rs.dist_centrum,
                        rs.stars
                        FROM room r
                        JOIN
                        resort rs ON r.resort_id = rs.id
                        WHERE
                        r.id NOT IN(
                            SELECT
                                b.room_id
                            FROM
                                booking b
                            WHERE
                                (
                                    ('{in_date}' BETWEEN b.in_date AND b.out_date)
                                    OR('{out_date}' BETWEEN b.in_date AND b.out_date)
                                    OR(b.in_date BETWEEN '{in_date}' AND '{out_date}')
                                )
                        )
                        {beachDistance}
                        {centrumDistance}
                        AND EXISTS


            (
            SELECT 1
            FROM resort_x_amenities ra
            JOIN amenities a 
            ON ra.amenities_id = a.id
            WHERE ra.resort_id = rs.id
            {Amenity}
            )

        {roomSqm} 
        {cityChoice} 
        {orderByResult}
"

    ))
                        {
                            await using (var reader = await cmd.ExecuteReaderAsync())
                            {
                                Console.Clear();
                                Console.WriteLine("SEARCH RESULT:");
                                Console.WriteLine("--------------------------------------------------------------------------------------------------------------------------");
                                Console.WriteLine("| Resort ID\t|Room ID\t| SQM\t| Price\t\t| Resort Name\t\t| Beach | Centrum | City\t | Stars |");
                                Console.WriteLine("--------------------------------------------------------------------------------------------------------------------------");

                                while (await reader.ReadAsync())
                                {
                                    (int resortid, int roomId, int sqm, decimal price, string resortName, int distToBeach, int distToCentrum, string city, int stars) = (
                                     reader.GetInt32(1),
                                     reader.GetInt32(0),
                                     reader.GetInt32(2),
                                     reader.GetDecimal(3),
                                     reader.GetString(4),
                                     reader.GetInt32(6),
                                     reader.GetInt32(7),
                                     reader.GetString(5),
                                     reader.GetInt32(8)
                                    );
                                    Console.Write($"| {resortid + "\t\t"}| {roomId + "\t\t"}| {sqm + "\t"}| {price + "\t"}|");
                                    if (resortName.Length > 14)
                                    {
                                        await Console.Out.WriteAsync($"{resortName + ""}| {distToBeach + "\t"}| {distToCentrum}\t  |");
                                    }
                                    else
                                    {
                                        await Console.Out.WriteAsync($"{resortName + "\t\t"}| {distToBeach + "\t"}| {distToCentrum}\t  |");
                                    }

                                    if (city.Length > 6)
                                    {
                                        await Console.Out.WriteAsync($" {city}\t | {stars}     |\n");

                                    }
                                    else
                                    {
                                        await Console.Out.WriteAsync($" {city}\t | {stars}     |\n");
                                    }

                                }
                                Console.WriteLine("--------------------------------------------------------------------------------------------------------------------------");

                                await Console.Out.WriteLineAsync();
                                await Console.Out.WriteLineAsync();
                                break;
                            }


                        }

                    case "4":
                        int room_id = 0;
                        Booking n = new Booking(db);
                        await n.New(in_date, out_date);
                        continue;

                    case "5":
                        await Console.Out.WriteLineAsync("Return to main menu.");
                        loop = false;
                        searchmenu = false;
                        Console.Clear();
                        break;

                    default:
                        continue;



                }
            }
            break;

        }
    }
}

