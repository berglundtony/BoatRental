using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingBoatSystem
{
    public class Booking
    {
        enum CategoryName { Jolle, Segelbåt };
        static CategoryName categoryname;
        List<int> _bookingnumbers = new List<int>();
        int bookingnumber;
        decimal basicprice;
        decimal hourprice;
        decimal hourpricesmallboat;
        decimal hourpricebigboat;
        decimal totalprice;
        decimal multiplyBigBoat = 1.5m;
        decimal multiplySmallBoat = 1.2m;
        decimal multiplyhourBigBoat = 1.4m;
        decimal multiplyhourSmallBoat = 1.3m;

        /// <summary>
        /// Here the rent of the boat is registred in the database
        /// </summary>
        /// <param name="personnumber"></param>
        /// <param name="boatid"></param>
        /// <returns></returns>
        public bool RentABoatRegistry(string personnumber, int boatid)
        {
            var rentopportunity = new Data.Booking();
            try
            {
                using (var DB = new Data.BoatBookingSystemEntities())
                {
                    rentopportunity.PersonNumber = personnumber;
                    rentopportunity.BoatID = boatid;
                    rentopportunity.DeliveyDateTime = DateTime.Now;

                    DB.Bookings.Add(rentopportunity);
                    DB.SaveChanges();
                }
                return true;

            }
            catch (Exception ex)
            {
                string.Format("The rent registry could't be saved because of \"{0}\" .", ex.Message);
                return false;
            }
        }
        /// <summary>
        /// You know the booking number when you return the boat, and the returndate stores in the database.
        /// </summary>


        public bool ReturnBoatByBookingNumber(int bookingnumber)
        {
            try
            {
                using (var DB = new Data.BoatBookingSystemEntities())
                {
                    var booking = DB.Bookings.FirstOrDefault(x => x.BookingNumber.Equals(bookingnumber));

                    if (booking != null)
                    {
                        booking.ReturnDateTime = DateTime.Now;
                    }
                    DB.Entry(booking).State = System.Data.Entity.EntityState.Modified;
                    DB.SaveChanges();
                }
                return true;

            }
            catch (Exception ex)
            {
                string.Format("The return registry could't be saved because of \"{0}\" .", ex.Message);
                return false;
            }
        }

        /// <summary>
        /// Get the bookingnumber by the PersonIdentyNumber
        /// </summary>

        public int CheckLatestRentByPersonIdentityNumber(string personidentitynumber)
        {
            try
            {
                using (var DB = new Data.BoatBookingSystemEntities())
                {

                    var bookings = (from b in DB.Bookings
                                    join u in DB.Boats on b.BoatID equals u.BoatID
                                    join c in DB.Categories on u.CatID equals c.CatID
                                    where b.PersonNumber.Equals(personidentitynumber)
                                    orderby b.DeliveyDateTime descending
                                    select new
                                    {
                                        BookingNumber = b.BookingNumber,
                                        BoatName = u.Name,
                                        Category = c.Name,
                                        OverFourtyFeet = c.OverSizeFourty,
                                        DeliveryDate = b.DeliveyDateTime
                                    }).FirstOrDefault();


                    bookingnumber = bookings.BookingNumber;

                }
                return bookingnumber;
            }
            catch (Exception ex)
            {
                string.Format("We could not find your rent information because of \"{0}\" .", ex.Message);
                bookingnumber = 0;
                return bookingnumber;

            }
        }
        /// <summary>
        /// Here we find out were wich the latest booking is for a specific person.
        /// </summary>
        /// <param name="personidentitynumber"></param>
        /// <returns></returns>

        public int LatestBookingNumberByPersonIdentityNumber(string personidentitynumber)
        {
            try
            {
                using (var DB = new Data.BoatBookingSystemEntities())
                {
                    var bookings = DB.Bookings
                    .Where(b => b.PersonNumber == personidentitynumber).OrderByDescending(b => b.DeliveyDateTime)
                    .Select(b => b.BookingNumber)
                    .FirstOrDefault();

                    bookingnumber = bookings;

                }
                return bookingnumber;
            }
            catch (Exception ex)
            {
                string.Format("We could not find your rent information because of \"{0}\" .", ex.Message);
                bookingnumber = 0;
                return bookingnumber;
            }
        }

        public List<int> GetAllYourBookingsByPersonIdentityNumber(string personidentitynumber)
        {
            var bookingnumberlist = new List<int>();
            try
            {
                using (var DB = new Data.BoatBookingSystemEntities())
                {
                    var bookings = DB.Bookings
                    .Where(b => b.PersonNumber == personidentitynumber).OrderByDescending(b => b.DeliveyDateTime)
                    .Select(b => b.BookingNumber)
                    .ToList();

                    foreach(var booking in bookings)
                    {
                        bookingnumberlist.Add(booking);
                    }
                } 
            }
            catch (Exception ex)
            {
                string.Format("We could not find your rent information because of \"{0}\" .", ex.Message);    
            }
            return bookingnumberlist;
        }
        /// <summary>
        /// Here we got the price of the rental of the boat.This method is for the test method.
        /// </summary>
        /// <param name="bookingnumber"></param>
        /// <returns></returns>

        public bool GetThePriceOfTheBoatRentForTesting(int bookingnumber)
        {
            DateTime deliveryTime;
            DateTime? returnTime;
            DateTime newReturnTime;
            TimeSpan duration;
            int boatnumber;
            try
            {
                using (var DB = new Data.BoatBookingSystemEntities())
                {
                    var booking = DB.Bookings.FirstOrDefault(x => x.BookingNumber.Equals(bookingnumber));

                    if (booking != null && booking.ReturnDateTime != null)
                    {
                        deliveryTime = booking.DeliveyDateTime;
                        returnTime = booking.ReturnDateTime;
                        newReturnTime = (DateTime)booking.ReturnDateTime;
                        duration = newReturnTime.Subtract(deliveryTime);
                        var days = duration.Days;
                        if (days > 0)
                            days = days * 24;
                        var hours = duration.Hours + days;
                        var minutes = duration.Minutes;
                        if (minutes >= 1)
                            hours += 1;
                        boatnumber = booking.BoatID;
                        GetRentalPrice(hours, boatnumber);
                        return true;
                    }
                    else
                    {
                        string.Format("The price could't be delivered because there is no return datetime of this rental.");
                        return false;
                    }
                }
            }
            catch (Exception ex)
            {
                string.Format("The price could't be delivered because of \"{0}\" .", ex.Message);
                return false;
            }

        }
        /// <summary>
        /// This method is the same as above but it returns a price intead of a bool value;
        /// </summary>
        /// <param name="bookingnumber"></param>
        /// <returns></returns>

        public decimal GetThePriceOfTheBoatRent(int bookingnumber)
        {
            DateTime deliveryTime;
            DateTime? returnTime;
            DateTime newReturnTime;
            TimeSpan duration;
            int boatnumber;
            try
            {
                using (var DB = new Data.BoatBookingSystemEntities())
                {
                    var booking = DB.Bookings.FirstOrDefault(x => x.BookingNumber.Equals(bookingnumber));

                    if (booking != null && booking.ReturnDateTime != null)
                    {
                        deliveryTime = booking.DeliveyDateTime;
                        returnTime = booking.ReturnDateTime;
                        newReturnTime = (DateTime)booking.ReturnDateTime;
                        duration = newReturnTime.Subtract(deliveryTime);
                        var days = duration.Days;
                        if (days > 0)
                            days = days * 24;
                        var hours = duration.Hours + days;
                        var minutes = duration.Minutes;
                        if (minutes >= 1)
                            hours += 1;
                        boatnumber = booking.BoatID;
                        totalprice = GetRentalPrice(hours, boatnumber);
                    }
                    else
                    {
                        string.Format("The price could't be delivered because there is no return datetime of this rental.");
                    }
                }
            }
            catch (Exception ex)
            {
                string.Format("The price could't be delivered because of \"{0}\" .", ex.Message);
            }
            return totalprice;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="hours"></param>
        /// <param name="boatnumber"></param>
        /// <returns></returns>

        private decimal GetRentalPrice(int hours, int boatnumber)
        {
            using (var DB = new Data.BoatBookingSystemEntities())
            {
                var category = DB.Bookings.Join(DB.Boats, booking =>
                booking.BoatID,
                boat => boat.BoatID,
                (booking, boat) => new { Booking = booking, Boat = boat, CategoryID = boat.CatID })
                .Where(boat => boat.Boat.BoatID.Equals(boatnumber)).FirstOrDefault();

                var catname = DB.Categories.Where(c => c.CatID == category.CategoryID)
                    .Include(n => n.Name)
                    .Include(s => s.OverSizeFourty)
                    .Select(c => new
                    {
                        CategoryName = c.Name,
                        IsBiggerOrLike40Foot = c.OverSizeFourty

                    }).FirstOrDefault();

                var prices = (from p in DB.Prices select p).FirstOrDefault();

                categoryname = (CategoryName)Enum.Parse(typeof(CategoryName), catname.CategoryName.ToString());

                switch (categoryname)
                {
                    case Booking.CategoryName.Segelbåt:
                        if ((bool)catname.IsBiggerOrLike40Foot)
                        {
                            basicprice = decimal.Multiply(prices.BasicFee, multiplyBigBoat);
                            hourpricebigboat = decimal.Multiply(prices.HourFee, multiplyhourBigBoat);
                            hourprice = decimal.Multiply(hourpricebigboat, hours);
                            return totalprice = decimal.Add(basicprice, hourprice);
                        }
                        else
                        {
                            basicprice = decimal.Multiply(prices.BasicFee, multiplySmallBoat);
                            hourpricesmallboat = decimal.Multiply(prices.HourFee, multiplyhourSmallBoat);
                            hourprice = decimal.Multiply(hourpricesmallboat, hours);
                            return totalprice = decimal.Add(basicprice, hourprice);
                        }
                    case Booking.CategoryName.Jolle:
                        basicprice = prices.BasicFee;
                        hourprice = decimal.Multiply(prices.HourFee, hours);
                        return totalprice = decimal.Add(basicprice, hourprice);

                    default:
                        basicprice = prices.BasicFee;
                        hourprice = decimal.Multiply(prices.HourFee, hours);
                        return totalprice = decimal.Add(basicprice, hourprice);
                }
            }
        }
        public int GetNumerOfHoursForTheRental(int bookingnumber)
        {
            DateTime deliveryTime;
            DateTime? returnTime;
            DateTime newReturnTime;
            TimeSpan duration;
            int hours = 0;
            try
            {
                using (var DB = new Data.BoatBookingSystemEntities())
                {
                    var booking = DB.Bookings.FirstOrDefault(x => x.BookingNumber.Equals(bookingnumber));


                    if (booking != null && booking.ReturnDateTime != null)
                    {
                        deliveryTime = booking.DeliveyDateTime;
                        returnTime = booking.ReturnDateTime;
                        newReturnTime = (DateTime)booking.ReturnDateTime;
                        duration = newReturnTime.Subtract(deliveryTime);
                        var days = duration.Days;
                        if (days > 0)
                            days = days * 24;
                        hours = duration.Hours + days;
                        var minutes = duration.Minutes;
                        if (minutes >= 1)
                            hours += 1;
                    }
                }
        
            }
            catch (Exception ex)
            {
                string.Format("The hours could't be delivered because of \"{0}\" .", ex.Message);
            }
            return hours;
        }
         
    }
}
