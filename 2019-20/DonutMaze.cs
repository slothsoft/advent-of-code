using System;
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

    private record Portal(string Name, Point Location);

    private const string StartPortal = "AA";
    private const string EndPortal = "ZZ";

    private const char Walkable = '.';
    private const char Wall = '#';
    private const char PortalChar = 'P';
    private const char End = 'E';
    private const char Start = 'S';

    private const int Border = 2;

    private readonly char[][] _maze;
    private readonly int _ringSize;
    private readonly int _holeSize;
    private readonly Portal[] _portals;
    private readonly IDictionary<Point, Point> _portalConnections;

    public DonutMaze(string[] donutAsStrings, int ringSize) {
        _ringSize = ringSize;
        _holeSize = donutAsStrings[donutAsStrings.Length / 2].Length - 2 * _ringSize;

        _maze = ParseMaze(donutAsStrings);
        _portals = ParsePortals(donutAsStrings);
        _portalConnections = MapPortals(_portals);
        Console.WriteLine(this);
    }

    private static char[][] ParseMaze(string[] donutAsStrings) {
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
        return ParseHorizontalPortals(donutAsStrings, 0, 2)
            .Concat(ParseHorizontalPortals(donutAsStrings, Border + _ringSize, -1))
            .Concat(ParseHorizontalPortals(donutAsStrings, donutAsStrings.Length - 2 * Border - _ringSize, 2))
            .Concat(ParseHorizontalPortals(donutAsStrings, donutAsStrings.Length - Border, -1))
            .Concat(ParseVerticalPortals(donutAsStrings, 0, 2))
            .Concat(ParseVerticalPortals(donutAsStrings, Border + _ringSize, -1))
            .Concat(ParseVerticalPortals(donutAsStrings, donutAsStrings[0].Length - 2 * Border - _ringSize, 2))
            .Concat(ParseVerticalPortals(donutAsStrings, donutAsStrings[0].Length - Border, -1))
            .ToArray();
    }

    private IEnumerable<Portal> ParseHorizontalPortals(string[] donutAsStrings, int index, int portalY) {
        Console.WriteLine(index);
        for (var i = 0; i < donutAsStrings[index].Length; i++) {
            if (char.IsLetter(donutAsStrings[index][i])) {
                if (!char.IsLetter(donutAsStrings[index + 1][i])) {
                    continue; // might be from another portal
                }

                var portal = new Portal(char.ToString(donutAsStrings[index][i]) + donutAsStrings[index + 1][i],
                    new Point(i - Border, index + portalY - Border));
                Console.WriteLine(portal);
                yield return portal;
            }
        }
    }

    private IEnumerable<Portal> ParseVerticalPortals(string[] donutAsStrings, int index, int portalX) {
        Console.WriteLine(index);
        for (var i = 0; i < donutAsStrings.Length; i++) {
            if (char.IsLetter(donutAsStrings[i][index])) {
                if (!char.IsLetter(donutAsStrings[i][index + 1])) {
                    continue; // might be from another portal
                }

                var portal = new Portal(char.ToString(donutAsStrings[i][index]) + donutAsStrings[i][index + 1],
                    new Point(index + portalX - Border, i - Border));
                Console.WriteLine(portal);
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

        return result;
    }

    public int SolveReturnSteps() {
        var solutions = new List<Point[]>();
        var currentSteps = new List<Point>();
        currentSteps.Add(_portals.Single(p => p.Name == StartPortal).Location);
        Solve(solutions, currentSteps);
        return solutions.OrderBy(s => s.Length).First().Length;
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