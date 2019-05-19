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
    class Benchmark
    {

        public static void benchmark(int max_n, int avg_iterations, String basepath, int brute_max_n) { 

            using (var avgtw = new StreamWriter(basepath + "average__results.csv", false))
            {
                avgtw.WriteLine("N;satur;BruteMs;BruteChromatic;SLMs;SLChromatic;BFSMs;BFSChromatic;DSaturMs;DSaturChromatic;");
                for (int n = 1; n < max_n; n++)
                {
                    Results res = RunTests(basepath + n + "_sparse.csv", avg_iterations, n, 0.1, brute_max_n);

                    avgtw.WriteLine(String.Format("{0};{1};{2};{3};{4};{5};{6};{7};{8};{9};", n, 0.1
                        , res.AvgBruteMs(), res.AvgBruteChromaticNum()
                        , res.AvgSLMs(), res.AvgSLChromaticNum()
                        ,res.AvgBFSMs(), res.AvgBFSChromaticNum()
                        , res.AvgDSMs(), res.AvgDSChromaticNum()));
                    res = RunTests(basepath + n + "_medium.csv", avg_iterations, n, 0.5, brute_max_n);
                    avgtw.WriteLine(String.Format("{0};{1};{2};{3};{4};{5};{6};{7};{8};{9};", n, 0.5
                    , res.AvgBruteMs(), res.AvgBruteChromaticNum()
                    , res.AvgSLMs(), res.AvgSLChromaticNum()
                    , res.AvgBFSMs(), res.AvgBFSChromaticNum()
                    , res.AvgDSMs(), res.AvgDSChromaticNum()));

                    res = RunTests(basepath + n + "_dense.csv", avg_iterations, n, 0.9, brute_max_n);
                    avgtw.WriteLine(String.Format("{0};{1};{2};{3};{4};{5};{6};{7};{8};{9};", n, 0.9
                    , res.AvgBruteMs(), res.AvgBruteChromaticNum()
                    , res.AvgSLMs(), res.AvgSLChromaticNum()
                    , res.AvgBFSMs(), res.AvgBFSChromaticNum()
                    , res.AvgDSMs(), res.AvgDSChromaticNum()));
                }

            }

            Console.WriteLine("Benchmark done. Press key to exit...");
            Console.ReadKey();
        }

        public static void benchmarkGraph(Graph graph, int avg_iterations, bool usebrute)
        {
            Results results = new Results();
            Console.WriteLine("Benchmarking graph with {0} vertices and {1} edges.", graph.VerticesCount, graph.EdgesCount);

            for (int i = 0; i < avg_iterations; i++)
            {
                Result res = new Result();
                res.vertices = graph.VerticesCount;
                res.edges = graph.EdgesCount;

                int[] result;
                long elapsedMs;
                Stopwatch watch;

                Console.WriteLine("Starting iteration " + i);

                if (usebrute) { 
                watch = System.Diagnostics.Stopwatch.StartNew();
                    result = BruteForce.Solve(ref graph);
                    watch.Stop();
                    elapsedMs = watch.ElapsedMilliseconds;
                    bool tmp = graph.IsColoringValid();
                    res.bruteChromaticNum = graph.ChromaticNumber();
                    res.bruteTimeMs = elapsedMs;
                    res.bruteValid = graph.IsColoringValid();
                }

                watch = System.Diagnostics.Stopwatch.StartNew();
                result = SmallestLast.Solve(ref graph);
                watch.Stop();
                elapsedMs = watch.ElapsedMilliseconds;
                res.smallestlastChromaticNum = graph.ChromaticNumber();
                res.smallestlastTimeMs = elapsedMs;
                res.smallestlastValid = graph.IsColoringValid();

                watch = System.Diagnostics.Stopwatch.StartNew();
                result = DSatur.Solve(ref graph);
                watch.Stop();
                elapsedMs = watch.ElapsedMilliseconds;
                res.dsaturChromaticNum = graph.ChromaticNumber();
                res.dsaturTimeMs = elapsedMs;
                res.dsaturValid = graph.IsColoringValid();

                watch = System.Diagnostics.Stopwatch.StartNew();
                result = BFSColoring.Solve(ref graph);
                watch.Stop();
                elapsedMs = watch.ElapsedMilliseconds;
                res.bfsChromaticNum = graph.ChromaticNumber();
                res.bfsTimeMs = elapsedMs;
                res.bfsValid = graph.IsColoringValid();

                results.Add(res);

                //Console.WriteLine(String.Format("{0};{1};{2};{3};{4};{5};{6};{7};",
                 //   res.bruteTimeMs, res.bruteChromaticNum,
                 //   res.smallestlastTimeMs, res.smallestlastChromaticNum,
                 //   res.bfsTimeMs, res.bfsChromaticNum,
                 //   res.dsaturTimeMs, res.dsaturChromaticNum));
            }
            Console.WriteLine("Brute ms;XBrute;BFS ms;Xbfs;DS ms;Xds;SL ms;Xsl");
            Console.WriteLine(String.Format("{0};{1};{2};{3};{4};{5};{6};{7};"
                , results.AvgBruteMs(), results.AvgBruteChromaticNum()
                , results.AvgBFSMs(), results.AvgBFSChromaticNum()
                , results.AvgDSMs(), results.AvgDSChromaticNum()
                , results.AvgSLMs(), results.AvgSLChromaticNum()));

            Console.WriteLine("Benchmark done. Press key to exit...");
            Console.ReadKey();
        }

        public static Results RunTests(String path, int iterations, int n, double satur, int brute_max_n)
        {
            Results results = new Results();
            using (var tw = new StreamWriter(path, false))
            {
                Console.WriteLine("Running tests for {0}", path);
                tw.WriteLine("BruteMs;BruteChromatic;BruteValid;SLMs;SLChromatic;SLValid;SLDist;BFSMs;BFSChromatic;BFSValid;BFSDist;DSMs;DSChromatic;DSValid;DSDist;");
                for (int i = 0; i < iterations; i++)
                {
                    Result res = Test(tw, n, satur, n <= brute_max_n);
                    results.Add(res);
                }
            }
            return results;
        }

        public static Result Test(StreamWriter tw, int graphSize, double saturation, bool useBrute)
        {
            Result res = new Result();
            Graph graph = RandomGraph.GenerateGraph(graphSize, saturation);
            res.vertices = graphSize;
            res.edges = graph.EdgesCount;

            int[] result;
            long elapsedMs;
            Stopwatch watch;

            if (useBrute)
            {
                watch = System.Diagnostics.Stopwatch.StartNew();
                result = BruteForce.Solve(ref graph);
                watch.Stop();
                elapsedMs = watch.ElapsedMilliseconds;
                bool tmp = graph.IsColoringValid();
                res.bruteChromaticNum = graph.ChromaticNumber();
                res.bruteTimeMs = elapsedMs;
                res.bruteValid = graph.IsColoringValid();
            }

            watch = System.Diagnostics.Stopwatch.StartNew();
            result = SmallestLast.Solve(ref graph);
            watch.Stop();
            elapsedMs = watch.ElapsedMilliseconds;
            res.smallestlastChromaticNum = graph.ChromaticNumber();
            res.smallestlastTimeMs = elapsedMs;
            res.smallestlastValid = graph.IsColoringValid();

            watch = System.Diagnostics.Stopwatch.StartNew();
            result = DSatur.Solve(ref graph);
            watch.Stop();
            elapsedMs = watch.ElapsedMilliseconds;
            res.dsaturChromaticNum = graph.ChromaticNumber();
            res.dsaturTimeMs = elapsedMs;
            res.dsaturValid = graph.IsColoringValid();

            watch = System.Diagnostics.Stopwatch.StartNew();
            result = BFSColoring.Solve(ref graph);
            watch.Stop();
            elapsedMs = watch.ElapsedMilliseconds;
            res.bfsChromaticNum = graph.ChromaticNumber();
            res.bfsTimeMs = elapsedMs;
            res.bfsValid = graph.IsColoringValid();

            Console.WriteLine(String.Format("{0};{1};{2};{3};{4};{5};{6};{7};",
                res.bruteTimeMs, res.bruteChromaticNum, 
                res.smallestlastTimeMs, res.smallestlastChromaticNum,
                res.bfsTimeMs, res.bfsChromaticNum,
                res.dsaturTimeMs, res.dsaturChromaticNum));

            tw.WriteLine(String.Format("{0};{1};{2};{3};{4};{5};{6};{7};{8};{9};{10};{11};{12};{13};", res.bruteTimeMs, res.bruteChromaticNum, res.bruteValid,
                res.smallestlastTimeMs, res.smallestlastChromaticNum, res.smallestlastValid, res.smallestlastChromaticNum - res.bruteChromaticNum,
                res.bfsTimeMs, res.bfsChromaticNum, res.bfsValid, res.bfsChromaticNum - res.bruteChromaticNum,
                res.dsaturTimeMs, res.dsaturChromaticNum, res.dsaturValid, res.dsaturChromaticNum - res.bruteChromaticNum));

            return res;
        }

        public class Result
        {
            public int vertices;
            public int edges;

            public long dsaturTimeMs;
            public int dsaturChromaticNum;
            public bool dsaturValid;

            public long bfsTimeMs;
            public int bfsChromaticNum;
            public bool bfsValid;

            public long smallestlastTimeMs;
            public int smallestlastChromaticNum;
            public bool smallestlastValid;

            public long bruteTimeMs;
            public int bruteChromaticNum;
            public bool bruteValid;
        }

        public class Results
        {
            private List<Result> list = new List<Result>();

            private long totalBruteMs;
            private int totalBruteChrom;

            private long totalDSMs;
            private int totalDSChrom;

            private long totalBFSMs;
            private int totalBFSChrom;

            private long totalSLMs;
            private int totalSLChrom;

            public void Add(Result r)
            {
                list.Add(r);
                totalBruteChrom += r.bruteChromaticNum;
                totalBruteMs += r.bruteTimeMs;

                totalDSChrom += r.dsaturChromaticNum;
                totalDSMs += r.dsaturTimeMs;

                totalBFSChrom += r.bfsChromaticNum;
                totalBFSMs += r.bfsTimeMs;

                totalSLChrom += r.smallestlastChromaticNum;
                totalSLMs += r.smallestlastTimeMs;
            }

            public double AvgBruteMs()
            {
                return (double) totalBruteMs / (double) list.Count;
            }
            public double AvgBruteChromaticNum()
            {
                return (double)totalBruteChrom / (double)list.Count;
            }

            public double AvgDSMs()
            {
                return (double)totalDSMs / (double)list.Count;
            }
            public double AvgDSChromaticNum()
            {
                return (double)totalDSChrom / (double)list.Count;
            }


            public double AvgBFSMs()
            {
                return (double)totalBFSMs / (double)list.Count;
            }
            public double AvgBFSChromaticNum()
            {
                return (double)totalBFSChrom / (double)list.Count;
            }


            public double AvgSLMs()
            {
                return (double)totalSLMs / (double)list.Count;
            }
            public double AvgSLChromaticNum()
            {
                return (double)totalSLChrom / (double)list.Count;
            }
        }

    }
}
