using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AE
{
    class Program
    {
        static void Main(string[] args)
        {
            var input = Console.ReadLine().Split(new [] { '\t', ' ', ',' });
            if (input.Length < 2 || input.Length > 2)
            {
                Console.WriteLine("ERROR");
                return;
            }

            int charge = 0;
            int given = 0;
            if (!int.TryParse(input[0], out charge) || !int.TryParse(input[1], out given))
            {
                Console.WriteLine("ERROR");
                return;
            }


            if (charge < 0 || given < 0 || charge > 1000 || given > 1000)
            {
                Console.WriteLine("ERROR");
                return;
            }

            if (charge % 5 != 0 && given % 5 != 0)
            {
                Console.WriteLine("ERROR");
                return;
            }

            if (given < charge)
            {
                Console.WriteLine("ERROR");
                return;
            }

            int change = given - charge;

            int dollar = 0;
            while(change >= 100)
            {
                dollar++;
                change -= 100;
            }

            int quarter = 0;

            while (change >= 25)
            {
                quarter++;
                change -= 25;
            }

            int dime = 0;
            while (change >= 10)
            {
                dime++;
                change -= 10;
            }

            int nickle = 0;
            while (change >= 5)
            {
                nickle++;
                change -= 5;
            }

            if (change != 0)
            {
                Console.WriteLine("ERROR");
                return;
            }

            Console.WriteLine(dollar + " " + quarter + " " + dime + " " + nickle);
        }
    }
}
