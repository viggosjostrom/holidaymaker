using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Npgsql;


namespace holidaymaker;


public class Booking
{
    string dbUri = "Host=localhost;Port=5455;Username=postgres;Password=postgres;Database=holidaymaker"; //Inloggning till databasen port, password osv


    public async void New()

    {
        await using var db = NpgsqlDataSource.Create(dbUri);

        await using (var cmd = db.CreateCommand("INSERT INTO booking (room_id, customer_id, in_date, out_date, extra_bed, all_inclusive, half_pension) VALUES ($1, $2, $3, $4, $5, $6, $7)"))
        {

            Console.Write("Enter room number to book: ");
            cmd.Parameters.AddWithValue(Console.ReadLine());
            Console.WriteLine();

            Console.Write("Enter customer to book: ");
            int.TryParse(Console.ReadLine(), out int customerID);
            cmd.Parameters.AddWithValue(customerID);
            Console.WriteLine();

            Console.Write("Check In date (YYYY-MM-DD): ");
            DateOnly.TryParse(Console.ReadLine(), out DateOnly dateIn);
            cmd.Parameters.AddWithValue(dateIn);
            Console.WriteLine();

            // ska inte kunna vara ett datum innan check in
            Console.Write("Check Out date (YYYY-MM-DD): ");
            DateOnly.TryParse(Console.ReadLine(), out DateOnly dateOut);
            cmd.Parameters.AddWithValue(dateOut);
            Console.WriteLine();

            Console.Write("Extra bed? true/false: ");
            bool.TryParse(Console.ReadLine(), out bool bedBool);
            cmd.Parameters.AddWithValue(bedBool);
            Console.WriteLine();

            Console.Write("All inclusive? true/false: ");
            bool.TryParse(Console.ReadLine(), out bool allInclusiveBool);
            cmd.Parameters.AddWithValue(allInclusiveBool);
            Console.WriteLine();

            // skip om all inclusive = true
            Console.Write("Half Pension? true/false: ");
            bool.TryParse(Console.ReadLine(), out bool halfPensionBool);
            cmd.Parameters.AddWithValue(halfPensionBool);
            Console.WriteLine();

            await cmd.ExecuteNonQueryAsync();
        }
    }

    public void Edit()
    {

    }

    public void Delete()
    {

    }


}

