using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace BookingBoatSystem
{
    public interface ICalculator
    {
        int Add(int num1, int num2);
        int Mul(int num1, int num2);
    }

    public class Calculator : ICalculator
    {

        public int Add(int num1, int num2)
        {
            int result = num1 + num2;
            return result;
        }

        public int Mul(int num1, int num2)
        {
            int result = num1 + num2;
            return result;
        }


    }

    public class Booking
    {
        public bool RentABoatRegistry(int personnumber, int catid, int boatid, DateTime deliverydatetime)
        {
            var rentopportunity = new BoatRental.Booking();
            try
            {
                using (var DB = new BoatRental.BoatBookingSystemEntities())
                {
                    rentopportunity.PersonNumber = personnumber;
                    rentopportunity.CatID = catid;
                    rentopportunity.BoatID = boatid;
                    rentopportunity.DeliveyDateTime = DateTime.Now;

                    DB.Bookings.Add(rentopportunity);
                    DB.SaveChanges();
                }
                return true;

            }catch(Exception ex)
            {
                string.Format("The rentregistry could't be saved because of \"{0}\" .", ex.Message);
                return false;
            }

   
        }
    }
}
