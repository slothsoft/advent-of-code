using System;
using System.Collections.Generic;
using System.Linq;

namespace AoC._24;

/// <summary>
/// <a href="https://adventofcode.com/2022/day/24">Day 24: Blizzard Basin</a>
/// </summary>
public class BlizzardBasin {
    internal class BasinMap {
        private static readonly Tile[] AllTiles = (Tile[])Enum.GetValues(typeof(Tile));

        private static readonly Tile[] BlizzardTiles = {
            Tile.DownBlizzard, Tile.LeftBlizzard, Tile.RightBlizzard, Tile.UpBlizzard
        };

        public int Minute { get; private set; }
        private readonly int[][] _tiles;
        private readonly int[][] _temp;

        public BasinMap(string[] lines) {
            // init arrays
            _tiles = new int[lines[0].Length][];
            for (var x = 0; x < _tiles.Length; x++) {
                _tiles[x] = new int[lines.Length];
            }

            _temp = new int[_tiles.Length][];
            for (var x = 0; x < _temp.Length; x++) {
                _temp[x] = new int[_tiles[0].Length];
            }

            // now parse the map
            for (var y = 0; y < lines.Length; y++) {
                for (var x = 0; x < lines[y].Length; x++) {
                    _tiles[x][y] = (int)GetTileFor(lines[y][x]);
                }
            }
        }

        private static Tile GetTileFor(char c) {
            foreach (var tile in AllTiles) {
                if (tile.ToChar() == c) {
                    return tile;
                }
            }

            throw new ArgumentException("Do not know tile of input map: " + c);
        }

        public void ExecuteNextMinute() {
            for (var x = 0; x < _tiles.Length; x++) {
                for (var y = 0; y < _tiles[x].Length; y++) {
                    _temp[x][y] = _tiles[x][y] == (int)Tile.Wall ? (int)Tile.Wall : 0;
                }
            }

            for (var x = 0; x < _tiles.Length; x++) {
                for (var y = 0; y < _tiles[x].Length; y++) {
                    if (_tiles[x][y].IsMoveable()) {
                        MoveTile(x, y);
                    }
                }
            }

            for (var x = 0; x < _tiles.Length; x++) {
                for (var y = 0; y < _tiles[x].Length; y++) {
                    _tiles[x][y] = _temp[x][y];
                }
            }

            Minute++;
        }

        private void MoveTile(int x, int y) {
            var tileAsInt = _tiles[x][y];
            foreach (var tile in BlizzardTiles) {
                if (tile.IsAt(tileAsInt)) {
                    var newX = x + tile.GetXPlus();
                    var newY = y + tile.GetYPlus();
                    while (_tiles[newX][newY] == (int)Tile.Wall) {
                        newX = (newX + tile.GetXPlus() + _tiles.Length) % _tiles.Length;
                        newY = (newY + tile.GetYPlus() + _tiles[x].Length) % _tiles[x].Length;
                    }

                    _temp[newX][newY] |= (int)tile;
                }
            }
        }

        public override string ToString() {
            var result = "";
            for (var y = 0; y < _tiles[0].Length; y++) {
                for (var x = 0; x < _tiles.Length; x++) {
                    result += GetCharFor(_tiles[x][y]);
                }

                result += "\r\n";
            }

            return result;
        }

        private char GetCharFor(int tileAsInt) {
            foreach (var tile in AllTiles) {
                if (tileAsInt == (int)tile) {
                    return tile.ToChar();
                }
            }

            // we have multiple blizzards (or a bug) on our hands
            var result = 0;
            foreach (var tile in AllTiles) {
                if (tile.IsAt(tileAsInt)) {
                    result++;
                }
            }

            return result.ToString()[0];
        }

        public IEnumerable<(int, int)> FindAccessiblePoints(int startX, int startY) {
            if (_tiles[startX][startY] == (int)Tile.Empty) {
                yield return (startX, startY);
            }

            foreach (var tile in BlizzardTiles) {
                var newX = startX + tile.GetXPlus();
                var newY = startY + tile.GetYPlus();
                if (newY >= 0 && newY < _tiles[newX].Length && _tiles[newX][newY] == (int)Tile.Empty) {
                    yield return (newX, newY);
                }
            }
        }

        public (int, int) FindStartPoint() {
            for (var x = 0; x < _tiles.Length; x++) {
                if (_tiles[x][0] == (int)Tile.Empty) {
                    return (x, 0);
                }
            }

            throw new ArgumentException("Could not find start point for: \n" + ToString());
        }

        public (int, int) FindEndPoint() {
            for (var x = 0; x < _tiles.Length; x++) {
                if (_tiles[x][_tiles[x].Length - 1] == (int)Tile.Empty) {
                    return (x, _tiles[x].Length - 1);
                }
            }

            throw new ArgumentException("Could not find end point for: \n" + ToString());
        }
    }

    internal enum Tile {
        Empty = 0b00000,
        UpBlizzard = 0b00001,
        DownBlizzard = 0b00010,
        LeftBlizzard = 0b00100,
        RightBlizzard = 0b01000,
        Wall = 0b10000,
    }

    internal readonly BasinMap map;

    public BlizzardBasin(string[] lines) {
        map = new BasinMap(lines);
    }

    public int CalculateQuickestPath() {
        var start = map.FindStartPoint();
        var end = map.FindEndPoint();

        return SolveQuickestPath(start, end);
    }

    internal int SolveQuickestPath((int, int) start, (int, int) end) {
        IList<(int, int)> currentPoints = new List<(int, int)> {start};
        do {
            currentPoints = SolveNextMinute(currentPoints);
        } while (!currentPoints.Contains(end));

        return map.Minute;
    }

    private IList<(int, int)> SolveNextMinute(IEnumerable<(int, int)> currentPoints) {
        map.ExecuteNextMinute();
        return currentPoints.SelectMany((point, _) => map.FindAccessiblePoints(point.Item1, point.Item2)).Distinct()
            .ToList();
    }

    public int CalculateQuickestPathBackAndForth() {
        var start = map.FindStartPoint();
        var end = map.FindEndPoint();

        SolveQuickestPath(start, end);
        SolveQuickestPath(end, start);
        SolveQuickestPath(start, end);

        return map.Minute;
    }
}

internal static class TileExtensions {
    public static char ToChar(this BlizzardBasin.Tile tile) {
        return tile switch {
            BlizzardBasin.Tile.Empty => '.',
            BlizzardBasin.Tile.UpBlizzard => '^',
            BlizzardBasin.Tile.DownBlizzard => 'v',
            BlizzardBasin.Tile.LeftBlizzard => '<',
            BlizzardBasin.Tile.RightBlizzard => '>',
            BlizzardBasin.Tile.Wall => '#',
            _ => throw new ArgumentOutOfRangeException("Do not know Tile " + tile)
        };
    }

    public static bool IsAt(this BlizzardBasin.Tile tile, int tileAsInt) {
        return (tileAsInt & (int)tile) != 0;
    }

    public static bool IsMoveable(this int tile) {
        return tile != (int)BlizzardBasin.Tile.Empty && tile != (int)BlizzardBasin.Tile.Wall;
    }

    public static int GetXPlus(this BlizzardBasin.Tile tile) {
        return tile switch {
            BlizzardBasin.Tile.Empty => 0,
            BlizzardBasin.Tile.UpBlizzard => 0,
            BlizzardBasin.Tile.DownBlizzard => 0,
            BlizzardBasin.Tile.LeftBlizzard => -1,
            BlizzardBasin.Tile.RightBlizzard => +1,
            BlizzardBasin.Tile.Wall => 0,
            _ => throw new ArgumentOutOfRangeException("Do not know Tile " + tile)
        };
    }

    public static int GetYPlus(this BlizzardBasin.Tile tile) {
        return tile switch {
            BlizzardBasin.Tile.Empty => 0,
            BlizzardBasin.Tile.UpBlizzard => -1,
            BlizzardBasin.Tile.DownBlizzard => +1,
            BlizzardBasin.Tile.LeftBlizzard => 0,
            BlizzardBasin.Tile.RightBlizzard => 0,
            BlizzardBasin.Tile.Wall => 0,
            _ => throw new ArgumentOutOfRangeException("Do not know Tile " + tile)
        };
    }
}