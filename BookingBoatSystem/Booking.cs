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
        int bookingnumber;
        double price;
        List<int> _bookingnumbers = new List<int>();

        public bool RentABoatRegistry(string personnumber, int boatid, DateTime deliverydatetime)
        {
            var rentopportunity = new Data.Booking();
            try
            {
                using (var DB = new Data.BoatBookingSystemEntities())
                {
                    rentopportunity.PersonNumber = personnumber;
                    rentopportunity.BoatID = boatid;
                    rentopportunity.DeliveyDateTime = deliverydatetime;

                    DB.Bookings.Add(rentopportunity);
                    DB.SaveChanges();
                }
                return true;

            }catch(Exception ex)
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
                                    select new
                                    {
                                        BookingNumber = b.BookingNumber,
                                        BoatName = u.Name,
                                        Category = c.Name,
                                        OverFourtyFeet = c.OverSizeFourty,
                                        DeliveryDate = b.DeliveyDateTime
                                    }).ToList(); 

                    foreach (var item in bookings)
                    {
                        if (item.DeliveryDate.Date == DateTime.Now.Date)
                        {
                            _bookingnumbers.Add(item.BookingNumber);
                        }
                        else
                        {
                            _bookingnumbers.Add(0);
                        }
                    }
                    bookingnumber = _bookingnumbers.Last();
                   
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

        public bool GetThePriceOfTheBoutRent(int bookingnumber)
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

                    if (booking != null)
                    {
                        deliveryTime = booking.DeliveyDateTime;
                        returnTime = booking.ReturnDateTime;
                        newReturnTime = (DateTime)booking.ReturnDateTime;
                        duration = newReturnTime.Subtract(deliveryTime);
                        var hours = duration.Hours;
                        var minutes = duration.Minutes;
                        boatnumber = booking.BoatID
                        GetRentalPrice(hours, minutes, boatnumber);

                    }  
                }
                return true;

            }
            catch (Exception ex)
            {
                string.Format("The return registry could't be saved because of \"{0}\" .", ex.Message);
                return false;
            }

        }

        private double GetRentalPrice(int hours, int minutes,int boatnumber)
        {
            using(var DB = new Data.BoatBookingSystemEntities())
            {
                var boattype = (from b in DB.Bookings
                                join s in DB.Boats on b.BoatID equals s.BoatID
                                join c in DB.Categories on s.CatID equals c.CatID
                                where b.BoatID == boatnumber select c);
            }
        }
}
    }
}
