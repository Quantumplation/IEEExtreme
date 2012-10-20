using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AA
{
    class Program
    {
        static void Main(string[] args)
        {
            int bunnies = int.Parse(Console.ReadLine());// int.Parse(args[0]);
            Queue<Tuple<int, int>> babies = new Queue<Tuple<int,int>>();
            for (int x = 0; x < 365; x++)
            {                
                if (x % 30 == 0)
                {
                    bunnies = (int)((float)bunnies * 0.75f);
                }
                if (x != 0 && x % 15 == 0)
                    babies.Enqueue(Tuple.Create(x, (int)(((float)bunnies) * 0.9f)));
                if (babies.Count != 0 && x - babies.Peek().Item1 >= 30)
                    bunnies += (int)(babies.Dequeue().Item2 * 0.7f);
                if (bunnies == 0)
                    break;
            }
            Console.WriteLine(bunnies);
        }
    }
}
