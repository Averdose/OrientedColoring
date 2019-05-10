using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrientedColoring.GraphHelper
{
    public class Graph
    {
        private List<Edge>[] _matrix;
        public int[] ColorsMatrix { get; set; }
        public int EdgesCount { get; private set; }
        public int VerticesCount { get { return _matrix.Length; } }

        public Graph(int vertices)
        {
            InitializeGraph(vertices);
        }

        public Graph(string filename)
        {
            if(!filename.Contains("."))
            {
                filename = filename + ".txt";
            }
            int line = 0;
            int fileEdgesCount = 0;
            using (StreamReader sr = File.OpenText(filename))
            {
                string s = String.Empty;
                
                while ((s = sr.ReadLine()) != null)
                {
                    if(line == 0)
                    {
                        int verticesCount = int.Parse(s);
                        InitializeGraph(verticesCount);
                    }
                    else if (line ==  1)
                    {
                        fileEdgesCount = int.Parse(s);
                    }
                    else
                    {
                        string[] vertices = s.Split(',');
                        if (vertices.Length != 2)
                        {
                            throw new IOException("Error while parsing file line " + (line+1));
                        }
                        int from = int.Parse(vertices[0]);
                        int to = int.Parse(vertices[1]);
                        if(from >= VerticesCount || to >= VerticesCount)
                        {
                            throw new IOException("Cannot create edge ("+from +","+to+"). Vertex index too big (indices are indexed from 0)");
                        }
                        AddEdge(from, to);
                    }
                    line++;
                }

                if(EdgesCount != fileEdgesCount)
                {
                    throw new IOException("Edge count missmatch.");
                }
            }

        }

        public bool IsColoringValid()
        {
            int c = VerticesCount;
            bool[,] colors = new bool[VerticesCount, VerticesCount];
            List<Edge> edges = Edges();
            foreach(Edge e in edges)
            {
                int fromColor = this.ColorsMatrix[e.From];
                int toColor = this.ColorsMatrix[e.To];
                if (fromColor == toColor) return false;
                if (colors[toColor, fromColor]) return false;
                colors[fromColor, toColor] = true;
            }
            return true;
        }

        private void InitializeGraph(int vertices)
        {
            _matrix = new List<Edge>[vertices];
            ColorsMatrix = new int[vertices];
            for (int i = 0; i < vertices; i++)
            {
                _matrix[i] = new List<Edge>();
                ColorsMatrix[i] = i;
            }
        }

        public bool AddEdge(int from, int to, double weight = 1)
        {
            if (weight == 0) return false;
            if (_matrix[from].Any(e => e.To == to)) return false;
            _matrix[from].Add(new Edge(from, to, weight));
            _matrix[to].Add(new Edge(from, to, weight));
            EdgesCount++;
            return true;
        }
        /// <summary>
        /// finds the index of vertice with least amount of edges excluding vertices int removed list
        /// </summary>
        /// <param name="removed"></param>
        /// <returns></returns>
        public int GetSmallestIndex(List<int> removed)
        {
            int smallest = int.MaxValue;
            int index = -1;
            for(int i = 0; i < _matrix.Length; i++)
            {
                if (smallest > _matrix[i].Count && !removed.Any(r => r == i))
                {
                    smallest = _matrix[i].Count;
                    index = i;
                }
            }
            return index;
        }
        /// <summary>
        /// removes vertice from graph at index
        /// currently rather slow implementation by converting to List
        /// </summary>
        /// <param name="index"></param>
        public void RemoveVertice(int index)
        {
            foreach(List<Edge> edges in _matrix)
            {
                edges.RemoveAll(e => e.To == index);
            }
            List<List<Edge>> temp = new List<List<Edge>>(_matrix);
            temp.RemoveAt(index);
            _matrix = temp.ToArray();
        }

        public List<Edge> OutEdges(int from)
        {
            return _matrix[from];
        }

        public List<Edge> Edges()
        {
            var edges = new List<Edge>();
            for(int i = 0; i< VerticesCount; i++) {
                edges = edges.Concat(_matrix[i]).ToList<Edge>();
            }
            return edges;
        }
    }
}
