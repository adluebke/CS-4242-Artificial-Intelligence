using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;

namespace ANN_Project
{
    public class NeuralNetwork
    {
        private Collection<Node> Input_Layer;
        private Collection<Node> Output_Layer;
        private Collection<Node> Hidden_Layer;
        private Collection<Collection<Node>> Layers;

        private int[] Outputs;

        public NeuralNetwork(int inNodes, int hidNodes, int outNodes)
        {
            this.Input_Layer = new Collection<Node>();
            this.Hidden_Layer = new Collection<Node>();
            this.Output_Layer = new Collection<Node>();
            this.Outputs = new int[] {0, 1, 2, 3, 4, 5, 6, 7, 8, 9};

            // Add input nodes
            for (int i = 0; i < inNodes; i++)
            {
                Input_Layer.Add(new Node(64));
            }
            // Add hidden nodes
            for (int i = 0; i < hidNodes; i++)
            {
                Hidden_Layer.Add(new Node(inNodes));
            }
            // Add output nodes and values
            for (int i = 0; i < outNodes; i++)
            {
                Output_Layer.Add(new Node(hidNodes));
                //Output_Layer[i].Activation = i;
            }

            this.Layers = new Collection<Collection<Node>>() {Input_Layer, Hidden_Layer, Output_Layer};
        }

        public void Begin(Collection<double> inputs)
        {

            // Add input values
            for (int i = 0; i < inputs.Count - 1; i++)
            {
                Input_Layer[i].Activation = inputs[i];
            }

            ////For each layer
            //for(int layer = 1; layer < Layers.Count; layer++)
            //{
            //    // For each node in the current layer, use each node in the prev layer to calculate activation
            //    foreach (Node j in Layers[layer])
            //    {
            //        List<double> weightedSum = new List<double>();

            //        for (int i = 0; i < Layers[(layer - 1)].Count; i++)
            //        {
            //            var iNode = Layers[(layer - 1)][i];
            //            var temp = iNode.Weight * iNode.Activation;

            //            weightedSum.Add(temp);
            //        }

            //        var in_J = weightedSum.Sum();

            //        j.Activation = Sigmoid(in_J);
            //    }
            //}

            foreach (Node j in Hidden_Layer)
            {
                List<double> weightedSum = new List<double>();

                for (int i = 0; i < Input_Layer.Count; i++)
                {
                    var jWeight = j.Weights[i];
                    var temp = jWeight * Input_Layer[i].Activation;

                    weightedSum.Add(temp);
                }

                var in_J = weightedSum.Sum();

                j.Activation = Sigmoid(in_J);

            }

            foreach (Node j in Output_Layer)
            {
                List<double> weightedSum = new List<double>();

                for (int i = 0; i < Hidden_Layer.Count; i++)
                {
                    var jWeight = j.Weights[i];
                    var temp = jWeight * Hidden_Layer[i].Activation;

                    weightedSum.Add(temp);
                }

                var in_J = weightedSum.Sum();

                j.Activation = Sigmoid(in_J);

            }

            // Propogate deltas backward from output layer to input layer
            foreach (Node j in Output_Layer)
            {
                List<double> weightedSum = new List<double>();

                for (int i = 0; i < Hidden_Layer.Count; i++)
                {
                    var jWeight = j.Weights[i];
                    var temp = jWeight * Hidden_Layer[i].Activation;

                    weightedSum.Add(temp);
                }

                var in_J = weightedSum.Sum();
                j.Delta = DeriveSigmoid(in_J) * (Outputs[Output_Layer.IndexOf(j)] - j.Activation);             
            }

            foreach (Node i in Hidden_Layer)
            {
                List<double> weightedSum = new List<double>();
                List<double> weights = new List<double>();

                for (int x = 0; x < Input_Layer.Count; x++)
                {
                    var iWeight = i.Weights[x];
                    var temp = iWeight * Input_Layer[x].Activation;
                    
                    weightedSum.Add(temp);
                }

                for (int x = 0; x < Output_Layer.Count; x++)
                {
                    var jWeight = Output_Layer[x].Weights[Hidden_Layer.IndexOf(i)];
                    var temp = jWeight * Output_Layer[x].Delta;

                    weights.Add(temp);
                }

                var in_J = weightedSum.Sum();
                var prop = weights.Sum();

                i.Delta = DeriveSigmoid(in_J) * prop;
            }

            foreach (Node i in Input_Layer)
            {
                List<double> weightedSum = new List<double>();
                List<double> weights = new List<double>();

                for (int x = 0; x < Input_Layer.Count; x++)
                {
                    var iWeight = i.Weights[x];
                    var temp = iWeight * Input_Layer[x].Activation;

                    weightedSum.Add(temp);
                }

                for (int x = 0; x < Hidden_Layer.Count; x++)
                {
                    var iWeight = i.Weights[x];
                    var temp = iWeight * Input_Layer[x].Activation;

                    weights.Add(temp);
                }

                var in_J = weightedSum.Sum();
                var prop = weights.Sum();

                i.Delta = DeriveSigmoid(in_J) * prop;
            }

            foreach (Node j in Hidden_Layer)
            {
                for (int i = 0; i < j.Weights.Length; i++)
                {
                    j.Weights[i] = j.Weights[i] + (0.1 * Input_Layer[i].Activation * j.Delta);
                }
            }

            foreach (Node j in Output_Layer)
            {
                for (int i = 0; i < j.Weights.Length; i++)
                {
                    j.Weights[i] = j.Weights[i] + (0.1 * Hidden_Layer[i].Activation * j.Delta);
                }
            }
        }

        public double Sigmoid(double z)
        {
            return 1.0 / (1.0 + Math.Exp(-z));
        }

        public double DeriveSigmoid(double z)
        {
            return Sigmoid(z) * (1.0 - Sigmoid(z));
        }
    }
}