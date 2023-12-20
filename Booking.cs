using Npgsql;
using System.ComponentModel.Design;
using System.Data.SqlTypes;
using System.Runtime.InteropServices;
using System.Transactions;

namespace holidaymaker;

public class Booking(NpgsqlDataSource db)
{
    public async Task New(DateOnly in_date, DateOnly out_date)
    {
        bool resort = true;
        bool room = false;
        bool customer = true;
        bool datefirst = false;
        bool dateSecond = false;
        while (true)
        {
            await using (var cmd = db.CreateCommand("INSERT INTO booking (resort_id, room_id, in_date, out_date) VALUES ($1, $2, $3, $4) RETURNING id"))
            {
                if (resort)
                {
                    Console.Write("Enter resort id: ");
                    if (int.TryParse(Console.ReadLine(), out int resort_id))
                    {
                        await using (var resortCheck = db.CreateCommand("SELECT COUNT(*) FROM public.resort WHERE id = $1"))
                        {
                            resortCheck.Parameters.AddWithValue(resort_id);
                            long? numberCheck = (long?)await resortCheck.ExecuteScalarAsync();
                            if (numberCheck > 0)
                            {
                                cmd.Parameters.AddWithValue(resort_id);
                                resort = false;
                                room = true;
                            }
                            else
                            {
                                Console.WriteLine("Resort id does not exist.");
                                continue;
                            }
                        }
                    }
                    else
                    {
                        Console.WriteLine("Wrong input");
                        continue;
                    }
                    if (room)
                    {
                        Console.Write("Enter room id: ");
                        if (int.TryParse(Console.ReadLine(), out int room_id))
                        {
                            await using (var roomCheck = db.CreateCommand("SELECT COUNT(*) FROM public.room WHERE id = $1"))
                            {
                                roomCheck.Parameters.AddWithValue(room_id);
                                long? numberCheck = (long?)await roomCheck.ExecuteScalarAsync();
                                if (numberCheck > 0)
                                {
                                    cmd.Parameters.AddWithValue(room_id);
                                    room = false;
                                    datefirst = true;
                                    Console.Clear();
                                }
                                else
                                {
                                    Console.WriteLine("Room id does not exist.");
                                    continue;
                                }
                            }
                        }
                        else
                        {
                            Console.WriteLine("Wrong input");
                            continue;
                        }
                    }
                    if (datefirst)
                    {
                        cmd.Parameters.AddWithValue(in_date);
                        datefirst = false;
                        dateSecond = true;
                    }
                    if (dateSecond)
                    {
                        cmd.Parameters.AddWithValue(out_date);
                        Console.Clear();
                    }
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
                        await using (var command =
                                     db.CreateCommand($"INSERT INTO customer (firstname, lastname, email, phone, date_of_birth) VALUES ($1, $2, $3, $4, $5) RETURNING id"))
                        {
                            string? firstname = string.Empty;
                            string? lastname = string.Empty;
                            string? email = string.Empty;
                            string? phone = string.Empty;

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
                    await Console.Out.WriteLineAsync("Du har nu genomfört bokningen!");
                    break;
                }
            }
        }
    }

    public async Task Edit()
    {
        Console.Clear();
        Console.WriteLine("Please enter your bookingID: ");
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
                if (int.TryParse(Console.ReadLine(), out int eRoom))
                {
                    switch (eRoom)
                    {
                        case 1:
                            Console.Clear();
                            await using (var cmd = db.CreateCommand($"UPDATE public.booking SET room_id = $1 WHERE id = {bookingID}"))
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
                                Console.WriteLine("You have now edited the room, press any key to continue");
                                Console.ReadKey();
                                Console.Clear();
                            }
                            break;

                        case 2:
                            Console.Clear();
                            await using (var cmd = db.CreateCommand($"UPDATE public.booking SET in_date = $1 WHERE id = {bookingID}"))
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

                        case 3:
                            Console.Clear();
                            await using (var cmd = db.CreateCommand($"UPDATE public.booking SET out_date = $1 WHERE id = {bookingID}"))
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

                        case 4:
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
                            Console.WriteLine("\n\n1: Add extras");
                            Console.WriteLine("2: Delete extras");
                            Console.WriteLine("3: View avalible extras");
                            Console.WriteLine("0: Return to main menu");
                            if (int.TryParse(Console.ReadLine(), out int extry))
                            {
                                switch (extry)
                                {
                                    case 1:
                                        string qAvailableViewExtras = @$"SELECT extras.name, price, extras.id FROM extras";
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
                                    INSERT INTO public.booking_x_extras (booking_id, extras_id) VALUES ({bookingID},{extrasIDs});"))
                                            {
                                                await cmd.ExecuteNonQueryAsync();
                                            }
                                            Console.WriteLine($"Added extra with ID: {extrasIDs}");
                                            Console.WriteLine("Press any key to continue");
                                            Console.ReadKey();
                                            Console.Clear();
                                        }
                                        else
                                        {
                                            Console.WriteLine("Invalid option.\nPress any key to return to main menu");
                                            Console.ReadKey();
                                            Console.Clear();
                                            continue;
                                        }
                                        break;

                                    case 2:
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

                                        Console.WriteLine("\n\nWhich extra would you like to delete? (Enter the extrasID)");
                                        if (int.TryParse(Console.ReadLine(), out int extrasID))
                                        {
                                            await using (var cmd = db.CreateCommand(@$"
                                    DELETE FROM public.booking_x_extras WHERE booking_x_extras.extras_id = {extrasID} AND booking_id = {bookingID};"))
                                            {
                                                await cmd.ExecuteNonQueryAsync();
                                            }
                                            Console.WriteLine($"You deleted extra with ID: {extrasID}");
                                            Console.WriteLine("Press any key to continue");
                                            Console.ReadKey();
                                            Console.Clear();
                                        }
                                        else
                                        {
                                            Console.WriteLine("Invalid option.\nPress any key to return to main menu");
                                            Console.ReadKey();
                                            Console.Clear();
                                            continue;
                                        }
                                        break;

                                    case 3:
                                        string qAllViewExtras = @$"SELECT extras.name, price, extras.id FROM extras";
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
                                        Console.WriteLine("Press any key to continue");
                                        Console.ReadKey();
                                        break;

                                    case 0:
                                        Console.Clear();
                                        edit = false;
                                        break;

                                    default:
                                        Console.WriteLine("Invalid option.\nPress any key to return to main menu");
                                        Console.ReadKey();
                                        Console.Clear();
                                        continue;
                                }
                                break;
                                throw new Exception("Unknown error");
                            }
                            else
                            {
                                Console.WriteLine("Wrong input.\nPress any key to return to main menu");
                                Console.ReadKey();
                                Console.Clear();
                                continue;
                            }

                        case 0:
                            Console.Clear();
                            edit = false;
                            break;

                        default:
                            Console.WriteLine("Invalid option.\nPress any key to return to main menu");
                            Console.ReadKey();
                            Console.Clear();
                            continue;
                    }
                    throw new Exception("Unknown error");
                }
                else
                {
                    Console.WriteLine("Wrong input.\nPress any key to return to main menu");
                    Console.ReadKey();
                    Console.Clear();
                    continue;
                }
            }
        }
        else
        {
            Console.WriteLine("Invalid option.\nPress any key to return to main menu");
            Console.ReadKey();
            Console.Clear();
        }
    }

    public async Task Delete()
    {
        while (true)
        {
            Console.Clear();
            Console.WriteLine("Which booking would you like to delete? (Enter the bookingID)");
            if (int.TryParse(Console.ReadLine(), out int bookingID))
            {
                Console.Clear();
                Console.WriteLine("1: Yes.");
                Console.WriteLine("2: No. ");
                if (int.TryParse(Console.ReadLine(), out int delete))
                if (delete == 1)
                {
                    await using (var cmd2 = db.CreateCommand($"DELETE FROM public.booking WHERE id = {bookingID}"))
                    {
                        await using (var cmd1 = db.CreateCommand($"DELETE FROM public.customer_x_booking WHERE booking_id = {bookingID}"))
                        {
                            await cmd1.ExecuteNonQueryAsync();
                        }
                        await cmd2.ExecuteNonQueryAsync();
                    }
                    Console.Clear();
                    break;
                }
                else if (delete == 2)
                {
                    Console.Clear();
                    break;
                }
                else
                {
                    Console.Clear();
                    Console.WriteLine("Wrong input!");
                    break;
                }
            }
            else
            {
                Console.WriteLine("Invalid option.\nPress any key to return to main menu");
                Console.ReadKey();
                Console.Clear();
            }
        }
    }
}
