using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace holidaymaker;

public class Register(NpgsqlDataSource db)
{
    public async Task Customer()
    {
        bool customer = true;
        while (customer)
        {
            await using (var cmd = db.CreateCommand("INSERT INTO customer (firstname, lastname, email, phone, date_of_birth) VALUES (@FirstName, @LastName, @Email, @Phone, @DateOfBirth)"))
            {
                Console.Write("Firstname: ");
                string? fName = Console.ReadLine();
                if (fName.Length > 1)
                {
                    cmd.Parameters.AddWithValue("@Firstname", fName);
                }
                else
                {
                    Console.Clear();
                    Console.WriteLine("You must enter a name");
                    continue;
                }
                Console.WriteLine();

                Console.Write("Lastname: ");
                string? lName = Console.ReadLine();
                if (lName.Length > 1)
                {
                    cmd.Parameters.AddWithValue("@LastName", lName);
                }
                else
                {
                    Console.WriteLine("You must enter a lastname");
                    Console.Clear();
                    continue;
                }
                Console.WriteLine();

                Console.Write("Email: ");
                string? eMail = Console.ReadLine();
                if (eMail.Length > 4)
                {
                    cmd.Parameters.AddWithValue("@Email", eMail);
                }
                else
                {
                    Console.WriteLine("You must enter a email");
                    Console.Clear();
                    continue;
                }
                Console.WriteLine();

                Console.Write("Phonenumber: ");
                string? phone = Console.ReadLine();
                if (phone.Length > 4)
                {
                    cmd.Parameters.AddWithValue("@Phone", phone);
                }
                else
                {
                    Console.WriteLine("You must enter a phonenumber");
                    Console.Clear();
                    continue;
                }
                Console.WriteLine();

                Console.Write("Date of birth (YYYY-MM-DD): ");
                if (DateOnly.TryParse(Console.ReadLine(), out DateOnly date))
                {
                    cmd.Parameters.AddWithValue("@DateOfBirth", date);
                }
                else
                {
                    Console.WriteLine("Please use the format YYYY-MM-DD");
                    Console.Clear();
                    continue;
                }
                Console.WriteLine();

                await cmd.ExecuteNonQueryAsync();
                customer = false;
            }
            Console.WriteLine("Register successful! Press any key to continue..");
            Console.ReadKey();
            Console.Clear();
        }
    }
}
