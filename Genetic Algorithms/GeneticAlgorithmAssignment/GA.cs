using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using System.Text;
using NCalc;

namespace GeneticAlgorithmAssignment
{
    public class GA
    {
        public List<int> InitPop { get; set; }
        public List<string> BinaryPop { get; set; }
        public const double X = -7.62;
        public const double Y = 11.43;
        public const double Z = 1.06;
        public Random Rand { get; set; }

        public GA(List<int> initPop)
        {
            this.InitPop = initPop;
            this.BinaryPop = IntToBinary(this.InitPop);
            this.Rand = new Random();
        }

        public double Start()
        {
            Random Index1 = new Random();
            Random Index2 = new Random();
            Random MutateProb = new Random();
            var newBinPop = new List<string>();
            var newPop = new List<int>();
            //var maxValue = Double.MinValue;

            for (int epochs = 0; epochs < this.BinaryPop.Count; epochs++)
            {
                var bin1 = BinaryPop[Index1.Next(0, 9)];
                var bin2 = BinaryPop[Index2.Next(0, 9)];

                var child = Reproduce(bin1, bin2);

                if (MutateProb.Next(0, 100) > 93)
                    child = Mutate(child);

                newBinPop.Add(child);
            }

            newPop = BinaryToInt(newBinPop);

            var maxValue = Evaluate(newPop);

            return maxValue;
        }

        public string Reproduce(string x, string y)
        {
            Random rand = new Random();
            int n = x.Length;
            int c = rand.Next(1, n);
            string child = String.Empty;

            child += x.Substring(0, c);
            child += y.Substring(c, n-child.Length);

            return child;
        }

        public string Mutate(string binaryStr)
        {
            Random rand = new Random();
            string mutation = binaryStr;

            StringBuilder sb = new StringBuilder(mutation);
            int i = rand.Next(0, binaryStr.Length);

            if (mutation[i] == '0')
            {
                sb[i] = '1';
            }
            else
            {
                sb[i] = '0';
            }

            return sb.ToString();
        }

        public Double Evaluate(List<int> population)
        {
            #region Function Variables

            var op1 = GenerateOperator();
            var op2 = GenerateOperator();
            var op3 = GenerateOperator();
            var op4 = GenerateOperator();

            var v1 = GenerateVariable();
            var v2 = GenerateVariable();
            var v3 = GenerateVariable();
            var v4 = GenerateVariable();

            var s1 = GenerateSign();
            var s2 = GenerateSign();
            var s3 = GenerateSign();
            var s4 = GenerateSign();
            var s5 = GenerateSign();
            var s6 = GenerateSign();
            var s7 = GenerateSign();
            var s8 = GenerateSign();
            var s9 = GenerateSign();
            var s10 = GenerateSign();

            var n1 = RandomFromPop(population);
            var n2 = RandomFromPop(population);
            var n3 = RandomFromPop(population);
            var n4 = RandomFromPop(population);
            var n5 = RandomFromPop(population);
            var n6 = RandomFromPop(population);
            var n7 = RandomFromPop(population);
            var n8 = RandomFromPop(population);
            var n9 = RandomFromPop(population);
            var n10 = RandomFromPop(population);

            var trig = GenerateTrig();

            #endregion

            var s = $"Pow((Pow({s1}{n1} {v1} , {s2}{n2}) {op1} Pow({s3}{n3} {v2} , {s4}{n4})), {s5}{n5}) {op2} Pow({s6}{n6} {v3} , {s7}{n7}) {op3} Exp({s8}{n8}) {op4} {trig}(Pow({s9}{n9} {v4} , {s10}{n10}))";

            s = s.Replace(" x ", "*-7.62");
            s = s.Replace(" y ", "*11.43");
            s = s.Replace(" z ", "*1.06");

            NCalc.Expression e = new NCalc.Expression(s);
            e.Parameters["x"] = X;
            e.Parameters["y"] = Y;
            e.Parameters["z"] = Z;

            var result = e.Evaluate();

            Console.WriteLine($"The function evaluated is {s}");
            return Convert.ToDouble(result);
        }

        public string GenerateOperator()
        {
            var prob = Rand.Next(0, 100);

            if (prob < 25)
            {
                return "+";
            }
            else if (prob >= 25 && prob < 50)
            {
                return "-";
            }
            else if(prob >= 50 && prob < 75)
            {
                return "*";
            }
            else
            {
                return "/";
            }
        }

        public string GenerateSign()
        {
            var prob = Rand.Next(0, 100);

            if (prob < 50)
            {
                return "-";
            }
            else
            {
                return "";
            }
        }

        public string GenerateVariable()
        {
            var prob = Rand.Next(0, 100);

            if (prob < 33)
            {
                return "x";
            }
            else if (prob >= 33 && prob < 66)
            {
                return "y";
            }
            else
            {
                return "z";
            }
        }

        public string GenerateTrig()
        {            
            var prob = Rand.Next(0, 100);

            if (prob < 33)
            {
                return "Sin";
            }
            else if (prob >= 33 && prob < 66)
            {
                return "Cos";
            }
            else
            {
                return "Tan";
            }
        }

        public string RandomFromPop(List<int> pop)
        {
            var prob = Rand.Next(0, (pop.Count - 1));

            return pop[prob].ToString();
        }

        /// <summary>
        /// Converts a given int list to an equivalent list of 5-bit binary strings
        /// </summary>
        /// <param name="pop"> List of numbers to be converted </param>
        /// <returns> New list with binary numbers </returns>
        public List<string> IntToBinary(List<int> pop)
        {
            List<string> binaryPop = new List<string>();

            // Add the binary value of each number into a new list.
            foreach (int i in pop)
            {
                binaryPop.Add(Convert.ToString(i, 2));
            }

            //Pad the variable to be 5 bits
            for (int i = 0; i < binaryPop.Count; i++)
            {
                binaryPop[i] = binaryPop[i].PadLeft(5, '0');
            }

            return binaryPop;
        }

        public List<int> BinaryToInt(List<string> pop)
        {
            List<int> population = new List<int>();

            // Add the binary value of each number into a new list.
            foreach (string s in pop)
            {
                population.Add(Convert.ToInt32(s, 10));
            }

            for (int i = 0; i < population.Count; i++)
            {
                if (population[i] > 9)
                {
                    population[i] = 9;
                }
                else if (population[i] < 0)
                {
                    population[i] = 0;
                }
            }

            return population;
        }

    }
}