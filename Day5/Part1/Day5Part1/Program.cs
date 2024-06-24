using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace day_05
{
    public class Puzzle
    {
        private readonly IEnumerable<long> seeds;
        private readonly List<Map> almanac;

        public Puzzle(string input)
        {
            var parts = input.Trim().Split(new[] { "\r\n\r\n", "\n\n" }, StringSplitOptions.RemoveEmptyEntries);

            seeds = parts[0].Split(' ').Skip(1).Select(long.Parse);

            almanac = new List<Map>();
            foreach (var map in parts.Skip(1))
            {
                var lines = map.Trim().Split('\n');
                var newMap = new Map();
                foreach (var line in lines.Skip(1))
                {
                    newMap.RangeMaps.Add(RangeMap.Parse(line));
                }
                almanac.Add(newMap);
            }
        }

        public long Part1()
        {
            long closestLocation = long.MaxValue;

            foreach (var seed in seeds.ToList()) // Convert seeds to list to allow modification
            {
                var currentSeed = seed;
                foreach (var map in almanac)
                {
                    currentSeed = map.Convert(currentSeed);
                }
                closestLocation = Math.Min(closestLocation, currentSeed);
            }

            return closestLocation;
        }


        public long Part2()
        {
            var ranges = new List<Range>();
            var seedsList = new List<long>(seeds);

            for (int i = 0; i < seedsList.Count; i += 2)
            {
                var testRange = new Range(seedsList[i], seedsList[i] + seedsList[i + 1] - 1);
                var tempRanges = new List<Range>() { testRange };

                foreach (var map in almanac)
                {
                    tempRanges = map.Cut(tempRanges.ToArray()).ToList();
                }

                ranges.AddRange(tempRanges);
            }

            return ranges.Min(r => r.Start);
        }
    }

    public class Map
    {
        public IList<RangeMap> RangeMaps { get; } = new List<RangeMap>();

        public long Convert(long value)
        {
            foreach (var range in RangeMaps)
            {
                if (range.Contains(value))
                {
                    return range.Map(value);
                }
            }
            return value;
        }

        public Range[] Cut(Range[] ranges)
        {
            var result = new List<Range>();
            var tempRanges = new List<Range>(ranges);

            while (tempRanges.Count != 0)
            {
                var range = tempRanges.First();
                tempRanges.RemoveAt(0);

                var didCut = false;

                foreach (var map in RangeMaps)
                {
                    var mask = new Range(map.source, map.source + map.length - 1);
                    var delta = map.destination - map.source;

                    if (mask.Contains(range.Start) || mask.Contains(range.End))
                    {
                        var newStart = range.Start + delta;
                        var newEnd = range.End + delta;

                        if (mask.Contains(range.Start) && !mask.Contains(range.End))
                        {
                            result.Add(new Range(newStart, mask.End));
                            tempRanges.Add(new Range(mask.End + 1, range.End));
                        }
                        else if (!mask.Contains(range.Start) && mask.Contains(range.End))
                        {
                            result.Add(new Range(newStart, newEnd));
                        }
                        else if (mask.Contains(range.Start) && mask.Contains(range.End))
                        {
                            result.Add(new Range(newStart, newEnd));
                        }
                        else if (range.Start < mask.Start && range.End > mask.End)
                        {
                            result.Add(new Range(newStart, mask.End));
                            tempRanges.Add(new Range(range.Start, mask.Start - 1));
                            tempRanges.Add(new Range(mask.End + 1, range.End));
                        }

                        didCut = true;
                        break;
                    }
                }

                if (!didCut)
                {
                    result.Add(range);
                }
            }

            return result.ToArray();
        }
    }

    public class Range
    {
        public long Start { get; }
        public long End { get; }

        public Range(long start, long end)
        {
            Start = start;
            End = end;
        }

        public bool Contains(long value) => value >= Start && value <= End;
    }

    public class RangeMap
    {
        public long destination;
        public long source;
        public long length;

        public RangeMap(long destination, long source, long length)
        {
            this.destination = destination;
            this.source = source;
            this.length = length;
        }

        public bool Contains(long value) => value >= source && value < source + length;

        public long Map(long value) => Contains(value) ? value - source + destination : value;

        public static RangeMap Parse(string line)
        {
            var parts = line.Split(' ');
            return new RangeMap(long.Parse(parts[0]), long.Parse(parts[1]), long.Parse(parts[2]));
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            var input = File.ReadAllText("input.txt");
            var puzzle = new Puzzle(input);

            Console.WriteLine($"Part 1: {puzzle.Part1()}");
            Console.WriteLine($"Part 2: {puzzle.Part2()}");
            Console.ReadLine();
        }
    }
}
