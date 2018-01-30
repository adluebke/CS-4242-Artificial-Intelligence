using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;

namespace ANN_Project
{
    class Program
    {
        static void Main(string[] args)
        {
            string fileName = @"C:\TEMP\optdigits_train.txt";

            NeuralNetwork roseANNa = new NeuralNetwork(64, 40, 10);

            var fileLines = System.IO.File.ReadAllLines(fileName);

            foreach (var singleLine in fileLines)
            {
                var numList = singleLine.Split(',').Select(double.Parse).ToList();
                var numCollection = new Collection<double>(numList);

                roseANNa.Begin(numCollection);
            }

            fileName = @"C:\TEMP\optdigits_test.txt";
            fileLines = System.IO.File.ReadAllLines(fileName);

            foreach (var singleLine in fileLines)
            {
                var numList = singleLine.Split(',').Select(double.Parse).ToList();
                var numCollection = new Collection<double>(numList);

                roseANNa.Begin(numCollection);

                Console.WriteLine();
            }

            Console.WriteLine();

        }
    }
}