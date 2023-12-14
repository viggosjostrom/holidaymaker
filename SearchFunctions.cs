using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace holidaymaker;

public class SearchFunctions(NpgsqlDataSource db)
{
    public async Task<string> SearchAvaliableRooms()
    {
        string searchResult = string.Empty;

        await using (var cmd = db.CreateCommand(@"SELECT
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
                ('2023-01-01' BETWEEN b.in_date AND b.out_date)
                OR ('2022-01-10' BETWEEN b.in_date AND b.out_date)
                OR (b.in_date BETWEEN '2023-01-01' AND '2023-01-10')
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
            AND LOWER(a.name) LIKE ALL (ARRAY['%pool%', '%%']) -- Replace with the desired amenities
    )
    AND r.sqm >= 10; -- Replace with the minimum room size
")) 

            await cmd.ExecuteNonQueryAsync();

        return searchResult;


    }
}
