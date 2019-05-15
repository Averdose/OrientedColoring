using OrientedColoring.GraphHelper;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrientedColoring
{
    class Program
    {
        const int GRAPHSIZE = 6;
        const double EDGESATURATION = 0.6;
        const bool USEBRUTEFORCE = true;
        static void Main(string[] args)
        {
            while (true)
            {
                /*
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
                */
                Graph graph = RandomGraph.GenerateGraph(GRAPHSIZE, EDGESATURATION);
                int[] result;
                long elapsedMs;
                Stopwatch watch;
                Console.WriteLine("Created graph with " + graph.VerticesCount + " vertices and " + graph.EdgesCount + " edges.");
                if (USEBRUTEFORCE)
                {
                    Console.WriteLine("------------------------------------------------------");
                    Console.WriteLine("Solving with BruteForce");
                    watch = System.Diagnostics.Stopwatch.StartNew();
                    result = BruteForce.Solve(ref graph);
                    watch.Stop();
                    elapsedMs = watch.ElapsedMilliseconds;

                    PrintResult(result, graph, elapsedMs, "BruteForce");
                }
                Console.WriteLine("------------------------------------------------------");
                Console.WriteLine("Solving with SmallestLast");
                watch = System.Diagnostics.Stopwatch.StartNew();
                result = SmallestLast.Solve(ref graph);
                watch.Stop();
                elapsedMs = watch.ElapsedMilliseconds;
                PrintResult(result, graph, elapsedMs, "SmallestLast");
                Console.WriteLine("------------------------------------------------------");

                Console.WriteLine("Solving with DSatur");
                watch = System.Diagnostics.Stopwatch.StartNew();
                result = DSatur.Solve(ref graph);
                watch.Stop();
                elapsedMs = watch.ElapsedMilliseconds;
                PrintResult(result, graph, elapsedMs, "DSatur");
                Console.WriteLine("######################################################\n\n");

                Console.WriteLine("Solving with BFS");
                watch = System.Diagnostics.Stopwatch.StartNew();
                result = BFSColoring.Solve(ref graph);
                watch.Stop();
                elapsedMs = watch.ElapsedMilliseconds;
                PrintResult(result, graph, elapsedMs, "BFS");
                Console.WriteLine("------------------------------------------------------");
                return;
            }

        }

        private static void PrintResult(int[] result, Graph g, long elapsed, string algorithm)
        {
            if (result.Any(r => r == -1))
            {
                Console.WriteLine("The {0} algorithm couldnt find any coloring", algorithm);
            }
            else
            {
                string ansString = string.Join(" ", result);
                Console.WriteLine("Algorithm exectued in {0}ms", elapsed);
                Console.WriteLine("Graph coloring is valid: {0}", g.IsColoringValid());
                Console.WriteLine("Colors array [{0}]",ansString);
                Console.WriteLine("The oriented chromatic number is " + result.Distinct().Count().ToString());
            }
        }
    }
}
