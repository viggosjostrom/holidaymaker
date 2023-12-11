using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
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

    public async void Edit()
    {
        string input = string.Empty;
        Console.WriteLine("Please enter your bookingID: ");
        int.TryParse(Console.ReadLine(), out int bookingID);

        await using var db = NpgsqlDataSource.Create(dbUri);



        while (true)
        {

            Console.WriteLine("What would you like to edit?");
            Console.WriteLine("1: Change room");
            Console.WriteLine("2: Change customer");
            Console.WriteLine("3: Change check in date");
            Console.WriteLine("4: Change checkout date");
            Console.WriteLine("5: Change extra services");
            Console.WriteLine("0: EXIT");
            switch (Console.ReadLine())

            {
                case "1":
                    Console.Clear();

                    await using (var cmd = db.CreateCommand($"UPDATE public.booking SET room_id = $1 WHERE id = {bookingID}"))
                    {
                        Console.WriteLine("New room choice: ");
                        cmd.Parameters.AddWithValue(Console.ReadLine());
                        await cmd.ExecuteNonQueryAsync();

                    }



                    break;

                case "2":
                    break;

                case "3":

                    break;

                case "4":

                    break;

                case "5":

                    break;

                case "0": // till main menu
                    System.Environment.Exit(1337);
                    Console.Clear();
                    break;

                default:
                    Console.WriteLine("Invalid option");
                    Console.WriteLine("Press any key to return to main menu");
                    Console.ReadKey();
                    //menu = true;
                    Console.Clear();
                    continue;
                    throw new Exception("CRASH!");
            }

        }
    }

    public void Delete()
    {

    }


}

