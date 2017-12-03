using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace BoatRental
{
    public static class Admin
    {
        public static void ShowMenu()
        {
            int option = 0;
            Console.Clear();
            Console.WriteLine(" ================================");
            Console.WriteLine(" Välj tjänst");
            Console.WriteLine(" ================================");
            Console.WriteLine(" 1. Se kategorier");
            Console.WriteLine(" 2. Lägg till kategori");
            Console.WriteLine(" 3. Lägg till ny båt");
            Console.WriteLine(" 4. Se alla båtar");
            Console.WriteLine(" 5. Lägg till pris");
            Console.WriteLine(" 6. Ändra pris");
            Console.WriteLine(" 7. Se priser");
            Console.WriteLine(" 8. Avsluta\r\n");

            option = int.Parse(Console.ReadLine());

            while (option != 9)
            {
                if (option == 1)
                {
                    ShowAllCategories();
                }
                else if (option == 2)
                {
                    CreateCategories();
                }
                else if (option == 3)
                {
                    CreateNewBoat();
                }
                else if (option == 4)
                {
                    ShowAllBoats();
                }
                else if (option == 5)
                {
                    AddPrice();
                }
                else if (option == 6)
                {
                    ChangePrice();
                }
                else if (option == 7)
                {
                    ShowPrice();
                }
                else
                {
                    Program.Start();
                }
            }
        }

        private static void ShowAllCategories()
        {
            Console.Clear();
            Console.WriteLine("===============================");
            Console.WriteLine("Kategorier");
            Console.WriteLine("===============================");

            try
            {
                using (var DB = new Data.BoatBookingSystemEntities1())
                {
                    var categories = DB.Categories.ToList();

                    foreach (var r in categories)
                    {
                        Console.WriteLine("***************************************************************");
                        Console.WriteLine(" ID: " + r.CatID);
                        Console.WriteLine(" Namn: " + r.Name);

                        if (r.Name == "Segelbåt")
                        {
                            if (r.OverSizeFourty == true)
                            {
                                Console.WriteLine(" Över eller lika med 40 tum.");
                            }
                            else
                            {
                                Console.WriteLine(" mindre än 40 tum.");
                            }
                        }
                        Console.WriteLine();
                    }
                }
            }
            catch (Exception ex)
            {
                Console.Write("Can't find categories because of, \"{0}\" .", ex.Message);
            }
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine(" Återgå till meny - tryck valfri tangent");
            Console.ReadKey();
            ShowMenu();
        }

        public static void CreateCategories()
        {
            var category = new Data.Category();

            {
                Console.WriteLine(" Är det säkert att du vill lägga till en ny kategori?");
                Console.WriteLine("Avsluta: j/n");
                string exit = Console.ReadLine();
                if (exit == "j")
                {
                    ShowMenu();
                }
                else
                {
                    Console.WriteLine("================================");
                    Console.WriteLine("Lägg till ny kategori");
                    Console.WriteLine("================================");
                    Console.Write("\r\nKategorinamn: ");
                    category.Name = Console.ReadLine();
                    Console.Write("\r\nÄr Storleken över eller lika med 40 fot? j/n: ");
                    string choice = Console.ReadLine();
                    if (choice == "j")
                    {
                        category.OverSizeFourty = true;


                    }
                    else
                    {
                        category.OverSizeFourty = false;
                    }

                    using (var DB = new Data.BoatBookingSystemEntities1())
                    {
                        try
                        {
                            DB.Categories.Add(category);
                            DB.SaveChanges();
                            var id = category.CatID;
                        }
                        catch (Exception ex)
                        {
                            Console.Write("The category could't be saved becouse of \"{0}\" .", ex.Message);

                        }
                    }
                }
            }
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine(" Återgå till meny - tryck valfri tangent");
            Console.ReadKey();
            ShowMenu();
        }

        private static void CreateNewBoat()
        {
            var boat = new Data.Boat();
            int output = 0;

            Console.Clear();
            Console.WriteLine("================================");
            Console.WriteLine("Lägg till ny Båt");
            Console.WriteLine("================================");
            Console.WriteLine("Vilket namn skall båten ha? ");
            boat.Name = Console.ReadLine();

            using (var DB = new Data.BoatBookingSystemEntities1())
            {
                var categories = DB.Categories.ToList();

                foreach (var r in categories)
                {
                    Console.WriteLine("***************************************************************");
                    Console.WriteLine(" ID: " + r.CatID);
                    Console.WriteLine(" Namn: " + r.Name);

                    if (r.Name == "Segelbåt")
                    {
                        if (r.OverSizeFourty == true)
                        {
                            Console.WriteLine(" Över 40 tum.");
                        }
                        else
                        {
                            Console.WriteLine(" mindre än 40 tum.");
                        }
                    }
                    Console.WriteLine();
                }
            }
            Console.WriteLine("\r\nVälj Kategori_id: 1, 2, 3 osv.");

            using (var DB = new Data.BoatBookingSystemEntities1())
            {
                if (int.TryParse(Console.ReadLine(), out output))
                {
                    try
                    {
                        boat.CatID = output;
                        DB.Boats.Add(boat);
                        DB.SaveChanges();
                        ShowAllBoats();
                    }
                    catch (Exception ex)
                    {
                        Console.Write("The boat could't be saved because of \"{0}\" .", ex.Message);
                    }
                }
            }
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine(" Återgå till meny - tryck valfri tangent");
            Console.ReadKey();
        }

        private static void ShowAllBoats()
        {

            using (var DB = new Data.BoatBookingSystemEntities1())
            {
                var boats = DB.Boats.Join(DB.Categories, cat => cat.CatID, boat => boat.CatID, (boat, cat) => new { BoatID = boat.BoatID, BoatName = boat.Name, CatName = cat.Name, SizeOver40Feets = cat.OverSizeFourty }).ToList();
                Console.Clear();

                foreach (var r in boats)
                {

                    Console.WriteLine("***************************************************************");
                    Console.WriteLine(" ID: " + r.BoatID);
                    Console.WriteLine(" Båtnamn: " + r.BoatName);
                    Console.WriteLine(" Kategori: " + r.CatName);

                    if (r.CatName == "Segelbåt")
                    {
                        if (r.SizeOver40Feets == true)
                        {
                            Console.WriteLine(" över eller lika med 40 tum.");
                        }
                        else
                        {
                            Console.WriteLine(" mindre än 40 tum.");
                        }
                    }

                }
                Console.WriteLine();
                Console.WriteLine();
                Console.WriteLine(" Återgå till meny - tryck valfri tangent");
                Console.ReadKey();

            }
        }
        private static void AddPrice()
        {
            var price = new Data.Price();
            decimal output = 0;
            bool exit = false;


            while (!exit)
            {
                Console.WriteLine(" Gå tillbaka till huvudmenyn j/n:");
                string choise = Console.ReadLine();
                if (choise == "j")
                {
                    Program.Start();
                }
                else
                {
                    Console.Clear();
                    Console.WriteLine("================================");
                    Console.WriteLine("Lägg till nytt pris");
                    Console.WriteLine("================================");
                    Console.WriteLine();
                    Console.WriteLine("Ange grundpris: ");

                    using (var DB = new Data.BoatBookingSystemEntities1())
                    {
                        try
                        {
                            if (decimal.TryParse(Console.ReadLine(), out output))
                            {
                                price.BasicFee = output;
                            }
                            Console.WriteLine();
                            Console.WriteLine("Ange timpris: ");
                            if (decimal.TryParse(Console.ReadLine(), out output))
                            {
                                price.HourFee = output;
                            }
                            Console.WriteLine("================================");
                            Console.WriteLine("Lägg till decimaltal för multipliceringssats för stor båt.");
                            Console.WriteLine("================================");
                            Console.WriteLine();
                            Console.WriteLine("Ange hur mycket grundpriset på en båt över eller lika med 40 fot skall multipliceras med: ");
                            Console.WriteLine();
                            if (decimal.TryParse(Console.ReadLine(), out output))
                            {
                                price.BasicPriceBigBoatAlgorithm = output;
                            }
                            Console.WriteLine("Ange hur mycket timpriset på en båt över eller lika med 40 fot skall multipliceras med: ");
                            Console.WriteLine();
                            if (decimal.TryParse(Console.ReadLine(), out output))
                            {
                                price.HourPriceBigBoatAlgorithm = output;
                            }
                            Console.WriteLine("================================");
                            Console.WriteLine("Lägg till decimaltal för multipliceringssats för liten båt.");
                            Console.WriteLine("================================");
                            Console.WriteLine();
                            Console.WriteLine("Ange hur mycket grundpriset på en båt under 40 fot skall multipliceras med: ");
                            Console.WriteLine();
                            if (decimal.TryParse(Console.ReadLine(), out output))
                            {
                                price.BasicPriceSmallBoatAlgorithm = output;
                            }
                            Console.WriteLine("Ange hur mycket timpriset på en båt under 40 fot skall multipliceras med: ");
                            DB.Prices.Add(price);
                            DB.SaveChanges();
                        }
                        catch (Exception ex)
                        {
                            Console.Write("The price could't be saved because of \"{0}\" .", ex.Message);
                        }
                    }
                }
            }
        }
        private static void ChangePrice()
        {
            using (var DB = new Data.BoatBookingSystemEntities1())
            {
                var prices = DB.Prices.ToList();
                Console.Clear();

                foreach (var p in prices)
                {
                    Console.WriteLine("***************************************************************");
                    Console.WriteLine(" Grundavgift: " + p.BasicFee);
                    Console.WriteLine(" Timavgift: " + p.HourFee);
                }
            }

        }

        private static void ShowPrice()
        {
            using (var DB = new Data.BoatBookingSystemEntities1())
            {
                var prices = DB.Prices.ToList();
                Console.Clear();

                foreach (var p in prices)
                {
                    Console.WriteLine("***************************************************************");
                    Console.WriteLine(" Grundavgift: " + p.BasicFee);
                    Console.WriteLine(" Timavgift: " + p.HourFee);
                }
            }
        }
    }
}


