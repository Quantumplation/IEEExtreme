using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AM
{        
    class Shape {}
    class Square : Shape { public int sideLength; }
    class Rectangle : Shape { public int height, width; }
    class Triangle : Shape { public int x1, y1, x2, y2, x3, y3; }
    class Parallelogram : Shape { public int x1, y1, x2, y2, x3, y3, x4, y4; }
    class None : Shape { }


    class Program
    {


        static void Main(string[] args)
        {
            string sizeInput = Console.ReadLine();
            string bitmapInput = Console.ReadLine();

            var splitSize = sizeInput.Split(' ');
            var splitBitmap = bitmapInput.Split(' ');

            var arrs = Find(int.Parse(splitSize[0]), int.Parse(splitSize[1]), splitBitmap.Select(x => int.Parse(x)).ToArray()).Select(x => { return x; }).OrderBy(x => x.GetType().Name).ToList();
            foreach (var s in arrs.Take(arrs.Count - 1))
            {
                Console.Write(s.GetType().Name + ", ");
            }
            Console.WriteLine(arrs.Last().GetType().Name);
        }

        struct State
        {
            public bool Value;
            public bool Claimed;
        }

        static IEnumerable<Shape> Find(int rows, int col, int[] data)
        {   
            State[,] area = new State[rows,col];

            int currRow = 0, currCol = 0;
            foreach (var d in data)
            {
                foreach (var c in ToBinary(d))
                {
                    area[currRow, currCol].Value = c == '1';
                    currCol++;
                    if (currCol == col)
                    {
                        currCol = 0;
                        currRow++;
                    }
                }
            }

            for (int x = 0; x < rows; x++)
            {
                for (int y = 0; y < col; y++)
                    Console.Write(area[x, y].Value ? "1" : "0");
                Console.WriteLine();
            }
            List<Shape> shapes = new List<Shape>();
            for (int x = 0; x < rows; x++)
            {
                for (int y = 0; y < col; y++)
                {
                    if (area[x, y].Claimed)
                        continue;
                    if(area[x,y].Value)
                        shapes.Add(TryShape(x, y, rows, col, area));
                }
            }
            return shapes.Where(x => x != null);
        }

        static Shape TryShape(int x, int y, int width, int height, State[,] area)
        {
            int left = scanLine(width, height, x, y, 0, 1, area);
            int down = scanLine(width, height, x, y, 1, 0, area);
            int leftUp = scanLine(width, height, x, y, -1, 1, area);
            int leftDown = scanLine(width, height, x, y, 1, 1, area);
            int rightDown = scanLine(width, height, x, y, 1, -1, area);

            if (left > 1 && down > 1)
            {
                int botSide = scanLine(width, height, x + down - 1, y, 0, 1, area);
                int minWidth = Math.Min(botSide, left);
                if (minWidth > 1)
                {
                    int rightSide = scanLine(width, height, x, y + minWidth - 1, 1, 0, area);
                    if (rightSide == down)
                    {
                        consumeLine(x, y, 0, 1, left, area);
                        consumeLine(x, y, 1, 0, down, area);
                        consumeLine(x + down - 1, y, 0, 1, left, area);
                        consumeLine(x, y + minWidth - 1, 1, 0, down, area);
                        if (down == left)
                        {
                            return new Square();
                        }
                        else
                            return new Rectangle();
                    }
                }

                //
                // ----
                // | /
                // |/
                // 

                if (down > 2)
                {
                    int diagonal = scanLine(width, height, x, y + left - 1, 1, -1, area);
                    if (diagonal == left && left == down)
                    {
                        consumeLine(x, y, 0, 1, left, area);
                        consumeLine(x, y, 1, 0, down, area);
                        consumeLine(x, y + left - 1, 1, -1, diagonal, area);
                        return new Triangle();
                    }
                }
            }

            if (left > 1 && down == 1)
            {
                //
                //    ----
                //   /  /
                //  /  /
                // ----
                //
                if (rightDown > 1)
                {
                    int bottom = scanLine(width, height, x + (rightDown - 1), y - (rightDown - 1), 0, 1, area);
                    if (bottom == left)
                    {
                        int lastLine = scanLine(width, height, x, y + left - 1, 1, -1, area);
                        if (lastLine == rightDown)
                        {
                            consumeLine(x, y, 0, 1, left, area);
                            consumeLine(x, y, 1, -1, rightDown, area);
                            consumeLine(x + (rightDown - 1), y - (rightDown - 1), 0, 1, bottom, area);
                            consumeLine(x, y + left - 1, 1, -1, lastLine, area);
                            return new Parallelogram();
                        }
                    }
                }

                if (leftDown > 1)
                {
                    //
                    // ----
                    // \   \
                    //  \   \
                    //   -----
                    //
                    int bottom = scanLine(width, height, x + (leftDown - 1), y + (leftDown - 1), 0, 1, area);
                    if (bottom == left)
                    {
                        int lastLine = scanLine(width, height, x, y + left - 1, 1, 1, area);
                        if (lastLine == leftDown)
                        {
                            consumeLine(x, y, 0, 1, left, area);
                            consumeLine(x, y, 1, 1, leftDown, area);
                            consumeLine(x + (leftDown - 1), y + (leftDown - 1), 0, 1, bottom, area);
                            consumeLine(x, y + left - 1, 1, 1, lastLine, area);
                            return new Parallelogram();
                        }
                    }
                }

                if (leftDown == left && left > 2)
                {
                    //
                    // ----
                    //  \ |
                    //   \|
                    //
                    int lastSide = scanLine(width, height, x, y + left - 1, 1, 0, area);
                    if (lastSide == leftDown)
                    {
                        consumeLine(x, y, 0, 1, left, area);
                        consumeLine(x, y, 1, 1, leftDown, area);
                        consumeLine(x, y + left - 1, 1, 0, lastSide, area);
                        return new Triangle();
                    }
                }
                else
                {
                    if (leftDown * 2 - 1 == left && left > 2 && leftDown > 2)
                    {
                        //
                        //  ----
                        //  \  /
                        //   \/
                        //
                        int lastSide = scanLine(width, height, x + leftDown - 1, y + leftDown - 1, -1, 1, area);
                        if (lastSide == leftDown)
                        {
                            consumeLine(x, y, 0, 1, left, area);
                            consumeLine(x, y, 1, 1, leftDown, area);
                            consumeLine(x + leftDown - 1, y + leftDown - 1, -1, 1, lastSide, area);
                            return new Triangle();
                        }
                    }
                    if (leftUp * 2 - 1 == left && left > 2 && leftUp > 2)
                    {
                        //
                        //  /\
                        // /  \
                        // ----
                        //
                        int lastSide = scanLine(width, height, x - (leftUp - 1), y + leftUp - 1, 1, 1, area);
                        if (lastSide == leftUp)
                        {
                            consumeLine(x, y, 0, 1, left, area);
                            consumeLine(x, y, -1, 1, leftUp, area);
                            consumeLine(x - (leftUp - 1), y + leftUp - 1, 1, 1, lastSide, area);
                            return new Triangle();
                        }
                    }
                }
            }

            if (left == 1 && down > 1)
            {
                if (leftDown > 1)
                {

                    //
                    // |\
                    // | \
                    //  \ |
                    //   \|
                    //
                    int right = scanLine(width, height, x + leftDown - 1, y + leftDown - 1, 1, 0, area);
                    if (right == down)
                    {
                        int lastSide = scanLine(width, height, x + down - 1, y, 1, 1, area);
                        if (lastSide == leftDown)
                        {
                            consumeLine(x, y, 1, 0, down, area);
                            consumeLine(x, y, 1, 1, leftDown, area);
                            consumeLine(x + leftDown - 1, y + leftDown - 1, 1, 0, right, area);
                            consumeLine(x + down - 1, y, 1, 1, lastSide, area);
                            return new Parallelogram();
                        }
                    }

                    if (down > 2)
                    {
                        // 
                        // |\
                        // | \
                        // ----
                        //
                        int bot = scanLine(width, height, x + down - 1, y, 0, 1, area);
                        if (bot == leftDown && leftDown == down)
                        {
                            consumeLine(x, y, 1, 0, down, area);
                            consumeLine(x, y, 1, 1, leftDown, area);
                            consumeLine(x + down - 1, y, 0, 1, bot, area);
                            return new Triangle();
                        }

                        if (leftDown * 2 - 1 == down && leftDown > 2)
                        {
                            //
                            // |\
                            // | \
                            // | /
                            // |/
                            //
                            int lastSide = scanLine(width, height, x + leftDown - 1, y + leftDown - 1, 1, -1, area);
                            if (lastSide == leftDown)
                            {
                                consumeLine(x, y, 1, 0, down, area);
                                consumeLine(x, y, 1, 1, leftDown, area);
                                consumeLine(x + leftDown - 1, y + leftDown - 1, 1, -1, lastSide, area);
                                return new Triangle();
                            }
                        }
                    }
                }
                if (rightDown > 1)
                {
                    // 
                    //    /|
                    //   / |
                    //   | /
                    //   |/
                    //
                    int leftSide = scanLine(width, height, x + (rightDown - 1), y - (rightDown - 1), 1, 0, area);
                    if (leftSide == down)
                    {
                        int lastSide = scanLine(width, height, x + down - 1, y, 1, -1, area);
                        if (lastSide == rightDown)
                        {
                            consumeLine(x, y, 1, 0, down, area);
                            consumeLine(x, y, 1, -1, rightDown, area);
                            consumeLine(x + (rightDown - 1), y - (rightDown - 1), 1, 0, leftSide, area);
                            consumeLine(x + down - 1, y, 1, -1, lastSide, area);
                            return new Parallelogram();
                        }
                    }

                    //
                    //  /|
                    // / |
                    //----
                    //
                    if (down > 2)
                    {
                        int bot = scanLine(width, height, x + down - 1, y, 0, -1, area);
                        if (bot == rightDown && rightDown == down)
                        {
                            consumeLine(x, y, 1, 0, down, area);
                            consumeLine(x, y, 1, -1, rightDown, area);
                            consumeLine(x + down - 1, y, 0, -1, bot, area);
                            return new Triangle();
                        }

                        if (rightDown * 2 - 1 == down && rightDown > 2)
                        {
                            //
                            //  /|
                            // / |
                            // \ |
                            //  \|
                            //
                            int lastSide = scanLine(width, height, x + (rightDown - 1), y - (rightDown - 1), 1, 1, area);
                            if (lastSide == rightDown)
                            {
                                consumeLine(x, y, 1, 0, down, area);
                                consumeLine(x, y, 1, -1, rightDown, area);
                                consumeLine(x + (rightDown - 1), y - (rightDown - 1), 1, 1, lastSide, area);
                                return new Triangle();
                            }
                        }
                    }
                }
            }

            return null;
        }

        static int scanLine(int width, int height, int x, int y, int dx, int dy, State[,] area)
        {
            int count = 0;
            for (int r = x, c = y; r >= 0 && c>= 0 && r < width && c < height; r += dx, c += dy )
            {
                if (area[r, c].Value && !area[r, c].Claimed)
                {
                    count++;
                }
                else
                    break;
            }
            return count;
        }

        static void consumeLine(int x, int y, int dx, int dy, int steps, State[,] area)
        {
            for (int r = x, c = y; steps > 0; r += dx, c += dy, steps--)
            {
                area[r, c].Claimed = true;
            }
        }

        static string ToBinary(int x)
        {
            if (x > 255)
                throw new Exception();
            StringBuilder b = new StringBuilder();
            while (x > 0)
            {
                b.Insert(0, x % 2);
                x /= 2;
            }
            b.Insert(0, "00000000");
           if (b.Length > 8)
                b.Remove(0, b.Length - 8);
            return b.ToString();
        }
    }
}
