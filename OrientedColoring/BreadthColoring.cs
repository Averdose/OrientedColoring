using OrientedColoring.GraphHelper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrientedColoring
{
    public class BreadthColoring
    {
        /// <summary>
        /// Solves the oriented coloring problem for graph using the Breadth first coloring algorithm
        /// </summary>
        /// <param name="graph"></param>
        /// <returns>
        /// color assigned to verice x is equeal to ans[x]
        /// if any of the values in the returned array is -1 than an oriented coloring does not exist
        /// </returns>
        public static int[] Solve(Graph graph)
        {
            bool[,] helper = new bool[graph.VerticesCount, graph.VerticesCount];
            int[] answer = Enumerable.Repeat(-1, graph.VerticesCount).ToArray();
            bool[] visited = new bool[graph.VerticesCount];
            Queue<int> q = new Queue<int>();
            q.Enqueue(graph.GetSmallestIndex(new List<int>()));
            int numVisited = 0;
            while(q.Count > 0)
            {
                int index = q.Dequeue();
                visited[index] = true;
                numVisited++;
                for (int c = 0; c < graph.VerticesCount; c++)
                {
                    if (Utils.IsLegal(c, index, answer, helper, graph))
                    {
                        Utils.FillHelper(ref helper, answer, c, index, graph);
                        answer[index] = c;
                        break;
                    }
                }
                if (numVisited != graph.VerticesCount)
                {
                    q.Enqueue(graph.GetSmallestIndex(new List<int>()));
                }

            }
            return answer;
        }
    }
}
