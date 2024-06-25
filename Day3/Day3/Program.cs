using System;
using System.Collections.Generic;
using System.IO;

class Program
{
    static void Main()
    {
        string[] schematic = File.ReadAllLines("input.txt");

        int rows = schematic.Length;
        int cols = schematic[0].Length;
        int sum = 0;

        // Define the possible movements to check adjacent cells including diagonals
        int[] dRow = { -1, -1, -1, 0, 0, 1, 1, 1 };
        int[] dCol = { -1, 0, 1, -1, 1, -1, 0, 1 };

        for (int r = 0; r < rows; r++)
        {
            for (int c = 0; c < cols; c++)
            {
                if (char.IsDigit(schematic[r][c]))
                {
                    int number = schematic[r][c] - '0';
                    bool isPartNumber = false;

                    // Check all adjacent cells
                    for (int i = 0; i < 8; i++)
                    {
                        int newRow = r + dRow[i];
                        int newCol = c + dCol[i];

                        if (IsValid(newRow, newCol, rows, cols) && IsSymbol(schematic[newRow][newCol]))
                        {
                            isPartNumber = true;
                            break;
                        }
                    }

                    if (isPartNumber)
                    {
                        sum += number;
                    }
                }
            }
        }

        Console.WriteLine("The sum of all part numbers is: " + sum);
    }

    static bool IsValid(int r, int c, int rows, int cols)
    {
        return r >= 0 && r < rows && c >= 0 && c < cols;
    }

    static bool IsSymbol(char ch)
    {
        return !char.IsDigit(ch) && ch != '.';
    }
}
