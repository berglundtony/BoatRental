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
            IsSaved = registry.RentABoatRegistry("8909210033", 6, DateTime.Now);
            Assert.IsTrue(IsSaved);
        }

        /// <summary>
        /// You know the booking number when you return the boat, and the returndate stores in the database.
        /// </summary>

        [TestMethod()]
        public void ReturnBoatByBookingNumberTest()
        {
            var returnregistry = new Booking();
            bool IsSaved = false;
            IsSaved = returnregistry.ReturnBoatByBookingNumber(26);
            Assert.IsTrue(IsSaved);
        }
        /// <summary>
        /// Get the bookingnumber by the PersonIdentyNumber
        /// </summary>

        [TestMethod()]
        public void ReturnBoatByPersonIdentityNumberTest()
        {
            var returnregistry = new Booking();
            int expectedBookingNumber = returnregistry.LatestBookingNumberByPersonIdentityNumber("8809210033");
            int BookingNumber;
            BookingNumber = returnregistry.CheckLatestRentByPersonIdentityNumber("8809210033");
            Assert.AreEqual(BookingNumber, expectedBookingNumber);
        }
    }
}