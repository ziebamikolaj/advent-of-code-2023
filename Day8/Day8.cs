using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace AdventOfCode
{
    class Day8
    {

        class Node
        {
            public string Label { get; private set; }
            public Node LeftNode { get; set; }
            public Node RightNode { get; set; }

            public Node(string label)
            {
                Label = label;
            }
            public override string ToString()
            {
                return Label;
            }
        }

        class Network
        {
            public List<Node> Nodes { get; private set; } = new List<Node>();
            public List<char> Instructions { get; private set; } = new List<char>();

            public Network(string[] fileContent)
            {
                foreach (var line in fileContent)
                {
                    if (line == "") continue;
                    if (!line.Contains("="))
                    {
                        Instructions.AddRange(line.ToCharArray());
                        continue;
                    }
                    var parts = line.Split(new string[] { " = " }, StringSplitOptions.None);
                    var label = parts[0];
                    var connections = parts[1].Trim('(', ')').Split(new string[] { ", " }, StringSplitOptions.None);

                    var currentNode = FindOrCreateNode(label);
                    currentNode.LeftNode = FindOrCreateNode(connections[0]);
                    currentNode.RightNode = FindOrCreateNode(connections[1]);
                }
            }
            private Node FindOrCreateNode(string label)
            {
                var node = Nodes.Find(n => n.Label == label);
                if (node == null)
                {
                    node = new Node(label);
                    Nodes.Add(node);
                }
                return node;
            }
            public int GetLengthToZZZNode()
            {
                var currentNode = Nodes.Find(n => n.Label == "AAA");
                var length = 0;
                while (currentNode.Label != "ZZZ")
                {
                    if (Instructions[length % Instructions.Count] == 'L')
                    {
                        currentNode = currentNode.LeftNode;
                    }
                    else
                    {
                        currentNode = currentNode.RightNode;
                    }
                    length++;
                }
                return length;
            }
            private List<Node> GetNodesEndingOnA()
            {
                List<Node> nodes = new List<Node>();
                foreach (var node in Nodes)
                {
                    if (node.Label.EndsWith("A"))
                    {
                        nodes.Add(node);
                    }
                }
                return nodes;
            }
            private int GetLengthOfCycleToZNode(Node node)
            {
                var currentNode = node;
                var length = 0;
                while (!currentNode.Label.EndsWith("Z") || length == 0)
                {
                    if (Instructions[length % Instructions.Count] == 'L')
                    {
                        currentNode = currentNode.LeftNode;
                    }
                    else
                    {
                        currentNode = currentNode.RightNode;
                    }
                    length++;
                }
                return length;
            }
            private static double GCD(double a, double b)
            {
                while (b != 0)
                {
                    double temp = b;
                    b = a % b;
                    a = temp;
                }
                return a;
            }

            private static double LCM(double a, double b)
            {
                return (a / GCD(a, b)) * b;
            }
            private static double LCMOfList(List<int> numbers)
            {
                double lcm = numbers[0];
                for (int i = 1; i < numbers.Count; i++)
                {
                    lcm = LCM(lcm, numbers[i]);
                }
                return lcm;
            }
            public double LengthToZZZNodesFromANodes()
            {
                var currentNodes = GetNodesEndingOnA();
                Dictionary<Node, int> cycleLength = new Dictionary<Node, int>();
                foreach (var node in currentNodes)
                {
                    cycleLength.Add(node, GetLengthOfCycleToZNode(node));
                }
                double lcm = LCMOfList(cycleLength.Values.ToList());
                return lcm;
            }
        }


        static void FirstPart(string fileContent)
        {
            Console.WriteLine("First part:");
            Network network = new Network(fileContent.Split(new string[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries));

            Console.WriteLine("Length to ZZZ node: " + network.GetLengthToZZZNode());
        }
        static void SecondPart(string fileContent)
        {
            Console.WriteLine("Second part:");
            Console.WriteLine();
            Network network = new Network(fileContent.Split(new string[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries));
            Console.WriteLine(network.LengthToZZZNodesFromANodes());
        }
        public static void ShowResults()
        {
            var fileContent = File.ReadAllText("../../Day8/day8input.txt");
            Console.WriteLine("Day 8:");
            FirstPart(fileContent);
            SecondPart(fileContent);
        }
    }
}