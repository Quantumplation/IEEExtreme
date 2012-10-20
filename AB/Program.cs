using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AB
{
    class Solution
    {
        static void Main(string[] args)
        {
            Dictionary<Tuple<int, int>, float> dist = new Dictionary<Tuple<int, int>, float>();
            dist.Add(Tuple.Create(0, 0), 0);

            for (int r = 1; r <= 200; r++)
            {
                float high = r * 10;
                int low = r * 10 - 5;

                int x = -r, y = 0;

                dist.Add(Tuple.Create(x, y), high);
                for (int c = 1; c < r; c++)
                {
                    if (c % 2 == 0)
                        x++;
                    y++;
                    dist.Add(Tuple.Create(x, y), low);
                }

                if (r % 2 == 0)
                    x++;
                y++;

                dist.Add(Tuple.Create(x, y), high);
                for (int c = 1; c < r; c++)
                {
                    x++;
                    dist.Add(Tuple.Create(x, y), low);
                }

                x++;

                dist.Add(Tuple.Create(x, y), high);
                for (int c = 1; c < r; c++)
                {
                    y--;
                    if (c % 2 == r % 2)
                        x++;
                    dist.Add(Tuple.Create(x,y), low);
                }
                y--;

                    x++;

                dist.Add(Tuple.Create(x, y), high);
                for (int c = 1; c < r; c++)
                {
                    y--;
                    if (c % 2 == 1)
                        x--;
                    dist.Add(Tuple.Create(x, y), low);
                }

                y--;
                if(r % 2 == 1)
                    x--;

                dist.Add(Tuple.Create(x, y), high);
                for (int c = 1; c < r; c++)
                {
                    x--;
                    dist.Add(Tuple.Create(x, y), low);
                }

                x--;

                dist.Add(Tuple.Create(x, y), high);
                for (int c = 1; c < r; c++)
                {
                    y++;
                    if(c%2 != r%2)
                        x--;
                    dist.Add(Tuple.Create(x, y), low);
                }
            }
            var input = Console.ReadLine();
            var split = input.Split(new char[] { ',', '.' });
            int x1 = int.Parse(split[0]);
            int y1 = int.Parse(split[1]);
            int x2 = int.Parse(split[2]);
            int y2 = int.Parse(split[3]);

            int dy = y2 - y1;
            int dx = x2 - x1;

            try
            {
                Console.WriteLine(dist[Tuple.Create(dx, dy)]);
            }
            catch
            {
                throw new Exception(input);
            }
        }
    }
}
