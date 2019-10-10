using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MathNet.Numerics.LinearAlgebra;
using MathNet.Numerics.LinearAlgebra.Double;

namespace ProjectEuler
{
    class _213
    {
        public static void Run(int N, int M, int rings)
        {

            var initstate = StateMatrix(N, M);
            var state = StateMatrix(N, M);
            var init = InitMatrix(N, M);

            for (int i = 1; i < rings; i++)
            {
                state = state * initstate;
            }

            Matrix<double> result = init * state;

            var rows = result.EnumerateColumns();
            double total_unoccupied = 0;
            foreach (var row in rows)
            {
                var items = row.Enumerate();
                double prob_unoccupied = 1;
                double sanity = 0;
                foreach (var item in items)
                {
                    sanity += item;
                    prob_unoccupied *= 1 - item;
                }
                total_unoccupied += prob_unoccupied;
            }

            Console.WriteLine("Total unoccoupied: " + String.Format("{0:0.000000}", total_unoccupied));
        }

        private static Matrix<double> InitMatrix(int N, int M)
        {
            var init1 = Matrix<double>.Build;
            var init = init1.Dense(N * M, N * M, (i, j) => i == j ? 1 : 0);
            return init;
        }

        private static Matrix<double> StateMatrix(int N, int M)
        {
            var state1 = Matrix<double>.Build;
            var state = state1.Dense(N * M, N * M, 0);

            int topleft = 0;
            int topright = N - 1;
            int bottomleft = N * (M - 1);
            int bottomright = N * M - 1;

            int down(int a)
            {
                return a + N;
            }

            int up(int a)
            {
                return a - N;
            }

            int right(int a)
            {
                return a + 1;
            }

            int left(int a)
            {
                return a - 1;
            }

            // build corners
            state[topleft, right(topleft)] = 0.5;
            state[topleft, down(topleft)] = 0.5;
            state[topright, left(topright)] = 0.5;
            state[topright, down(topright)] = 0.5;
            state[bottomleft, right(bottomleft)] = 0.5;
            state[bottomleft, up(bottomleft)] = 0.5;
            state[bottomright, up(bottomright)] = 0.5;
            state[bottomright, left(bottomright)] = 0.5;


            if (N > 2)
            {
                // build top row
                for (int i = right(topleft); i < topright; i++)
                {
                    state[i, right(i)] = 1.0 / 3.0;
                    state[i, left(i)] = 1.0 / 3.0;
                    state[i, down(i)] = 1.0 / 3.0;
                }

                // build bottom row
                for (int i = right(bottomleft); i < bottomright; i++)
                {
                    state[i, right(i)] = 1.0 / 3.0;
                    state[i, left(i)] = 1.0 / 3.0;
                    state[i, up(i)] = 1.0 / 3.0;
                }
            }

            if (M > 2)
            {
                // build west column
                for (int i = down(topleft); i < bottomleft; i += N)
                {
                    state[i, right(i)] = 1.0 / 3.0;
                    state[i, down(i)] = 1.0 / 3.0;
                    state[i, up(i)] = 1.0 / 3.0;
                }

                // build east column
                for (int i = down(topright); i < bottomright; i += N)
                {
                    state[i, left(i)] = 1.0 / 3.0;
                    state[i, down(i)] = 1.0 / 3.0;
                    state[i, up(i)] = 1.0 / 3.0;
                }
            }

            if (N > 2 && M > 2)
            {
                // build interior
                for (int i = right(topleft); i < topright; i++)
                {
                    for (int j = down(topleft); j < bottomleft; j += N)
                    {
                        state[i + j, left(i + j)] = 0.25;
                        state[i + j, right(i + j)] = 0.25;
                        state[i + j, up(i + j)] = 0.25;
                        state[i + j, down(i + j)] = 0.25;
                    }
                }
            }
            return state;
        }
    }
}
