using System;
using System.Collections.Generic;

namespace DecisionTree
{
    public class DecisionTree
    {
        public Node root { get; set; }

        public DecisionTree()
        {
            root = null;
        }

        public DecisionTree(string val)
        {
            root = new Node(val);
        }

        /// <summary>
        /// Takes in a value of type string. Then it creates a new node based on that value and search for an equivalent node using
        /// either DFS or BFS below.
        /// </summary>
        /// <param name="val"></param>
        /// <returns></returns>
        public string Lookup(string val)
        {
            Node inNode = new Node(val);
            Random rand = new Random();  // This is what we will use to calculate probability later on, so hold onto your seat.
            int r;

            // Comment out one or the other to choose which type of search you want to use.
            Node outNode = BreadthFirstSearch(inNode);
            //Node outNode = DepthFirstSearch(inNode);

            // Return an empty string if the command entered is not found in the search above.
            if (outNode.Value == String.Empty)
                return String.Empty;

            // While the node isn't a leaf, traverse down the tree.
            while (!outNode.IsLeaf())
            {
                // If there are 3 children, we're going to need to use a probability of 1/3 for each of them.
                if (outNode.LeafCount() == 3)
                {
                    r = rand.Next(0, 99);

                    if (r < 33)
                    {
                        outNode = outNode.Left;
                    }
                    else if (r >= 33 && r < 66)
                    {
                        outNode = outNode.Middle;
                    }
                    else if (r > 66)
                    {
                        outNode = outNode.Right;
                    }
                }
                // If there is only one child, it should only be the left node.
                else if (outNode.LeafCount() == 1)
                {
                    outNode = outNode.Left;
                }
                // For a node with two children, then we do a 1/2 probability of getting one or the other.
                else
                {
                    r = rand.Next(0, 100);

                    if (r < 50)
                    {
                        outNode = outNode.Left;
                    }
                    else if (r >= 50)
                    {
                        outNode = outNode.Right;
                    }

                }
            }

            return outNode.Value;
        }

        public Node DepthFirstSearch(Node n)
        {
            Stack<Node> search = new Stack<Node>();
            Node currentNode = new Node(root.Value);

            search.Push(root);

            while (search.Count != 0)
            {
                currentNode = search.Pop();

                if (currentNode.Value == n.Value)
                {
                    return currentNode;
                }
                else
                {
                    if(currentNode.Left != null)
                        search.Push(currentNode.Left);
                    
                    if(currentNode.Middle != null)
                        search.Push(currentNode.Middle);

                    if(currentNode.Right != null)
                        search.Push(currentNode.Right);
                }
            }

            Node failedNode = new Node(String.Empty);
            return failedNode;
        }

        public Node BreadthFirstSearch(Node n)
        {
            Queue<Node> search = new Queue<Node>();
            Node currentNode = new Node(root.Value);

            search.Enqueue(root);

            while (search.Count != 0)
            {
                currentNode = search.Dequeue();

                if (currentNode.Value == n.Value)
                {
                    return currentNode;
                }
                else
                {
                    if (currentNode.Left != null)
                        search.Enqueue(currentNode.Left);

                    if (currentNode.Middle != null)
                        search.Enqueue(currentNode.Middle);

                    if (currentNode.Right != null)
                        search.Enqueue(currentNode.Right);
                }
            }

            Node failedNode = new Node(String.Empty);
            return failedNode;
        }

    }
}