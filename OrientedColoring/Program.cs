using OrientedColoring.GraphHelper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrientedColoring
{
    class Program
    {
        static void Main(string[] args)
        {
            while (true)
            {
                Console.WriteLine("Input graph file name:");
                var filename = Console.ReadLine();
                Console.WriteLine("Reading from file: " + filename);
                Graph graph = null;
                try
                {
                    graph = new Graph(filename);
                }
                catch (Exception e)
                {
                    Console.WriteLine("Encoutered exception: " + e.Message);
                    Console.ReadKey();
                    Environment.Exit(0);
                }
                Console.WriteLine("Created graph with " + graph.VerticesCount + " vertices and " + graph.EdgesCount + " edges.");
                Console.WriteLine("------------------------------------------------------");
                Console.WriteLine("Solving with SmallestLast");
                int[] result = SmallestLast.Solve(ref graph);
                PrintResult(result, graph, "SmallestLast");
                Console.WriteLine("------------------------------------------------------");

                Console.WriteLine("Solving with BFS");
                result = graph.BFSDirectedColoring();
                PrintResult(result, graph, "BFS");
                Console.WriteLine("------------------------------------------------------");

                Console.WriteLine("Solving with DSatur");
                result = DSatur.Solve(ref graph);
                PrintResult(result, graph, "DSatur");
                Console.WriteLine("######################################################\n\n");
                
            }

        }

        private static void PrintResult(int[] result, Graph g, string algorithm)
        {
            if (result.Any(r => r == -1))
            {
                Console.WriteLine("The {0} algorithm couldnt find any coloring", algorithm);
            }
            else
            {
                string ansString = string.Join(" ", result);
                Console.WriteLine("Graph coloring is valid: {0}", g.IsColoringValid());
                Console.WriteLine("Colors array [{0}]",ansString);
                Console.WriteLine("The oriented chromatic number is " + result.Distinct().Count().ToString());
            }
        }
    }
}
