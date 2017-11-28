using BookingBoatSystem;
using Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoatRental
{
    public static class Rental
    {
        public static void ShowBoats()
        {
            var booking = new BookingBoatSystem.Booking();

            int option = 0;
            bool theboatisbooked = false;
            GetMenu();

            option = int.Parse(Console.ReadLine());

            while (option != 5 && option != 0)
            {
                if (option == 1)
                {
                    try
                    {
                        using (var DB = new Data.BoatBookingSystemEntities())
                        {
                            var boats = DB.Boats.Join(DB.Categories, cat => cat.CatID, boat => boat.CatID, (boat, cat) => new { BoatID = boat.BoatID, BoatName = boat.Name, CatName = cat.Name, SizeOver40Feets = cat.OverSizeFourty }).ToList();
                            var bookings = (from b in DB.Bookings select b).ToList();
                            Console.Clear();

                            foreach (var r in boats)
                            {
                                if (!bookings.Any(b => b.BoatID == r.BoatID && b.ReturnDateTime == null))
                                {
                                    Console.WriteLine("***************************************************************");
                                    Console.WriteLine(" Båtnummer: " + r.BoatID);
                                    Console.WriteLine(" Name: " + r.BoatName);
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
                          
                            }
                            Console.WriteLine();
                            Console.WriteLine();
                            Console.WriteLine("******* Bokning av båtar ********");
                            Console.Write(" Ange båtnummer: ");
                            var boatnumber = Console.ReadLine();
                            Console.Write(" Ange personnummer: ");
                            var personnumber = Console.ReadLine();
                            int boatid;
                            if (int.TryParse(boatnumber, out boatid))
                                theboatisbooked = booking.RentABoatRegistry(personnumber, boatid);
                            if (theboatisbooked == true)
                            {
                                Console.WriteLine();
                                Console.WriteLine("*** Båten är bokad ***");
                            }
                        }
                        Console.WriteLine(" Återgå till meny - tryck valfri tangent");
                        Console.ReadKey();
                        ShowBoats();
                    }
                    catch (Exception ex)
                    {
                        Console.Write("The boats could't be showed becouse of \"{0}\" .", ex.Message);
                    }
                }
                else if (option == 2)
                {
                    //ShowAllCategories();
                }
                else if (option == 3)
                {
                    //CreateNewBoat();
                }
                else if (option == 4)
                {
                    //ShowAllBoats();
                }
                else
                {
                    Console.ReadKey();
                }
            }
        }
        private static void GetMenu()
        {
            Console.Clear();
            Console.WriteLine("================================");
            Console.WriteLine("Välj tjänst");
            Console.WriteLine("================================");
            Console.WriteLine("1. Boka en båt");
            Console.WriteLine("2. Återlämna båt");
            Console.WriteLine("3. Kolla bokningsnummer");
            Console.WriteLine("4. Avsluta\r\n");
        }
    }
}

