using System;
using System.Collections.Generic;
using System.Linq;

namespace AoC._17;

/// <summary>
/// Extraction of the actual tetris logic so the puzzle algorithm is nicer.
/// </summary>
public class Tetris : BlockArray {
    public Stone? Stone { get; private set; }

    public Tetris(int width, int initialHeight) : base(width, initialHeight) {
    }

    public int HighestBlockY => Blocks
        .Select((b, i) => new {Blocks = b, Index = i})
        .Where(o => o.Blocks.Any(b => b))
        .Select(o => o.Index)
        .DefaultIfEmpty(Blocks.Count)
        .Min();

    public void CreateStone(string stone, int x, int y) {
        Stone = new Stone(stone) {
            X = x,
            Y = y,
        };
    }

    public string Stringify() {
        var result = "";
        var minY = Math.Min(Stone?.Y ?? HighestBlockY, HighestBlockY);
        for (var y = minY; y < Blocks.Count; y++) {
            result += '|';
            for (var x = 0; x < Width; x++) {
                if ((Stone?.Contains(x - Stone.X, y - Stone.Y) ?? false) && Stone.IsSolid(x - Stone.X, y - Stone.Y)) {
                    result += '@';
                } else if (Contains(x, y)) {
                    result += (y >= 0 && Blocks[y][x]) ? '#' : '.';
                } else {
                    result += '.';
                }
            }

            result += '|';
            result += "\r\n";
        }

        result += '+';
        for (var x = 0; x < Blocks[0].Length; x++) {
            result += '-';
        }

        result += '+';

        return result;
    }

    public void MoveStoneRight() {
        if (Stone == null) {
            return;
        }

        if (Stone.X + Stone.Width < Width) {
            MoveStoneToPosition(Stone.X + 1, Stone.Y);
        }
    }

    public void MoveStoneLeft() {
        if (Stone == null) {
            return;
        }

        if (Stone.X > 0) {
            MoveStoneToPosition(Stone.X - 1, Stone.Y);
        }
    }

    public bool MoveStoneDown() {
        if (Stone == null) {
            return false;
        }

        if (Stone.Y + Stone.Height < Height && MoveStoneToPosition(Stone.X, Stone.Y + 1)) {
            // successful movement
            return true;
        }
        // stone cannot be moved down, so set it on the board
        MakeStonePermanent();
        return false;
    }

    private void MakeStonePermanent() {
        if (Stone == null) {
            return;
        }
        for (var y = 0; y < Stone.Height; y++) {
            for (var x = 0; x < Stone.Width; x++) {
                if (Stone.IsSolid(x, y)) {
                    Blocks[y + Stone.Y][x + Stone.X] = true;
                }
            }
        }

        Stone = null;
    }

    private bool MoveStoneToPosition(int newStoneX, int newStoneY) {
        if (Stone == null) {
            return false;
        }

        for (var y = 0; y < Stone.Height; y++) {
            for (var x = 0; x < Stone.Width; x++) {
                if (Stone.IsSolid(x, y) && Contains(newStoneX + x, newStoneY + y) && IsSolid(newStoneX + x, newStoneY + y)) {
                    // a solid object of the stone collides with a solid object of the board
                    return false;
                }
            }
        }

        Stone.X = newStoneX;
        Stone.Y = newStoneY;
        return true;
    }

    public void AddRowAtTop() {
        Blocks.Insert(0, new bool[Width]);
    }
}

public class Stone : BlockArray {
    private const char LineBreak = '\n';

    internal Stone(string stoneAsString) : base(stoneAsString.Split(LineBreak)[0].Length, stoneAsString.Split(LineBreak).Length) {
        var stoneSplit = stoneAsString.Split(LineBreak);
        for (var y = 0; y < Blocks.Count; y++) {
            for (var x = 0; x < Width; x++) {
                Blocks[y][x] = stoneSplit[y][x] == '#';
            }
        }
    }

    public int X { get; internal set; }
    public int Y { get; internal set; }
}

public class BlockArray {
    internal readonly IList<bool[]> Blocks;

    internal BlockArray(int width, int initialHeight) {
        Width = width;

        Blocks = new List<bool[]>();
        for (var i = 0; i < initialHeight; i++) {
            Blocks.Add(new bool[width]);
        }
    }

    public int Width { get; }
    public int Height => Blocks.Count;

    public bool Contains(int x, int y) {
        return x >= 0 && y >= 0 && y < Blocks.Count && x < Blocks[y].Length;
    }

    public bool IsSolid(int x, int y) {
        return Blocks[y][x];
    }
}