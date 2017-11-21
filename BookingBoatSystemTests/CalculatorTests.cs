using NUnit.Framework;
using BookingBoatSystem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace BookingBoatSystem.Tests
{
    [TestFixture]
    public class CalculatorTests
    {

        [Test]
        public void ShouldAddTwoNumbers()
        {
            ICalculator sut = new Calculator();
            int expectedResult = sut.Add(7, 8);
            Assert.That(expectedResult, Is.EqualTo(15));
        }


        [Test]
        public void ShouldMulTwoNumbers()
        {
            ICalculator sut = new Calculator();
            int expectedResult = sut.Mul(7, 8);
            Assert.That(expectedResult, Is.EqualTo(56));
        }
    }
}