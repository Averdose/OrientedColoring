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
        public int VerticesCount { get; private set; }

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
            VerticesCount = vertices;
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
            EdgesCount++;
            return true;
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
