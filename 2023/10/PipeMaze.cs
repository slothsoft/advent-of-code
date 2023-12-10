using System;
using System.Collections.Generic;
using System.Linq;

namespace AoC;

/// <summary>
/// <a href="https://adventofcode.com/2023/day/10">Day 10: Pipe Maze</a>: Find the single giant loop starting at S. How many steps along the loop does it take
/// to get from the starting position to the point farthest from the starting position?
/// </summary>
public class PipeMaze {
    internal struct Tile {
        public Tile(char value, bool connectsNorth = false, bool connectsSouth = false, bool connectsEast = false, bool connectsWest = false) {
            Value = value;
            ConnectsNorth = connectsNorth;
            ConnectsSouth = connectsSouth;
            ConnectsEast = connectsEast;
            ConnectsWest = connectsWest;
        }

        public char Value { get; }
        public bool ConnectsNorth { get; }
        public bool ConnectsSouth { get; }
        public bool ConnectsEast { get; }
        public bool ConnectsWest { get; }
        public bool IsStartTile => Value == startTileWithNowhereToGo.Value;

        internal IEnumerable<(int X, int Y)> FetchNextTiles(int x, int y) {
            if (ConnectsNorth) {
                yield return (x, y - 1);
            }

            if (ConnectsSouth) {
                yield return (x, y + 1);
            }

            if (ConnectsEast) {
                yield return (x + 1, y);
            }

            if (ConnectsWest) {
                yield return (x - 1, y);
            }
        }

        public override string ToString() => $"Tile {Value}: N={ConnectsNorth}, S={ConnectsSouth}, E={ConnectsEast}, W={ConnectsWest}";
    }

    internal enum TilePlacement {
        Unknown,
        Outside,
        Inside,
        Loop,
    }

    internal static Tile startTileWithNowhereToGo = new('S');
    internal static Tile floor = new('.');

    internal static Tile[] possibleTiles = {
        new('|', connectsNorth: true, connectsSouth: true), //
        new('-', connectsEast: true, connectsWest: true), //
        new('L', connectsEast: true, connectsNorth: true), //
        new('J', connectsNorth: true, connectsWest: true), //
        new('7', connectsSouth: true, connectsWest: true), //
        new('F', connectsSouth: true, connectsEast: true), //
        floor, //
        startTileWithNowhereToGo, //
    };

    public PipeMaze(IEnumerable<string> input) {
        Tiles = input.ParseMatrix(c => possibleTiles.Single(t => t.Value == c));
        (StartX, StartY) = ReplaceStartPoint();
    }

    internal Tile[][] Tiles { get; }
    internal int StartX { get; }
    internal int StartY { get; }

    private (int X, int Y) ReplaceStartPoint() {
        for (var x = 0; x < Tiles.Length; x++) {
            for (var y = 0; y < Tiles[x].Length; y++) {
                if (Tiles[x][y].Value == startTileWithNowhereToGo.Value) {
                    var connectsNorth = y > 0 && Tiles[x][y - 1].ConnectsSouth;
                    var connectsSouth = y < Tiles[x].Length - 1 && Tiles[x][y + 1].ConnectsNorth;
                    var connectsEast = x < Tiles.Length - 1 && Tiles[x + 1][y].ConnectsWest;
                    var connectsWest = x > 0 && Tiles[x - 1][y].ConnectsEast;
                    Tiles[x][y] = new Tile(startTileWithNowhereToGo.Value, connectsNorth, connectsSouth, connectsEast, connectsWest);
                    return (x, y);
                }
            }
        }

        throw new ArgumentException("Could not find start tile");
    }

    internal long CalculateLoopLength() {
        return CalculateLoopCoordinates().Count();
    }

    private IEnumerable<(int X, int Y)> CalculateLoopCoordinates() {
        var lastxAndY = (X: StartX, Y: StartY);
        var xAndY = Tiles[StartX][StartY].FetchNextTiles(StartX, StartY).First();

        yield return xAndY;

        do {
            var nextXAndY = Tiles[xAndY.X][xAndY.Y].FetchNextTiles(xAndY.X, xAndY.Y)
                .Single(otherXAndY => otherXAndY.X != lastxAndY.X || otherXAndY.Y != lastxAndY.Y);
            yield return nextXAndY;

            lastxAndY = xAndY;
            xAndY = nextXAndY;
        } while (xAndY.X != StartX || xAndY.Y != StartY);
    }

    public long CalculateEnclosedAreaSize() {
        var loopCoordinates = CalculateLoopCoordinates().ToArray();
        var tilePlacements = new TilePlacement[Tiles.Length][];
        for (var i = 0; i < tilePlacements.Length; i++) {
            tilePlacements[i] = new TilePlacement[Tiles[i].Length];
        }

        var insideCount = 0L;
        var outside = true;
        
        for (var y = 0; y < Tiles[0].Length; y++) {
            var northCount = 0;
            var southCount = 0;
            
            for (var x = 0; x < Tiles.Length; x++) {
                if (loopCoordinates.Contains((X: x, Y: y))) {
                    // we are on the edge of the loop
                    if (Tiles[x][y].ConnectsNorth) {
                        northCount++;
                    }
                    if (Tiles[x][y].ConnectsSouth) {
                        southCount++;
                    }

                    if (southCount > 0 && northCount > 0) {
                        // the loop pipe went from top to bottom and so we changed outside to inside and the other way around
                        northCount--;
                        southCount--;
                        outside = !outside;
                    }
                    
                    tilePlacements[x][y] = TilePlacement.Loop;
                } else if (outside) {
                    tilePlacements[x][y] = TilePlacement.Outside;
                } else {
                    tilePlacements[x][y] = TilePlacement.Inside;
                    insideCount++;
                }
            }
        }

        Stringify(tilePlacements);
        return insideCount;
    }

    private void Stringify(TilePlacement[][] tilePlacements) {
        for (var y = 0; y < Tiles[0].Length; y++) {
            for (var x = 0; x < Tiles.Length; x++) {
                var value = StringifyTile(tilePlacements[x][y], Tiles[x][y]);
                Console.Write(value);
            }
            Console.WriteLine();
        }
    }

    private static string StringifyTile(TilePlacement tilePlacement, Tile tile) {
        return tilePlacement switch {
            TilePlacement.Inside => "ℹ\ufe0f",
            TilePlacement.Outside => "\u2b55",
            TilePlacement.Loop => tile.Value.ToString(),
            _ => " "
        };
    }
}