using System;
using System.IO;
using System.Linq;

namespace day_06
{
    class Program
    {
        static void Main()
        {
            string input = File.ReadAllText("input.txt");

            var puzzle = new Puzzle(input);

            Console.WriteLine($"Part 1: {puzzle.Part1()}");
            Console.WriteLine($"Part 2: {puzzle.Part2()}");

            Console.ReadLine();
        }
    }

    public class Puzzle
    {
        private readonly int[] times;
        private readonly int[] distances;

        public Puzzle(string input)
        {
            var parts = input.Trim().Split('\n');
            times = parts[0].Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries).Skip(1).Select(int.Parse).ToArray();
            distances = parts[1].Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries).Skip(1).Select(int.Parse).ToArray();
        }

        public int Part1()
        {
            int totalWays = 1;

            for (int i = 0; i < times.Length; i++)
            {
                int time = times[i];
                int distance = distances[i];

                int minHoldTime = Enumerable.Range(0, time).FirstOrDefault(ms => (time - ms) * ms > distance);
                int maxHoldTime = Enumerable.Range(0, time).LastOrDefault(ms => (time - ms) * ms > distance);

                int validWays = maxHoldTime - minHoldTime + 1;

                totalWays *= validWays;
            }

            return totalWays;
        }

        public int Part2()
        {
            int totalWays = 0;

            for (int i = 0; i < times.Length; i++)
            {
                int time = times[i];
                int distance = distances[i];

                for (int holdTime = time; holdTime <= 2 * time; holdTime++)
                {
                    if ((holdTime - time) * time > distance)
                    {
                        totalWays++;
                    }
                }
            }

            return totalWays;
        }
    }
}
