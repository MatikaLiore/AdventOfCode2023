using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

public class Scratchcards
{
    public static void Main()
    {
        var samplePath = "./sample.txt";
        var sample2Path = "./sample2.txt";
        var inputPath = "./input.txt";

        var lines = File.ReadAllLines(inputPath);
        Part1(lines);
        Part2(inputPath);
        Console.ReadLine();
    }

    public static void Part1(string[] lines)
    {
        var cards = lines.Select(ParseLine).ToArray();
        int accumulativeTotal = cards.Sum(card =>
        {
            int count = card.WinningNumbers.Intersect(card.MyNumbers).Count();
            if (count > 0)
            {
                // Calculate the score based on the number of matches
                int points = Enumerable.Range(0, count).Sum(i => i < 2 ? 1 : 2 << (i - 2));
                return points;
            }
            return 0;
        });

        Console.WriteLine($"Part1: {accumulativeTotal}");
    }

    public static void Part2(string inputPath)
    {
        var input = File.ReadAllLines(inputPath);
        // Initialize the array - each card has at least one way to win
        int[] cardCount = Enumerable.Repeat(1, input.Length).ToArray();

        // Loop over each card
        for (int cardId = 0; cardId < input.Length; cardId++)
        {
            string line = input[cardId];
            var card = ParseLine(line);

            // Count the number of winning numbers matched in this card
            var matchCount = card.WinningNumbers.Intersect(card.MyNumbers).Count();

            // Update card counts for subsequent cards based on the matches
            for (int i = 0; i < matchCount; i++)
            {
                cardCount[cardId + 1 + i] += cardCount[cardId];
            }
        }

        Console.WriteLine($"Part2: {cardCount.Sum()}");
    }

    public static Card ParseLine(string line)
    {
        var parts = line.Split(':');
        var numbers = parts[1].Split('|');
        var winningNumbers = ExtractNumbers(numbers[0]);
        var myNumbers = ExtractNumbers(numbers[1]);

        return new Card(winningNumbers, myNumbers);
    }

    public static int[] ExtractNumbers(string input)
    {
        return input.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries)
                    .Select(str => int.Parse(str.Trim()))
                    .ToArray();
    }

    public class Card
    {
        public int[] WinningNumbers { get; }
        public int[] MyNumbers { get; }

        public Card(int[] winningNumbers, int[] myNumbers)
        {
            WinningNumbers = winningNumbers;
            MyNumbers = myNumbers;
        }
    }
}
