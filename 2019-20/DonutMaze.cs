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
    internal const string StartPortal = "AA";
    internal const string EndPortal = "ZZ";

    private const char Walkable = '.';
    private const char Wall = '#';
    private const char PortalChar = 'P';
    internal const char End = 'E';
    internal const char Start = 'S';

    private const int Border = 2;

    internal readonly char[][] Maze;
    private readonly int _ringSize;
    private readonly int _holeSize;
    internal readonly Portal[] Portals;
    internal readonly IDictionary<Point, Point> PortalConnections;

    public DonutMaze(string[] donutAsStrings, int ringSize) {
        _ringSize = ringSize;
        _holeSize = donutAsStrings[donutAsStrings.Length / 2].Length - 2 * _ringSize;

        Maze = ParseMaze(donutAsStrings);
        Portals = ParsePortals(donutAsStrings);
        PortalConnections = MapPortals(Portals);
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

    private static IEnumerable<Portal> ParseHorizontalPortals(string[] donutAsStrings, int index, int portalY, bool outerPortal) {
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

    private static IEnumerable<Portal> ParseVerticalPortals(string[] donutAsStrings, int index, int portalX, bool outerPortal) {
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
                Maze[portalByName.Value[0].Location.X][portalByName.Value[0].Location.Y] = Start;
            } else if (portalByName.Key == EndPortal) {
                Assert.AreEqual(1, portalByName.Value.Length, "There is only 1 end portal!");
                Maze[portalByName.Value[0].Location.X][portalByName.Value[0].Location.Y] = End;
            } else {
                Assert.AreEqual(2, portalByName.Value.Length, "There must be 2 of each portal! (" + portalByName.Key + ")");
                Maze[portalByName.Value[0].Location.X][portalByName.Value[0].Location.Y] = PortalChar;
                Maze[portalByName.Value[1].Location.X][portalByName.Value[1].Location.Y] = PortalChar;
                result.Add(portalByName.Value[0].Location, portalByName.Value[1].Location);
                result.Add(portalByName.Value[1].Location, portalByName.Value[0].Location);
            }
        }

        // clear the hole
        for (var x = _ringSize; x < Maze.Length - _ringSize; x++) {
            for (var y = _ringSize; y < Maze[x].Length - _ringSize; y++) {
                Maze[x][y] = ' ';
            }
        }
        return result;
    }

    internal IEnumerable<Point> FindPossibleNextPositions(Point position) {
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
        if (x >= Maze.Length || y >= Maze[0].Length)
            return false;
        return Maze[x][y] == Walkable || Maze[x][y] == PortalChar || Maze[x][y] == End;
    }

    public Portal FetchPortal(Point location) {
        return Portals.Single(p => p.Location == location);
    }
    
    public override string ToString() {
        var result = $"Ring Size: {_ringSize}\n" +
                     $"Hole Size: {_holeSize}\n";
        for (var y = 0; y < Maze[0].Length; y++) {
            for (var x = 0; x < Maze.Length; x++) {
                result += Maze[x][y];
            }

            result += "\n";
        }

        return result;
    }

}

public record Point(int X, int Y);

public record Portal(string Name, Point Location, bool OuterPortal);