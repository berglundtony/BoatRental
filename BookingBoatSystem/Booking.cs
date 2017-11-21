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

        public bool ReturnABoatRegistry(int boatid, DateTime returndatetime)
        {
            var rentopportunity = new Data.Booking();
            try
            {
                using (var DB = new Data.BoatBookingSystemEntities())
                {
                    DB.Bookings.Where(x => x.BoatID.Equals(boatid))
 
                    rentopportunity.BoatID = boatid;
                    rentopportunity.DeliveyDateTime = deliverydatetime;

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
    }
}
