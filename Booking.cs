using Npgsql;
using System.ComponentModel.Design;
using System.Data.SqlTypes;
using System.Runtime.InteropServices;
using System.Transactions;


namespace holidaymaker;

public class Booking(NpgsqlDataSource db)
{
    public async Task New()
    {
        bool resort = true;
        bool room = false;
        bool customer = true;
        bool datefirst = false;
        bool dateSecond = false;
        Console.Clear();
        bool newbooking = true;
        while (newbooking)
        {

            await using (var cmd = db.CreateCommand(
                            "INSERT INTO booking (resort_id, room_id, in_date, out_date) VALUES ($1, $2, $3, $4) RETURNING id"))
            {
                if (resort)
                {
                    Console.Write("Enter resort id: ");
                    if (!int.TryParse(Console.ReadLine(), out int resort_id))
                    {
                        Console.Clear();
                        Console.WriteLine("Wrong input, try again! ");
                        continue;
                    }

                    cmd.Parameters.AddWithValue(resort_id);
                    resort = false;
                    room = true;
                    Console.Clear();
                }

                if (room)
                {
                    Console.Write("Enter room id: ");
                    if (!int.TryParse(Console.ReadLine(), out int room_id))
                    {
                        Console.Clear();
                        Console.WriteLine("Wrong input, try again! ");
                        continue;
                    }
                    cmd.Parameters.AddWithValue(room_id);
                    room = false;
                    datefirst = true;
                    Console.Clear();
                }



                if (datefirst)
                {
                    Console.Write("Enter in date (YYYY-MM-DD): ");
                    if (!DateOnly.TryParse(Console.ReadLine(), out DateOnly in_date))
                    {
                        Console.Clear();
                        Console.WriteLine("Wrong input, try again! ");
                        continue;
                    }

                    cmd.Parameters.AddWithValue(in_date);
                    datefirst = false;
                    dateSecond = true;
                    Console.Clear();
                }

                if (dateSecond)
                {
                    Console.Write("Enter out date (YYYY-MM-DD): ");
                    if (!DateOnly.TryParse(Console.ReadLine(), out DateOnly out_date))
                    {
                        Console.Clear();
                        Console.WriteLine("Wrong input, try again! ");
                        continue;
                    }

                    cmd.Parameters.AddWithValue(out_date);
                    Console.Clear();
                }

                //IF IN AND OUTDATE IS NOT VIABLE THEN LOOP DATES.
                int? lastBookingId = (int?)await cmd.ExecuteScalarAsync();


                int totalCustomers = 0;
                if (customer)
                {
                    await Console.Out.WriteLineAsync("How many customers?");
                    if (!int.TryParse(Console.ReadLine(), out int customerCount))
                    {
                        Console.Clear();
                        Console.WriteLine("Wrong input");
                        continue;
                    }
                    //LÄGG TILL FUNKTION OM 0 CUSTOMERS
                    totalCustomers = customerCount;
                    if (totalCustomers > 0)
                    {
                        customer = false;

                    }
                    else
                    {
                        Console.Clear();
                        Console.WriteLine("Needs atleast one customer");
                        continue;
                    }
                }


                int count = 1;
                for (int i = 0; i < totalCustomers; i++)
                {

                    await using (var command = db.CreateCommand($"INSERT INTO customer (firstname, lastname, email, phone, date_of_birth) VALUES ($1, $2, $3, $4, $5) RETURNING id"))
                    {
                        string? firstname = string.Empty;
                        string lastname = string.Empty;
                        string email = string.Empty;
                        string phone = string.Empty;


                        Console.Write("Customer " + count + ". Enter firstname: ");
                        command.Parameters.AddWithValue(firstname = Console.ReadLine());
                        Console.Clear();


                        Console.WriteLine("Name: " + firstname);

                        Console.Write("Customer " + count + ". Enter lastname: ");
                        command.Parameters.AddWithValue(lastname = Console.ReadLine());
                        Console.Clear();
                        Console.WriteLine("Name: " + firstname + " " + lastname);

                        Console.Write("Customer " + count + ". Email: ");
                        command.Parameters.AddWithValue(email = Console.ReadLine());
                        Console.Clear();
                        Console.WriteLine("Name: " + firstname + " " + lastname + " Email: " + email);

                        Console.Write("Customer " + count + ". Phone: ");
                        command.Parameters.AddWithValue(phone = Console.ReadLine());
                        Console.Clear();
                        Console.WriteLine("Name: " + firstname + " " + lastname + " Email: " + email + " Phone: " + phone);
                        bool dob = true;
                        while (dob)
                        {

                            Console.Write("Customer " + count + ". Date of Birth: ");
                            if (!DateOnly.TryParse(Console.ReadLine(), out DateOnly DoB))
                            {
                                await Console.Out.WriteLineAsync("Wrong date format YYYY-MM-DD.");
                                continue;
                            }
                            dob = false;
                            command.Parameters.AddWithValue(DoB);
                            Console.WriteLine();
                        }

                        int? newCustomerId = (int?)await command.ExecuteScalarAsync();

                        await using (var comm = db.CreateCommand($"INSERT INTO customer_x_booking (booking_id, customer_id) VALUES ({lastBookingId}, {newCustomerId})"))
                        {
                            await comm.ExecuteNonQueryAsync();
                        }
                        count++;
                    }
                }
                newbooking = false;
                await Console.Out.WriteLineAsync("Bra jobbat!!!!");
                Console.ReadKey();

                break;
            }

        }
    }

    public async Task Edit()
    {
        Console.Clear();
        Console.WriteLine("Please enter your bookingID: "); // Om inte bookningID finns?
        if (int.TryParse(Console.ReadLine(), out int bookingID))
        {
            bool edit = true;

            while (edit)
            {
                Console.Clear();
                Console.WriteLine("What would you like to edit?");
                Console.WriteLine("1: Change room");
                Console.WriteLine("2: Change check in date");
                Console.WriteLine("3: Change checkout date");
                Console.WriteLine("4: Change extras");
                Console.WriteLine("0: Return to main menu");

                switch (Console.ReadLine())
                {
                    case "1":

                        Console.Clear();

                        await using (var cmd = db.CreateCommand(
                                         $"UPDATE public.booking SET room_id = $1 WHERE id = {bookingID}"))
                        {
                            Console.WriteLine("New room choice: ");
                            if (!int.TryParse(Console.ReadLine(), out int editRoom))
                            {
                                Console.Clear();
                                Console.WriteLine("Wrong input, try again! Press any key to try again");
                                Console.ReadKey();
                                continue;
                            }

                            cmd.Parameters.AddWithValue(editRoom);
                            await cmd.ExecuteNonQueryAsync();

                            Console.Clear();
                            Console.WriteLine("You have now edited the room");
                            Thread.Sleep(3000);
                            Console.Clear();
                        }

                        break;

                    case "2":

                        Console.Clear();

                        await using (var cmd = db.CreateCommand(
                                         $"UPDATE public.booking SET in_date = $1 WHERE id = {bookingID}"))
                        {
                            Console.WriteLine("New Date (checkin): ");


                            if (!DateOnly.TryParse(Console.ReadLine(), out DateOnly dateIn))
                            {
                                Console.Clear();
                                Console.WriteLine("Wrong input, try again! Press any key to try again");
                                Console.WriteLine("YYYY-MM-DD");
                                Console.ReadKey();
                                continue;
                            }

                            cmd.Parameters.AddWithValue(dateIn);
                            await cmd.ExecuteNonQueryAsync();
                            break;

                        }


                    case "3":

                        Console.Clear();

                        await using (var cmd = db.CreateCommand(
                                         $"UPDATE public.booking SET out_date = $1 WHERE id = {bookingID}"))
                        {
                            Console.WriteLine("New Date (checkout): ");
                            if (!DateOnly.TryParse(Console.ReadLine(), out DateOnly dateOut))
                            {
                                Console.Clear();
                                Console.WriteLine("Wrong input, try again! Press any key to try again");
                                Console.WriteLine("YYYY-MM-DD");
                                Console.ReadKey();
                                continue;
                            }

                            cmd.Parameters.AddWithValue(dateOut);
                            await cmd.ExecuteNonQueryAsync();
                            break;
                        }


                    case "4":

                        Console.Clear();


                        string qViewExtras = @$"
                                        SELECT extras.name, booking_x_extras.booking_id, extras.id
                                        FROM extras
                                        JOIN booking_x_extras ON extras.id = booking_x_extras.extras_id
                                        WHERE booking_id = {bookingID}
                                         ";


                        await using (var cmd = db.CreateCommand(qViewExtras))
                        await using (var reader = await cmd.ExecuteReaderAsync())
                        {
                            Console.WriteLine("Your current extras: ");
                            while (await reader.ReadAsync())
                            {
                                Console.WriteLine("ID: " + reader.GetInt32(2));
                                Console.WriteLine("Label: " + reader.GetString(0));
                                Console.WriteLine();
                            }
                        }
                        Console.WriteLine();
                        Console.WriteLine();
                        Console.ReadKey();



                        Console.WriteLine("1: Add extras"); // Ska inte gå att lägga till om redan finns! Hur göras om två "extras" finns på samma bokning?
                        Console.WriteLine("2: Delete extras");
                        Console.WriteLine("3: View avalible extras");
                        Console.WriteLine("0: Return to main menu");


                        switch (Console.ReadLine())
                        {

                            case "1":// får inte till rätt query för att kolla om den redan finns eller inte?

                                string qAvailableViewExtras = @$"
                                        SELECT extras.name, price, extras.id
                                        FROM extras";

                                await using (var cmd = db.CreateCommand(qAvailableViewExtras))
                                await using (var reader = await cmd.ExecuteReaderAsync())
                                {
                                    Console.Clear();
                                    Console.WriteLine("Available extras: ");
                                    while (await reader.ReadAsync())
                                    {
                                        Console.WriteLine("ID: " + reader.GetInt32(2));
                                        Console.WriteLine("Label: " + reader.GetString(0));
                                        Console.WriteLine("Price: " + reader.GetDecimal(1));
                                        Console.WriteLine();
                                    }
                                }

                                Console.WriteLine("Which extra would you like to add? (Enter the extrasID)");
                                if (int.TryParse(Console.ReadLine(), out int extrasIDs))
                                {
                                    await using (var cmd = db.CreateCommand(@$"
                                    INSERT INTO public.booking_x_extras (booking_id, extras_id)
                                    VALUES
                                    ({bookingID},{extrasIDs});"))

                                    {
                                        await cmd.ExecuteNonQueryAsync();
                                    }
                                    Console.WriteLine($"Added extra with ID: {extrasIDs}");
                                    Thread.Sleep(3000);
                                    Console.Clear();
                                }
                                else
                                {
                                    Console.WriteLine("Extras does not exist, try again");
                                    Console.WriteLine("Press any key to return to menu");
                                    Console.ReadKey();
                                    Console.Clear();
                                }

                                break;


                            case "2": // Finns det mer än en extra av samma typ tas alla bort. Tex finns 2 st "ID 1" tas båda bort om man väljer "1"
                                Console.Clear();
                                await using (var cmd = db.CreateCommand(qViewExtras))
                                await using (var reader = await cmd.ExecuteReaderAsync())
                                {
                                    Console.WriteLine("Your current extras: ");
                                    while (await reader.ReadAsync())
                                    {
                                        Console.WriteLine("ID: " + reader.GetInt32(2));
                                        Console.WriteLine("Label: " + reader.GetString(0));
                                        Console.WriteLine();
                                    }
                                }
                                Console.WriteLine();
                                Console.WriteLine();

                                Console.WriteLine("Which extra would you like to delete? (Enter the extrasID)");
                                if (int.TryParse(Console.ReadLine(), out int extrasID))
                                {
                                    await using (var cmd = db.CreateCommand(@$"
                                    DELETE FROM public.booking_x_extras 
                                    WHERE booking_x_extras.extras_id = {extrasID} AND booking_id = {bookingID};"))
                                    {
                                        await cmd.ExecuteNonQueryAsync();
                                    }

                                    Console.WriteLine($"You deleted extra with ID: {extrasID}");
                                    Thread.Sleep(3000);
                                    Console.Clear();
                                }
                                else
                                {
                                    Console.WriteLine("Extras does not exist, try again");
                                    Console.WriteLine("Press any key to return to menu");
                                    Console.ReadKey();
                                    Console.Clear();
                                }

                                break;

                            case "3":

                                string qAllViewExtras = @$"
                                        SELECT extras.name, price, extras.id
                                        FROM extras";

                                await using (var cmd = db.CreateCommand(qAllViewExtras))
                                await using (var reader = await cmd.ExecuteReaderAsync())
                                {
                                    Console.WriteLine("All extras: ");
                                    while (await reader.ReadAsync())
                                    {
                                        Console.WriteLine("ID: " + reader.GetInt32(2));
                                        Console.WriteLine("Label: " + reader.GetString(0));
                                        Console.WriteLine("Price: " + reader.GetDecimal(1));
                                        Console.WriteLine();
                                    }
                                }

                                Console.ReadKey();

                                break;

                            case "0":
                                Console.Clear();
                                edit = false;
                                break;


                            default:

                                Console.WriteLine("Invalid option");
                                Console.WriteLine("Press any key to return to menu");
                                Console.ReadKey();
                                Console.Clear();
                                continue;
                                throw new Exception("CRASH!");
                        }



                        break;



                    case "0":
                        Console.Clear();
                        edit = false;
                        break;


                    default:
                        Console.WriteLine("Invalid option");
                        Console.WriteLine("Press any key to return to menu");
                        Console.ReadKey();
                        Console.Clear();
                        continue;
                        throw new Exception("CRASH!");
                }
            }
        }
        else
        {
            Console.WriteLine("Wrong input please try again. Enter a bookingID (number) or 0 to exit");
            Console.WriteLine("Press any key to return to menu");
            Console.ReadKey();
            Console.Clear();
        }
    }

    public async Task Delete()
    {
        Console.WriteLine("Which booking would you like to delete? (Enter the bookingID)");
        if (int.TryParse(Console.ReadLine(), out int bookingID)) // Lade till en if till tryparse
        {

            await using (var cmd = db.CreateCommand($"DELETE FROM booking WHERE booking.id = {bookingID}"))
            {
                await cmd.ExecuteNonQueryAsync();
            }

        }
        else // Lade till en else till tryparse
        {
            Console.WriteLine("Booking does not exist, try again");
            Console.WriteLine("Press any key to return to menu");
            Console.ReadKey();
            Console.Clear();
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

        if (int.TryParse(Console.ReadLine(),
                out int input)) // Lade till en if med tryparse för att kunna hantera oväntade inputs
        {
            //int input = int.Parse(Console.ReadLine());
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


            await using (var cmd = db.CreateCommand(
                             $@" SELECT * FROM public.resort JOIN room ON resort_id = resort.id {orderByResult};"))
            {
                await cmd.ExecuteNonQueryAsync();
            }
        }

        else
        {
            Console.WriteLine("Wrong input, please try again");
            Console.ReadKey();
            Console.Clear();
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