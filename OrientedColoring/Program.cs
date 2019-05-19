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

        static void Benchmark(string[] args)
        {
            String basepath = "C:\\Users\\nikow\\Desktop\\OrientedColoring\\benchmark\\";
            //         int max_n = 25;
                    int avg_iterations = 10;
            //        int brute_max_n = 8;
            //        Benchmark.benchmark(max_n, avg_iterations, basepath, brute_max_n);
            Graph graph = new Graph("evencycle");
            //Benchmark.benchmarkGraph(graph, avg_iterations, true);
        }

        static void Main(string[] args)
        {
            if (args.Count() >= 3)
            {
                int i = 0;
                string[] output = new string[5];
                int graphSize = Convert.ToInt32(args[0]);
                double saturation = Convert.ToDouble(args[1]);
                bool useBrute = Convert.ToBoolean(args[2]);
                Console.WriteLine("Begin program with params: n={0}, saturation={1}, use brute={2}", graphSize, saturation, useBrute);
                string path = "";
                if (args.Count() == 4)
                {
                    path = args[3];
                }
                else
                {
                    path = "output.txt";
                }
                Graph graph = RandomGraph.GenerateGraph(graphSize, saturation);
                int[] result;
                long elapsedMs;
                Stopwatch watch;
                output[i] = String.Format("Generated graph with {0} vertices and {1} edges.", graph.VerticesCount, graph.EdgesCount);
                Console.WriteLine(output[i]);
                i++;

                watch = System.Diagnostics.Stopwatch.StartNew();
                result = SmallestLast.Solve(ref graph);
                watch.Stop();
                elapsedMs = watch.ElapsedMilliseconds;
                output[i] = String.Format("SL finished in {0} ms. Valid coloring={1} X(G)={2}. Exact coloring={3}", elapsedMs, graph.IsColoringValid(), graph.ColorsMatrix.Distinct().Count(), string.Join(" ", result));
                Console.WriteLine(output[i]);
                i++;

                watch = System.Diagnostics.Stopwatch.StartNew();
                result = DSatur.Solve(ref graph);
                watch.Stop();
                elapsedMs = watch.ElapsedMilliseconds;
                output[i] = String.Format("DSATUR finished in {0} ms. Valid coloring={1} X(G)={2}. Exact coloring={3}", elapsedMs, graph.IsColoringValid(), graph.ColorsMatrix.Distinct().Count(), string.Join(" ", result));
                Console.WriteLine(output[i]);
                i++;

                watch = System.Diagnostics.Stopwatch.StartNew();
                result = BFSColoring.Solve(ref graph);
                watch.Stop();
                elapsedMs = watch.ElapsedMilliseconds;
                output[i] = String.Format("BFS finished in {0} ms. Valid coloring={1} X(G)={2}. Exact coloring={3}", elapsedMs, graph.IsColoringValid(), graph.ColorsMatrix.Distinct().Count(), string.Join(" ", result));
                Console.WriteLine(output[i]);

                if (useBrute)
                {
                    watch = System.Diagnostics.Stopwatch.StartNew();
                    result = BruteForce.Solve(ref graph);
                    watch.Stop();
                    elapsedMs = watch.ElapsedMilliseconds;
                    bool tmp = graph.IsColoringValid();

                    output[i] = String.Format("Brute force finished in {0} ms. Valid coloring={1} X(G)={2}. Exact coloring={3}", elapsedMs, graph.IsColoringValid(), graph.ColorsMatrix.Distinct().Count(), string.Join(" ", result));
                    Console.WriteLine(output[i]);
                    i++;
                }

                using (var tw = new StreamWriter(path, false))
                {
                    foreach (string s in output)
                    {
                            tw.WriteLine(s);
                    }
                }
                Console.WriteLine("Finished program. Press key to exit...");
                Console.ReadKey();
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

                    Console.WriteLine("Should I run brute force? (y/n)");
                    var bruteanswer = Console.ReadLine();
                    bool usebrute = bruteanswer == "y";

                    if (usebrute)
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
                    Console.WriteLine("------------------------------------------------------");

                    Console.WriteLine("Solving with BFS");
                    watch = System.Diagnostics.Stopwatch.StartNew();
                    result = BFSColoring.Solve(ref graph);
                    watch.Stop();
                    elapsedMs = watch.ElapsedMilliseconds;
                    PrintResult(result, graph, elapsedMs, "BFS");
                    Console.WriteLine("######################################################\n\n");

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
