using Microsoft.VisualStudio.TestTools.UnitTesting;
using BookingBoatSystem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingBoatSystem.Tests
{
    [TestClass()]
    public class BookingTests
    {
        [TestMethod()]
        public void RentABoatRegistryTest()
        {
            var registry = new Booking();
            bool IsSaved = false;
            IsSaved = registry.RentABoatRegistry("8909210033", 1);
            Assert.IsTrue(IsSaved);
        }

        /// <summary>
        /// You know the booking number when you return the boat, and the returndate stores in the database.
        /// </summary>

        [TestMethod()]
        public void ReturnBoatByBookingNumberTest()
        {
            var returnregistry = new Booking();
            var latestrent = returnregistry.CheckLatestRentByPersonIdentityNumber("7103140435");
            bool IsSaved = false;
            IsSaved = returnregistry.ReturnBoatByBookingNumber(40);
            Assert.IsTrue(IsSaved);
        }
        /// <summary>
        /// Get the bookingnumber by the PersonIdentyNumber
        /// </summary>

        [TestMethod()]
        public void ReturnBoatByPersonIdentityNumberTest()
        {
            var returnregistry = new Booking();
            int expectedBookingNumber = returnregistry.LatestBookingNumberByPersonIdentityNumber("8909210033");
            int BookingNumber;
            BookingNumber = returnregistry.CheckLatestRentByPersonIdentityNumber("8909210033");
            Assert.AreEqual(BookingNumber, expectedBookingNumber);
        }

        [TestMethod()]
        public void GetThePriceOfTheBoutRentTest()
        {
            var booking = new Booking();
            bool GotPrice = false;
            int BookingNumber;
            BookingNumber = booking.CheckLatestRentByPersonIdentityNumber("8909210033");
            GotPrice = booking.GetThePriceOfTheBoutRent(BookingNumber);
            Assert.IsTrue(GotPrice);
        }
    }
}