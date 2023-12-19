using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace AoC;

/// <summary>
/// <a href="https://adventofcode.com/2023/day/18">Day 16: Day 18: Lavaduct Lagoon</a>:
/// The Elves are concerned the lagoon won't be large enough; if they follow their dig plan, how many cubic meters of lava could it hold?
/// </summary>
public class LavaductLagoon {
    public interface ITile {
        public char DisplayChar { get; }
    }

    public struct EdgeTile : ITile {
        public EdgeTile(string color) {
            Color = color;
        }

        public string Color { get; }
        public char DisplayChar => '#';
    }

    public struct TrenchTile : ITile {
        public char DisplayChar => 'X';
    }

    public struct OutsideTile : ITile {
        public static OutsideTile Instance { get; } = new();
        public char DisplayChar => 'o';
    }

    public struct Direction {
        public static Direction Up = new('U', 0, -1);
        public static Direction Down = new('D', 0, 1);
        public static Direction Right = new('R', 1, 0);
        public static Direction Left = new('L', -1, 0);
        public static Direction[] All = {Up, Down, Right, Left};

        public static Direction ForDisplayChar(char displayChar) {
            foreach (var direction in All) {
                if (displayChar == direction.DisplayChar) {
                    return direction;
                }
            }

            throw new ArgumentException("Do not know display char " + displayChar);
        }

        public Direction(char displayChar, int xPlus, int yPlus) {
            DisplayChar = displayChar;
            XPlus = xPlus;
            YPlus = yPlus;
        }

        public int XPlus { get; }
        public int YPlus { get; }
        public char DisplayChar { get; }
        public override string ToString() => $"Direction({DisplayChar})";
    }

    private ITile?[][] _tiles;
    private int _xOffset;
    private int _yOffset;

    public LavaductLagoon(IEnumerable<string> input, bool useHexValues = false) {
        (_tiles, _xOffset, _yOffset) = ParseTiles(input, useHexValues);
    }

    public static (ITile[][] Tiles, int XOffset, int YOffset) ParseTiles(IEnumerable<string> input, bool useHexValues) {
        var inputAsArray = input.ToArray();
        var (xMin, xMax) = FetchArrayMinMax(inputAsArray, Direction.Left.DisplayChar, Direction.Right.DisplayChar, useHexValues);
        var (yMin, yMax) = FetchArrayMinMax(inputAsArray, Direction.Up.DisplayChar, Direction.Down.DisplayChar, useHexValues);

        var result = new ITile[xMax - xMin + 1][];
        for (var i = 0; i < result.Length; i++) {
            result[i] = new ITile[yMax - yMin + 1];
        }

        var (x, y) = (-xMin, -yMin);

        foreach (var line in inputAsArray) {
            var (newX, newY) = (x, y);

            var split = line.Split(' ');
            var direction = Direction.ForDisplayChar(line[0]);
            var steps = ExtractDistance(line, useHexValues);
            newX += direction.XPlus * steps;
            newY += direction.YPlus * steps;

            if (newX == x) {
                AddVerticalLine(result, newX, y, newY, split[2]);
            } else {
                AddHorizontalLine(result, newY, x, newX, split[2]);
            }

            (x, y) = (newX, newY);
        }

        return (result, xMin, yMin);
    }

    private static void AddHorizontalLine(ITile[][] tiles, int y, int fromX, int toX, string color) {
        if (toX < fromX) {
            (fromX, toX) = (toX, fromX);
        }

        var tile = new EdgeTile(color.Replace("(", "").Replace(")", ""));
        for (var x = fromX; x <= toX; x++) {
            tiles[x][y] = tile;
        }
    }

    private static void AddVerticalLine(ITile[][] tiles, int x, int fromY, int toY, string color) {
        if (toY < fromY) {
            (fromY, toY) = (toY, fromY);
        }

        var tile = new EdgeTile(color.Replace("(", "").Replace(")", ""));
        for (var y = fromY; y <= toY; y++) {
            tiles[x][y] = tile;
        }
    }

    private static (int min, int max) FetchArrayMinMax(IEnumerable<string> input, char minusChar, char plusChar, bool useHexValues) {
        var (min, max) = (0, 0);
        var current = 0;
        foreach (var line in input) {
            if (line[0] == minusChar) {
                current -= ExtractDistance(line, useHexValues);
            } else if (line[0] == plusChar) {
                current += ExtractDistance(line, useHexValues);
            }

            min = Math.Min(min, current);
            max = Math.Max(max, current);
        }

        return (min, max);
    }

    private static int ExtractDistance(string line, bool useHexValues) {
        if (useHexValues) {
            return int.Parse(line.Split(' ')[2][2..^2], NumberStyles.HexNumber);
        }

        return line.Split(' ')[1].ExtractDigitsAsInt();
    }

    public string Stringify() {
        var result = "";
        for (var y = 0; y < _tiles[0].Length; y++) {
            for (var x = 0; x < _tiles.Length; x++) {
                if (_tiles[x][y] == null) {
                    result += '.';
                } else {
                    result += _tiles[x][y]!.DisplayChar;
                }
            }

            result += "\n";
        }

        return result;
    }

    public void MarkOutsideTiles() {
        for (var x = 0; x < _tiles.Length; x++) {
            if (_tiles[x][0] == null) {
                MarkOutsideTilesStartingWith(x, 0);
            }

            if (_tiles[x][^1] == null) {
                MarkOutsideTilesStartingWith(x, _tiles[0].Length - 1);
            }
        }

        for (var y = 0; y < _tiles[0].Length; y++) {
            if (_tiles[0][y] == null) {
                MarkOutsideTilesStartingWith(0, y);
            }

            if (_tiles[^1][y] == null) {
                MarkOutsideTilesStartingWith(_tiles.Length - 1, y);
            }
        }
    }

    private void MarkOutsideTilesStartingWith(int x, int y) {
        var pointsToHandle = new HashSet<(int X, int Y)> {(x, y)};

        do {
            var firstPointToHandle = pointsToHandle.First();
            pointsToHandle.Remove(firstPointToHandle);

            if (_tiles[firstPointToHandle.X][firstPointToHandle.Y] is null) {
                _tiles[firstPointToHandle.X][firstPointToHandle.Y] = OutsideTile.Instance;

                foreach (var direction in Direction.All) {
                    var newX = firstPointToHandle.X + direction.XPlus;
                    var newY = firstPointToHandle.Y + direction.YPlus;
                    if (newX >= 0 && newY >= 0 && newX < _tiles.Length && newY < _tiles[newX].Length) {
                        if (_tiles[newX][newY] is null) {
                            pointsToHandle.Add((newX, newY));
                        }
                    }
                }
            }
        } while (pointsToHandle.Count > 0);
    }

    public long CalculateInsideTiles() {
        MarkOutsideTiles();
        return _tiles.SelectMany(t => t).Count(t => t == null || t is EdgeTile);
    }
}