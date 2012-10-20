using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AW
{
    class Program
    {
        static void Main(string[] args)
        {
            int count = int.Parse(Console.ReadLine());
            StringBuilder output = new StringBuilder();
            for (int x = 0; x < count; x++)
            {
                var line = Console.ReadLine().Split(' ');
                var cs = Count(line[1], int.Parse(line[0]));
                output.Append(String.Join(",", cs.Values) + "\n");
            }
            Console.WriteLine(output.ToString());
        }

        static Dictionary<int, int> Count(string query, int bits)
        {
            Func<bool, bool, bool> AND = (a, b) => a && b;
            Func<bool, bool, bool> OR = (a, b) => a || b;
            Func<bool, bool> NOT = (a) => !a;

            bool[] state = new bool[26];
            state[0] = false;
            state[1] = true;

            Stack<Action> prevSteps = new Stack<Action>();
            prevSteps.Push(() => { });

            char nextUnused = (char)((bits) + 'a');

            HashSet<int> seen = new HashSet<int>();
            HashSet<int> used = new HashSet<int>();
            
            for (int x = 0; x < query.Length-1; x += 2)
            {
                if (!seen.Contains(char.ToLower(query[x]) - 'a'))
                    seen.Add(char.ToLower(query[x]) - 'a');
                if (!seen.Contains(char.ToLower(query[x + 1]) - 'a'))
                    seen.Add(char.ToLower(query[x + 1]) - 'a');
                bool notA = Char.IsUpper(query[x]);
                bool notB = Char.IsUpper(query[x + 1]);

                Action step = () => { };
                if (char.ToLower(query[x]) <= char.ToLower(query[x + 1]))
                {
                    var prev = prevSteps.Peek();
                    int indexReadA = (int)(Char.ToLower(query[x]) - 'a');
                    int indexReadB = (int)(Char.ToLower(query[x + 1]) - 'a');
                    int indexStore = (int)(char.ToLower(nextUnused) - 'a');
                    used.Add(indexStore);
                    nextUnused++;
                    step = (() => 
                    { 
                        prev();
                        state[indexStore] = AND((notA) ? !state[indexReadA] : state[indexReadA], (notB) ? !state[indexReadB] : state[indexReadB]);
                    });
                }
                if (char.ToLower(query[x]) > char.ToLower(query[x + 1]))
                {
                    var prev = prevSteps.Peek();
                    int indexReadA = (int)(Char.ToLower(query[x]) - 'a');
                    int indexReadB = (int)(Char.ToLower(query[x + 1]) - 'a');
                    int indexStore = (int)(Char.ToLower(nextUnused) - 'a');
                    used.Add(indexStore);
                    nextUnused++;
                    step = (() => 
                    { 
                        prev();
                        state[indexStore] = OR((notA) ? !state[indexReadA] : state[indexReadA], (notB) ? !state[indexReadB] : state[indexReadB]); 
                    });
                }
                prevSteps.Push(step);
            }

            List<int> watched = used.Where(x => !seen.Contains(x)).ToList();

            var eval = prevSteps.Pop();

            Dictionary<int, int> counts = new Dictionary<int,int>();
            watched.Select(x => { counts.Add(x, 0); return x; }).ToList();
            for (int x = 0; x < Math.Pow(2, bits); x++ )
            {
                Set(state, x);
                eval();
                watched.Select(a => { if (state[a]) counts[a]++; return a; }).ToList();
            }

            return counts;
        }

        static void Set(Boolean[] state, int val)
        {
            for (int x = 0; x < state.Length; x++)
            {
                state[x] = val % 2 == 1;
                val /= 2;
            }
        }
    }
}
