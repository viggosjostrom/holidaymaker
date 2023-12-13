using holidaymaker;
using Npgsql;
using System.Linq.Expressions;
using System.Runtime.InteropServices;

//await SetupDB.NewDB();
string dbUri = "Host=localhost;Port=5455;Username=postgres;Password=postgres;Database=holidaymaker"; //Inloggning till databasen port, password osv
await using var db = NpgsqlDataSource.Create(dbUri);

while (true)
{
    Console.WriteLine("1: Register new customer");
    Console.WriteLine("2: New booking");
    Console.WriteLine("3: Edit booking");
    Console.WriteLine("4: Delete booking");
    Console.WriteLine("5: Search room");
    Console.WriteLine("0: EXIT");
    
    if (int.TryParse(Console.ReadLine(), out int userInput))
    {
    switch (userInput)
    {
        case 1:

            Register r = new Register(db);
            await r.Customer();
            continue;

        case 2:
            Booking b = new Booking(db);
            await b.New();
            continue;

        case 3:

            Booking e = new Booking(db);
            await e.Edit(); 
            continue;

        case 4:
            Booking d = new Booking(db);
            await d.Delete();
            continue;

        case 5:
            Booking s = new Booking(db);
            await s.OrderBy();
            continue;

        case 0:
            System.Environment.Exit(666);
            Console.Clear();
            break;

        default:
            Console.WriteLine("Invalid option.\nPress any key to return to main menu");
            Console.ReadKey();
            Console.Clear();
            continue;
            throw new Exception("unexpected CRASH!");
    }

    }
    else
    {
        // Lade till detta eftersom jag har lagt in en if(tryparse) så om man gör fel så ska detta dyka upp
        Console.WriteLine("Wrong input, please try again. Enter a number between 1 - 5 for and option or enter 0 to exit the program.\n");
        Console.Write("Press any key to go back to main menu");
        Console.ReadKey();
        continue;
    }
    // Är osäker på om vi behöver detta eller vad det ska stå här.
    throw new Exception("Program not loaded, shuting down");
}
