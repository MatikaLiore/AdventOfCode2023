using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

public class Point
{
    public int X { get; }
    public int Y { get; }

    public Point(int x, int y)
    {
        X = x;
        Y = y;
    }

    public Point Up => new Point(X, Y - 1);
    public Point Down => new Point(X, Y + 1);
    public Point Left => new Point(X - 1, Y);
    public Point Right => new Point(X + 1, Y);

    public override bool Equals(object obj)
    {
        if (obj == null || GetType() != obj.GetType())
            return false;

        var other = (Point)obj;
        return X == other.X && Y == other.Y;
    }

    public override int GetHashCode()
    {
        unchecked
        {
            int hash = 17;
            hash = hash * 23 + X.GetHashCode();
            hash = hash * 23 + Y.GetHashCode();
            return hash;
        }
    }
}

public class Pipe
{
    public char Symbol { get; }

    public Pipe(char symbol)
    {
        Symbol = symbol;
    }

    public IEnumerable<Point> Adjacent(Point at)
    {
        switch (Symbol)
        {
            case 'L': return new[] { at.Up, at.Right };
            case 'J': return new[] { at.Up, at.Left };
            case '|': return new[] { at.Up, at.Down };
            case '-': return new[] { at.Left, at.Right };
            case '7': return new[] { at.Left, at.Down };
            case 'F': return new[] { at.Right, at.Down };
            case 'S': return Array.Empty<Point>();
            case '.': return Array.Empty<Point>();
            default: throw new Exception($"Unknown pipe: {Symbol}");
        }
    }
}

public class Puzzle
{
    private readonly Dictionary<Point, Pipe> grid;
    private readonly IList<Point> path;

    public Puzzle(string input)
    {
        grid = input.Trim().Split('\n')
            .Select((line, row) => (line, row))
            .Aggregate(new Dictionary<Point, Pipe>(), (acc, x) =>
            {
                var (line, row) = x;
                foreach (var (ch, col) in line.Select((ch, col) => (ch, col)))
                    acc[new Point(col, row)] = new Pipe(ch);
                return acc;
            });

        path = FindPath(grid.First(p => p.Value.Symbol == 'S').Key).ToList();
    }

    public int Part1() => path.Count / 2;

    public int Part2()
    {
        var minX = path.Min(p => p.X);
        var maxX = path.Max(p => p.X);
        var minY = path.Min(p => p.Y);
        var maxY = path.Max(p => p.Y);

        int count = 0;
        for (var y = minY; y <= maxY; y++)
        {
            bool isIn = false;
            for (var x = minX; x <= maxX; x++)
            {
                var point = new Point(x, y);

                if (path.Contains(point))
                {
                    var symbol = grid[point].Symbol;
                    if (symbol == '|' || symbol == 'F' || symbol == '7')
                    {
                        isIn = !isIn;
                    }
                }
                else
                {
                    if (isIn)
                    {
                        count++;
                    }
                }
            }
        }
        return count;
    }

    private IEnumerable<Point> FindPath(Point startPoint)
    {
        var path = new List<Point> { startPoint };
        var startPipe = DeterminePipe(startPoint);
        grid[startPoint] = startPipe;
        var previous = startPoint;
        var current = startPipe.Adjacent(startPoint).First();
        path.Add(current);
        while (current != startPoint)
        {
            var pipe = grid[current];
            var next = pipe.Adjacent(current).First(p => p != previous);
            previous = current;
            current = next;
            path.Add(current);
        }

        return path;
    }

    private Pipe DeterminePipe(Point startPoint)
    {
        Pipe up = grid.TryGetValue(startPoint.Up, out var upPipe) ? upPipe : null;
        Pipe down = grid.TryGetValue(startPoint.Down, out var downPipe) ? downPipe : null;
        Pipe left = grid.TryGetValue(startPoint.Left, out var leftPipe) ? leftPipe : null;
        Pipe right = grid.TryGetValue(startPoint.Right, out var rightPipe) ? rightPipe : null;

        if (up != null && up.Adjacent(startPoint).Contains(startPoint)
            && right != null && right.Adjacent(startPoint).Contains(startPoint))
        {
            return new Pipe('L');
        }
        else if (up != null && up.Adjacent(startPoint).Contains(startPoint)
            && down != null && down.Adjacent(startPoint).Contains(startPoint))
        {
            return new Pipe('|');
        }
        else if (up != null && up.Adjacent(startPoint).Contains(startPoint)
            && left != null && left.Adjacent(startPoint).Contains(startPoint))
        {
            return new Pipe('J');
        }
        else if (right != null && right.Adjacent(startPoint).Contains(startPoint)
            && down != null && down.Adjacent(startPoint).Contains(startPoint))
        {
            return new Pipe('F');
        }
        else if (right != null && right.Adjacent(startPoint).Contains(startPoint)
            && left != null && left.Adjacent(startPoint).Contains(startPoint))
        {
            return new Pipe('-');
        }
        else if (down != null && down.Adjacent(startPoint).Contains(startPoint)
            && left != null && left.Adjacent(startPoint).Contains(startPoint))
        {
            return new Pipe('7');
        }
        else
        {
            throw new Exception($"Unknown pipe at {startPoint}");
        }
    }

    public static void Main(string[] args)
    {
        var puzzle = new Puzzle(File.ReadAllText("input.txt"));

        Console.WriteLine(puzzle.Part1());
        Console.WriteLine(puzzle.Part2());
    }
}
