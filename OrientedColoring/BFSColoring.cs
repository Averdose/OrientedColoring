using OrientedColoring.GraphHelper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrientedColoring
{
    public class BFSColoring
    {
        /// <summary>
        /// Solves the oriented coloring problem for graph using the Breadth first coloring algorithm
        /// </summary>
        /// <param name="graph"></param>
        /// <returns>
        /// color assigned to verice x is equeal to ans[x]
        /// if any of the values in the returned array is -1 than an oriented coloring does not exist
        /// </returns>
        public static int[] Solve( ref Graph graph)
        {
            int n = graph.VerticesCount;
            bool[] visited = new bool[n];
            int[] newColoring = Enumerable.Repeat(-1, n).ToArray();
            bool[,] colors = new bool[n, n];
            Queue<int> queue = new Queue<int>();
            queue.Enqueue(UnvisitedVertexWithMinEdges(graph, visited));
            while (queue.Count > 0)
            {
                int vertex = queue.Dequeue();
                visited[vertex] = true;
                List<int> in_neighbours = graph.InEdges(vertex).Select(e => e.From).ToList();
                List<int> out_neighbours = graph.OutEdges(vertex).Select(e => e.To).ToList();

                int c = MinValidColor(graph, in_neighbours, out_neighbours, colors, newColoring);

                newColoring[vertex] = c;
                foreach (int i in in_neighbours)
                {
                    if (newColoring[i] != -1)
                    {
                        colors[newColoring[i], c] = true;
                    }
                }
                foreach (int o in out_neighbours)
                {
                    if (newColoring[o] != -1)
                    {
                        colors[c, newColoring[o]] = true;
                    }
                    if (!visited[o])
                    {
                        queue.Enqueue(o);
                    }
                }

                if (queue.Count == 0)
                {
                    for (int i = 0; i < n; i++)
                    {
                        if (!visited[i])
                        {
                            queue.Enqueue(i);
                            break;
                        }
                    }
                }
            }

            graph.ColorsMatrix = newColoring;
            return newColoring;
        }

        private static int MinValidColor(Graph graph, List<int> in_neighbours, List<int> out_neighbours, bool[,] colors, int[] newColoring)
        {
            int color = -1;
            int n = graph.VerticesCount;
            List<int> neighbours = in_neighbours.Concat(out_neighbours).ToList();
            bool[] colorReservedByNeighbours = new bool[n];
            foreach (int neighb in neighbours)
            {
                if (newColoring[neighb] != -1)
                {
                    colorReservedByNeighbours[newColoring[neighb]] = true;
                }
            }

            for (int i = 0; i < n; i++)
            {
                if (!colorReservedByNeighbours[i])
                {
                    bool directionBlocked = false;
                    foreach (int o in out_neighbours)
                    {
                        if (newColoring[o] != -1 && colors[newColoring[o], i])
                        {
                            //color already exist in opposite direction
                            directionBlocked = true;
                            continue;
                        }
                    }
                    foreach (int in_n in in_neighbours)
                    {
                        if (newColoring[in_n] != -1 && colors[i, newColoring[in_n]])
                        {
                            //color already exist in opposite direction
                            directionBlocked = true;
                            continue;
                        }
                    }
                    if (directionBlocked)
                    {
                        continue;
                    }
                    color = i;
                    break;
                }
            }

            return color;
        }

        private static int UnvisitedVertexWithMinEdges(Graph graph, bool[] visited)
        {
            int minVertex = -1;
            int minEdgesCount = int.MaxValue;
            for (int i = 0; i < graph.VerticesCount; i++)
            {
                if (!visited[i])
                {
                    if (graph.OutEdges(i).Count < minEdgesCount)
                    {
                        minEdgesCount = graph.OutEdges(i).Count;
                        minVertex = i;
                    }
                }
            }
            return minVertex;
        }
    }
}
