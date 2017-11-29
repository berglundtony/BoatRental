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
        //public static object HttpContext { get; private set; }

        static void Main(string[] args)
        {
            int option = 0;
            while (option != 3)
            {
                Console.Clear();
                Console.WriteLine("Välj ett alternativ (skriv 1, 2 eller 3 och tryck enter):\r\n ");
                Console.WriteLine("1. Välj admin");
                Console.WriteLine("2. Välj bokning");
                Console.WriteLine("3. Avsluta\r\n");

                option = int.Parse(Console.ReadLine());

                if (option == 1)
                {
                    Admin.CreateCategories();
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
            return;
        }

        public static void Start()
        {
            int option = 0;
            while (option != 3)
            {
                Console.Clear();
                Console.WriteLine("Välj ett alternativ (skriv 1, 2 eller 3 ocg tryck enter):\r\n ");
                Console.WriteLine("1. Välj admin");
                Console.WriteLine("2. Välj bokning");
                Console.WriteLine("3. Avsluta\r\n");

                option = int.Parse(Console.ReadLine());

                if (option == 1)
                {
                    Admin.CreateCategories();

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
            System.Environment.Exit(-1);
        }

    }
}
