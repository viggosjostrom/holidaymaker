using holidaymaker;
using Npgsql;
using System.Linq.Expressions;
using System.Runtime.InteropServices;

//await SetupDB.NewDB();
//kommentera bort för ny Databas setup

string dbUri = "Host=localhost;Port=5455;Username=postgres;Password=postgres;Database=holidaymaker";
await using var db = NpgsqlDataSource.Create(dbUri);

while (true)
{

    Console.WriteLine("1: New booking");
    Console.WriteLine("2: Edit booking");
    Console.WriteLine("3: Delete booking");
    Console.WriteLine("4: Register new customer");
    Console.WriteLine("0: EXIT");

    if (int.TryParse(Console.ReadLine(), out int userInput))
    {
        switch (userInput)
        {

            case 1:
                Console.Clear();
                Search avaliable = new Search(db);
                await avaliable.Rooms();
                continue;

            case 2:
                Booking info = new Booking(db);
                await info.Edit();
                continue;

            case 3:
                Booking booking = new Booking(db);
                await booking.Delete();
                continue;

            case 4:
                Register register = new Register(db);
                await register.Customer();
                continue;

            case 0:
                Console.Clear();
                Ending.Text();
                Ending.PlayMelody();
                System.Environment.Exit(666);
                break;

            default:
                Console.WriteLine("Invalid option.\nPress any key to return to main menu");
                Console.ReadKey();
                Console.Clear();
                continue;
        }

    }
    else
    {
        Console.WriteLine("Invalid option.\nPress any key to return to main menu");
        Console.ReadKey();
        Console.Clear();
        continue;
    }

    throw new Exception("Unknown error");
}
