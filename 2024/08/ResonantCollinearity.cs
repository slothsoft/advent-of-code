using System;
using System.Collections.Generic;
using System.Linq;

namespace AoC.day8;

/// <summary>
/// <a href="https://adventofcode.com/2024/day/8">Day 8: Resonant Collinearity</a>
/// </summary>
public class ResonantCollinearity {
    internal struct Node(char frequency, Point location) {
        internal char Frequency { get; } = frequency;
        internal Point Location { get; } = location;
        public override string ToString() => $"{Frequency} {Location}";
    }

    internal struct Point(int x, int y) {
        internal int X { get; } = x;
        internal int Y { get; } = y;
        public override string ToString() => $"({X}, {Y})";
    }

    public ResonantCollinearity(IEnumerable<string> input) {
        var inputAsArray = input.ToArray();
        Input = ParseInput(inputAsArray);
        Height = inputAsArray.Length;
        Width = inputAsArray[0].Length;
    }

    internal Node[] Input { get; }
    int Width { get; }
    int Height { get; }

    internal static Node[] ParseInput(IEnumerable<string> input) {
        var result = new List<Node>();
        var y = 0;
        
        foreach (var line in input) {
            for (var x = 0; x < line.Length; x++) {
                if (line[x] != '.') {
                    result.Add(new Node(line[x], new Point(x, y)));
                }
            }
            y++;
        }

        return result.ToArray();
    }

    public long CalculateAntinodesCount() {
        var frequencies = Input.Select(n => n.Frequency).Distinct().ToArray();
        foreach (var point in CalculateAntinodes('r').Distinct()) {
            Console.WriteLine(point);
        }
        return frequencies.SelectMany(CalculateAntinodes).Distinct().Count();
    }
    
    private IEnumerable<Point> CalculateAntinodes(char frequeny) {
        IEnumerable<Point> result = [];

        var nodesWithFrequency = Input.Where(n => n.Frequency == frequeny).ToArray();

        for (var i = 0; i < nodesWithFrequency.Length; i++) {
            var node1 = nodesWithFrequency[i];
            for (var j = i + 1; j < nodesWithFrequency.Length; j++) {
                var node2 = nodesWithFrequency[j];
                result = result.Concat(CalculateAntinodes(node1.Location, node2.Location));
            }
        }
        return result.Distinct();
    }

    private IEnumerable<Point> CalculateAntinodes(Point point1, Point point2) {
        var xDiff = point1.X - point2.X;
        var yDiff = point1.Y - point2.Y;

        var antiPoint1 = new Point(point1.X + xDiff, point1.Y + yDiff);
        var antiPoint2 = new Point(point2.X - xDiff, point2.Y - yDiff);

        if (IsOnMap(antiPoint1)) yield return antiPoint1;
        if (IsOnMap(antiPoint2)) yield return antiPoint2;
    }
    
    private bool IsOnMap(Point point) {
        return point.X >= 0 && point.X < Width && point.Y >= 0 && point.Y < Height;
    }
}
