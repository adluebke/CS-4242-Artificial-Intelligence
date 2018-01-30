using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using System.Xml.Linq;


/*
 * To switch between DFS and BFS, go to DecisionTree.cs -> Lookup and comment one or the other out.
 * I've put comments in that classes because most of the work is done in there.
 */

namespace DecisionTree
{
    class Program
    {
        static void Main(string[] args)
        {
            bool quit = false;
            DecisionTree myTree = new DecisionTree("root");

            #region XML Reading and Tree Building

            XmlReader doc = XmlReader.Create("C:\\Users\\Andrew\\Documents\\Visual Studio 2017\\Projects\\DecisionTree\\DecisionTree\\DecisionTree.xml");
            string responseName = String.Empty;
            

            while (doc.Read())
            {
                if ((doc.NodeType == XmlNodeType.Element) && (doc.Name == "node") && (doc.GetAttribute("response") == ""))
                {
                    responseName = doc.GetAttribute("behavior");
                }

                if ((doc.NodeType == XmlNodeType.Element) && (doc.Name == "node") && (doc.GetAttribute("behavior") == ""))
                {
                    responseName = doc.GetAttribute("response");
                }

                // This is ugly.
                #region Tree Building

                switch (responseName)
                {
                    case "Idle":
                    {
                        myTree.root.Right = new Node(responseName);
                        break;
                    }
                    case "Use Computer":
                    {
                        myTree.root.Right.Left = new Node(responseName);
                        break;
                    }
                    case "Patrol":
                    {
                        myTree.root.Right.Right = new Node(responseName);
                        break;
                    }
                    case "Incoming Projectile":
                    {
                        myTree.root.Left = new Node(responseName);
                        break;
                    }
                    case "Evade":
                    {
                        myTree.root.Left.Left = new Node(responseName);
                        break;
                    }
                    case "Combat":
                    {
                        myTree.root.Middle = new Node(responseName);
                        break;
                    }
                    case "Melee":
                    {
                        myTree.root.Middle.Left = new Node(responseName);
                        break;
                    }
                    case "Flee":
                    {
                        myTree.root.Middle.Left.Left = new Node(responseName);
                        break;
                    }
                    case "Attack":
                    {
                        myTree.root.Middle.Left.Right = new Node(responseName);
                        break;
                    }
                    case "Ranged":
                    {
                        myTree.root.Middle.Right = new Node(responseName);
                        break;
                    }
                    case "Weapon 1":
                    {
                        myTree.root.Middle.Right.Left = new Node(responseName);
                        break;
                    }
                    case "Weapon 2":
                    {
                        myTree.root.Middle.Right.Middle = new Node(responseName);
                        break;
                    }
                    case "Weapon 3":
                    {
                        myTree.root.Middle.Right.Right = new Node(responseName);
                        break;
                    }
                    default:
                    {
                        break;
                    }

                    #endregion

                }
            }

            #endregion

            
            while (!quit)
            {
                Console.Write("Event ('quit' to exit) : " );
                var command = Console.ReadLine();
                if (command == "quit")
                {
                    quit = true;
                    break;
                }
                Console.WriteLine($"Response = {myTree.Lookup(command)}");
            }
        }
    }
}