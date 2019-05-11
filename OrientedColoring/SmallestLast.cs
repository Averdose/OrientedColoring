using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OrientedColoring.GraphHelper;


namespace OrientedColoring
{
    public class SmallestLast
    {
        /// <summary>
        /// Solves the oriented coloring problem for graph using the smallest last algorithm
        /// </summary>
        /// <param name="graph"></param>
        /// <returns>
        /// color assigned to verice x is equeal to ans[x]
        /// if any of the values in the returned array is -1 than an oriented coloring does not exist
        /// </returns>
        public static int[] Solve(ref Graph graph)
        {
            Graph g = graph.Clone();
            bool[,] helper = new bool[graph.VerticesCount, graph.VerticesCount];
            int[] answer = Enumerable.Repeat(-1, graph.VerticesCount).ToArray();
            List<int> order = new List<int>();
            List<int> removed = new List<int>();
            while(removed.Count != graph.VerticesCount)
            {
                int index = g.GetSmallestIndex(removed);
                order.Insert(0, index);
                removed.Add(index);
                g.RemoveVertice(index);
            }
            foreach(int index in order)
            {
                for(int c = 0; c < graph.VerticesCount; c++)
                {
                    if (Utils.IsLegal(c, index, answer, helper, graph))
                    {
                        Utils.FillHelper(ref helper, answer, c, index, graph);
                        graph.ColorsMatrix[index] = c;
                        answer[index] = c;
                        break;
                    }
                }
            }
            return answer;
        }

    }
}
