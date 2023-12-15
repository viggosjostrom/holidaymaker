using Npgsql;
using NpgsqlTypes;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace holidaymaker;

public class SearchFunctions(NpgsqlDataSource db)
{
    public async Task AvaliableRooms()
    {

        while (true)
        {
            string beachDistance = string.Empty;
            string centrumDistance = string.Empty;
            string roomSqm = string.Empty;
            string Amenity = string.Empty;

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
                await Console.Out.WriteLineAsync("Pick an option.\n1. Search filter\n2. Order by.\n3. Finish search.");
                string? searchInput = Console.ReadLine();
                switch (searchInput)
                {
                    case "1":
                        await Console.Out.WriteLineAsync("Pick an option.\n1. City\n2. Sqm.\n3. Amenities.\n4. Dist to Beach.\n5. Dist to Centrum.\n6. Return.");
                        string? caseOne = Console.ReadLine();

                        switch (caseOne) //alla extra sökval som amenities and distance/sqm/city
                        {


                            case "1": //city
                                Console.Clear();
                                break;

                            case "2": //Sqm
                                Console.Clear();
                                Console.WriteLine("Choose minimun desired room-size (enter '0' if no preference): ");
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
                                string? amenityWord = Console.ReadLine();
                                Amenity += $"AND LOWER(a.name) LIKE '%{amenityWord}%' ";
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

                    case "2": //order by sqm, location, stars etc.
                        continue;




                    case "3": //finish search

                        await using (var cmd = db.CreateCommand($@"SELECT
        r.id AS room_id,
        r.name AS room_name,
        r.sqm,
        r.price,
        rs.name AS resort_name,
        rs.city,
        rs.dist_beach,
        rs.dist_centrum
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
"

    ))
                        {
                            await using (var reader = await cmd.ExecuteReaderAsync())
                            {
                                Console.Clear();
                                Console.WriteLine("SEARCH RESULT:");
                                Console.WriteLine("--------------------------------------------------------------------------------------------------------------------------");
                                Console.WriteLine("Room ID\t| Room Name\t| SQM\t| Price\t\t| Resort Name\t\t\t| Dist Beach\t| Dist Centrum\t | City");
                                Console.WriteLine("---------------------------------------------------------------------------------------------------------------------------");

                                while (await reader.ReadAsync())
                                {
                                    (int roomId, string roomName, int sqm, decimal price, string resortName, int distToBeach, int distToCentrum, string city) = (
                                     reader.GetInt32(0),
                                     reader.GetString(1),
                                     reader.GetInt32(2),
                                     reader.GetDecimal(3),
                                     reader.GetString(4),
                                     reader.GetInt32(6),
                                     reader.GetInt32(7),
                                     reader.GetString(5)
                                    );
                                    Console.WriteLine($"{roomId + "\t"}| {roomName + "\t"}| {sqm + "\t\t\t"}| {price + "\t"}| {resortName + "\t\t\t"}| {distToBeach + "\t\t"}| {distToCentrum}\t|{city}|");
                                }

                                Console.WriteLine("-----------------------------------------------------------------------------------------------------------------------------");
                                await Console.Out.WriteLineAsync();
                                //add searchfilter here!!!
                                await Console.Out.WriteLineAsync();
                                break;
                            }




                        }
                    default:
                        continue;



                }
            }

        }

    }
}

