using holidaymaker;
using Npgsql;
using System.Linq.Expressions;
using System.Runtime.InteropServices;

//await SetupDB.NewDB();

while (true)
{

    Console.WriteLine("1: New booking");
    Console.WriteLine("2: Delete booking");
    Console.WriteLine("3: Edit booking");
    Console.WriteLine("0: EXIT");
    switch (Console.ReadLine())

    {
        case "1":

            Booking customer = new Booking();
            customer.New();
            break;

        case "2":

            break;

        case "3":

            break;


        case "0":
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
