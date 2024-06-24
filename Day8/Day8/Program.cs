using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

class Program
{
    static void Main()
    {
        Part1("input.txt");
        Part2("input.txt");
        Console.ReadLine();
    }

    static void Part1(string filePath)
    {
        var lines = File.ReadAllLines(filePath);

        char[] instructions = lines[0].ToCharArray();
        var nodes = lines.Skip(2).Select(NextStep.Parse).ToList();

        Dictionary<string, NextStep> mappings = nodes.ToDictionary(node => node.Key, node => node.Step);

        string key = "AAA"; // defined starting point.
        long currentIndex = 0;
        long stepsTaken = 0;

        while (key != "ZZZ")
        {
            key = instructions[currentIndex] == 'L' ? mappings[key].Left : mappings[key].Right;
            stepsTaken++;
            currentIndex = (currentIndex + 1) % instructions.Length;
        }

        Console.WriteLine($"Part 1: {stepsTaken}");
    }

    static void Part2(string filePath)
    {
        var lines = File.ReadAllLines(filePath);
        char[] instructions = lines[0].ToCharArray();
        var nodes = lines.Skip(2).Select(NextStep.Parse).ToDictionary(x => x.Key, x => (x.Step.Left, x.Step.Right));

        // Find all nodes ending with 'A'
        Queue<string> queue = new Queue<string>(nodes.Where(p => p.Key.EndsWith("A")).Select(p => p.Key));
        HashSet<string> currentNodes = new HashSet<string>(queue);

        int stepsTaken = 0;

        // Perform BFS until all nodes in queue end with 'Z'
        while (queue.Count > 0)
        {
            int levelSize = queue.Count;

            for (int i = 0; i < levelSize; i++)
            {
                string currentKey = queue.Dequeue();

                // Follow both left and right paths
                string leftNode = instructions[stepsTaken % instructions.Length] == 'L' ? nodes[currentKey].Left : nodes[currentKey].Right;
                string rightNode = instructions[stepsTaken % instructions.Length] == 'L' ? nodes[currentKey].Right : nodes[currentKey].Left;

                // If the left path leads to a Z node, print the steps and return
                if (leftNode.EndsWith("Z"))
                {
                    Console.WriteLine($"Part 2: {stepsTaken + 1}");
                    return;
                }
                // Otherwise, add to queue and mark visited
                if (!currentNodes.Contains(leftNode))
                {
                    queue.Enqueue(leftNode);
                    currentNodes.Add(leftNode);
                }

                // If the right path leads to a Z node, print the steps and return
                if (rightNode.EndsWith("Z"))
                {
                    Console.WriteLine($"Part 2: {stepsTaken + 1}");
                    return;
                }
                // Otherwise, add to queue and mark visited
                if (!currentNodes.Contains(rightNode))
                {
                    queue.Enqueue(rightNode);
                    currentNodes.Add(rightNode);
                }
            }

            stepsTaken++;
        }
    }
}

public class NextStep
{
    public string Left { get; }
    public string Right { get; }

    private static readonly Regex regex = new Regex(@"(?<key>\w{3}) = \((?<left>\w{3}), (?<right>\w{3})\)");

    public NextStep(string left, string right)
    {
        Left = left;
        Right = right;
    }

    public static (string Key, NextStep Step) Parse(string line)
    {
        Match match = regex.Match(line);

        var key = match.Groups["key"].Value;
        var left = match.Groups["left"].Value;
        var right = match.Groups["right"].Value;

        return (key, new NextStep(left, right));
    }
}