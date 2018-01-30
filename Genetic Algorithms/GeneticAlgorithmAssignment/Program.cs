using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeneticAlgorithmAssignment
{
    class Program
    {
        static void Main(string[] args)
        {
            List<int> tests = new List<int>() {1, 1, 2, 2, 3, 4, 5, 5, 6, 6, 7, 8, 8, 9, 9};
            List<double> results = new List<double>(); 

            GA test = new GA(tests);

            for (int i = 0; i < 1000; i++)
            {
                results.Add(test.Start());
                Console.WriteLine($"Iteration {i} Result: {results[i]}\n");
            }

            var output = results.Max();

            Console.WriteLine($"\nThe Highest Value Found from 1000 Iterations: {output}");
        }
    }
}
