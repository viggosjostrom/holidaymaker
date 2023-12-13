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
    switch (Console.ReadLine())

    {
        case "1":

            Register r = new Register(db);
            await r.Customer();
            continue;

        case "2":
            Booking b = new Booking(db);
            await b.New();
            continue;

        case "3":

            Booking e = new Booking(db);
            await e.Edit(); 
            continue;

        case "4":
            Booking d = new Booking(db);
            await d.Delete();
            continue;

        case "5":
            Booking s = new Booking(db);
            await s.OrderBy();
            continue;

        case "0":
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
