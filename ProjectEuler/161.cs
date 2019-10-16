using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;

namespace ProjectEuler
{
    class _161
    {
        static int[] tri_0 = { 0b111 }; //7
        static int[] tri_1 = { 0b1, 0b1, 0b1 }; //1,1,1
        static int[] tri_2 = { 0b11, 0b10 }; //3,2
        static int[] tri_3 = { 0b11, 0b01 }; //3,1
        static int[] tri_4 = { 0b10, 0b11 }; //2,3
        static int[] tri_5 = { 0b01, 0b11 }; //1,3
        static int[][] triominoes;
        static Dictionary<string, int> combinations;

        public static void Init()
        {
            triominoes = new int[][]{ tri_0, tri_1, tri_2, tri_3, tri_4, tri_5 };
            combinations = new Dictionary<string, int>();
            foreach (int[] triomino in triominoes)
            {
                combinations[stringify(triomino)] = 1;
            }
        }

        public static string stringify(int[] a)
        {
            return string.Join("|", a);
        }

        public static void Run()
        {
            // write a function that takes a triomino and a pattern, and returns true if 
            // that triomino can put over the top leftmost item in pattern
            // to do that, make it a generic point in pattern
            // make 
            // mask function
            //            int[] data = new int[] { 0b110, 0b110, 0b110, 0b110, 0b110, 0b110, 0b110, 0b110, 0b110 };
            int[] data = new int[] { 0b111, 0b111 };
            Init();

            int sum = compute(data);
            // now need to be able to apply mask to a dataset larger than the mask.
            // we want to look at the subset of the dataset that matches the mask.

            Console.WriteLine("Hi");
            Console.ReadKey();
        }

        public static int compute(int[] data)
        {
            if (combinations.ContainsKey(stringify(data)))
                return combinations[stringify(data)];

            int sum = 0;
            foreach (int[] triomino in triominoes)
            {
               if (isMask(triomino, data))
                {
                    int[] data_minus_triomino = removeTriomino(triomino, data);
                    int s = compute(data_minus_triomino);
                    sum += s;
                }
            }
            combinations[stringify(data)] = sum;
            return sum;
        }


        public static int[] trimData(int[] data)
        {
            int l = data.Length;
            if (data[l - 1] == 0)
                return trimData(data.Take(l - 1).ToArray());
            else if (data[0] == 0)
                return trimData(data.Skip(1).ToArray());
            else
            {
                return data;
            }
        }

        // assuming triomino masks on data, remove it
        public static int[] removeTriomino(int[] triomino, int[] data)
        {
            int[] returndata = new int[data.Length];
            int i = 0;
            foreach (int m in triomino)
            {
                returndata[i] = m ^ data[i];
                i++;
            }
            while (i < data.Length)
            {
                returndata[i] = data[i];
                i++;
            }
            return trimData(returndata);
        }

        public static bool isMask(int [] mask, int [] data)
        {
            int i = 0;
            foreach (int m in mask)
            {
                if (i >= data.Length)
                    return false;
                if ((m & data[i]) != m)
                    return false;
                i++;
            }
            return true;
        }
    }

    public class pattern
    {
        public int[] data;

        public override int GetHashCode()
        {
            Tuple.Create(data);
            return base.GetHashCode();
        }
    }
}
