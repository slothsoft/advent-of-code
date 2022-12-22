using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;

namespace AoC._22;

/// <summary>
/// <a href="https://adventofcode.com/2022/day/22">Day 22: Monkey Map</a>
/// </summary>
public class MonkeyMap {
    public enum Field {
        BlankSpace = ' ',
        OpenTile = '.',
        SolidWall = '#',
    }

    public enum Facing {
        Right = 0,
        Down = 1,
        Left = 2,
        Up = 3,
    }

    public class Map {
        internal readonly IDictionary<string, Square> squares;
        private readonly int _mapWidth;
        private readonly int _mapHeight;
        private readonly int _squareSize;
        private readonly Square _emptySquare;

        internal int x = int.MaxValue;
        internal int y;
        internal Facing facing = Facing.Right;

        public Map(string[] lines, bool cube) {
            _mapHeight = lines.Length - 2;
            _mapWidth = lines.Take(lines.Length - 2).Max(l => l.Length);
            _squareSize = _mapWidth % 3 == 0 ? _mapWidth / 3 : _mapWidth / 4;
            _emptySquare = new Square(0, 0, _squareSize, Field.BlankSpace);

            // init squares
            squares = new Dictionary<string, Square>();
            for (var sx = 0; sx < _mapWidth; sx += _squareSize) {
                for (var sy = 0; sy < _mapHeight; sy += _squareSize) {
                    if (sx >= lines[sy].Length || lines[sy][sx] == (char)Field.BlankSpace) {
                        // this quadrant is empty
                    } else {
                        // the quadrant has a square
                        var quadrantX = sx / _squareSize;
                        var quadrantY = sy / _squareSize;
                        squares[CreateKey(quadrantX, quadrantY)] = new Square(quadrantX, quadrantY, _squareSize, lines);

                        if (sy == y) {
                            x = Math.Min(x, sx);
                        }
                    }
                }
            }

            // init connection between squares

            foreach (var square in squares.Values) {
                // these squares are directly next to each other
                var key = CreateKey(square.quadrantX - 1, square.quadrantY);
                if (squares.ContainsKey(key)) {
                    square.Left = new SimpleTranslation(squares[key]);
                    squares[key].Right = new SimpleTranslation(square);
                }

                key = CreateKey(square.quadrantX, square.quadrantY + 1);
                if (squares.ContainsKey(key)) {
                    square.Down = new SimpleTranslation(squares[key]);
                    squares[key].Up = new SimpleTranslation(square);
                }
            }

            if (cube) {
                ConnectCubeSquares();
            } else {
                ConnectFlatSquares();
            }
        }

        private void ConnectCubeSquares() {
            if (squares.ContainsKey(CreateKey(3, 2))) {
                ConnectExampleCubeSquares();
            } else {
                ConnectInputCubeSquares();
            }
        }

        private void ConnectExampleCubeSquares() {
            // ... ... 2-0 ...
            // 0-1 1-1 2-1 ...
            // ... ... 2-2 3-2
            squares[CreateKey(1, 1)].Down = new DownToRightTranslation(squares[CreateKey(2, 2)]);

            squares[CreateKey(2, 0)].Up = new UpToDownTranslation(squares[CreateKey(2, 2)]);

            squares[CreateKey(2, 1)].Right = new RightToDownTranslation(squares[CreateKey(3, 2)]);

            squares[CreateKey(2, 2)].Down = new DownToUpTranslation(squares[CreateKey(2, 0)]);
            squares[CreateKey(2, 2)].Left = new LeftToUpTranslation(squares[CreateKey(2, 2)]);

            squares[CreateKey(3, 2)].Up = new UpToLeftTranslation(squares[CreateKey(2, 1)]);
        }

        private void ConnectInputCubeSquares() {
            // ... 1-0 2-0
            // ... 1-1 ...
            // 0-2 1-2 ...
            // 0-3 ... ...
        }

        private void ConnectFlatSquares() {
            foreach (var square in squares.Values) {
                if (square.Right == null) {
                    for (var qx = 0; qx <= square.quadrantX; qx++) {
                        var key = CreateKey(qx, square.quadrantY);
                        if (squares.ContainsKey(key)) {
                            square.Right = new SimpleTranslation(squares[key]);
                            squares[key].Left = new SimpleTranslation(square);
                            break;
                        }
                    }
                }

                if (square.Down == null) {
                    for (var qy = 0; qy <= square.quadrantY; qy++) {
                        var key = CreateKey(square.quadrantX, qy);
                        if (squares.ContainsKey(key)) {
                            square.Down = new SimpleTranslation(squares[key]);
                            squares[key].Up = new SimpleTranslation(square);
                            break;
                        }
                    }
                }
            }
        }

        private static string CreateKey(int x, int y) {
            return x + "-" + y;
        }

        internal void MoveStepsForward(int steps) {
            for (var i = 0; i < steps; i++) {
                MoveStepForward();
            }
        }

        private void MoveStepForward() {
            var newX = GetNextStepX(x);
            var newY = GetNextStepY(y);
            MoveToTile(newX, newY);
        }

        private int GetNextStepX(int newX) => newX + facing.GetNextStepXPlus();
        private int GetNextStepY(int newY) => newY + facing.GetNextStepYPlus();

        private void MoveToTile(int newX, int newY) {
            var field = GetField(newX, newY);
            switch (field) {
                case Field.OpenTile: {
                    x = newX;
                    y = newY;
                    break;
                }
                case Field.SolidWall: {
                    break; // don't do nuthin
                }
                case Field.BlankSpace: {
                    // stepped from one square into an empty one
                    var quadrantX = x / _squareSize;
                    var quadrantY = y / _squareSize;

                    var (wrapX, wrapY, wrapFacing) = squares[CreateKey(quadrantX, quadrantY)].TranslateToNeighbor(newX, newY, facing);
                    MoveToTile(wrapX, wrapY, wrapFacing);
                    break;
                }
            }
        }

        private Field GetField(int newX, int newY) {
            var quadrantX = newX < 0 ? -1 : newX / _squareSize;
            var quadrantY = newY < 0 ? -1 : newY / _squareSize;
            return GetSquareAt(quadrantX, quadrantY).GetRelativeField(newX - (quadrantX * _squareSize), newY - (quadrantY * _squareSize));
        }

        private void MoveToTile(int newX, int newY, Facing newFacing) {
            MoveToTile(newX, newY);
            if (x == newX && y == newY) {
                facing = newFacing;
            }
        }

        internal void TurnRight() {
            TurnLeft();
            TurnLeft();
            TurnLeft();
        }

        internal void TurnLeft() {
            facing = facing.GetLeftTurn();
        }

        public override string ToString() {
            var result = "";
            for (var sy = 0; sy < _mapHeight; sy += _squareSize) {
                var array = new string[_squareSize];
                for (var sx = 0; sx < _mapWidth; sx += _squareSize) {
                    var quadrantX = sx / _squareSize;
                    var quadrantY = sy / _squareSize;
                    var other = GetSquareAt(quadrantX, quadrantY).Stringify(x - (quadrantX * _squareSize), y - (quadrantY * _squareSize), facing);
                    for (var i = 0; i < array.Length; i++) {
                        array[i] += other[i];
                    }
                }

                for (var i = 0; i < array.Length; i++) {
                    result += array[i];
                    result += "\r\n";
                }
            }

            return result;
        }

        private Square GetSquareAt(int quadrantX, int quadrantY) {
            var key = CreateKey(quadrantX, quadrantY);
            return squares.ContainsKey(key) ? squares[key] : _emptySquare;
        }

        public int CalculatePassword() {
            var row = y + 1;
            var col = x + 1;
            return (1000 * row) + (4 * col) + (int)facing;
        }

        internal void ChangePosition(int newX, int newY, Facing newFacing) {
            x = newX;
            y = newY;
            facing = newFacing;
        }
    }

    public class Square {
        internal readonly int quadrantX;
        internal readonly int quadrantY;
        internal readonly int size;
        private readonly Field[][] _fields;

        private Square(int quadrantX, int quadrantY, int size) {
            this.quadrantX = quadrantX;
            this.quadrantY = quadrantY;
            this.size = size;

            // init arrays
            _fields = new Field[this.size][];
            for (var x = 0; x < _fields.Length; x++) {
                _fields[x] = new Field[this.size];
            }
        }

        public Square(int quadrantX, int quadrantY, int size, Field field) : this(quadrantX, quadrantY, size) {
            // init fields
            for (var x = 0; x < _fields.Length; x++) {
                for (var y = 0; y < _fields[0].Length; y++) {
                    _fields[x][y] = field;
                }
            }
        }

        public Square(int quadrantX, int quadrantY, int size, string[] lines) : this(quadrantX, quadrantY, size) {
            // set field values
            for (var x = 0; x < _fields.Length; x++) {
                for (var y = 0; y < _fields[x].Length; y++) {
                    _fields[x][y] = (Field)lines[y + (quadrantY * size)][x + (quadrantX * size)];
                }
            }
        }

        internal ITranslation? Up { get; set; }
        internal ITranslation? Down { get; set; }
        internal ITranslation? Left { get; set; }
        internal ITranslation? Right { get; set; }

        internal Field GetRelativeField(int x, int y) {
            return _fields[x][y];
        }

        public string[] Stringify(int playerX, int playerY, Facing playerFacing) {
            var result = new string[_fields.Length];
            for (var y = 0; y < _fields[0].Length; y++) {
                result[y] = "";
                for (var x = 0; x < _fields.Length; x++) {
                    if (x == playerX && y == playerY) {
                        result[y] += playerFacing.GetChar();
                    } else {
                        result[y] += (char)_fields[x][y];
                    }
                }
            }

            return result;
        }

        public override string ToString() => string.Join("\n", Stringify(-1, -1, Facing.Right));

        internal (int, int, Facing) TranslateToNeighbor(int absoluteX, int absoluteY, Facing facing) {
            var relativeX = absoluteX - (quadrantX * size);
            var relativeY = absoluteY - (quadrantY * size);

            if (relativeX < 0) {
                Assert.NotNull(Left, $"Left neighbor of {quadrantX}|{quadrantY} not found!");
                return Left!.Translate(relativeX, relativeY, facing);
            }

            if (relativeY < 0) {
                Assert.NotNull(Up, $"Up neighbor of {quadrantX}|{quadrantY} not found!");
                return Up!.Translate(relativeX, relativeY, facing);
            }

            if (relativeX >= size) {
                Assert.NotNull(Right, $"Right neighbor of {quadrantX}|{quadrantY} not found!");
                return Right!.Translate(relativeX, relativeY, facing);
            }

            if (relativeY >= size) {
                Assert.NotNull(Down, $"Down neighbor of {quadrantX}|{quadrantY} not found!");
                return Down!.Translate(relativeX, relativeY, facing);
            }

            throw new ArgumentException($"{absoluteX},{absoluteY} (relative {relativeX},{relativeY}) was not out of bounds.");
        }
    }

    internal interface ITranslation {
        (int, int, Facing) Translate(int relativeX, int relativeY, Facing facing);
    }

    private class SimpleTranslation : ITranslation {
        private readonly Square _toSquare;
        public SimpleTranslation(Square toSquare) => _toSquare = toSquare;

        public (int, int, Facing) Translate(int relativeX, int relativeY, Facing facing) {
            return (
                (_toSquare.quadrantX * _toSquare.size) + ((relativeX + _toSquare.size) % _toSquare.size),
                (_toSquare.quadrantY * _toSquare.size) + ((relativeY + _toSquare.size) % _toSquare.size),
                facing
            );
        }

        public override string ToString() => $"-> {_toSquare.quadrantX}-{_toSquare.quadrantY}";
    }

    private class RightToDownTranslation : ITranslation {
        private readonly Square _toSquare;
        public RightToDownTranslation(Square toSquare) => _toSquare = toSquare;

        public (int, int, Facing) Translate(int relativeX, int relativeY, Facing facing) {
            return (
                (_toSquare.size * (_toSquare.quadrantX + 1)) - 1 - relativeY,
                _toSquare.size * _toSquare.quadrantY,
                Facing.Down
            );
        }

        public override string ToString() => $"-> {_toSquare.quadrantX}-{_toSquare.quadrantY}";
    }

    private class UpToLeftTranslation : ITranslation {
        private readonly Square _toSquare;
        public UpToLeftTranslation(Square toSquare) => _toSquare = toSquare;

        public (int, int, Facing) Translate(int relativeX, int relativeY, Facing facing) {
            return (
                (_toSquare.size * (_toSquare.quadrantX + 1)) - 1,
                (_toSquare.size * _toSquare.quadrantY) - 1 + relativeX,
                Facing.Left
            );
        }

        public override string ToString() => $"-> {_toSquare.quadrantX}-{_toSquare.quadrantY}";
    }

    private class UpToDownTranslation : ITranslation {
        private readonly Square _toSquare;
        public UpToDownTranslation(Square toSquare) => _toSquare = toSquare;

        public (int, int, Facing) Translate(int relativeX, int relativeY, Facing facing) {
            return (
                (_toSquare.size * _toSquare.quadrantX) + relativeX,
                (_toSquare.size * (_toSquare.quadrantY + 1)) - 1,
                Facing.Up
            );
        }

        public override string ToString() => $"-> {_toSquare.quadrantX}-{_toSquare.quadrantY}";
    }

    private class DownToUpTranslation : ITranslation {
        private readonly Square _toSquare;
        public DownToUpTranslation(Square toSquare) => _toSquare = toSquare;

        public (int, int, Facing) Translate(int relativeX, int relativeY, Facing facing) {
            return (
                (_toSquare.size * _toSquare.quadrantX) + relativeX,
                _toSquare.size * _toSquare.quadrantY,
                Facing.Up
            );
        }

        public override string ToString() => $"-> {_toSquare.quadrantX}-{_toSquare.quadrantY}";
    }

    private class DownToRightTranslation : ITranslation {
        private readonly Square _toSquare;
        public DownToRightTranslation(Square toSquare) => _toSquare = toSquare;

        public (int, int, Facing) Translate(int relativeX, int relativeY, Facing facing) {
            return (
                _toSquare.size * _toSquare.quadrantX,
                (_toSquare.size * (_toSquare.quadrantY + 1)) - relativeX,
                Facing.Right
            );
        }

        public override string ToString() => $"-> {_toSquare.quadrantX}-{_toSquare.quadrantY}";
    }

    private class LeftToUpTranslation : ITranslation {
        private readonly Square _toSquare;
        public LeftToUpTranslation(Square toSquare) => _toSquare = toSquare;

        public (int, int, Facing) Translate(int relativeX, int relativeY, Facing facing) {
            return (
                (_toSquare.size * (_toSquare.quadrantX + 1)) - 1 - relativeY,
                (_toSquare.size * (_toSquare.quadrantY + 1)) - 1,
                Facing.Down
            );
        }

        public override string ToString() => $"-> {_toSquare.quadrantX}-{_toSquare.quadrantY}";
    }

    internal readonly Map fields;
    private readonly string _defaultMovements;

    public MonkeyMap(string[] lines, bool cube = false) {
        fields = new Map(lines, cube);
        _defaultMovements = lines[^1];
    }

    public void Move(string? movements = null) {
        var usedMovements = movements ?? _defaultMovements;
        if (char.IsDigit(usedMovements[0])) {
            MoveStepsForward(usedMovements);
        } else {
            TurnAround(usedMovements);
        }
    }

    private void MoveStepsForward(string movements) {
        var numberAsString = movements.Split('L', 'R')[0];
        fields.MoveStepsForward(int.Parse(numberAsString));

        if (movements.Length > numberAsString.Length) {
            TurnAround(movements[numberAsString.Length..]);
        }
    }

    private void TurnAround(string movements) {
        if (movements[0] == 'L') {
            fields.TurnLeft();
        } else {
            fields.TurnRight();
        }

        if (movements.Length > 1) {
            MoveStepsForward(movements[1..]);
        }
    }

    public int CalculatePassword() {
        return fields.CalculatePassword();
    }
}

public static class FacingExtensions {
    public static char GetChar(this MonkeyMap.Facing facing) {
        return facing switch {
            MonkeyMap.Facing.Right => '>',
            MonkeyMap.Facing.Left => '<',
            MonkeyMap.Facing.Down => 'v',
            MonkeyMap.Facing.Up => '^',
            _ => throw new ArgumentOutOfRangeException("Do not know Facing " + facing)
        };
    }

    public static MonkeyMap.Facing GetLeftTurn(this MonkeyMap.Facing facing) {
        return facing switch {
            MonkeyMap.Facing.Right => MonkeyMap.Facing.Up,
            MonkeyMap.Facing.Left => MonkeyMap.Facing.Down,
            MonkeyMap.Facing.Down => MonkeyMap.Facing.Right,
            MonkeyMap.Facing.Up => MonkeyMap.Facing.Left,
            _ => throw new ArgumentOutOfRangeException("Do not know Facing " + facing)
        };
    }

    public static int GetNextStepXPlus(this MonkeyMap.Facing facing) {
        return facing switch {
            MonkeyMap.Facing.Right => 1,
            MonkeyMap.Facing.Left => -1,
            MonkeyMap.Facing.Down => 0,
            MonkeyMap.Facing.Up => 0,
            _ => throw new ArgumentOutOfRangeException("Do not know Facing " + facing)
        };
    }

    public static int GetNextStepYPlus(this MonkeyMap.Facing facing) {
        return facing switch {
            MonkeyMap.Facing.Right => 0,
            MonkeyMap.Facing.Left => 0,
            MonkeyMap.Facing.Down => 1,
            MonkeyMap.Facing.Up => -1,
            _ => throw new ArgumentOutOfRangeException("Do not know Facing " + facing)
        };
    }
}