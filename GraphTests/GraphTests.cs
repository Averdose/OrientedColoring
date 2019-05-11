using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OrientedColoring;
using OrientedColoring.GraphHelper;

namespace OrientedColoringTests
{
    [TestClass]
    public class GraphTests
    {
        [TestMethod]
        public void CreateGraph()
        {
            Graph g = new Graph(1);
        }

        [TestMethod]
        public void CreateGraphWithEdges()
        {
            Graph g = new Graph(3);
            g.AddEdge(0, 1);
            g.AddEdge(1, 2);
            g.AddEdge(2, 0);
            Assert.AreEqual(3, g.EdgesCount);
            List<Edge> edges = g.Edges();
            Assert.AreEqual(3, edges.Count);
        }

        [TestMethod]
        public void IsDefaultColoringValid()
        {
            Graph g = new Graph(3);
            g.AddEdge(0, 1);
            g.AddEdge(1, 2);
            g.AddEdge(2, 0);
            Assert.IsTrue(g.IsColoringValid());
        }

        [TestMethod]
        public void IsBrokenColoringValid()
        {
            Graph g = new Graph(3);
            g.AddEdge(0, 1);
            g.AddEdge(1, 2);
            g.AddEdge(2, 0);
            g.ColorsMatrix[2] = 1;
            Assert.IsFalse(g.IsColoringValid());
        }

        [TestMethod]
        public void IsGoodDirectedColoringValid()
        {
            Graph g = new Graph(4);
            g.AddEdge(0, 1);
            g.AddEdge(1, 2);
            g.AddEdge(2, 3);
            g.AddEdge(3, 1);
            g.ColorsMatrix[0] = 0;
            g.ColorsMatrix[1] = 1;
            g.ColorsMatrix[2] = 2;
            g.ColorsMatrix[3] = 0;
            Assert.IsTrue(g.IsColoringValid());
        }

        [TestMethod]
        public void IsColoringWithANegativeColorValid()
        {
            Graph g = new Graph(4);
            g.AddEdge(0, 1);
            g.AddEdge(1, 2);
            g.AddEdge(2, 3);
            g.AddEdge(3, 1);
            g.ColorsMatrix[0] = 0;
            g.ColorsMatrix[1] = 1;
            g.ColorsMatrix[2] = 2;
            g.ColorsMatrix[3] = -1;
            Assert.IsFalse(g.IsColoringValid());
        }

        [TestMethod]
        public void IsBadDirectedColoringValid()
        {
            Graph g = new Graph(4);
            g.AddEdge(0, 1);
            g.AddEdge(1, 2);
            g.AddEdge(2, 3);
            g.AddEdge(3, 1);
            g.ColorsMatrix[0] = 0;
            g.ColorsMatrix[1] = 1;
            g.ColorsMatrix[2] = 0;
            g.ColorsMatrix[3] = 2;
            Assert.IsFalse(g.IsColoringValid());
        }

        [TestMethod]
        public void IsBFSDirectedColoringValid()
        {
            Graph g = new Graph(4);
            g.AddEdge(0, 1);
            g.AddEdge(1, 2);
            g.AddEdge(2, 3);
            g.AddEdge(3, 1);
            g.BFSDirectedColoring();
            Assert.IsTrue(g.IsColoringValid());
        }

        [TestMethod]
        public void IsSmallestLastDirectedColoringValid()
        {
            Graph g = new Graph(4);
            g.AddEdge(0, 1);
            g.AddEdge(1, 2);
            g.AddEdge(2, 3);
            g.AddEdge(3, 1);
            int[] coloring = SmallestLast.Solve(ref g);
            Assert.IsTrue(g.IsColoringValid());
        }

        [TestMethod]
        public void IsDSaturDirectedColoringValid()
        {
            Graph g = new Graph(4);
            g.AddEdge(0, 1);
            g.AddEdge(1, 2);
            g.AddEdge(2, 3);
            g.AddEdge(3, 1);
            int[] coloring = DSatur.Solve(ref g);
            Assert.IsTrue(g.IsColoringValid());
        }

        [TestMethod]
        public void IsImpossibleColoringValid()
        {
            Graph g = new Graph(2);
            g.AddEdge(0, 1);
            g.AddEdge(1, 0);
            g.BFSDirectedColoring();
            Assert.IsFalse(g.IsColoringValid());
        }

        [TestMethod]
        public void IsFiveCycleColoringValidForBFS()
        {
            Graph g = new Graph(5);
            g.AddEdge(0, 1);
            g.AddEdge(1, 2);
            g.AddEdge(2, 3);
            g.AddEdge(3, 4);
            g.AddEdge(4, 0);
            g.BFSDirectedColoring();
            Assert.IsTrue(g.IsColoringValid());
        }

        [TestMethod]
        public void IsFiveCycleColoringValidForSmallestLast()
        {
            Graph g = new Graph(5);
            g.AddEdge(0, 1);
            g.AddEdge(1, 2);
            g.AddEdge(2, 3);
            g.AddEdge(3, 4);
            g.AddEdge(4, 0);
            SmallestLast.Solve(ref g);
            Assert.IsTrue(g.IsColoringValid());
        }

        [TestMethod]
        public void IsFiveCycleColoringValidForDSatur()
        {
            Graph g = new Graph(5);
            g.AddEdge(0, 1);
            g.AddEdge(1, 2);
            g.AddEdge(2, 3);
            g.AddEdge(3, 4);
            g.AddEdge(4, 0);
            DSatur.Solve(ref g);
            Assert.IsTrue(g.IsColoringValid());
        }
    }
}
