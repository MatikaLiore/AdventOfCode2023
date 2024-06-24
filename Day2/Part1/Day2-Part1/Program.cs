using System;
using System.Collections.Generic;
using System.IO;

public class Solution
{
    public static void Main()
    {
        string[] input = File.ReadAllLines("input.txt");
        int[] availableCubes = { 12, 13, 14 }; // Red, Green, Blue cubes available

        List<int> possibleGames = new List<int>();

        foreach (string line in input)
        {
            string[] parts = line.Split(':');
            int gameId = int.Parse(parts[0].Substring(5).Trim()); // Extract game ID

            string[] subsets = parts[1].Split(';'); // Split subsets of cubes revealed

            bool possible = true;

            foreach (string subset in subsets)
            {
                string[] cubes = subset.Trim().Split(',');
                int redCount = 0, greenCount = 0, blueCount = 0;

                foreach (string cube in cubes)
                {
                    string[] details = cube.Trim().Split();
                    int count = int.Parse(details[0]);
                    string color = details[1].ToLower();

                    switch (color)
                    {
                        case "red":
                            redCount += count;
                            break;
                        case "green":
                            greenCount += count;
                            break;
                        case "blue":
                            blueCount += count;
                            break;
                        default:
                            break;
                    }
                }

                // Check if any subset exceeds available cubes
                if (redCount > availableCubes[0] || greenCount > availableCubes[1] || blueCount > availableCubes[2])
                {
                    possible = false;
                    break;
                }
            }

            if (possible)
            {
                possibleGames.Add(gameId);
            }
        }

        int sumOfIds = 0;
        foreach (int gameId in possibleGames)
        {
            sumOfIds += gameId;
        }

        Console.WriteLine("Sum of IDs of possible games: " + sumOfIds);
        Console.ReadLine();
    }
}
