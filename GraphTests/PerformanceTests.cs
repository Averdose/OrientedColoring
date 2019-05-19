using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OrientedColoring;
using OrientedColoring.GraphHelper;

namespace OrientedColoringTests
{
    [TestClass]
    public class PerformanceTests
    {
        [TestMethod]
        public void SparseGraphs()
        {
            String basepath = "C:\\Users\\nikow\\Desktop\\OrientedColoring\\tests";
            int max_n = 20;
            int iterations = 2;
            using (var avgtw = new StreamWriter(basepath + "average_results.csv", false))
            {
                avgtw.WriteLine("N;dsatur;AvgDsatur;Avg");

                for (int n = 1; n < max_n; n++)
                {
                    List<Result>  sparseres = RunTests(basepath + n + "_sparse.csv", iterations, n, 0.1);

                    List<Result> mediumres = RunTests(basepath + n + "_sparse.csv", iterations, n, 0.5);

                    List<Result> denseres = RunTests(basepath + n + "_sparse.csv", iterations, n, 0.9);


                }

            }
        }

        public List<Result> RunTests(String path, int iterations, int n, double satur)
        {
            List<Result> results = new List<Result>();
            using (var tw = new StreamWriter(path, false))
            {
                Console.WriteLine("Running tests for {0}", path);
                tw.WriteLine("BruteMs;BruteChromatic;BruteValid;SLMs;SLChromatic;SLValid;SLDist;BFSMs;BFSChromatic;BFSValid;BFSDist;DSMs;DSChromatic;DSValid;DSDist;");
                for (int i = 0; i < iterations; i++)
                {
                    Result res = Test(tw, 5, 0.1, n < 3);
                    results.Add(res);
                }
            }
            return results;
        }

        public Result Test(StreamWriter tw, int graphSize, double saturation, bool useBrute)
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

            tw.WriteLine(String.Format("{0};{1};{2};{3};{4};{5};{6};{7};{8};{9};{10};{11};{12};{13};", res.bruteTimeMs, res.bruteChromaticNum, res.bruteValid,
                res.smallestlastTimeMs, res.smallestlastChromaticNum, res.smallestlastValid, res.smallestlastChromaticNum-res.bruteChromaticNum,
                res.bfsTimeMs, res.bfsChromaticNum, res.bfsValid, res.bfsChromaticNum - res.bruteChromaticNum,
                res.dsaturTimeMs, res.dsaturChromaticNum, res.dsaturValid, res.dsaturChromaticNum-res.bruteChromaticNum));

            return res;
        }

    }

    public class Result
    {
        public int vertices;
        public int edges;

        public double dsaturTimeMs;
        public int dsaturChromaticNum;
        public bool dsaturValid;

        public double bfsTimeMs;
        public int bfsChromaticNum;
        public bool bfsValid;

        public double smallestlastTimeMs;
        public int smallestlastChromaticNum;
        public bool smallestlastValid;

        public double bruteTimeMs;
        public int bruteChromaticNum;
        public bool bruteValid;
    }
}
