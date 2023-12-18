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
                Console.Clear();
                Console.WriteLine("1: Custom search");
                Console.WriteLine("2: See all (OrderBy)");
                if (int.TryParse(Console.ReadLine(), out int userInput2))
                {
                    switch (userInput2)
                    {
                        case 1:
                            Console.Clear();
                            SearchFunctions q = new SearchFunctions(db);
                            await q.AvaliableRooms();
                            break;


                        case 2:
                            Console.Clear();
                            Booking s = new Booking(db);
                            await s.OrderBy();
                            break;
                    }
                }

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
        Console.WriteLine("Wrong input");
        Console.WriteLine("Press any key to go back to main menu");
        Console.ReadKey();
        continue;
    }
    // Är osäker på om vi behöver detta eller vad det ska stå här.
    throw new Exception("Program not loaded, shuting down");
}