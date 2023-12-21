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

    public StepCounter(IEnumerable<string> input) {
        _garden = input.ParseCharMatrix();
    }

    public int GardenCount => _garden.Length;

    public long CalculateReachablePlotsCount(int steps, bool infiniteGarden = false) {
        // remove the points that are odd if the steps are even or the other way around
        return CalculateReachablePlots(steps, infiniteGarden).Count(kv => (kv.Value % 2) == (steps % 2));
    }

    public string StringifyReachablePlots(int steps) {
        var reachablePlots = CalculateReachablePlots(steps, false);
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

    private IDictionary<Point, int> CalculateReachablePlots(int steps, bool infiniteGarden) {
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
                if (!infiniteGarden & (newX < 0 || newY < 0 || newX >= _garden.Length || newY >= _garden[0].Length)) {
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

    private static int Modulo(int a, int b) {
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

    // prime factors of steps 26_501_365 = 5, 11, 481843
    // grid size of input.txt: 131 * 131 = 17_161
    // the result without stones should be square standing on the corner
    // -> the area should be (Xmax - Xmin) * (Ymax - Ymin) / 2 = (2 * steps) * (2 * steps) / 2 = 2 * steps²
    // -> but how to remove the stones?
    // [S] is at 66|66 (or 65|65 starting at 0), so 65 fields are to the left, right, top and bottom
    // 26_501_365 = 202_300 * 131 + 65
    // -> so the map around the start point until the edge, and then 202300 full maps into either direction
    
    public long CalculateReachablePlotsCountViaAlgorithm(int steps) {
        var entireGardensLength = steps / _garden.Length;
        var restSteps = steps % _garden.Length;
        
        // It seems we can use a quadratic function to calculate the result
        // ( https://www.mathepower.com/en/quadraticfunctions.php )
        // ================================================================
        // For S*Len+2 |         | For S*Len+3 |
        // 0 => _____8 |         | 0 => ____13 | 
        // 1 => _15576 | +_15568 | 1 => _15797 |
        // 2 => _60970 | +_45394 | 2 => _61407 |
        // 4 => 241236 | +180266 |             |
        // v-v-v-v-v-v--------------------------
        // f(x) = 14913 * x² + 655 * x + 8
        // f(x) =   a   * x² +  b  * x + c
        // -------------------------------------
        // I    c = (value for S = 0)
        // II   a + b + c = (value for S = 1)  |  2a + 2b + 2c = 2 * (value for S = 1)
        // III  4a + 2b + c = (value for S = 2) 
        // III - II  2a - c =  (value for S = 2) - 2 * (value for S = 1)
        //           2a =  (value for S = 2) - 2 * (value for S = 1) + c
        //           a = (value for S = 2)/2 - (value for S = 1) + c/2
        // >II  b = (value for S = 1) - a - c  

        var valueForS0 = CalculateReachablePlotsCount(restSteps, true);
        var valueForS1 = CalculateReachablePlotsCount((1 * _garden.Length) + restSteps, true);
        var valueForS2 = CalculateReachablePlotsCount((2 * _garden.Length) + restSteps,true);
        
        // Console.WriteLine("S0 = " + valueForS0);
        // Console.WriteLine("S1 = " + valueForS1);
        // Console.WriteLine("S2 = " + valueForS2);

        var c = valueForS0;
        var a = valueForS2 / 2 - valueForS1 + valueForS0 / 2;
        var b = valueForS1 - a - c;
        
        // Console.WriteLine("a = " + a);
        // Console.WriteLine("b = " + b);
        // Console.WriteLine("c = " + c);
        
        // so now just calculate via formula a * x² + b * x + c
        return (a * entireGardensLength * entireGardensLength) + (b * entireGardensLength) + c;
    }
}