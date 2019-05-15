using OrientedColoring.GraphHelper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrientedColoring
{
    public static class RandomGraph
    {
        /// <summary>
        /// Generates random graph with v vertices with a specified number of edges
        /// </summary>
        /// <param name="v">natural number of vertices</param>
        /// <param name="saturation">number between 0 and 1 specifying edge saturation level</param>
        /// <returns></returns>
        public static Graph GenerateGraph(int v, double saturation)
        {
            Random random = new Random();
            Graph g = new Graph(v);
            bool[,] edges = new bool[v, v];
            List<TwoInts> positions = new List<TwoInts>();
            for(int i = 0; i < v; i ++)
            {
                for(int j = i + 1; j < v; j++)
                {
                   positions.Add(new TwoInts(i, j));
                }
            }
            int maxEdges = positions.Count;
            int edgeNum = Convert.ToInt32(saturation * maxEdges);

            for (int i = 0; i < edgeNum; i++)
            {
                int index = random.Next(maxEdges - i);
                TwoInts toFill = positions[index];
                positions.RemoveAt(index);
                if(random.Next(2) == 1)
                {
                    g.AddEdge(toFill.A, toFill.B);
                }
                else
                {
                    g.AddEdge(toFill.B, toFill.A);
                }
            }
            return g;

        }
    }

    class TwoInts
    {
        int a, b;

        public int A { get => a; set => a = value; }
        public int B { get => b; set => b = value; }

        public TwoInts(int _a, int _b)
        {
            a = _a;
            b = _b;
        }
    }
}
