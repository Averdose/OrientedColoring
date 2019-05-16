using OrientedColoring.GraphHelper;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
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
            if (args.Count() >= 3)
            {
                int i = 0;
                string[] output = new string[5];
                int graphSize = Convert.ToInt32(args[0]);
                double saturation = Convert.ToDouble(args[1]);
                bool isBrute = Convert.ToBoolean(args[2]);
                string path = "";
                if (args.Count() == 4)
                {
                    path = args[3];
                }
                Graph graph = RandomGraph.GenerateGraph(graphSize, saturation);
                int[] result;
                long elapsedMs;
                Stopwatch watch;
                output[i] = graph.VerticesCount + "," + graph.EdgesCount;
                i++;
                if (isBrute)
                {
                    watch = System.Diagnostics.Stopwatch.StartNew();
                    result = BruteForce.Solve(ref graph);
                    watch.Stop();
                    elapsedMs = watch.ElapsedMilliseconds;
                    bool tmp = graph.IsColoringValid();
                    output[i] = "BruteForce," + elapsedMs + "," + graph.ColorsMatrix.Distinct().Count() + "," + tmp + "," + string.Join(" ", result);
                    i++;
                }
                watch = System.Diagnostics.Stopwatch.StartNew();
                result = SmallestLast.Solve(ref graph);
                watch.Stop();
                elapsedMs = watch.ElapsedMilliseconds;
                output[i] = "SL," + elapsedMs + "," + graph.ColorsMatrix.Distinct().Count() + "," + graph.IsColoringValid() + "," + string.Join(" ", result);
                i++;

                watch = System.Diagnostics.Stopwatch.StartNew();
                result = DSatur.Solve(ref graph);
                watch.Stop();
                elapsedMs = watch.ElapsedMilliseconds;
                output[i] = "DSatur," + elapsedMs + "," + graph.ColorsMatrix.Distinct().Count() + "," + graph.IsColoringValid() + "," + string.Join(" ", result);
                i++;

                watch = System.Diagnostics.Stopwatch.StartNew();
                result = BFSColoring.Solve(ref graph);
                watch.Stop();
                elapsedMs = watch.ElapsedMilliseconds;
                output[i] ="BFS," +  elapsedMs + "," + graph.ColorsMatrix.Distinct().Count() + "," + graph.IsColoringValid() + "," + string.Join(" ", result);
                if (args.Count() == 4)
                {
                    using (var tw = new StreamWriter(path, false))
                    {
                        foreach (string s in output)
                        {
                                tw.WriteLine(s);
                        }
                    }
                }
                else
                {
                    foreach(string s in output)
                    {
                        Console.WriteLine(s);
                    }
                }
            }
            else
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
