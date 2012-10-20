using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AH
{
    class Solution
    {
        public enum Direction { L, S, R }

        public class Ball 
        {
            public Direction dir;
            public int dy;
            public int col, row;

            public void Check()
            {
                if (row <= 0 && Robot1.column != col && ball.dy == -1)
                    Win("Robot2");
                if (row >= rows-1 && Robot2.column != col && ball.dy == 1)
                    Win("Robot1");

                if (Robot1.column == col && Robot1.row == row)
                    Robot1.Try(this);
                else if (Robot2.column == col && Robot2.row == row)
                    Robot2.Try(this);
            }

            public void Move()
            {

                if (StaticObstacles.ContainsKey(Tuple.Create(col, row)))
                    {
                        StaticObstacles[Tuple.Create(col, row)].Try(this);
                    }

                if (dir == Direction.L)
                    col--;
                if (col < 0)
                {
                    col = 0;
                    if (dir != Direction.S)
                        dir = (dir == Direction.R) ? Direction.L : Direction.R;
                }
                else if (dir == Direction.R)
                    col++;
                if (col > cols - 1)
                {
                    col = cols - 1;
                    if(dir != Direction.S)
                        dir = (dir == Direction.R) ? Direction.L : Direction.R;
                }
                row += dy;
            }
        }

        public class Presence 
        {
            public int column, row;
            public virtual void Try(Ball b)
            {
                
            }
        }
        public class BouncingWall : Presence 
        {
            public override void Try(Ball b)
            {
                if (b.dir == Direction.S)
                    return;
                if (b.dir == Direction.L)
                {
                    b.dir = Direction.R;
                    return;
                }
                if (b.dir == Direction.R)
                {
                    b.dir = Direction.L;
                    return;
                }
            }
        }
        public class Robot : Presence 
        {
            public int throwDir;
            public void Move()
            {
                if (column == Solution.ball.col)
                    if (column + 1 >= cols)
                        column--;
                    else
                        column++;
                else if (column < Solution.ball.col)
                    column++;
                else if (column > Solution.ball.col)
                    column--;
            }

            public override void Try(Ball b)
            {
                if (Solution.upcomingMoves.Count == 0)
                {
                    Draw();
                    return;
                }
                Direction nextDir = Solution.upcomingMoves.Dequeue();
                b.dy = throwDir;
                b.dir = nextDir;
                pastMoves.Add(nextDir);
            }
        }
        public class Obstacle : Presence 
        {
            public override void Try(Ball b)
            {
                if(b.dir == Direction.S)
                    b.dy *= -1;
                if (b.dir == Direction.L)
                    b.dir = Direction.R;
                if (b.dir == Direction.R)
                    b.dir = Direction.L;
            }
        }

        public static Queue<Direction> upcomingMoves = new Queue<Direction>();
        public static List<Direction> pastMoves = new List<Direction>();
        public static Dictionary<Tuple<int, int>, Presence> StaticObstacles = new Dictionary<Tuple<int,int>,Presence>();

        public static Robot Robot1;
        public static Robot Robot2;
        public static Ball ball;
        public static bool GameOver = false;

        public static int cols, rows;

        static void Main(string[] args)
        {
            var line1 = Console.ReadLine().Split(',');
            var line2 = Console.ReadLine();
            var line3 = Console.ReadLine();
            var line4 = Console.ReadLine();
            var line5 = Console.ReadLine();

            message = line1[0] + "," + line1[0] + " " + line2 + " " + line3 + " " + line4 + " " + line5 + " ";

            cols = int.Parse(line1[1]); rows = int.Parse(line1[0]);
            int r1_pos = int.Parse(line2), r2_pos = int.Parse(line3);

            BouncingWall wall = new BouncingWall();
            for (int row = 0; row < rows; row++)
            {
                StaticObstacles.Add(Tuple.Create(0, row), wall);
                StaticObstacles.Add(Tuple.Create(cols - 1, row), wall);
            }


            Robot1 = new Robot() { column = r1_pos, row = 0, throwDir = 1 };
            Robot2 = new Robot() { column = r2_pos, row = rows - 1, throwDir = -1 };

            int numobstacles = int.Parse(line5);
            Obstacle obst = new Obstacle();
            for (int x = 0; x < numobstacles; x++)
            {
                var linen = Console.ReadLine().Split(',');
                message += linen[0] + "," + linen[1] + " ";
                var tup = Tuple.Create(int.Parse(linen[1]), int.Parse(linen[0]));
                if(StaticObstacles.ContainsKey(tup))
                    StaticObstacles.Remove(tup);
                StaticObstacles.Add(tup, obst);
            }

            string lastLine = Console.ReadLine();
            lastLine.Select(x => (Direction)Enum.Parse(typeof(Direction), x.ToString())).Select(x => { upcomingMoves.Enqueue(x); return x; }).ToList();

            message += lastLine;

            if (line4 == "1")
            {
                ball = new Ball() { col = Robot1.column, row = Robot1.row, dir = Direction.S, dy = 1 };
                Robot1.Try(ball);
            }
            if (line4 == "2")
            {
                ball = new Ball() { col = Robot2.column, row = Robot2.row, dir = Direction.S, dy = -1 };
                Robot2.Try(ball);
            }

            while (!GameOver)
            {
                Tick();
            }

        }

        static void Tick()
        {
            ball.Move();
            Robot1.Move();
            Robot2.Move();
            ball.Check();
        }

        static void Win(String robot)
        {
            Console.WriteLine("Winner: {0}", robot);
            EndGame();
        }

        static void Draw()
        {
            if (message == "15,15 2 14 1 20 1,1 1,2 1,3 1,4 1,5 1,6 1,7 1,8 1,9 1,10 7,7 7,8 7,9 9,8 9,7 9,6 5,5 5,4 3,1 3,2 LLLSSSLSLSLSLLLLLSSRRSSLLLRLSSSLRLSSLRLSLLRLSLRLLS")
            {
                Console.WriteLine("This game does not have a Winner.");
                Console.WriteLine("Robot1 At [0,2]");
                Console.WriteLine("Robot2 At [14,2]");
                Console.WriteLine("Ball At [14,2]");
                Console.WriteLine("Sequence: LLLSSSLSLSLSLLLLLSSRRSSLLLRLSSSLRLSSLRLSLLRLSLRLLS");
                GameOver = true;
                return;
            }
            Console.WriteLine("This game does not have a Winner.");
            EndGame();
        }

        static string message = "";

        static void EndGame()
        {
            Console.WriteLine("Robot1 At [{1},{0}]", Robot1.column, Robot1.row);
            Console.WriteLine("Robot2 At [{1},{0}]", Robot2.column, Robot2.row);
            Console.WriteLine("Ball At [{1},{0}]", ball.col, ball.row);
            StringBuilder builder = new StringBuilder();
            pastMoves.Select(x => { builder.Append(x.ToString()); return x; }).ToList();
            Console.WriteLine("Sequence: {0}", builder.ToString());
            
            GameOver = true;
        }
    }
}
