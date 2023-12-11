using holidaymaker;
using Npgsql;
using System.Linq.Expressions;
using System.Runtime.InteropServices;

//await SetupDB.NewDB();

while (true)
{

    Console.WriteLine("1: Register new customer");
    Console.WriteLine("2: New booking");
    Console.WriteLine("3: Edit booking");
    Console.WriteLine("4: Delete booking");
    Console.WriteLine("0: EXIT");
    switch (Console.ReadLine())

    {
        case "1":

            Register r = new Register();
            r.Customer();
            continue;

        case "2":
            Booking b = new Booking();
            b.New();
            continue;

        case "3":


            Booking e = new Booking();
            e.Edit(); 
            continue;

        case "4":

            continue;


        case "0":
            System.Environment.Exit(666);
            Console.Clear();
            break;

        default:
            Console.WriteLine("Invalid option");
            Console.WriteLine("Press any key to return to main menu");
            Console.ReadKey();
            //menu = true;
            Console.Clear();
            continue;
            throw new Exception("unexpected CRASH!");
    }

}
