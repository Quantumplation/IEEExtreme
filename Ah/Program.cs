using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AF
{
    class Solution
    {
        struct receipt
        {
            public receipt(string digits, int cost)
            {
                id = digits.Select(x => int.Parse(x.ToString())).Reverse().ToList();
                if(id.Count == 8)
                    id.Add(0);

                int S = id.Zip(new int[] { 0, 2, 4, 8, 16, 32, 64, 128, 256 }, (x, y) => x * y).Sum();
                int Y = S % 11;
                if (id.Count < 8 || id.Count > 9)
                    valid = false;
                else if (Y == 10 && id[0] == 0)
                    valid = true;
                else if (Y == id[0])
                    valid = true;
                else
                    valid = false;
                this.cost = cost;
            }

            public bool valid;
            List<int> id;
            public int cost;
        }

        static void Main(string[] args)
        {
            
            Console.WriteLine(receipts().Where(x => x.valid).Sum(x => x.cost));

        }

        static IEnumerable<receipt> receipts()
        {
            string line = Console.In.ReadLine();
            while(line != null && line != "")
            {
                var split = line.Split(' ');
                yield return new receipt(split[0], int.Parse(split[1]));
                line = Console.In.ReadLine();
            }
        }
    }
}
