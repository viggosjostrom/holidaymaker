using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace holidaymaker;

public class Register(NpgsqlDataSource db)
{

    string dbUri = "Host=localhost;Port=5455;Username=postgres;Password=postgres;Database=holidaymaker"; //Inloggning till databasen port, password osv



    public string? firstName = string.Empty;
    public string? lastName = string.Empty;
    public string? email = string.Empty;
    public string? phone = string.Empty;
    public string? dateOfBirth = string.Empty;

    public async Task Customer()

    {


        await using (var cmd = db.CreateCommand("INSERT INTO customer (firstname, lastname, email, phone, date_of_birth) VALUES ($1, $2, $3, $4, $5)"))
        {

            Console.Write("Firstname: ");
            cmd.Parameters.AddWithValue(Console.ReadLine());
            Console.WriteLine();

            Console.Write("Lastname: ");
            cmd.Parameters.AddWithValue(Console.ReadLine());
            Console.WriteLine();

            Console.Write("Email: ");
            cmd.Parameters.AddWithValue(Console.ReadLine());
            Console.WriteLine();

            Console.Write("Phonenumber: ");
            cmd.Parameters.AddWithValue(Console.ReadLine());
            Console.WriteLine();

            Console.Write("Date of birth (YYYY-MM-DD): ");
            DateOnly.TryParse(Console.ReadLine(), out DateOnly date);
            cmd.Parameters.AddWithValue(date);
            Console.WriteLine();


            await cmd.ExecuteNonQueryAsync();
        }



        Console.WriteLine("innan break");
        Console.ReadKey();

    }

}
