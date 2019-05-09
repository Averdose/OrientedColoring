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
            Console.WriteLine("Input graph file name:");
            var filename = Console.ReadLine();
            Console.WriteLine("Reading from file: " + filename);

            Console.ReadKey();

        }
    }
}
