using System;
using System.Collections.Generic;
using System.IO;

namespace Day1
{
    internal class Program
    {
        static void Main(string[] args)
        {
            // Specify the path to your input file
            string filePath = "input.txt";

            // Read all lines from the file into a List<string>
            List<string> input_data = ReadInputFromFile(filePath);

            // Calculate the sum of all calibration values
            int result = CalculateCalibrationSum(input_data);

            // Print the result
            Console.WriteLine(result);
            Console.ReadLine();
        }

        public static List<string> ReadInputFromFile(string filePath)
        {
            List<string> lines = new List<string>();

            try
            {
                // Read all lines from the file
                lines = new List<string>(File.ReadAllLines(filePath));
            }
            catch (Exception e)
            {
                Console.WriteLine("Error reading the file: " + e.Message);
            }

            return lines;
        }

        public static int CalculateCalibrationSum(List<string> input_data)
        {
            // Initialize sum
            int totalSum = 0;

            // Process each line
            foreach (string line in input_data)
            {
                // Find the first and last digits
                char firstDigit = '\0';
                char lastDigit = '\0';

                foreach (char c in line)
                {
                    if (char.IsDigit(c))
                    {
                        if (firstDigit == '\0')
                        {
                            firstDigit = c;
                        }
                        lastDigit = c;
                    }
                }

                // Convert firstDigit and lastDigit to integers and calculate the sum
                if (firstDigit != '\0' && lastDigit != '\0')
                {
                    int num = int.Parse(firstDigit.ToString() + lastDigit.ToString());
                    totalSum += num;
                }
            }

            return totalSum;
        }

    }
}
