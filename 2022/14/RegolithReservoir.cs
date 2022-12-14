using System;
using System.Collections.Generic;
using System.Linq;

namespace AoC._14;

/// <summary>
/// <a href="https://adventofcode.com/2022/day/14">Day 14: Regolith Reservoir</a>: The distress signal
/// leads you to a giant waterfall! Actually, hang on - the signal seems like it's coming from the
/// waterfall itself, and that doesn't make any sense. However, you do notice a little path that leads
/// behind the waterfall.
/// </summary>
public class RegolithReservoir {
    private record Point(int X, int Y);

    private record Rock(Point[] Coordinates);

    private readonly IList<Rock> _rockFormations;
    private readonly char[][] _cave;

    public RegolithReservoir(string[] inputStrings, bool addFloor = false) {
        _rockFormations = ParseInputStrings(inputStrings);
        _cave = CreateCaveMap(_rockFormations, addFloor);
    }

    private static IList<Rock> ParseInputStrings(string[] inputStrings) {
        return inputStrings.Select(ParseInputString).ToList();
    }

    private static Rock ParseInputString(string inputString) {
        // 503,4 -> 502,4 -> 502,9 -> 494,9
        return new Rock(inputString
            .Split(" -> ")
            .Select(p => p.Split(","))
            .Select(p => new Point(int.Parse(p[0]), int.Parse(p[1])))
            .ToArray());
    }

    private static char[][] CreateCaveMap(IList<Rock> rockFormations, bool addFloor) {
        var maxX = rockFormations.SelectMany(r => r.Coordinates).Max(c => c.X) + 1;
        var maxY = rockFormations.SelectMany(r => r.Coordinates).Max(c => c.Y) + 1;

        if (addFloor) {
            maxX *= 2; // with a floor, the x for the sand pyramid must be bigger
            maxY += 2;
        }
        
        // set up the array
        var result = new char[maxX][];
        for (var x = 0; x < maxX; x++) {
            result[x] = new char[maxY];
            
            for (var y = 0; y < maxY; y++) {
                result[x][y] = '.';
            }
        }
        
        // set up the lines
        foreach (var rock in rockFormations) {
            var startPoint = rock.Coordinates[0];
            for (var i = 1; i < rock.Coordinates.Length; i++) {
                CreateCaveMapLine(result, startPoint, rock.Coordinates[i]);
                startPoint = rock.Coordinates[i];
            }
        }
        
        // set up the floor (if necessary) 
        
        if (addFloor) {
            for (var x = 0; x < maxX; x++) {
                result[x][maxY - 1] = '#';
            }
        }
        
        return result;
    }

    private static void CreateCaveMapLine(char[][] cave, Point startPoint, Point endPoint) {
        if (startPoint.X == endPoint.X) {
            // vertical line
            var minY = Math.Min(startPoint.Y, endPoint.Y);
            var maxY = Math.Max(startPoint.Y, endPoint.Y);
            for (var y = minY; y <= maxY; y++) {
                cave[startPoint.X][y] = '#';
            }
        } else {
            // horizontal line
            var minX = Math.Min(startPoint.X, endPoint.X);
            var maxX = Math.Max(startPoint.X, endPoint.X);
            for (var x = minX; x <= maxX; x++) {
                cave[x][startPoint.Y] = '#';
            }
        }
    }

    public bool PourInMoreSand(int count) {
        for (var i = 0; i < count; i++) {
            if (!PourInSand())
                return false;
        }

        return true;
    }

    public bool PourInSand(int x = 500, int y = 0) {

        if (_cave[x][y] != '.') {
            // something prevents the sand from "spawning"
            return false;
        }
        
        var sandPosition = new Point(x, y);
        
        while (true) {
            if (sandPosition.Y + 1 >= _cave[sandPosition.X].Length) {
                // sand has fallen into the abyss
                return false;
            }
            var possibleNextPositions = new[] {
                new Point(sandPosition.X, sandPosition.Y + 1),
                new Point(sandPosition.X - 1, sandPosition.Y + 1),
                new Point(sandPosition.X + 1, sandPosition.Y + 1),
            };

            var moveSand = false;
            foreach (var possibleNextPosition in possibleNextPositions) {
                if (_cave[possibleNextPosition.X][possibleNextPosition.Y] == '.') {
                    sandPosition = possibleNextPosition;
                    moveSand = true;
                    break;
                }
            }

            if (!moveSand) {
                // sand cannot move, so it rests where it is
                _cave[sandPosition.X][sandPosition.Y] = 'o';
                return true;
            }
        }
    }

    public override string ToString() {
        var minX = _rockFormations.SelectMany(r => r.Coordinates).Min(c => c.X);
        
        var result = "";
        for (var y = 0; y < _cave[0].Length; y++) {
            for (var x = minX; x < _cave.Length; x++) {
                result += _cave[x][y];
            }

            result += "\r\n";
        }

        return result;
    }

    public int SimulateSandPouring() {
        var result = 0;
        while (PourInSand()) {
            result++;
        }
        return result;
    }
}