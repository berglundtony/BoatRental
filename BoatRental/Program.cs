using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace BoatRental
{
    class Program
    {
        static void Main(string[] args)
        {
            int option = 0;
            while (option != 3)
            {
                Console.Clear();
                Console.WriteLine();
                Console.WriteLine("Välj ett alternativ (skriv 1, 2 eller 3 och tryck enter):\r\n ");
                Console.WriteLine("1. Välj admin");
                Console.WriteLine("2. Välj bokning");
                Console.WriteLine("3. Avsluta\r\n");


                if (int.TryParse(Console.ReadLine(), out option))
                {
                    if (option == 1)
                    {
                        Admin.ShowMenu();
                    }
                    else if (option == 2)
                    {
                        Rental.OptionsRentalMenu();
                    }
                    else
                    {
                        return;
                    }

                }
                else
                {
                    Console.WriteLine("Valet måste vara en siffra");
                    Console.WriteLine();
                    Console.WriteLine(" Återgå till meny - tryck valfri tangent");
                    Console.ReadKey();
                }


            }
            return;
        }

        public static void Start()
        {
            int option = 0;
            while (option != 3)
            {
                Console.Clear();
                Console.WriteLine();
                Console.WriteLine("Välj ett alternativ (skriv 1, 2 eller 3 ocg tryck enter):\r\n ");
                Console.WriteLine("1. Välj admin");
                Console.WriteLine("2. Välj bokning");
                Console.WriteLine("3. Avsluta\r\n");


                if (int.TryParse(Console.ReadLine(), out option))
                {
                    if (option == 1)
                    {
                        Admin.ShowMenu();
                    }
                    else if (option == 2)
                    {
                        Rental.OptionsRentalMenu();
                    }
                    else
                    {
                        System.Environment.Exit(-1);
                    }

                }
                else
                {
                    Console.WriteLine("Valet måste vara en siffra");
                    Console.WriteLine();
                    Console.WriteLine(" Återgå till meny - tryck valfri tangent");
                    Console.ReadKey();
                }

            }
 
        }

    }
}
