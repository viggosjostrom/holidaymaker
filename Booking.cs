﻿using Npgsql;


namespace holidaymaker;


public class Booking(NpgsqlDataSource db)
{
    
    public async Task New()

    {

        await using (var cmd = db.CreateCommand("INSERT INTO booking (name, customer_id, in_date, out_date) VALUES ($1, $2, $3, $4)"))
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

            await cmd.ExecuteNonQueryAsync();
        }
    }

    public async Task Edit()
    {
        Console.Clear();
        string input = string.Empty;
        Console.WriteLine("Please enter your bookingID: ");
        int.TryParse(Console.ReadLine(), out int bookingID);


        bool edit = true;


        while (edit)
        {
            Console.Clear();
            Console.WriteLine("What would you like to edit?");
            Console.WriteLine("1: Change room");
            Console.WriteLine("2: Change customer");
            Console.WriteLine("3: Change check in date");
            Console.WriteLine("4: Change checkout date");
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

                        Console.Clear();
                        Console.WriteLine("You have now edited the room");
                        Thread.Sleep(3000);
                        Console.Clear();
                    }

                    break;

                case "2":
                    Console.Clear();

                    await using (var cmd = db.CreateCommand($"UPDATE public.booking SET customer_id = $1 WHERE id = {bookingID}"))
                    {
                        Console.WriteLine("Change customer: ");
                        int.TryParse(Console.ReadLine(), out int customerId);
                        cmd.Parameters.AddWithValue(customerId);
                        await cmd.ExecuteNonQueryAsync();
                    }

                    Console.Clear();
                    Console.WriteLine("You have now changed the customer");
                    Thread.Sleep(3000);
                    Console.Clear();

                    break;

                case "3":
                    Console.Clear();

                    await using (var cmd = db.CreateCommand($"UPDATE public.booking SET in_date = $1 WHERE id = {bookingID}"))
                    {
                        Console.WriteLine("New Date (checkin): ");
                        DateOnly.TryParse(Console.ReadLine(), out DateOnly dateIn);
                        cmd.Parameters.AddWithValue(dateIn);
                        await cmd.ExecuteNonQueryAsync();

                    }
                    break;

                case "4":
                    Console.Clear();

                    await using (var cmd = db.CreateCommand($"UPDATE public.booking SET out_date = $1 WHERE id = {bookingID}"))
                    {
                        Console.WriteLine("New Date (checkout): ");
                        DateOnly.TryParse(Console.ReadLine(), out DateOnly dateOut);
                        cmd.Parameters.AddWithValue(dateOut);
                        await cmd.ExecuteNonQueryAsync();

                    }
                    break;


                case "0":
                    Console.Clear();
                    edit = false;
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

    public async Task Delete()
    {

        Console.WriteLine("Which booking would you like to delete? (Enter the bookingID)");
        int.TryParse(Console.ReadLine(), out int bookingID);

        await using (var cmd = db.CreateCommand($"DELETE FROM booking WHERE booking.id = {bookingID}"))
        {

            await cmd.ExecuteNonQueryAsync();
        }

    }

    public async Task OrderBy()
    {



        Console.WriteLine("What do you want to order by?");
        Console.WriteLine("1: Distance beach");
        Console.WriteLine("2: Distance to centrum");
        Console.WriteLine("3: Price");
        Console.WriteLine("4: Stars");
        Console.WriteLine("");

        int input = int.Parse(Console.ReadLine());
        string orderByResult = string.Empty;
        switch (input)
        {

            case 1:
                Console.WriteLine("What is the max distance to the beach?");
                int.TryParse(Console.ReadLine(), out int maxBeach);
                orderByResult = $"WHERE dist_beach <= {maxBeach}";
                break;

            case 2:
                Console.WriteLine("What is the max distance to the centrum?");
                int.TryParse(Console.ReadLine(), out int maxCentrum);
                orderByResult = $"WHERE dist_centrum <= {maxCentrum}";
                break;

            case 3:
                orderByResult = "ORDER BY price ASC";
                break;

            case 4:
                orderByResult = "ORDER BY stars DESC";
                break;


            default:
                Console.WriteLine("Sorry this was not a valid choice.");
                break;
        }


        await using (var cmd = db.CreateCommand($@" SELECT * FROM public.resort JOIN room ON resort_id = resort.id {orderByResult};"))
        {
            await cmd.ExecuteNonQueryAsync();
        }
    }
    public async Task Search()
    {



        string query = "select * from booking";
        var reader = await db.CreateCommand(query).ExecuteReaderAsync();

        while (await reader.ReadAsync())
        {
            Console.WriteLine(reader.GetInt32(0) + ", " + reader.GetString(1));



        }

    }

    public class SearchPage(NpgsqlDataSource db)
    {
        public async Task<string> Allbookings()
        {
            string result = string.Empty;


            string query = "select * from booking";
            var reader = await db.CreateCommand(query).ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                // Console.WriteLine(reader.GetInt32(0) + ", " + reader.GetString(1));
                result += reader.GetInt32(0);
                result += ", ";
                result += reader.GetString(1);
                 

            }

            return result;

        }



    }


}


