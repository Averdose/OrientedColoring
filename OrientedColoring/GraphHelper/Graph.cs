using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrientedColoring.GraphHelper
{
    public class Graph
    {
        private List<Edge>[] _matrix;
        private string filename;

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
            this.filename = filename;
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
