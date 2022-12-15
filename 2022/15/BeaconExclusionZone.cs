using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using NUnit.Framework;

namespace AoC._15;

/// <summary>
/// <a href="https://adventofcode.com/2022/day/15">Day 15: Beacon Exclusion Zone </a>
/// </summary>
public class BeaconExclusionZone {
    private static readonly Regex Regex = new("(Sensor at x=|, y=|: closest beacon is at x=)");

    private readonly IList<Point> _beacons = new List<Point>();
    private readonly IList<Cycle> _cycles = new List<Cycle>();
    private int _minX = int.MaxValue;
    private int _maxX = int.MinValue;

    public void AddCycles(string[] lines) {
        foreach (var line in lines) {
            AddCycle(line);
        }
    }

    public void AddCycle(string line) {
        // line: Sensor at x=20, y=1: closest beacon is at x=15, y=3
        var lineSplit = Regex.Split(line).Where(s => s.All(c => char.IsDigit(c) || c == '-')).Where(s => s.Length > 0).ToArray();
        var numbers = lineSplit.Select(int.Parse).ToArray();
        Assert.AreEqual(4, lineSplit.Length, "Could not parse line: " + line);
        var beacon = new Point(numbers[2], numbers[3]);
        AddCycle(new Cycle(new Point(numbers[0], numbers[1]), beacon));
        _beacons.Add(beacon);
    }

    public void AddCycle(Cycle cycle) {
        _cycles.Add(cycle);
        _minX = Math.Min(_minX, cycle.Center.X - cycle.Radius - 1);
        _maxX = Math.Max(_maxX, cycle.Center.X + cycle.Radius + 1);
    }

    public long CalculateExclusionsInRow(int y) {
        var result = 0L;
        var beaconsInLine = _beacons.Where(b => b.Y == y).ToArray();
        for (var x = _minX; x <= _maxX; x++) {
            var testedPoint = new Point(x, y);
            foreach (var cycle in _cycles) {
                if (beaconsInLine.Contains(testedPoint)) {
                    continue;
                }

                if (cycle.ContainsPoint(testedPoint)) {
                    result++;
                    break;
                }
            }
        }

        return result;
    }

    public long CalculateTuningFrequency(int maxXandY) {
        // we only need to check the borders of all the cycles, because if there is only one point
        // the beacon can be at, it has to be bordered by at least 4 cycles
        foreach (var cycle in _cycles) {
            foreach (var point in cycle.CalculateBorderPoints().Where(p => p.X >= 0 && p.X <= maxXandY && p.Y >= 0 && p.Y <= maxXandY)) {
                var foundOtherCycle = false;
                foreach (var otherCycle in _cycles) {
                    if (otherCycle.ContainsPoint(point)) {
                        foundOtherCycle = true;
                        break;
                    }
                }
                if (!foundOtherCycle) {
                    return point.X * 4_000_000L + point.Y;
                }
            }
        }
        return -1;
    }
}

public record Point {
    public static int operator -(Point point1, Point point2) {
        return Math.Abs(point1.X - point2.X) + Math.Abs(point1.Y - point2.Y);
    }

    internal int X { get; set; }
    internal int Y { get; set; }

    public Point(int x, int y) {
        X = x;
        Y = y;
    }
}

public record Cycle(Point Center, int Radius) {
    public Cycle(Point center, Point otherPoint) : this(center, center - otherPoint) {
    }

    public bool ContainsPoint(Point point) {
        return Center - point <= Radius;
    }

    public IEnumerable<Point> CalculateBorderPoints() {
        var left = new Point(Center.X - Radius - 1, Center.Y);
        var right = new Point(Center.X + Radius + 1, Center.Y);

        yield return left;
        yield return right;
        
        for (var i = 0; i < Radius; i++) {
            yield return new Point(left.X + i, left.Y - i);
            yield return new Point(left.X + i, left.Y + i);
            
            yield return new Point(right.X - i, right.Y - i);
            yield return new Point(right.X - i, right.Y + i);
        }
        
        yield return new Point(Center.X, Center.Y - Radius - 1);
        yield return new Point(Center.X, Center.Y + Radius + 1);
    }
}