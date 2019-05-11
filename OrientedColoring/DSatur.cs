using OrientedColoring.GraphHelper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrientedColoring
{
    public class DSatur
    {
        public static int[] Solve(ref Graph g)
        {
            bool[,] helper = new bool[g.VerticesCount, g.VerticesCount];
            int[] answer = Enumerable.Repeat(-1, g.VerticesCount).ToArray();
            List<DsaturVertex> dCount = new List<DsaturVertex>();
            
            for (int i = 0; i < g.VerticesCount; i++)
            {
                dCount.Add(new DsaturVertex(i,0));
            }
            int index = g.GetLargestIndex();
            answer[index] = 0;
            g.ColorsMatrix[index] = 0;
            dCount.RemoveAt(index);
            FillDCount(ref dCount, index, 0, g);
            while(dCount.Count > 0)
            {
                dCount.Sort((c1, c2) => c1.Value.CompareTo(c2.Value));
                index = dCount[dCount.Count -1].Index;
                dCount.RemoveAt(dCount.Count -1);
                for (int c = 0; c < g.VerticesCount; c++)
                {
                    if (Utils.IsLegal(c, index, answer, helper, g))
                    {
                        Utils.FillHelper(ref helper, answer, c, index, g);
                        g.ColorsMatrix[index] = c;
                        answer[index] = c;
                        break;
                    }
                }
            }
            return answer;
        }
        
        private static void FillDCount(ref List<DsaturVertex> dcount, int index, int color, Graph g)
        {
            List<int> neighbours = g.InEdges(index).Select(e => e.From).ToList().Union(g.OutEdges(index).Select(e => e.To).ToList()).ToList();
            foreach(int vertex in neighbours)
            {
                int found = dcount.Select((v, i) => new { Index = i, Value = v }) // Pair up values and indexes
                    .Where(p => p.Value.Index == vertex) // Do the filtering
                    .Select(p => p.Index).ToArray()[0]; // Keep the index and drop the value
                dcount[found].Value++;
            }
        }
    }
}
