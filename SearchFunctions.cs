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
            int xdist_beach = 1000000;
            int xdist_centrum = 1000000;
            int sqmInt = 0;

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
                    Console.WriteLine("");
                    Console.WriteLine("Enter desired MAX distance to beach in meters: ");
                    string inputBeach = Console.ReadLine();
                    if (Int32.TryParse(inputBeach, out xdist_beach))
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


                Console.Clear();
                Console.WriteLine("SEARCH BOOKING");
                Console.WriteLine($"Choosen check in date: {in_date}");
                Console.WriteLine($"Choosen check out date: {out_date}");
                Console.WriteLine($"Choosen MAX distance to beach: {xdist_beach}");
                Console.WriteLine("");
                Console.WriteLine("Add distance to beach preference? y/n");
                string centrumPreference = string.Empty;
                centrumPreference = Console.ReadLine();
                if (centrumPreference == "y")
                {
                    Console.Clear();
                    Console.WriteLine("SEARCH BOOKING");
                    Console.WriteLine($"Choosen check in date: {in_date}");
                    Console.WriteLine($"Choosen check out date: {out_date}");
                    Console.WriteLine($"Choosen MAX distance to beach: {xdist_beach}");
                    Console.WriteLine("");
                    Console.WriteLine("Enter desired MAX distance to centrum in meters: ");
                    string inputCentrum = Console.ReadLine();
                    if (Int32.TryParse(inputCentrum, out xdist_centrum))
                    {

                    }
                    else
                    {
                        Console.Clear();
                        Console.WriteLine("Wrong input, try again! ");
                        continue;
                    }
                }

                Console.Clear();
                Console.WriteLine("SEARCH BOOKING");
                Console.WriteLine($"Choosen check in date: {in_date}");
                Console.WriteLine($"Choosen check out date: {out_date}");
                Console.WriteLine($"Choosen MAX distance to beach: {xdist_beach}");
                Console.WriteLine($"Choosen MAX distance to beach: {xdist_centrum}");
                Console.WriteLine("");
                Console.WriteLine("Choose minimun desired room-size (enter '0' if no preference): ");
                string inputSqm = Console.ReadLine();
                if (Int32.TryParse(inputSqm, out sqmInt))
                {

                }
                else
                {
                    Console.Clear();
                    Console.WriteLine("Wrong input, try again! ");
                    continue;
                }

                Console.Clear();
                Console.WriteLine("SEARCH INPUTS");
                Console.WriteLine($"Choosen check in date: {in_date}");
                Console.WriteLine($"Choosen check out date: {out_date}");
                Console.WriteLine($"Choosen MAX distance to beach: {xdist_beach}");
                Console.WriteLine($"Choosen MAX distance to beach: {xdist_centrum}");
                Console.WriteLine($"Chosen MIN size on room {sqmInt} m2");




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
    AND rs.dist_beach <={xdist_beach}  -- Replace with the max distance to the beach
    AND rs.dist_centrum <={xdist_centrum}  -- Replace with the max distance to the centrum
    AND EXISTS (
        SELECT
            1
        FROM
            resort_x_amenities ra
        JOIN
            amenities a ON ra.amenities_id = a.id
        WHERE
            ra.resort_id = rs.id
            AND LOWER(a.name) LIKE ALL (ARRAY['%%', '%%', '%%']) -- Replace with the desired amenities
    )
    AND r.sqm >= {sqmInt}; -- Replace with the minimum room size
"))
                {
                    await using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        Console.WriteLine("SEARCH RESULT:");
                        Console.WriteLine("---------------------------------------------------------------------------------------------------------------------------");
                        Console.WriteLine("Room ID\t| Room Name\t| SQM\t| Price\t| Resort Name\t| City\t\t| Distance to Beach\t\t| Distance to Centrum");
                        Console.WriteLine("---------------------------------------------------------------------------------------------------------------------------");

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
                            Console.WriteLine($"{roomId + "\t"}| {roomName + "\t"}| {sqm + "\t"}| {price + "\t"}| {resortName + "\t"}| {city + "\t\t"}| {distToBeach + "\t\t"}| {distToCentrum}");
                        }

                        Console.WriteLine("-----------------------------------------------------------------------------------------------------------------------------");
                    }
                }

                break;





            }
        }
    }
}