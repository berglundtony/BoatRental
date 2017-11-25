using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingBoatSystem
{
    public class Booking
    {
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

        public bool ReturnBoatByBookingNumber(int bookingnumber)
        {

            try
            {
                using (var DB = new Data.BoatBookingSystemEntities())
                {
                    var booking = DB.Bookings.Where(x => x.BookingNumber.Equals(bookingnumber)).FirstOrDefault();

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


        public void CheckBoatByPersonIdentityNumber(string personidentitynumber)
        {

            try
            {
                using (var DB = new Data.BoatBookingSystemEntities())
                {
                    //var bookings = DB.Bookings
                    //    .Join(DB.Boats, book => book.BoatID, boat => boat.BoatID, (book, boat) => book)
                    //    .Join(DB.Categories, boat => boat.BoatID, cat => cat.CatID, (boat, cat) => boat)
                    //    .Where(x => x.PersonNumber.Equals(personidentitynumber))
                    //    .Select(x => new {
                    //        BookingNumb = x.BookingNumber,
                    //        BoatID = x.BoatID,
                    //    }).ToList();


                    var bookings = (from b in DB.Bookings
                                   join u in DB.Boats on b.BoatID equals u.BoatID
                                   join c in DB.Categories on u.CatID equals c.CatID
                                   where b.PersonNumber.Equals(personidentitynumber)
                                   select new {
                                       BookingNumb = b.BookingNumber,
                                       BoatName = u.Name,
                                       Category = c.Name,
                                       OverFourtyFeet = c.OverSizeFourty
                                   }).ToList();



                }

            }
            catch (Exception ex)
            {
                string.Format("We could not find your rent information because of \"{0}\" .", ex.Message);
            }
        }
    }
}
