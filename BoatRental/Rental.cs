﻿using BookingBoatSystem;
using Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;
using System.Text;
using System.Threading.Tasks;

namespace BoatRental
{
    public static class Rental
    {
        enum CategoryName { SEGELBÅT, JOLLE };

        public static void OptionsRentalMenu()
        {
            var booking = new BookingBoatSystem.Booking();
            int option = 0;

            Console.Clear();
            Console.WriteLine("================================");
            Console.WriteLine("Välj tjänst");
            Console.WriteLine("================================");
            Console.WriteLine("1. Boka en båt");
            Console.WriteLine("2. Återlämna båt");
            Console.WriteLine("3. Kolla senaste bokningsnummer");
            Console.WriteLine("4. Se status på dina bokningar");
            Console.WriteLine("5. Avsluta\r\n");

            if (int.TryParse(Console.ReadLine(), out option))
            {

                while (option != 6 && option != 0)
                {
                    if (option == 1)
                    {
                        RentABoat();
                    }
                    else if (option == 2)
                    {
                        ReturnBoat();
                    }
                    else if (option == 3)
                    {
                        CheckBookingNumber();
                    }
                    else if (option == 4)
                    {
                        CheckAllYourRentals();
                    }
                    else
                    {
                        return;
                    }
                }
            }
            else
            {
                Console.WriteLine("Valet måste vara en siffra");
                Console.WriteLine();
                Console.WriteLine(" Återgå till meny - tryck valfri tangent");
                Console.ReadKey();
                OptionsRentalMenu();
            }
        }
        private static void RentABoat()
        {
            var booking = new BookingBoatSystem.Booking();
            bool theboatisbooked = false;

            try
            {
                using (var DB = new Data.BoatBookingSystemEntities1())
                {
                    var boats = DB.Boats.Join(DB.Categories, boat => boat.CatID, cat => cat.CatID, (boat, cat) => new { BoatID = boat.BoatID, BoatName = boat.Name, CatName = cat.Name, SizeOver40Feets = cat.OverSizeFourty }).ToList();
                    var bookings = DB.Bookings.ToList();
                    Console.Clear();

                    foreach (var r in boats)
                    {
                        if (!bookings.Any(b => b.BoatID == r.BoatID && b.ReturnDateTime == null))
                        {
                            Console.WriteLine("***************************************************************");
                            Console.WriteLine(" Båtnummer: " + r.BoatID);
                            Console.WriteLine(" Båtnamn: " + r.BoatName);
                            Console.Write(" Kategori: " + r.CatName);

                            if (r.CatName.ToUpper() == CategoryName.SEGELBÅT.ToString())
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
                    Console.WriteLine("******* Uthyrning av båt ********");
                    Console.Write(" Ange båtnummer: ");
                    var boatnumber = Console.ReadLine();
                    Console.Write(" Ange personnummer (10 siffror): ");
                    var personnumber = Console.ReadLine();
                    int boatid;
                    if (int.TryParse(boatnumber, out boatid))
                        theboatisbooked = booking.RentABoatRegistry(personnumber, boatid);
                    if (theboatisbooked == true)
                    {
                        Console.WriteLine();
                        Console.WriteLine("*** Båten är nu uthyrd ***");
                    }
                }
                Console.WriteLine();
                Console.WriteLine();
                Console.WriteLine(" Återgå till meny - tryck valfri tangent");
                Console.ReadKey();
                OptionsRentalMenu();
            }
            catch (Exception ex)
            {
                Console.Write("The boats could't be showed becouse of \"{0}\" .", ex.Message);
            }
        }

        private static void ReturnBoat()
        {
            var booking = new BookingBoatSystem.Booking();
            bool theboatisreturned = false;
            Console.Clear();
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine("******* Återlämning av båt ********");
            Console.WriteLine();
            Console.Write(" Ange bokningsnummer: ");
            var bookingnumber = Console.ReadLine();
            int bookingid;
            if (int.TryParse(bookingnumber, out bookingid))
                theboatisreturned = booking.ReturnBoatByBookingNumber(bookingid);
            if (theboatisreturned == true)
            {
                Console.WriteLine();
                Console.WriteLine("** Båten är nu återlämnad **");
                try
                {
                    using (var DB = new Data.BoatBookingSystemEntities1())
                    {
                        var therental = (DB.Bookings.Join(DB.Boats, rental => rental.BoatID, boat => boat.BoatID, (rental, boat) =>
                        new { BookingNumber = rental.BookingNumber, PersonNumber = rental.PersonNumber, DeliveryTime = rental.DeliveyDateTime, ReturnTime = rental.ReturnDateTime, BoatName = boat.Name, BoatID = boat.BoatID, CategoryID = boat.CatID }))
                        .Where(rental => rental.BookingNumber == bookingid).FirstOrDefault();
                        var category = DB.Categories.Where(c => c.CatID == therental.CategoryID)
                            .Include(n => n.Name)
                            .Include(s => s.OverSizeFourty)
                            .Select(c => new { CategoryName = c.Name, BoatSize = c.OverSizeFourty }).FirstOrDefault();

                        Console.WriteLine();
                        Console.WriteLine(" Båtnummer: " + therental.BoatID);
                        Console.WriteLine(" BåtNamn: " + therental.BoatName);
                        Console.Write(" Kategori: " + category.CategoryName);

                        if (category.CategoryName.ToUpper() == CategoryName.SEGELBÅT.ToString())
                        {
                            if (category.BoatSize == true)
                            {
                                Console.WriteLine(" över eller lika med 40 tum.");
                            }
                            else
                            {
                                Console.WriteLine(" mindre än 40 tum.");
                            }
                        }

                        Console.WriteLine(" Personnummer: " + therental.PersonNumber);
                        Console.WriteLine(" Startdatum: " + therental.DeliveryTime.ToLongDateString());
                        Console.WriteLine(" Startid: " + therental.DeliveryTime.ToShortTimeString());
                        Console.WriteLine(" Återlämningsdatum: " + therental.ReturnTime.Value.ToLongDateString());
                        if (therental.ReturnTime.HasValue)
                            Console.WriteLine(" Återlämningstid: " + therental.ReturnTime.Value.ToShortTimeString());
                        Console.WriteLine(" Antal timmar: " + booking.GetNumebrOfHoursForTheRental(bookingid));
                        Console.WriteLine(" Pris: " + booking.GetThePriceOfTheBoatRent(bookingid));
                    }
                }
                catch (Exception ex)
                {
                    Console.Write("The return rental could't be returned because of \"{0}\" .", ex.Message);
                }

            }
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine(" Återgå till meny - tryck valfri tangent");
            Console.ReadKey();
            OptionsRentalMenu();
        }
        private static void CheckBookingNumber()
        {
            var booking = new BookingBoatSystem.Booking();
            int bookingnumber;
            try
            {
                using (var DB = new Data.BoatBookingSystemEntities1())
                {
                    var bookings = DB.Bookings.ToList();
                    Console.Clear();
                    Console.WriteLine();
                    Console.WriteLine();
                    Console.WriteLine("******* Kolla bokningsnummret ********");
                    Console.WriteLine();
                    Console.Write(" Ange personnummer: ");
                    var personnumber = Console.ReadLine();
                    if (personnumber.Count() == 10)
                    {
                        bookingnumber = booking.LatestBookingNumberByPersonIdentityNumber(personnumber);
                        Console.WriteLine();
                        Console.WriteLine(" Ditt senaste bokniningsnummer är: " + bookingnumber);
                    }
                    else
                    {
                        Console.WriteLine();
                        Console.WriteLine(" Personnumret skall vara 10 siffror.");
                        Console.WriteLine(" Försök igen - tryck valfri tangent");
                        Console.ReadKey();
                        CheckBookingNumber();
                    }
                }
            }
            catch (Exception ex)
            {
                Console.Write("The bookingnumber could't be returned because of \"{0}\" .", ex.Message);
            }
            Console.WriteLine();
            Console.WriteLine(" Återgå till meny - tryck valfri tangent");
            Console.ReadKey();
            OptionsRentalMenu();
        }

        private static void CheckAllYourRentals()
        {
            var booking = new BookingBoatSystem.Booking();
            List<int> _bookingnumbers;
            try
            {
                Console.Clear();
                Console.WriteLine();
                Console.WriteLine("******* Kolla bokningsnummret ********");
                Console.WriteLine();
                Console.Write(" Ange personnummer: ");
                var personnumber = Console.ReadLine();
                if (personnumber.Count() == 10)
                {
                    _bookingnumbers = booking.GetAllYourBookingsByPersonIdentityNumber(personnumber);
                    Console.WriteLine();
                    Console.Write(" Dina bokningsnummer är: ");
                    foreach (var item in _bookingnumbers)
                    {
                        Console.Write(item + ",");
                    }
                }
                else
                {
                    Console.WriteLine();
                    Console.WriteLine(" Personnumret skall vara 10 siffror.");
                    Console.WriteLine(" Försök igen - tryck valfri tangent");
                    Console.ReadKey();
                    CheckAllYourRentals();
                }
            }
            catch (Exception ex)
            {
                Console.Write("The bookingnumber could't be returned because of \"{0}\" .", ex.Message);
            }
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine("******* Se status på dina bokningar ********");
            Console.WriteLine();
            Console.Write(" Skriv ett bokningsnummer: ");
            var bookingnumber = Console.ReadLine();
            int bookingid;
            if (int.TryParse(bookingnumber, out bookingid))
                GetStatusOfYourRentals(bookingid);
            Console.WriteLine();
            Console.WriteLine(" Återgå till meny - tryck valfri tangent");
            Console.ReadKey();
            OptionsRentalMenu();
        }
        private static void GetStatusOfYourRentals(int bookingid)
        {
            var booking = new BookingBoatSystem.Booking();
            List<Data.Category> _categorylist = new List<Data.Category>();
            try
            {
                using (var DB = new Data.BoatBookingSystemEntities1())
                {
                    var therental = (DB.Bookings.Join(DB.Boats, rental => rental.BoatID, boat => boat.BoatID, (rental, boat) =>
                    new { BookingNumber = rental.BookingNumber, PersonNumber = rental.PersonNumber, DeliveryTime = rental.DeliveyDateTime, ReturnTime = rental.ReturnDateTime, BoatName = boat.Name, BoatID = boat.BoatID, CategoryID = boat.CatID }))
                    .Where(rental => rental.BookingNumber == bookingid).FirstOrDefault();
                    var category = DB.Categories.Where(c => c.CatID == therental.CategoryID)
                        .Include(n => n.Name)
                        .Include(s => s.OverSizeFourty)
                        .Select(c => new { CategoryName = c.Name, BoatSize = c.OverSizeFourty }).FirstOrDefault();

                    Console.WriteLine();
                    Console.WriteLine(" Båtnummer: " + therental.BoatID);
                    Console.WriteLine(" BåtNamn: " + therental.BoatName);
                    Console.Write(" Kategori: " + category.CategoryName);

                    _categorylist = booking.GetValuesForCategoryName();
         

                    if (category.CategoryName.ToUpper() == CategoryName.SEGELBÅT.ToString())
                    {

                        if (category.BoatSize == true)
                        {
                            Console.WriteLine(" över eller lika med 40 tum.");
                        }
                        else
                        {
                            Console.WriteLine(" mindre än 40 tum.");
                        }
                    }
                    else
                    {
                        foreach (var item in _categorylist)
                        {
                            if (item.Name.ToUpper() == category.CategoryName.ToUpper() && item.OverSizeFourty == category.BoatSize)
                            {
                                if (category.BoatSize == true)
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
                    Console.WriteLine(" Startdatum: " + therental.DeliveryTime.ToLongDateString());
                    Console.WriteLine(" Startid: " + therental.DeliveryTime.ToShortTimeString());

                    if (therental.ReturnTime.HasValue)
                    {
                        Console.WriteLine(" Återlämningsdatum: " + therental.ReturnTime.Value.ToLongDateString());
                        Console.WriteLine(" Återlämningstid: " + therental.ReturnTime.Value.ToShortTimeString());
                        Console.WriteLine(" Antal timmar: " + booking.GetNumebrOfHoursForTheRental(bookingid));
                        Console.WriteLine(" Pris: " + booking.GetThePriceOfTheBoatRent(bookingid));
                    }
                    else
                    {
                        Console.WriteLine(" Båten är ännu inte tillbakalämnad");
                    }

                }
            }
            catch (Exception ex)
            {
                Console.Write("The return rental could't be returned because of \"{0}\" .", ex.Message);
            }
        }
    }
}


