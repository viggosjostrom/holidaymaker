using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace holidaymaker;

public class Ending
{
    
    public static void Text()
    {
        Console.WriteLine(" __  __ _____ ____  ______   __    __  __     __  __    _    ____  ");
        Console.WriteLine("|  \\/  | ____|  _ \\|  _ \\ \\ / /    \\ \\/ /    |  \\/  |  / \\  / ___| ");
        Console.WriteLine("| |\\/| |  _| | |_) | |_) \\ V /      \\  /_____| |\\/| | / _ \\ \\___ \\ ");
        Console.WriteLine("| |  | | |___|  _ <|  _ < | |       /  \\_____| |  | |/ ___ \\ ___) |");
        Console.WriteLine("|_|  |_|_____|_| \\_\\_| \\_\\|_|      /_/\\_\\    |_|  |_/_/   \\_\\____/ ");
    }

    public static void PlayMelody()
    {

        int E5 = 659;
        int G5 = 784;
        int C5 = 523;
        int D5 = 587;
        int E4 = 329;
        int C4 = 261;
        int D4 = 294;
        int F5 = 698;

        int duration = 400;
        int delay = 250;

        Console.Beep(E5, duration);
        Console.Beep(E5, duration);
        Console.Beep(E5, duration);

        Thread.Sleep(delay);

        Console.Beep(E5, duration);
        Console.Beep(E5, duration);
        Console.Beep(E5, duration);

        Thread.Sleep(delay);

        Console.Beep(E5, duration);
        Console.Beep(G5, duration);
        Console.Beep(C5, duration);
        Console.Beep(D5, duration);
        Console.Beep(E5, duration);

        Console.Beep(C4, duration);
        Console.Beep(D4, duration);
        Console.Beep(E4, duration);

        Console.Beep(F5, duration);
        Console.Beep(F5, duration);
        Console.Beep(F5, duration);

        Thread.Sleep(delay);

        Console.Beep(F5, duration);
        Console.Beep(E5, duration);
        Console.Beep(E5, duration);

        Thread.Sleep(delay);

        Console.Beep(E5, duration);
        Console.Beep(D5, duration);
        Console.Beep(D5, duration);
        Console.Beep(E5, duration);
        Console.Beep(D5, duration);

        Thread.Sleep(delay);

        Console.Beep(G5, duration);
    }

}




