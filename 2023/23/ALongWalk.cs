using System;
using System.Collections.Generic;
using System.Linq;

namespace AoC.day23;

/// <summary>
/// <a href="https://adventofcode.com/2023/day/23">Day 23: A Long Walk</a>
/// </summary>
public class ALongWalk {
    public interface ITile {
        static ITile CreateTile(char tileChar) {
            switch (tileChar) {
                case '.':
                    return PathTile.instance;
                case '#':
                    return ForestTile.instance;
                default:
                    return SlopeTile.instances[tileChar];
            }
        }

        bool IsWalkable { get; }
        IEnumerable<Point> FetchNextSteps(Point startPoint);
    }

    public class PathTile : ITile {
        public static readonly PathTile instance = new();

        public bool IsWalkable => true;
        
        public IEnumerable<Point> FetchNextSteps(Point startPoint) {
            foreach (var direction in Direction.All) {
                var newX = startPoint.X + direction.XPlus;
                var newY = startPoint.Y + direction.YPlus;
                if (newX >= 0 && newY >= 0) {
                    yield return new Point(newX, newY);
                }
            }
        }
    }

    public class ForestTile : ITile {
        public static readonly ForestTile instance = new();

        public bool IsWalkable => false;
        
        public IEnumerable<Point> FetchNextSteps(Point startPoint) {
            return Enumerable.Empty<Point>();
        }
    }

    public class SlopeTile : ITile {
        public static readonly Dictionary<char, SlopeTile> instances = Direction.All.ToDictionary(d => d.DisplayChar, d => new SlopeTile(d));

        private Direction _direction;

        public SlopeTile(Direction direction) {
            _direction = direction;
        }
        
        public bool IsWalkable => true;

        public IEnumerable<Point> FetchNextSteps(Point startPoint) {
            var newX = startPoint.X + _direction.XPlus;
            var newY = startPoint.Y + _direction.YPlus;
            if (newX >= 0 && newY >= 0) {
                yield return new Point(newX, newY);
            }
        }
    }

    public struct Direction {
        private static Direction North = new('^', 0, -1);
        private static Direction South = new('v', 0, 1);
        private static Direction East = new('>', 1, 0);
        private static Direction West = new('<', -1, 0);
        public static Direction[] All = {North, South, East, West};

        private Direction(char displayChar, int xPlus, int yPlus) {
            DisplayChar = displayChar;
            XPlus = xPlus;
            YPlus = yPlus;
        }

        public int XPlus { get; }
        public int YPlus { get; }
        public char DisplayChar { get; }

        public override string ToString() => $"Direction({DisplayChar})";
    }

    public record Point(int X, int Y);

    public ALongWalk(IEnumerable<string> input) {
        Input = input.ParseMatrix(ITile.CreateTile);
    }

    internal ITile[][] Input { get; }

    public long CalculateLongestHike() {
        var startPoint = FindSingleHikePoint(0);
        var endPoint = FindSingleHikePoint(Input[0].Length - 1);
        return CalculateLongestHike(new List<Point> {startPoint}, endPoint) - 1;
    }

    private Point FindSingleHikePoint(int y) {
        for (var x = 0; x < Input.Length; x++) {
            if (Input[x][y] is PathTile) {
                return new Point(x, y);
            }
        }

        throw new ArgumentException("Could not find hike point.");
    }

    private long CalculateLongestHike(ICollection<Point> hikePoints, Point endPoint) {
        while (true) {
            var lastPoint = hikePoints.Last();

            if (lastPoint == endPoint) {
                return hikePoints.Count;
            }

            var nextPoints = Input[lastPoint.X][lastPoint.Y]
                .FetchNextSteps(lastPoint)
                .Where(p => Input[p.X][p.Y].IsWalkable)
                .Where(p => !hikePoints.Contains(p))
                .ToArray();
            switch (nextPoints.Length) {
                case 1:
                    // there is only one way to go. Go it
                    hikePoints.Add(nextPoints[0]);
                    continue;
                case > 1: {
                    // there are multiple ways to go, so split up
                    var result = 0L;
                    foreach (var nextPoint in nextPoints) {
                        var newHikePoints = new List<Point>(hikePoints) {nextPoint};
                        result = Math.Max(result, CalculateLongestHike(newHikePoints, endPoint));
                    }

                    return result;
                }
                default:
                    // there are no ways to go D:
                    return 0;
            }
        }
    }
}