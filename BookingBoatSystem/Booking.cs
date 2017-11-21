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

    }
}
