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
            IsSaved = registry.RentABoatRegistry("8809210033", 5, DateTime.Now);
            Assert.IsTrue(IsSaved);
        }
    }
}