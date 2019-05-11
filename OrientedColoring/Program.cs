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
                int[] result = SmallestLast.Solve(ref graph);
                if (result.Any(r => r == -1))
                {
                    Console.WriteLine("There does not exist any proper oriented coloring for the given graph");
                }
                else
                {
                    string ansString = string.Join(" ", result);
                    Console.WriteLine("grph is valid {0}", graph.IsColoringValid());
                    Console.WriteLine(ansString);
                    Console.WriteLine("The oriented chromatic number is " + result.Distinct().Count().ToString());
                }
                return;
            }

        }
    }
}
