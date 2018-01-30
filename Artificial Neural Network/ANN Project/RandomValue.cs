using System;
using System.Security.Cryptography;

namespace ANN_Project
{
    public class RandomValue
    {
        public double RandomVal { get; set; }
        public RandomNumberGenerator RandNum { get; set; }
        public Random R { get; set; }

        public RandomValue()
        {
            RandNum = RandomNumberGenerator.Create();
            R = new Random(RandNum.GetHashCode());

            this.RandomVal = R.NextDouble();
        }

        public double Next()
        {
            return this.R.NextDouble();
        }
    }
}