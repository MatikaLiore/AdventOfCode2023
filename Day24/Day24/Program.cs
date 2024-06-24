using System;
using System.Collections.Generic;
using System.IO;

public class HailstoneIntersection
{
    public class Hailstone
    {
        public long px;
        public long py;
        public long pz;
        public long vx;
        public long vy;
        public long vz;

        public Hailstone(long px, long py, long pz, long vx, long vy, long vz)
        {
            this.px = px;
            this.py = py;
            this.pz = pz;
            this.vx = vx;
            this.vy = vy;
            this.vz = vz;
        }
    }

    public static void Main(string[] args)
    {
        string filePath = "input.txt";
        List<Hailstone> hailstones = ParseInput(filePath);

        // Define the test area bounds
        long minX = 200000000000000;
        long maxX = 400000000000000;
        long minY = 200000000000000;
        long maxY = 400000000000000;

        int intersections = CountIntersections(hailstones, minX, maxX, minY, maxY);

        Console.WriteLine($"Number of intersections: {intersections}");
    }

    public static List<Hailstone> ParseInput(string filePath)
    {
        List<Hailstone> hailstones = new List<Hailstone>();

        try
        {
            string[] lines = File.ReadAllLines(filePath);

            foreach (string line in lines)
            {
                string[] parts = line.Split(new char[] { '@', ',', ' ' }, StringSplitOptions.RemoveEmptyEntries);

                long px = long.Parse(parts[0]);
                long py = long.Parse(parts[1]);
                long pz = long.Parse(parts[2]);
                long vx = long.Parse(parts[3]);
                long vy = long.Parse(parts[4]);
                long vz = long.Parse(parts[5]);

                hailstones.Add(new Hailstone(px, py, pz, vx, vy, vz));
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error reading input file: {ex.Message}");
        }

        return hailstones;
    }

    public static bool Intersect(Hailstone h1, Hailstone h2, long minX, long maxX, long minY, long maxY)
    {
        long x1 = h1.px;
        long y1 = h1.py;
        long vx1 = h1.vx;
        long vy1 = h1.vy;

        long x2 = h2.px;
        long y2 = h2.py;
        long vx2 = h2.vx;
        long vy2 = h2.vy;

        // Check if paths are parallel
        if (vx1 * vy2 == vx2 * vy1)
        {
            return false;
        }

        // Solve for t1 and t2
        double t2 = (vx1 * (y1 - y2) - vy1 * (x1 - x2)) / (double)(vx2 * vy1 - vy2 * vx1);
        double t1 = (x1 - x2 + t2 * vx2) / (double)vx1;

        // Check if intersection occurs within positive time range
        if (t1 >= 0 && t2 >= 0)
        {
            // Calculate intersection point
            long intersectionX = (long)Math.Round(x1 + t1 * vx1);
            long intersectionY = (long)Math.Round(y1 + t1 * vy1);

            // Check if intersection point lies within bounds
            if (intersectionX >= minX && intersectionX <= maxX &&
                intersectionY >= minY && intersectionY <= maxY)
            {
                return true;
            }
        }

        return false;
    }

    public static int CountIntersections(List<Hailstone> hailstones, long minX, long maxX, long minY, long maxY)
    {
        int numIntersections = 0;

        // Compare each pair of hailstones
        for (int i = 0; i < hailstones.Count - 1; i++)
        {
            for (int j = i + 1; j < hailstones.Count; j++)
            {
                if (Intersect(hailstones[i], hailstones[j], minX, maxX, minY, maxY))
                {
                    numIntersections++;
                }
            }
        }

        return numIntersections;
    }
}
