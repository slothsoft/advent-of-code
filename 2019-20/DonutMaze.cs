using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;

namespace AoC;

/// <summary>
/// <a href="https://adventofcode.com/2019/day/20">Day 20: Donut Maze</a>: You notice a strange pattern
/// on the surface of Pluto and land nearby to get a closer look. Upon closer inspection, you realize
/// you've come across one of the famous space-warping mazes of the long-lost Pluto civilization!
///
/// Because there isn't much space on Pluto, the civilization that used to live here thrived by
/// inventing a method for folding spacetime. Although the technology is no longer understood,
/// mazes like this one provide a small glimpse into the daily life of an ancient Pluto citizen.
///
/// This maze is shaped like a donut. Portals along the inner and outer edge of the donut
/// can instantly teleport you from one side to the other. 
/// </summary>
public class DonutMaze {
    private record Point(int X, int Y);

    private record PointWithLevel(Point Point, int Level);

    private record Portal(string Name, Point Location, bool OuterPortal);

    private const string StartPortal = "AA";
    private const string EndPortal = "ZZ";

    private const char Walkable = '.';
    private const char Wall = '#';
    private const char PortalChar = 'P';
    private const char End = 'E';
    private const char Start = 'S';

    private const int Border = 2;

    public int MaxLevel { get; init; } = 10;
    public int MaxStepLength { get; init; } =  int.MaxValue;

    private readonly char[][] _maze;
    private readonly int _ringSize;
    private readonly int _holeSize;
    private readonly Portal[] _portals;
    private readonly IDictionary<Point, Point> _portalConnections;

    public DonutMaze(string[] donutAsStrings, int ringSize) {
        _ringSize = ringSize;
        _holeSize = donutAsStrings[donutAsStrings.Length / 2].Length - 2 * _ringSize;

        _maze = ParseMaze(donutAsStrings, _ringSize);
        _portals = ParsePortals(donutAsStrings);
        _portalConnections = MapPortals(_portals);
    }

    private static char[][] ParseMaze(string[] donutAsStrings, int ringSize) {
        var result = new char[donutAsStrings[0].Length - 2 * Border][];
        for (var i = 0; i < result.Length; i++) {
            result[i] = new char[donutAsStrings.Length - 2 * Border];
        }

        for (var x = 0; x < result.Length; x++) {
            for (var y = 0; y < result[x].Length; y++) {
                result[x][y] = donutAsStrings[y + Border][x + Border];
            }
        }

        return result;
    }

    private Portal[] ParsePortals(string[] donutAsStrings) {
        return ParseHorizontalPortals(donutAsStrings, 0, 2, true)
            .Concat(ParseHorizontalPortals(donutAsStrings, Border + _ringSize, -1, false))
            .Concat(ParseHorizontalPortals(donutAsStrings, donutAsStrings.Length - 2 * Border - _ringSize, 2, false))
            .Concat(ParseHorizontalPortals(donutAsStrings, donutAsStrings.Length - Border, -1, true))
            // another 4 lines of vertical portals
            .Concat(ParseVerticalPortals(donutAsStrings, 0, 2, true))
            .Concat(ParseVerticalPortals(donutAsStrings, Border + _ringSize, -1, false))
            .Concat(ParseVerticalPortals(donutAsStrings, donutAsStrings[0].Length - 2 * Border - _ringSize, 2, false))
            .Concat(ParseVerticalPortals(donutAsStrings, donutAsStrings[0].Length - Border, -1, true))
            .ToArray();
    }

    private IEnumerable<Portal> ParseHorizontalPortals(string[] donutAsStrings, int index, int portalY, bool outerPortal) {
        for (var i = 0; i < donutAsStrings[index].Length; i++) {
            if (char.IsLetter(donutAsStrings[index][i])) {
                if (!char.IsLetter(donutAsStrings[index + 1][i])) {
                    continue; // might be from another portal
                }

                var portal = new Portal(char.ToString(donutAsStrings[index][i]) + donutAsStrings[index + 1][i],
                    new Point(i - Border, index + portalY - Border), outerPortal);
                yield return portal;
            }
        }
    }

    private IEnumerable<Portal> ParseVerticalPortals(string[] donutAsStrings, int index, int portalX, bool outerPortal) {
        for (var i = 0; i < donutAsStrings.Length; i++) {
            if (char.IsLetter(donutAsStrings[i][index])) {
                if (!char.IsLetter(donutAsStrings[i][index + 1])) {
                    continue; // might be from another portal
                }

                var portal = new Portal(char.ToString(donutAsStrings[i][index]) + donutAsStrings[i][index + 1],
                    new Point(index + portalX - Border, i - Border), outerPortal);
                yield return portal;
            }
        }
    }

    private IDictionary<Point, Point> MapPortals(Portal[] portals) {
        var result = new Dictionary<Point, Point>();
        var portalsByName = portals.GroupBy(p => p.Name).ToDictionary(g => g.Key, g => g.ToArray());
        foreach (var portalByName in portalsByName) {
            if (portalByName.Key == StartPortal) {
                Assert.AreEqual(1, portalByName.Value.Length, "There is only 1 start portal!");
                _maze[portalByName.Value[0].Location.X][portalByName.Value[0].Location.Y] = Start;
            } else if (portalByName.Key == EndPortal) {
                Assert.AreEqual(1, portalByName.Value.Length, "There is only 1 end portal!");
                _maze[portalByName.Value[0].Location.X][portalByName.Value[0].Location.Y] = End;
            } else {
                Assert.AreEqual(2, portalByName.Value.Length, "There must be 2 of each portal! (" + portalByName.Key + ")");
                _maze[portalByName.Value[0].Location.X][portalByName.Value[0].Location.Y] = PortalChar;
                _maze[portalByName.Value[1].Location.X][portalByName.Value[1].Location.Y] = PortalChar;
                result.Add(portalByName.Value[0].Location, portalByName.Value[1].Location);
                result.Add(portalByName.Value[1].Location, portalByName.Value[0].Location);
            }
        }

        // clear the hole
        for (var x = _ringSize; x < _maze.Length - _ringSize; x++) {
            for (var y = _ringSize; y < _maze[x].Length - _ringSize; y++) {
                _maze[x][y] = ' ';
            }
        }
        return result;
    }

    public int SolveReturnSteps() {
        var solutions = new List<Point[]>();
        var currentSteps = new List<Point>();
        currentSteps.Add(_portals.Single(p => p.Name == StartPortal).Location);
        Solve(solutions, currentSteps);
        return solutions.MinBy(s => s.Length)?.Length ?? -1;
    }

    private void Solve(List<Point[]> solutions, List<Point> currentSteps) {
        var position = currentSteps[^1];
        var possibleNextPositions = FindPossibleNextPositions(position)
            .Where(p => !currentSteps.Contains(p)) // we will not backtrack
            .ToArray();

        if (possibleNextPositions.Length == 0) {
            return; // there is no way to go
        }

        if (possibleNextPositions.Length == 1) {
            // there is only one way to go, so go there
            Step(solutions, currentSteps, possibleNextPositions[0]);
            return;
        }

        foreach (var possibleNextPosition in possibleNextPositions) {
            Step(solutions, new List<Point>(currentSteps), possibleNextPosition);
        }
    }

    private IEnumerable<Point> FindPossibleNextPositions(Point position) {
        if (IsWalkable(position.X - 1, position.Y)) {
            yield return position with {X = position.X - 1};
        }

        if (IsWalkable(position.X + 1, position.Y)) {
            yield return position with {X = position.X + 1};
        }

        if (IsWalkable(position.X, position.Y - 1)) {
            yield return position with {Y = position.Y - 1};
        }

        if (IsWalkable(position.X, position.Y + 1)) {
            yield return position with {Y = position.Y + 1};
        }
    }

    private bool IsWalkable(int x, int y) {
        if (x < 0 || y < 0)
            return false;
        if (x >= _maze.Length || y >= _maze[0].Length)
            return false;
        return _maze[x][y] == Walkable || _maze[x][y] == PortalChar || _maze[x][y] == End;
    }

    private void Step(List<Point[]> solutions, List<Point> currentSteps, Point nextPosition) {
        if (_portalConnections.ContainsKey(nextPosition)) {
            // the next step is a portal field
            currentSteps.Add(nextPosition);
            currentSteps.Add(_portalConnections[nextPosition]);
            Solve(solutions, currentSteps);
        } else if (_maze[nextPosition.X][nextPosition.Y] == End) {
            // the next step is THE END
            solutions.Add(currentSteps.ToArray());
        } else {
            // the next step is a regular field
            currentSteps.Add(nextPosition);
            Solve(solutions, currentSteps);
        }
    }

    public int SolveRecursiveReturnSteps() {
        var solutions = new List<PointWithLevel[]>();
        var currentSteps = new List<PointWithLevel>();
        var location = _portals.Single(p => p.Name == StartPortal).Location;
        currentSteps.Add(new PointWithLevel(location, 0));
        SolveRecursive(solutions, currentSteps, 0);
        return solutions.MinBy(s => s.Length)?.Length ?? -1;
    }

    private void SolveRecursive(List<PointWithLevel[]> solutions, List<PointWithLevel> currentSteps, int currentLevel) {
        var position = currentSteps[^1];
        var possibleNextPositions = FindPossibleNextPositions(position.Point)
            .Where(p => currentSteps.All(s => s.Point != p || s.Level != currentLevel)) // we will not backtrack
            .Where(p => _maze[p.X][p.Y] != PortalChar || IsValidPortal(p, currentLevel))
            .ToArray();

        if (possibleNextPositions.Length == 0) {
            return; // there is no way to go
        }

        if (possibleNextPositions.Length == 1) {
            // there is only one way to go, so go there
            StepRecursive(solutions, currentSteps, possibleNextPositions[0], currentLevel);
            return;
        }

        foreach (var possibleNextPosition in possibleNextPositions) {
            StepRecursive(solutions, new List<PointWithLevel>(currentSteps), possibleNextPosition, currentLevel);
        }
    }

    private bool IsValidPortal(Point location, int currentLevel) {
        var portal = _portals.Single(p => p.Location == location);
        return portal.OuterPortal ? currentLevel > 0 : currentLevel <= MaxLevel;
    }

    private void StepRecursive(List<PointWithLevel[]> solutions, List<PointWithLevel> currentSteps, Point nextPosition, int currentLevel) {
        if (currentSteps.Count > MaxStepLength) {
            // we refuse to take another step because the algorithm needs to stop somewhere
            return;
        }
        if (currentSteps.Count > (solutions.MinBy(s => s.Length)?.Length ?? int.MaxValue)) {
            // we refuse to take another step because the algorithm has already found a better solution
            return;
        }
        
        if (_portalConnections.ContainsKey(nextPosition)) {
            // the next step is a portal field
            var portal = _portals.Single(p => p.Location == nextPosition);
            var nextLevel = portal.OuterPortal ? currentLevel - 1 : currentLevel + 1;

            currentSteps.Add(new PointWithLevel(nextPosition, currentLevel));
            currentSteps.Add(new PointWithLevel(_portalConnections[nextPosition], nextLevel));
            SolveRecursive(solutions, currentSteps, nextLevel);
        } else if (_maze[nextPosition.X][nextPosition.Y] == End) {
            // the next step is THE END (if it's not the top level, it's a dead end)
            if (currentLevel == 0) {
                solutions.Add(currentSteps.ToArray());
            }
        } else {
            // the next step is a regular field
            currentSteps.Add(new PointWithLevel(nextPosition, currentLevel));
            SolveRecursive(solutions, currentSteps, currentLevel);
        }
    }

    // this might improve performance because there are not too much crossings
    public void RemoveDeadEnds() {
        bool somethingWasChanged;
        do {
            somethingWasChanged = false;
            for (var x = 1; x < _maze.Length - 1; x++) {
                for (var y = 1; y < _maze[x].Length - 1; y++) {
                    if (_maze[x][y] == Walkable && FindPossibleNextPositions(new Point(x, y)).Count() <= 1) {
                        // it's a dead end, so remove it from the maze
                        _maze[x][y] = Wall;
                        somethingWasChanged = true;
                    }
                }
            }
        } while (somethingWasChanged);
    }

    public override string ToString() {
        var result = $"Ring Size: {_ringSize}\n" +
                     $"Hole Size: {_holeSize}\n";
        for (var y = 0; y < _maze[0].Length; y++) {
            for (var x = 0; x < _maze.Length; x++) {
                result += _maze[x][y];
            }

            result += "\n";
        }

        return result;
    }

}