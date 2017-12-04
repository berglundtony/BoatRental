using BookingBoatSystem;
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
        enum CategoryName { JOLLE, SEGELBÅT };

        public static void ShowMenu()
        {
            int option = 0;
            Console.Clear();
            Console.WriteLine(" ================================");
            Console.WriteLine(" Välj tjänst");
            Console.WriteLine(" ================================");
            Console.WriteLine(" 1. Se kategorier");
            Console.WriteLine(" 2. Lägg till ny kategori");
            Console.WriteLine(" 3. Lägg till ny båt");
            Console.WriteLine(" 4. Ändra data om båt");
            Console.WriteLine(" 5. Se alla båtar");
            Console.WriteLine(" 6. Lägg till pris");
            Console.WriteLine(" 7. Ändra pris");
            Console.WriteLine(" 8. Se priser");
            Console.WriteLine(" 9. Avsluta\r\n");

            if(int.TryParse(Console.ReadLine(), out option))
            {
                while (option < 10)
                {
                    if (option == 1)
                    {
                        ShowCategories();
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
                        ChangeBoat();
                    }
                    else if (option == 5)
                    {
                        ShowAllBoats();
                    }
                    else if (option == 6)
                    {
                        AddPrice();
                    }
                    else if (option == 7)
                    {
                        ChangePrice();
                    }
                    else if (option == 8)
                    {
                        ShowPrices();
                    }
                    else
                    {
                        Program.Start();
                    }
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
        private static void ShowCategories()
        {
            var booking = new Booking();
            List<Data.Category> categoryname = new List<Data.Category>();

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

                        categoryname = booking.GetValuesForCategoryName();

                        foreach (var cat in categoryname)
                        {
                            if (cat.Name == r.Name && cat.OverSizeFourty == r.OverSizeFourty)
                            {
                                if (r.OverSizeFourty == true)
                                {
                                    Console.Write(" över eller lika med 40 tum.");
                                }
                                else
                                {
                                    Console.Write(" mindre än 40 tum.");
                                }
                            }
                        }

                        Console.WriteLine();
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Can't find categories because of, \"{0}\" .", ex.Message);
            }

            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine(" Återgå till meny - tryck valfri tangent");
            Console.ReadKey();
            ShowMenu();

        }

        private static void GetCategories()
        {
            var booking = new Booking();
            List<Data.Category> categoryname = new List<Data.Category>();

            Console.Clear();
            Console.WriteLine("===============================");
            Console.WriteLine("Kategorier");
            Console.WriteLine("===============================");

            try
            {
                using (var DB = new Data.BoatBookingSystemEntities1())
                {
                    var categories = DB.Categories.ToList();
                    categoryname = booking.GetValuesForCategoryName();

                    foreach (var item in categories)
                    {
                        Console.WriteLine("***************************************************************");
                        Console.WriteLine(" ID: " + item.CatID);
                        Console.WriteLine(" Namn: " + item.Name);
                        foreach (var cat in categoryname)
                        {
                            if (cat.Name == item.Name && cat.OverSizeFourty == item.OverSizeFourty)
                            {
                                if (item.OverSizeFourty == true)
                                {
                                    Console.Write(" över eller lika med 40 tum.");
                                }
                                else
                                {
                                    Console.Write(" mindre än 40 tum.");
                                }
                            }
                        }
                        Console.WriteLine();
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Can't find categories because of, \"{0}\" .", ex.Message);
            }

        }

        public static void CreateCategories()
        {
            var category = new Data.Category();

            {
                Console.Clear();
                GetCategories();
                Console.WriteLine();
                Console.WriteLine(" Är det säkert att du vill lägga till en ny kategori? skriv j/n");
                string exit = Console.ReadLine();
                if (exit == "n")
                {
                    ShowMenu();
                }
                else
                {
                    Console.WriteLine(" ================================");
                    Console.WriteLine(" Lägg till ny kategori");
                    Console.WriteLine(" ================================");
                    Console.Write("\r\n Kategorinamn: ");
                    category.Name = Console.ReadLine();
                    Console.Write("\r\n Är Storleken över eller lika med 40 fot? j/n: ");
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
            GetCategories();
            Console.WriteLine("\r\nVälj Kategori_id: 1, 2, 3 osv.");

            using (var DB = new Data.BoatBookingSystemEntities1())
            {
                if (int.TryParse(Console.ReadLine(), out output))
                {
                    boat.CatID = output;
                }
                Console.WriteLine();
                ShowPrices();
                Console.WriteLine("\r\nVälj Pris_id: 1, 2, 3 osv.");
                if (int.TryParse(Console.ReadLine(), out output))
                {
                    boat.PriceID = output;
                }
                try
                {
                    DB.Boats.Add(boat);
                    DB.SaveChanges();
                    ShowAllBoats();
                }
                catch (Exception ex)
                {
                    Console.Write("The boat could't be saved because of \"{0}\" .", ex.Message);
                }

            }
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine(" Återgå till meny - tryck valfri tangent");
            Console.ReadKey();
        }

        private static void ChangeBoat()
        {
            var boat = new Data.Boat();
            int output = 0;
            var choice = "n";
            ShowBoats();
            Console.WriteLine();
            Console.WriteLine("**************** Ändra på data om båt **********************");
            Console.WriteLine();
            Console.WriteLine(" Vilken båt vill du ändra information om? ");
            Console.WriteLine(" Skriv det båt ID som ändringen gäller: ");
            Console.WriteLine();
            using (var DB = new Data.BoatBookingSystemEntities1())
            {
          
                if (int.TryParse(Console.ReadLine(), out output))
                {
                   var currentboat = DB.Boats.Where(x => x.BoatID == output).FirstOrDefault();
                    Console.Write(" Ändra namn på båten, j/n: ");
                    choice = Console.ReadLine();
                    if(choice == "j")
                    {
                        Console.WriteLine();
                        Console.Write(" Ändra namn: ");
                        var boatname = Console.ReadLine();
                        currentboat.Name = boatname;

                        Console.WriteLine();
                        Console.WriteLine(" Ändra kategori till båten, j/n: ");
                        choice = Console.ReadLine();
                        if (choice == "j")
                        {
                            GetCategories();
                            Console.WriteLine();
                            Console.WriteLine(" Ändra categori_id: ");
                            if (int.TryParse(Console.ReadLine(), out output))
                                currentboat.CatID = output;

                            Console.WriteLine();
                            Console.WriteLine(" Ändra priskategori till båten, j/n: ");
                            choice = Console.ReadLine();
                            if (choice == "j")
                            {
                                Console.WriteLine(" Ändra PrisID: ");
                                if (int.TryParse(Console.ReadLine(), out output))
                                    currentboat.PriceID = output;
                            }

                        }
                        else
                        {
                            Console.WriteLine();
                            Console.WriteLine(" Ändra priskategori till båten, j/n: ");
                            choice = Console.ReadLine();
                            if (choice == "j")
                            {
                                Console.WriteLine(" Ändra PrisID: ");
                                if (int.TryParse(Console.ReadLine(), out output))
                                    currentboat.PriceID = output;
                            }
             
                        }
                    }
                    else
                    {
                      
                        Console.WriteLine();
                        Console.Write(" Ändra kategori till båten, j/n: ");
                        choice = Console.ReadLine();
                        if (choice == "j")
                        {
                            GetCategories();
                            Console.WriteLine();
                            Console.Write(" Ändra categori_id: ");
                            if (int.TryParse(Console.ReadLine(), out output))
                            {
                                currentboat.CatID = output;
                            }    
                            else
                            {
                                Console.WriteLine(" Felaktig inmatning categori_id:et har ingen referens ");
                            }
                            Console.WriteLine();
                            Console.WriteLine(" Ändra priskategori till båten, j/n: ");
                            choice = Console.ReadLine();
                            if (choice == "j")
                            {
                                ShowPrices();
                                Console.WriteLine();
                                Console.Write(" Ändra PrisID: ");
                                if (int.TryParse(Console.ReadLine(), out output))
                                    currentboat.PriceID = output;
                            }
                        }
                        else
                        {
                            Console.WriteLine(" Ändra priskategori till båten, j/n: ");
                            choice = Console.ReadLine();
                            if (choice == "j")
                            {
                                Console.WriteLine(" Ändra PrisID: ");
                                if (int.TryParse(Console.ReadLine(), out output))
                                    currentboat.PriceID = output;
                            }
                            else
                            {
                                Console.WriteLine();
                                Console.WriteLine();
                                Console.WriteLine(" Återgå till meny - tryck valfri tangent");
                                Console.ReadKey();
                                ShowMenu();

                            }

                        }
                    }
                }
                try
                {
                    DB.SaveChanges();
                    ShowBoats();
                }
                catch (Exception ex)
                {
                    Console.WriteLine("The boat could't be saved because of \"{0}\" .", ex.Message);
                }
            }
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine(" Återgå till meny - tryck valfri tangent");
            Console.ReadKey();
            ShowMenu();

        }

        private static void ShowAllBoats()
        {
            List<Data.Category> categoryname = new List<Data.Category>();
            var booking = new Booking();

            using (var DB = new Data.BoatBookingSystemEntities1())
            {
                var boats = DB.Boats.Join(DB.Categories, cat => cat.CatID, boat => boat.CatID, (boat, cat) => new { BoatID = boat.BoatID, BoatName = boat.Name, CatName = cat.Name, SizeOver40Feets = cat.OverSizeFourty }).ToList();
                Console.Clear();

                foreach (var r in boats)
                {
                    Console.WriteLine("***************************************************************");
                    Console.WriteLine(" ID: " + r.BoatID);
                    Console.WriteLine(" Båtnamn: " + r.BoatName);
                    Console.Write(" Kategori: " + r.CatName);

                    categoryname = booking.GetValuesForCategoryName();

                    foreach (var cat in categoryname)
                    {
                        if (cat.Name == r.CatName && cat.OverSizeFourty == r.SizeOver40Feets)
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

                }
                Console.WriteLine();
                Console.WriteLine();
                Console.WriteLine(" Återgå till meny - tryck valfri tangent");
                Console.ReadKey();
                ShowMenu();

            }
        }
        private static void ShowBoats()
        {
            List<Data.Category> categoryname = new List<Data.Category>();
            var booking = new Booking();
            var counter = 0;

            using (var DB = new Data.BoatBookingSystemEntities1())
            {
                var boats = DB.Boats.Join(DB.Categories, cat => cat.CatID, boat => boat.CatID, (boat, cat) => new { BoatID = boat.BoatID, BoatName = boat.Name, PriceID = boat.PriceID, CatName = cat.Name, SizeOver40Feets = cat.OverSizeFourty }).ToList();
                Console.Clear();

                foreach (var r in boats)
                {
                    Console.WriteLine("***************************************************************");
                    Console.WriteLine(" ID: " + r.BoatID);
                    Console.WriteLine(" Båtnamn: " + r.BoatName);
                    Console.Write(" Kategori: " + r.CatName);

                    categoryname = booking.GetValuesForCategoryName();

                    foreach (var cat in categoryname)
                    {
                        if (cat.Name == r.CatName && cat.OverSizeFourty == r.SizeOver40Feets)
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
                    Console.WriteLine(" PrisID: " + r.PriceID);
                    counter++;
                    if (counter == boats.Count())
                        break;

                }
            }
        }

        private static void AddPrice()
        {
            var price = new Data.Price();
            decimal output = 0;
            bool exit = false;

            ShowPrices();

            while (!exit)
            {
                Console.WriteLine(" Vill du lägga till en ny priskategori? j/n:");
                string choise = Console.ReadLine();
                if (choise == "n")
                {
                    Console.WriteLine(" Vill du ändra tidigare satta priser j/n:");
                    string changepricechoise = Console.ReadLine();
                    if (changepricechoise == "j")
                    {
                        ChangePrice();
                    }
                    else
                    {
                        ShowMenu();
                    }
                }
                else
                {
                    Console.Clear();
                    Console.WriteLine("================================");
                    Console.WriteLine(" Lägg till nytt pris");
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
                            Console.WriteLine("===============================================================");
                            Console.WriteLine(" Lägg till multipliceringssats för grundavgift.");
                            Console.WriteLine("===============================================================");
                            Console.WriteLine();
                            Console.WriteLine(" Ange multipliceringssats till grundavgiften för liten båt (ange , som decimalavskiljare): ");
                            Console.WriteLine();
                            if (decimal.TryParse(Console.ReadLine(), out output))
                            {
                                price.BasicPriceSmallBoatAlgorithm = output;
                            }
                            Console.WriteLine("  Ange multipliceringssats till grundavgiften för stor båt (ange , som decimalavskiljare): ");
                            Console.WriteLine();
                            if (decimal.TryParse(Console.ReadLine(), out output))
                            {
                                price.BasicPriceBigBoatAlgorithm = output;
                            }
                            Console.WriteLine("=================================================");
                            Console.WriteLine(" Lägg till multipliceringssats för timpris.");
                            Console.WriteLine("=================================================");
                            Console.WriteLine();
                            Console.WriteLine("Ange hur mycket timpriset för en liten båt skall multipliceras med: ");
                            Console.WriteLine();
                            if (decimal.TryParse(Console.ReadLine(), out output))
                            {
                                price.HourPriceSmallBoatAlgorithm = output;
                            }
                            Console.WriteLine("Ange hur mycket timpriset för en stor båt skall multipliceras med: ");
                            if (decimal.TryParse(Console.ReadLine(), out output))
                            {
                                price.HourPriceBigBoatAlgorithm = output;
                            }
                            DB.Prices.Add(price);
                            DB.SaveChanges();
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine("The price could't be saved because of \"{0}\" .", ex.Message);
                        }
                    }
                }
            }
        }
        private static void ChangePrice()
        {
            int output = 0;

            using (var DB = new Data.BoatBookingSystemEntities1())
            {
                var prices = DB.Prices.ToList();
                Console.Clear();
                ShowPrices();

                Console.WriteLine(" ================================");
                Console.WriteLine(" Ändra pris");
                Console.WriteLine(" ================================");
                Console.WriteLine();
                Console.Write(" Vilken priskategori vill du ändra välj PrisID: ");
                var pris_id = Console.ReadLine();
                if (int.TryParse(pris_id, out output))
                {
                    var newprice = DB.Prices.Where(x => x.PriceID == output).FirstOrDefault();
                    Console.WriteLine();
                    Console.Write(" Ändra grundavgift: ");
                    decimal bfoutput; ;
                    var basicfee = Console.ReadLine();

                    if (decimal.TryParse(basicfee, out bfoutput))
                    {
                        newprice.BasicFee = bfoutput;
                    }
                    Console.WriteLine();
                    Console.Write(" Ändra timavgift: ");
                    decimal hfoutput;
                    var hourfee = Console.ReadLine();
                    if (decimal.TryParse(hourfee, out hfoutput))
                    {
                        newprice.HourFee = hfoutput;
                    }
                    Console.WriteLine();
                    Console.Write(" Ändra grundavgift multipliceringssats liten båt (ange , som decimalavskiljare): ");
                    decimal bmsboutput;
                    var bmsb = Console.ReadLine();
                    if (decimal.TryParse(bmsb, out bmsboutput))
                    {
                        newprice.BasicPriceSmallBoatAlgorithm = bmsboutput;
                    }
                    Console.WriteLine();
                    Console.Write(" Ändra grundavgift multipliceringssats stor båt (ange , som decimalavskiljare): ");
                    decimal gmsboutput;
                    var gmsb = Console.ReadLine();
                    if (decimal.TryParse(gmsb, out gmsboutput))
                    {
                        newprice.BasicPriceBigBoatAlgorithm = gmsboutput;
                    }
                    Console.WriteLine();
                    Console.Write(" Ändra timprisets multipliceringssats liten båt (ange , som decimalavskiljare): ");
                    decimal hmsboutput = 0;
                    var hmsb = Console.ReadLine();
                    if (decimal.TryParse(hmsb, out hmsboutput))
                    {
                        newprice.HourPriceSmallBoatAlgorithm = hmsboutput;
                    }
                    Console.WriteLine();
                    Console.Write(" Ändra timprisets multipliceringssats stor båt (ange , som decimalavskiljare): ");
                    decimal hmlboutput = 0;
                    var hmlb = Console.ReadLine();

                    if (decimal.TryParse(hmlb, out hmlboutput))
                    {
                        newprice.HourPriceBigBoatAlgorithm = hmlboutput;
                    }
            
                    try
                    {
                        DB.SaveChanges();
                        var changedprices = DB.Prices.ToList();

                        ShowPrices();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("The price could't be saved because of \"{0}\" .", ex.Message);
                    }

                }
            }
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine(" Återgå till meny - tryck valfri tangent");
            Console.ReadKey();
            ShowMenu();
        }

        private static void ShowPrices()
        {
            using (var DB = new Data.BoatBookingSystemEntities1())
            {
                var prices = DB.Prices.ToList();
                int counter = 0;

                foreach (var p in prices)
                {
                    Console.WriteLine();
                    Console.WriteLine("***************************************************************");
                    Console.WriteLine(" PriceID: " + p.PriceID);
                    Console.WriteLine(" Grundavgift: " + p.BasicFee);
                    Console.WriteLine(" Timavgift: " + p.HourFee);
                    Console.WriteLine(" Grundavgift multipliceringssats liten båt: " + p.BasicPriceSmallBoatAlgorithm);
                    Console.WriteLine(" Grundavgift multipliceringssats stor båt: " + p.BasicPriceBigBoatAlgorithm);
                    Console.WriteLine(" Timavgift multipliceringssats liten båt: " + p.HourPriceSmallBoatAlgorithm);
                    Console.WriteLine(" Timavgift multipliceringssats stor båt: " + p.HourPriceBigBoatAlgorithm);
                    Console.WriteLine();
                    Console.WriteLine("***************************************************************");

                    counter++;

                    if (counter == prices.Count())
                        break;
                }
            }
        }
    }
}


