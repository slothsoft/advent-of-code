using System;
using System.Collections.Generic;
using System.Linq;

namespace AoC.day21;

/// <summary>
/// <a href="https://adventofcode.com/2023/day/21">Day 21: Step Counter</a>
/// </summary>
public class StepCounter {
    public struct Direction {
        private static Direction North = new("N", 0, -1);
        private static Direction South = new("S", 0, 1);
        private static Direction East = new("E", 1, 0);
        private static Direction West = new("W", -1, 0);
        public static Direction[] All = {North, South, East, West};

        private Direction(string displayName, int xPlus, int yPlus) {
            DisplayName = displayName;
            XPlus = xPlus;
            YPlus = yPlus;
        }

        public int XPlus { get; }
        public int YPlus { get; }
        public string DisplayName { get; }

        public override string ToString() => $"Direction({DisplayName})";
    }

    public record Point(int X, int Y) {
    }

    private const char GARDEN_STONE = '#';
    private const char GARDEN_PLOT = '.';
    private const char GARDEN_START = 'S';

    private readonly char[][] _garden;
    private readonly bool _infiniteGarden;

    public StepCounter(IEnumerable<string> input, bool infiniteGarden = false) {
        _garden = input.ParseCharMatrix();
        _infiniteGarden = infiniteGarden;
    }

    public long CalculateReachablePlotsCount(int steps) {
        if (steps < 100) {
            // remove the points that are odd if the steps are even or the other way around
            return CalculateReachablePlots(steps).Count(kv => (kv.Value % 2) == (steps % 2));
        }
        
        // too many steps, we need to calculate this differently
        return 1L;
    }

    public string StringifyReachablePlots(int steps) {
        var reachablePlots = CalculateReachablePlots(steps);
        var result = "";
        for (var y = 0; y < _garden[0].Length; y++) {
            for (var x = 0; x < _garden.Length; x++) {
                if (reachablePlots.ContainsKey(new Point(x, y))) {
                    result += '0';
                } else {
                    result += _garden[x][y];
                }
            }

            result += "\n";
        }

        return result;
    }

    private IDictionary<Point, int> CalculateReachablePlots(int steps) {
        var gardenWidth = _garden.Length;
        var gardenHeight = _garden[0].Length;

        var startPoint = FindStartPoint();
        var reachableGardenPlots = new Dictionary<Point, int> {{startPoint, 0}};
        var pointsToHandle = new List<Point> {startPoint};

        while (pointsToHandle.Count > 0) {
            var pointToHandle = pointsToHandle.First();
            pointsToHandle.RemoveAll(p => p.Equals(pointToHandle));

            // move in every direction from this point
            foreach (var direction in Direction.All) {
                var newX = pointToHandle.X + direction.XPlus;
                var newY = pointToHandle.Y + direction.YPlus;

                // the point is not on the map
                if (!_infiniteGarden & (newX < 0 || newY < 0 || newX >= _garden.Length || newY >= _garden[0].Length)) {
                    continue;
                }

                // the point is a stone, so we can't move there
                if (_garden[Modulo(newX, gardenWidth)][Modulo(newY, gardenHeight)] == GARDEN_STONE) {
                    continue;
                }

                var newPoint = new Point(newX, newY);
                var newDistance = reachableGardenPlots[pointToHandle] + 1;

                // this point is too far away
                if (newDistance > steps) {
                    continue;
                }

                if (reachableGardenPlots.TryGetValue(newPoint, out var existingDistance)) {
                    // this specific point was already visited...
                    if (newDistance < existingDistance) {
                        // ...but we can visit it faster, so use this way
                        reachableGardenPlots[newPoint] = newDistance;
                        pointsToHandle.Add(newPoint);
                    }
                } else {
                    // this point was not visited yet, so let's do it now
                    reachableGardenPlots[newPoint] = newDistance;
                    pointsToHandle.Add(newPoint);
                }
            }
        }

        return reachableGardenPlots;
    }

    static int Modulo(int a, int b) {
        return (Math.Abs(a * b) + a) % b;
    }

    private Point FindStartPoint() {
        for (var x = 0; x < _garden.Length; x++) {
            for (var y = 0; y < _garden[x].Length; y++) {
                if (_garden[x][y] == GARDEN_START) {
                    return new Point(x, y);
                }
            }
        }

        throw new ArgumentException("Could not find start point!");
    }
}