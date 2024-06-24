using System;
using System.Collections.Generic;
using System.IO;

public class Solution
{
    public static void Main()
    {
        string[] input = File.ReadAllLines("input.txt");

        int[] minimumRed = new int[input.Length];
        int[] minimumGreen = new int[input.Length];
        int[] minimumBlue = new int[input.Length];

        for (int i = 0; i < input.Length; i++)
        {
            string line = input[i];
            string[] parts = line.Split(':');
            int gameId = int.Parse(parts[0].Substring(5).Trim()); // Extract game ID

            string[] subsets = parts[1].Split(';'); // Split subsets of cubes revealed

            foreach (string subset in subsets)
            {
                string[] cubes = subset.Trim().Split(',');
                foreach (string cube in cubes)
                {
                    string[] details = cube.Trim().Split();
                    int count = int.Parse(details[0]);
                    string color = details[1].ToLower();

                    switch (color)
                    {
                        case "red":
                            minimumRed[gameId - 1] = Math.Max(minimumRed[gameId - 1], count);
                            break;
                        case "green":
                            minimumGreen[gameId - 1] = Math.Max(minimumGreen[gameId - 1], count);
                            break;
                        case "blue":
                            minimumBlue[gameId - 1] = Math.Max(minimumBlue[gameId - 1], count);
                            break;
                        default:
                            break;
                    }
                }
            }
        }

        long sumOfPowers = 0;

        for (int i = 0; i < input.Length; i++)
        {
            long power = (long)minimumRed[i] * minimumGreen[i] * minimumBlue[i];
            sumOfPowers += power;
        }

        Console.WriteLine("Sum of the power of minimum sets: " + sumOfPowers);
        Console.ReadLine();
    }
}
