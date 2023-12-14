using Npgsql;
using System;
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

            {
                Console.WriteLine("SEARCH BOOKING");
                Console.WriteLine("Enter desired check in date (YYYY-MM-DD): ");
                if (!DateOnly.TryParse(Console.ReadLine(), out DateOnly in_date))
                {
                    Console.Clear();
                    Console.WriteLine("Wrong input, try again! ");
                    continue;
                }

                Console.Clear();
                Console.WriteLine("SEARCH BOOKING");
                Console.WriteLine($"Choosen check in date: {in_date}");
                Console.WriteLine("");
                Console.WriteLine("Enter desired check out date (YYYY-MM-DD): ");
                if (!DateOnly.TryParse(Console.ReadLine(), out DateOnly out_date))
                {
                    Console.Clear();
                    Console.WriteLine("Wrong input, try again! ");
                    continue;
                }

                Console.Clear();
                Console.WriteLine("SEARCH BOOKING");
                Console.WriteLine($"Choosen check in date: {in_date}");
                Console.WriteLine($"Choosen check out date: {out_date}");
                Console.WriteLine("");
                Console.WriteLine("Add distance to beach preference? y/n");
                string beachPreference = string.Empty;
                beachPreference = Console.ReadLine();
                if (beachPreference == "y")
                {
                    Console.Clear();
                    Console.WriteLine("SEARCH BOOKING");
                    Console.WriteLine($"Choosen check in date: {in_date}");
                    Console.WriteLine($"Choosen check out date: {out_date}");
                    Console.WriteLine("Enter desired MAX distance to beach in meters: ");
                    string input = Console.ReadLine();
                    if (Int32.TryParse(input, out int dist_beach))
                    {

                    }
                    else
                    {
                        Console.Clear();
                        Console.WriteLine("Wrong input, try again! ");
                        continue;
                    }




                }
                else
                {
                    Console.Clear();
                    Console.WriteLine("No preferences for distance to beach added");
                }


                await using (var cmd = db.CreateCommand($@"SELECT
    r.id AS room_id,
    r.name AS room_name,
    r.sqm,
    r.price,
    rs.name AS resort_name,
    rs.city,
    rs.dist_beach,
    rs.dist_centrum
FROM
    room r
JOIN
    resort rs ON r.resort_id = rs.id
WHERE
    r.id NOT IN (
        SELECT
            b.room_id
        FROM
            booking b
        WHERE
            (
                ('{in_date}' BETWEEN b.in_date AND b.out_date)
                OR ('{out_date}' BETWEEN b.in_date AND b.out_date)
                OR (b.in_date BETWEEN '{in_date}' AND '{out_date}')
            )
    )
    AND rs.dist_beach <=200  -- Replace with the max distance to the beach
    AND rs.dist_centrum <=1500  -- Replace with the max distance to the centrum
    AND EXISTS (
        SELECT
            1
        FROM
            resort_x_amenities ra
        JOIN
            amenities a ON ra.amenities_id = a.id
        WHERE
            ra.resort_id = rs.id
            AND LOWER(a.name) LIKE ALL (ARRAY['%%', '%%']) -- Replace with the desired amenities
    )
    AND r.sqm >= 10; -- Replace with the minimum room size
"))
                {
                    await using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        Console.WriteLine("SEARCH RESULT:");
                        Console.WriteLine("---------------------------------------------------------------------------------------------------------");
                        Console.WriteLine("Room ID\t| Room Name\t| SQM\t| Price\t| Resort Name\t| City\t| Dist to Beach\t| Dist to Centrum");
                        Console.WriteLine("---------------------------------------------------------------------------------------------------------");

                        while (await reader.ReadAsync())
                        {
                            (int roomId, string roomName, int sqm, decimal price, string resortName, string city, int distToBeach, int distToCentrum) = (
                             reader.GetInt32(0),
                             reader.GetString(1),
                             reader.GetInt32(2),
                             reader.GetDecimal(3),
                             reader.GetString(4),
                             reader.GetString(5),
                             reader.GetInt32(6),
                             reader.GetInt32(7)
   );

                            Console.WriteLine($"{roomId + "\t"} | {roomName + "\t"} | {sqm + "\t"} | {price + "\t"} | {resortName + "\t"} | {city + "\t\t"} | {distToBeach + "\t"} | {distToCentrum}");
                        }

                        Console.WriteLine("----------------------------------------------------------------------------------------------------------");
                    }
                }

                break;





            }
        }
    }
}