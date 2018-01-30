using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;

namespace DecisionTree
{
    public class Node
    {
        public string Value { get; set; }
        public Node Left { get; set; }
        public Node Right { get; set; }
        public Node Middle { get; set; }

        public Node(string value)
        {
            this.Value = value;

            Left = null;
            Right = null;
            Middle = null;
        }

        public int LeafCount()
        {
            int count = 0;

            if (this.Left != null)
                count++;
            if (this.Middle != null)
                count++;
            if (this.Right != null)
                count++;

            return count;
        }

        public bool IsLeaf()
        {
            if (this.Left == null && this.Middle == null && this.Right == null)
                return true;

            return false;
        }

    }
}
