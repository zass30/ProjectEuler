using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectEuler
{
    class _215
    {
        private static Dictionary<string, int> d = new Dictionary<string, int>();
        private static Dictionary<string, int> ov = new Dictionary<string, int>();
        private static int[] brickCombinations; // ways to make a 1 height wall of length X with 2x1 and 3x1 bricks
        private static List<string>[] brickPatterns;
        private static tower[] towers;

        public static void Run(int X, int Y)
        {
            generateBrickCombinations(X);
            generatePatterns(X);
            // towers are initialized at 1, now loop 
            for (int i = 1; i<Y; i++)
            {
                incrementTowers();
            }
            ulong sum = 0;
            foreach (tower t in towers)
            {
                sum += t.amount;
            }
            Console.WriteLine("W({0},{1})={2}", X, Y, sum);
            Console.ReadKey();
        }

        // a tower is a brick wall with a number of combinations, and a bottom pattern 
        public struct tower
        {
            public ulong amount;
            public pattern pattern;

        }

        public struct pattern
        {
            public int length;
            public string layout; // for example 2232, or 333 for a 9 length
            public int[] cracks; // where the cracks are
            public List<pattern> states; // the patterns that can be put below this pattern
        }

        public static void incrementTowers()
        {
            // create temp dictionary of heights
            // go through each tower
            // then each state transition
            // add 1 to height.
            // add to temp array
            // after loop, set each tower to temparray
            Dictionary<string, ulong> d = new Dictionary<string, ulong>();
            foreach (tower t in towers)
            {
                d[t.pattern.layout] = 0;
            }
            foreach (tower t in towers)
            {
                foreach (pattern p in t.pattern.states)
                {
                    d[p.layout] += t.amount;
                }
            }
            for (int i = 0; i < towers.Length; i++)
            {
                towers[i].amount = d[towers[i].pattern.layout];
            }
        }

        public static void generatePatterns(int len)
        {
            towers = new tower[brickCombinations[len]];
            int i = 0;
            foreach (string s in brickPatterns[len])
            {
                pattern p = new pattern();
                p.layout = s;
                p.length = s.Length;
                p.cracks = new int[p.length - 1];
                int l = 0;
                foreach (char c in s)
                {
                    if (l == s.Length - 1)
                        continue;
                    if (l == 0)
                        p.cracks[l] = (int)Char.GetNumericValue(c);
                    else
                        p.cracks[l] = (int)Char.GetNumericValue(c) + p.cracks[l - 1];
                    l++;
                }
                towers[i].pattern = p;
                towers[i].amount = 1;
                i++;                
            }

            generateStates(len);
        }

        // find which states can go to which other states. 
        public static void generateStates(int len)
        {
            for (int i = 0; i < towers.Length; i++)
            {
                towers[i].pattern.states = new List<pattern>();
                foreach (tower candidate in towers)
                {
                    if (!overlapsCracks(towers[i].pattern, candidate.pattern))
                        towers[i].pattern.states.Add(candidate.pattern);

                }
            }
        }

        public static bool overlapsCracks(pattern a, pattern b)
        {
            foreach (int i in a.cracks)
            {
                foreach (int j in b.cracks)
                {
                    if (i == j)
                        return true;
                }
            }
            return false;
        }

        public static void generateBrickCombinations(int a)
        {
            brickCombinations = new int[a+1];
            brickPatterns = new List<string>[a + 1];
            brickCombinations[0] = brickCombinations[1] = 0;
            brickPatterns[0] = new List<string>();
            brickPatterns[1] = new List<string>();
            brickCombinations[2] = brickCombinations[3] = 1;
            brickPatterns[2] = new List<string>();
            brickPatterns[2].Add("2");
            brickPatterns[3] = new List<string>();
            brickPatterns[3].Add("3");

            for (int i = 4; i <= a; i++)
            {
                brickCombinations[i] = brickCombinations[i - 2] + brickCombinations[i - 3];
                brickPatterns[i] = new List<string>();
                foreach (string s in brickPatterns[i - 2])
                {
                    brickPatterns[i].Add(s + "2");
                }
                foreach (string s in brickPatterns[i - 3])
                {
                    brickPatterns[i].Add(s + "3");
                }
            }
        }
    }
}
