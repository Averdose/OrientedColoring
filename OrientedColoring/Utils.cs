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
            List<Edge> neighbours = g.OutEdges(index).ToList();
            List<int> outN = neighbours.FindAll(e => e.From == index).Select(e => e.To).ToList();
            List<int> inN = neighbours.FindAll(e => e.To == index).Select(e => e.From).ToList();
            if(outN.Intersect(inN).Any())
            {
                return false;
            }
            foreach(int v in outN)
            {
                if (coloring[v] != -1)
                {
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
            List<Edge> neighbours = g.OutEdges(index).ToList();
            List<int> outN = neighbours.FindAll(e => e.From == index).Select(e => e.To).ToList();
            List<int> inN = neighbours.FindAll(e => e.To == index).Select(e => e.From).ToList();
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
}
