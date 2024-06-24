using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

public class Solution
{
    public static void Main()
    {
        string[] lines = File.ReadAllLines(@"input.txt");

        // Calculate the sum of all calibration values
        long sum = GetSum(lines);

        // Print the result
        Console.WriteLine(sum);
        Console.ReadLine();
    }

    // Function to calculate the sum of all calibration values
    public static long GetSum(string[] lines)
    {
        return lines.Select(GetNumber).Sum();
    }

    // Function to extract the calibration value from a line
    public static int GetNumber(string line)
    {
        List<string> combos = new List<string>();
        List<int> numbers = new List<int>();

        foreach (char c in line)
        {
            if (char.IsDigit(c))
            {
                numbers.Add(int.Parse(c.ToString()));
                combos.Clear();
            }
            else
            {
                foreach (var combo in combos)
                {
                    if (wordToInt.ContainsKey(combo))
                    {
                        numbers.Add(wordToInt[combo]);
                    }
                }
                combos = combos.Select(combo => combo + c).Concat(new[] { c.ToString() }).ToList();
            }
        }

        return numbers.First() * 10 + numbers.Last();
    }

    // Map to convert words to their corresponding numbers
    public static Dictionary<string, int> wordToInt = new Dictionary<string, int>
    {
        { "one", 1 },
        { "two", 2 },
        { "three", 3 },
        { "four", 4 },
        { "five", 5 },
        { "six", 6 },
        { "seven", 7 },
        { "eight", 8 },
        { "nine", 9 }
    };
}
