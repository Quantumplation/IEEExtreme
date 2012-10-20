using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AC
{
    enum Color
    {
        A,B,C,D,E,F,G,H,I,J,K,L,M,N,O,P,Q,R,S,T,U,V,W,X,Y,Z
    }

    struct Face
    {
        public static readonly int[] Indices = new int[] { 0, 1, 2, 5, 8, 7, 6, 3 };

        public Face(Color c)
        {
            tiles = new Color[9];
            for (int x = 0; x < 9; x++)
                tiles[x] = c;
        }

        public Color[] tiles;

        public void CW()
        {
            Color TempC = tiles[0];
            tiles[0] = tiles[6];
            tiles[6] = tiles[8];
            tiles[8] = tiles[2];
            tiles[2] = TempC;

            Color TempE = tiles[1];
            tiles[1] = tiles[3];
            tiles[3] = tiles[7];
            tiles[7] = tiles[5];
            tiles[5] = TempE;
        }
    }

    struct Cube
    {
        public Face up, left, front, right, back, down;

        public Cube(Color u, Color l, Color f, Color r, Color b, Color d)
        {
            up = new Face(u);
            left = new Face(l);
            front = new Face(f);
            right = new Face(r);
            back = new Face(b);
            down = new Face(d);
        }

        public void Do(string d)
        {
            switch (d)
            {
                case "U": U(); break;
                case "U2": U2(); break;
                case "U'": UP(); break;

                case "D": D(); break;
                case "D2": D2(); break;
                case "D'": DP(); break;

                case "F": F(); break;
                case "F2": F2(); break;
                case "F'": FP(); break;

                case "L": L(); break;
                case "L2": L2(); break;
                case "L'": LP(); break;

                case "B": B(); break;
                case "B2": B2(); break;
                case "B'": BP(); break;

                case "R": R(); break;
                case "R2": R2(); break;
                case "R'": RP(); break;
            }
        }

        public void U() { X(up); ROTCW(back, new int[] { 0, 1, 2 }, right, new int[] { 0, 1, 2 }, front, new int[] { 0, 1, 2 }, left, new int[] { 0, 1, 2 }); }
        public void U2() { X2(up); ROTCW(back, new int[] { 0, 1, 2 }, right, new int[] { 0, 1, 2 }, front, new int[] { 0, 1, 2 }, left, new int[] { 0, 1, 2 }); ROTCW(back, new int[] { 0, 1, 2 }, right, new int[] { 0, 1, 2 }, front, new int[] { 0, 1, 2 }, left, new int[] { 0, 1, 2 }); }
        public void UP() { XP(up); ROTCCW(back, new int[] { 0, 1, 2 }, right, new int[] { 0, 1, 2 }, front, new int[] { 0, 1, 2 }, left, new int[] { 0, 1, 2 }); }

        public void L() { X(left); ROTCW(up, new int[] { 0, 3, 6 }, front, new int[] { 0, 3, 6 }, down, new int[] { 0, 3, 6 }, back, new int[] { 8, 5, 2 }); }
        public void L2() { X2(left); ROTCW(up, new int[] { 0, 3, 6 }, front, new int[] { 0, 3, 6 }, down, new int[] { 0, 3, 6 }, back, new int[] { 8, 5, 2 }); ROTCW(up, new int[] { 0, 3, 6 }, front, new int[] { 0, 3, 6 }, down, new int[] { 0, 3, 6 }, back, new int[] { 8, 5, 2 }); }
        public void LP() { XP(left); ROTCCW(up, new int[] { 0, 3, 6 }, front, new int[] { 0, 3, 6 }, down, new int[] { 0, 3, 6 }, back, new int[] { 8, 5, 2 }); }

        public void F() { X(front); ROTCW(up, new int[] { 6, 7, 8 }, right, new int[] { 0, 3, 6 }, down, new int[] { 2, 1, 0 }, left, new int[] { 8, 5, 2 }); }
        public void F2() { X2(front); ROTCW(up, new int[] { 6, 7, 8 }, right, new int[] { 0, 3, 6 }, down, new int[] { 2, 1, 0 }, left, new int[] { 8, 5, 2 }); ROTCW(up, new int[] { 6, 7, 8 }, right, new int[] { 0, 3, 6 }, down, new int[] { 2, 1, 0 }, left, new int[] { 8, 5, 2 }); }
        public void FP() { XP(front); ROTCCW(up, new int[] { 6, 7, 8 }, right, new int[] { 0, 3, 6 }, down, new int[] { 2, 1, 0 }, left, new int[] { 8, 5, 2 }); }

        public void R() { X(right); ROTCW(up, new int[] { 8, 5, 2 }, back, new int[] { 0, 3, 6 }, down, new int[] { 8, 5, 2 }, front, new int[] { 8, 5, 2 }); }
        public void R2() { X2(right); ROTCW(up, new int[] { 8, 5, 2 }, back, new int[] { 0, 3, 6 }, down, new int[] { 8, 5, 2 }, front, new int[] { 8, 5, 2 }); ROTCW(up, new int[] { 8, 5, 2 }, back, new int[] { 0, 3, 6 }, down, new int[] { 8, 5, 2 }, front, new int[] { 8, 5, 2 }); }
        public void RP() { XP(right); ROTCCW(up, new int[] { 8, 5, 2 }, back, new int[] { 0, 3, 6 }, down, new int[] { 8, 5, 2 }, front, new int[] { 8, 5, 2 }); }

        public void B() { X(back); ROTCW(up, new int[] { 2, 1, 0 }, left, new int[] { 0, 3, 6 }, down, new int[] { 6, 7, 8 }, right, new int[] { 8, 5, 2 }); }
        public void B2() { X2(back); ROTCW(up, new int[] { 2, 1, 0 }, left, new int[] { 0, 3, 6 }, down, new int[] { 6, 7, 8 }, right, new int[] { 8, 5, 2 }); ROTCW(up, new int[] { 2, 1, 0 }, left, new int[] { 0, 3, 6 }, down, new int[] { 6, 7, 7 }, right, new int[] { 8, 5, 2 }); }
        public void BP() { XP(back); ROTCCW(up, new int[] { 2, 1, 0 }, left, new int[] { 0, 3, 6 }, down, new int[] { 6, 7, 8 }, right, new int[] { 8, 5, 2 }); }

        public void D() { X(down); ROTCW(back, new int[] { 6, 7, 8 }, left, new int[] { 6, 7, 8 }, front, new int[] { 6, 7, 8 }, right, new int[] { 6, 7, 8 }); }
        public void D2() { X2(down); ROTCW(back, new int[] { 6, 7, 8 }, left, new int[] { 6, 7, 8 }, front, new int[] { 6, 7, 8 }, right, new int[] { 6, 7, 8 }); ROTCW(back, new int[] { 6, 7, 8 }, left, new int[] { 6, 7, 8 }, front, new int[] { 6, 7, 8 }, right, new int[] { 6, 7, 8 }); }
        public void DP() { XP(down); ROTCCW(back, new int[] { 6, 7, 8 }, left, new int[] { 6, 7, 8 }, front, new int[] { 6, 7, 8 }, right, new int[] { 6, 7, 8 }); }

        public void ROTCW(Face f1, int[] f1i, Face f2, int[] f2i, Face f3, int[] f3i, Face f4, int[] f4i)
        {
            for (int x = 0; x < 3; x++)
            {
                Color temp = f4.tiles[f4i[x]];
                f4.tiles[f4i[x]] = f3.tiles[f3i[x]];
                f3.tiles[f3i[x]] = f2.tiles[f2i[x]];
                f2.tiles[f2i[x]] = f1.tiles[f1i[x]];
                f1.tiles[f1i[x]] = temp;
            }
        }

        public void ROTCCW(Face f1, int[] f1i, Face f2, int[] f2i, Face f3, int[] f3i, Face f4, int[] f4i)
        {
            for (int x = 0; x < 3; x++)
            {
                Color temp = f1.tiles[f1i[x]];
                f1.tiles[f1i[x]] = f2.tiles[f2i[x]];
                f2.tiles[f2i[x]] = f3.tiles[f3i[x]];
                f3.tiles[f3i[x]] = f4.tiles[f4i[x]];
                f4.tiles[f4i[x]] = temp;
            }
        }

        public void X(Face x)
        {
            x.CW();
        }
        public void X2(Face x)
        {
            x.CW();
            x.CW();
        }
        public void XP(Face x)
        {
            x.CW();
            x.CW();
            x.CW();
        }
    }

    class Solution
    {
        static void Main(string[] args)
        {
            string faces = Console.ReadLine();
            var faceColors = faces.Select(x => Enum.Parse(typeof(Color), x.ToString())).Cast<Color>().ToList();
            Cube c = new Cube(faceColors[0], faceColors[1], faceColors[2], faceColors[3], faceColors[4], faceColors[5]);

            string singmaster = Console.ReadLine();
            //singmaster = "UFDLRBU'F'D'L'R'B'U2F2D2L2R2B2";
            for (int x = 0; x < singmaster.Length; x++)
            {
                if (Char.IsLetter(singmaster[x]))
                {
                    if (x < singmaster.Length - 1 && singmaster[x + 1] == '\'')
                    {
                        //Console.WriteLine(singmaster[x] + "'");
                        c.Do(singmaster[x] + "'");
                        x++;
                    }
                    else if (x < singmaster.Length - 1 && singmaster[x + 1] == '2')
                    {
                        //Console.WriteLine(singmaster[x] + "2");
                        c.Do(singmaster[x] + "2");
                        x++;
                    }
                    else
                    {
                        //Console.WriteLine(singmaster[x]);
                        c.Do(singmaster[x] + "");
                    }
                }

            }

            for (int Y = 0; Y < 9; Y++)
                Console.Write(c.front.tiles[Y] + ((Y % 3 != 2) ? " " : "\n"));
        }
    }
}
