using System;
using System.Dynamic;

namespace ANN_Project
{
    public class Node
    {
        //public double Weight { get; set; }
        //public double Bias { get; set; }
        public double Activation { get; set; }
        public double Delta { get; set; }
        public double[] Weights { get; set; }

        public Node()
        {
            Weights = new double[100];
        }
        public Node(int prev_LayerSize)
        {
            RandomValue initWeight = new RandomValue();
            //RandomValue initBias = new RandomValue();

            Weights = new double[prev_LayerSize];

            for (int i = 0; i < prev_LayerSize; i++)
            {
                Weights[i] = initWeight.Next();
            }
            //this.Bias = initBias.RandomVal;
        }

    }
}