using System;
using System.Collections.Generic;
using System.Linq;

namespace AoC.day23;

/// <summary>
/// <a href="https://adventofcode.com/2023/day/23">Day 23: A Long Walk</a>
/// </summary>
public class ALongWalk {
    public interface ITile {
        static ITile CreateTile(char tileChar, bool slopesArePaths) {
            switch (tileChar) {
                case '.':
                    return PathTile.instance;
                case '#':
                    return ForestTile.instance;
                default:
                    if (slopesArePaths) {
                        return PathTile.instance;
                    }

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

        private readonly Direction _direction;

        private SlopeTile(Direction direction) {
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
        private static readonly Direction North = new('^', 0, -1);
        private static readonly Direction South = new('v', 0, 1);
        private static readonly Direction East = new('>', 1, 0);
        private static readonly Direction West = new('<', -1, 0);
        public static readonly Direction[] All = {North, South, East, West};

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

    public record Trail(Point StartPoint, Point EndPoint, long Length) {
        public virtual bool Equals(Trail? other) {
            if (other == null) {
                return false;
            }

            if (StartPoint == other.StartPoint && EndPoint == other.EndPoint) {
                return true;
            }

            if (EndPoint == other.StartPoint && StartPoint == other.EndPoint) {
                return true;
            }

            return false;
        }

        public override int GetHashCode() => HashCode.Combine(
            Math.Min(StartPoint.X, EndPoint.X),
            Math.Min(StartPoint.Y, EndPoint.Y),
            Math.Max(StartPoint.X, EndPoint.X),
            Math.Max(StartPoint.Y, EndPoint.Y));
    }

    public ALongWalk(IEnumerable<string> input, bool slopesArePaths = false) {
        Input = input.ParseMatrix(c => ITile.CreateTile(c, slopesArePaths));
    }

    internal ITile[][] Input { get; }

    public long CalculateLongestHike() {
        var startPoint = FindSingleHikePoint(0);
        var endPoint = FindSingleHikePoint(Input[0].Length - 1);
        var trails = CalculateAllTrails(startPoint, endPoint);
        return CalculateLongestHike(trails, startPoint, endPoint);
    }

    private Point FindSingleHikePoint(int y) {
        for (var x = 0; x < Input.Length; x++) {
            if (Input[x][y] is PathTile) {
                return new Point(x, y);
            }
        }

        throw new ArgumentException("Could not find hike point.");
    }
    
    private List<Trail> CalculateAllTrails(Point startPoint, Point endPoint) {
        var result = new List<Trail>();
        var pointsToHandle = new HashSet<Point> {startPoint};

        while (pointsToHandle.Count > 0) {
            var pointToHandle = pointsToHandle.First();
            pointsToHandle.Remove(pointToHandle);

            if (pointToHandle == endPoint) {
                continue;
            }

            foreach (var trail in CalculateAllTrailsFromStartingPoint(pointToHandle, endPoint)) {
                result.Add(trail);

                if (result.All(t => t.StartPoint != trail.EndPoint)) {
                    // the trails of the end point wher not calculated yet
                    pointsToHandle.Add(trail.EndPoint);
                }
            }
        }

        return result;
    }

    private IEnumerable<Trail> CalculateAllTrailsFromStartingPoint(Point startPoint, Point endPoint) {
        foreach (var nextStep in Input[startPoint.X][startPoint.Y].FetchNextSteps(startPoint).Where(IsWalkable)) {
            var trail = CalculateTrailFromStartingPoint(startPoint, nextStep, endPoint);
            if (trail != null) {
                yield return trail;
            }
        }
    }
    
    private bool IsWalkable(Point point) {
        return Input[point.X][point.Y].IsWalkable;
    }

    private Trail? CalculateTrailFromStartingPoint(Point startPoint, Point firstStep, Point endPoint) {
        var hikePoints = new List<Point> {startPoint, firstStep};

        while (true) {
            var lastPoint = hikePoints.Last();

            if (lastPoint == endPoint) {
                return new Trail(startPoint, lastPoint, hikePoints.Count - 1);
            }

            var nextPoints = Input[lastPoint.X][lastPoint.Y]
                .FetchNextSteps(lastPoint)
                .Where(IsWalkable)
                .Where(p => !hikePoints.Contains(p))
                .ToArray();
            switch (nextPoints.Length) {
                case 1:
                    // there is only one way to go. Go it
                    hikePoints.Add(nextPoints[0]);
                    continue;
                case > 1: {
                    // there are multiple ways to go, so the trail ends here
                    return new Trail(startPoint, lastPoint, hikePoints.Count - 1);
                }
                default:
                    // there are no ways to go 
                    return null;
            }
        }
    }

    private long CalculateLongestHike(ICollection<Trail> allTrails, Point startPoint, Point endPoint) {
        return CalculateLongestHike(allTrails, new List<Trail> {allTrails.Single(t => t.StartPoint == startPoint)}, endPoint);
    }

    private long CalculateLongestHike(ICollection<Trail> allTrails, ICollection<Trail> hikeTrails, Point endPoint) {
        while (true) {
            var lastTrail = hikeTrails.Last();

            if (lastTrail.EndPoint == endPoint) {
                return hikeTrails.Sum(t => t.Length);
            }

            var nextTrails = allTrails
                .Where(t => t.StartPoint == lastTrail.EndPoint)
                .Where(t => !hikeTrails.Contains(t))
                .OrderByDescending(t => t.Length)
                .ToArray();

            switch (nextTrails.Length) {
                case 1:
                    // there is only one way to go. Go it
                    hikeTrails.Add(nextTrails[0]);
                    continue;
                case > 1: {
                    // there are multiple ways to go, so split up
                    var result = 0L;
                    foreach (var nextTrail in nextTrails) {
                        var newHikePoints = new List<Trail>(hikeTrails) {nextTrail};
                        result = Math.Max(result, CalculateLongestHike(allTrails, newHikePoints, endPoint));
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