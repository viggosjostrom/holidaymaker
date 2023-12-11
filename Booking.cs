using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace holidaymaker;

public class Booking
{
    public string? firstName = string.Empty;
    public string? lastName = string.Empty;
    public void New()

    {
        Console.WriteLine("Skriv namn");
        firstName = Console.ReadLine();
        Console.WriteLine("Skriv efternamn");
        lastName = Console.ReadLine();







        Console.WriteLine("innan break");
        Console.ReadKey();

    }

    public void Edit()
    {

    }

    public void Delete()
    {

    }


}

