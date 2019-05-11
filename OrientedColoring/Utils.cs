using OrientedColoring.GraphHelper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrientedColoring
{
    public class Utils
    {
        /// <summary>
        /// checks if the "color" would be legal for vertice with index "index" in the graph g knowing the coloring "coloring"
        /// with a helper array "helperColoring" 
        /// </summary>
        /// <param name="color"></param>
        /// <param name="index"></param>
        /// <param name="coloring"></param>
        /// <param name="helperColoring">Stores information about the coloring done so far,
        /// if helperColoring[c1,c2] is true than there exists an edge from vertex with color c1 to vertex with color c2</param>
        /// <param name="g"></param>
        /// <returns>information if coloring is legal</returns>
        public static bool IsLegal(int color, int index, int[] coloring, bool[,] helperColoring, GraphHelper.Graph g)
        {
            List<int> outN = g.OutEdges(index).Select(e => e.To).ToList(); ;
            List<int> inN = g.InEdges(index).Select(e => e.From).ToList(); ;
            List<int> outNColors = new List<int>();
            List<int> inNColors = new List<int>();
            if (outN.Intersect(inN).Any())
            {
                return false;
            }
            foreach(int v in outN)
            {
                if (coloring[v] != -1)
                {
                    outNColors.Add(coloring[v]);
                    if (helperColoring[coloring[v], color])
                    {
                        return false;
                    }
                }
                if (coloring[v] == color)
                {
                    return false;
                }
            }
            foreach (int v in inN)
            {
                if (coloring[v] != -1)
                {
                    inNColors.Add(coloring[v]);
                    if (helperColoring[color, coloring[v]])
                    {
                        return false;
                    }
                }
                if (coloring[v] == color)
                {
                    return false;
                }
            }
            if (outNColors.Intersect(inNColors).Any())
            {
                return false;
            }
            return true;
        }
        /// <summary>
        /// used to fill helper array after assigning color "color" to vertex with index "index" in graph "g" given the current coloring
        /// </summary>
        /// <param name="helper">This array will be modified by the method accoriding to the following principle
        /// if helperColoring[c1,c2] is true than there exists an edge from vertex with color c1 to vertex with color c2</param>
        /// <param name="coloring"></param>
        /// <param name="color"></param>
        /// <param name="index"></param>
        /// <param name="g"></param>
        public static void FillHelper(ref bool[,] helper, int[] coloring, int color, int index, Graph g)
        {
            List<int> outN = g.OutEdges(index).Select(e => e.To).ToList(); ;
            List<int> inN = g.InEdges(index).Select(e => e.From).ToList(); ;
            foreach (int v in outN)
            {
                if (coloring[v] != -1)
                {
                    helper[color, coloring[v]] = true;
                }
            }
            foreach (int v in inN)
            {
                if (coloring[v] != -1)
                {
                    helper[coloring[v], color] = true;
                }
            }
        }
    }

    public class DsaturVertex
    {
        int index;
        int value;

        public int Index { get => index; set => index = value; }
        public int Value { get => value; set => this.value = value; }

        public DsaturVertex(int _index, int _value)
        {
            index = _index;
            value = _value;
        }
    }
}
