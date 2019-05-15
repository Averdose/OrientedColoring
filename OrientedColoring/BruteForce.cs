using OrientedColoring.GraphHelper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrientedColoring
{
    public static class BruteForce
    {
        public static int[] Solve( ref Graph graph)
        {
            int v = graph.VerticesCount;
            bool[,] helper;
            int[] answer;
            int currentBest = int.MaxValue;
            int[] bestAnswer = new int[v];
            var list = Enumerable.Range(0, v);
            var orders = GetPermutations(list,list.Count());
            foreach(IEnumerable<int> order in orders)
            {
                helper = new bool[v, v];
                answer = Enumerable.Repeat(-1, v).ToArray();
                foreach (int index in order)
                {
                    for (int c = 0; c < v; c++)
                    {
                        if (Utils.IsLegal(c, index, answer, helper, graph, 0))
                        {
                            Utils.FillHelper(ref helper, answer, c, index, graph);
                            graph.ColorsMatrix[index] = c;
                            answer[index] = c;
                            break;
                        }
                    }
                }
                if (!answer.Any(r => r == -1))
                {
                    int chromaticNumber = answer.Distinct().Count();
                    if(chromaticNumber < currentBest)
                    {
                        currentBest = chromaticNumber;
                        bestAnswer = answer;
                        graph.ColorsMatrix = answer;
                    }
                }

            }
            return bestAnswer;
        }


        /// <summary>
        /// Generates list of all permutations of the given list elements. All credit for genereting permutations goes to Pengyang
        /// The code has been modified the original can be found at https://stackoverflow.com/questions/756055/listing-all-permutations-of-a-string-integer
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <param name="length"></param>
        /// <returns></returns>
        static IEnumerable<IEnumerable<T>> GetPermutations<T>(IEnumerable<T> list, int length)
        {
            if (length == 1)
            {
                return list.Select(t => new T[] { t });
            }
            return GetPermutations(list, length - 1).SelectMany(t => list.Where(e => !t.Contains(e)),
                    (t1, t2) => t1.Concat(new T[] { t2 }));
        }
    }

}
