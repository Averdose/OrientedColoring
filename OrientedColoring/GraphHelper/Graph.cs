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
        public int EdgesCount { get; private set; }
        public int VerticesCount { get; }

        public Graph(int vertices)
        {
            VerticesCount = vertices;
            _matrix = new List<Edge>[vertices];
            InitializeMatrix(vertices);
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
                        VerticesCount = int.Parse(s);
                        _matrix = new List<Edge>[VerticesCount];
                        InitializeMatrix(VerticesCount);
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

        private void InitializeMatrix(int vertices)
        {
            for (int i = 0; i < vertices; i++)
            {
                _matrix[i] = new List<Edge>();
            }
        }

        public bool AddEdge(int from, int to, double weight = 1)
        {
            if (weight == 0) return false;
            if (_matrix[from].Any(e => e.To == to)
                || _matrix[to].Any(e => e.From == from)) return false;
            _matrix[from].Add(new Edge(from, to, weight));
            _matrix[to].Add(new Edge(to, from, weight));
            EdgesCount++;
            return true;
        }

        public Graph Clone()
        {
            var graph = new Graph(VerticesCount);
            foreach (var edges in _matrix)
            {
                edges.ForEach(e => graph.AddEdge(e.From, e.To, e.Weight));
            }
            return graph;
        }

        public IEnumerable<Edge> OutEdges(int from)
        {
            return _matrix[from];
        }
    }
}
