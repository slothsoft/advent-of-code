using System;
using System.Linq;

namespace AoC._22;

/// <summary>
/// <a href="https://adventofcode.com/2022/day/22">Day 22: Monkey Map</a>
/// </summary>
public class MonkeyMap {
    private enum Field {
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
        private readonly Field[][] _fields;
        private readonly int _squareSize;
        private readonly bool _cube;

        internal int x = int.MaxValue;
        internal int y;
        internal Facing facing = Facing.Right;

        private readonly int _mapWidth;
        private readonly int _mapHeight;

        public Map(string[] lines, bool cube) {
            _cube = cube;

            _mapHeight = lines.Length - 2;
            _mapWidth = lines.Take(_mapHeight).Max(l => l.Length);
            _squareSize = _mapHeight % 3 == 0 ? _mapHeight / 3 : _mapHeight / 4;

            // init arrays
            _fields = new Field[_mapWidth][];
            for (var ix = 0; ix < _fields.Length; ix++) {
                _fields[ix] = new Field[_mapHeight];
            }

            // set field values
            for (var iy = 0; iy < lines.Length - 2; iy++) {
                for (var ix = 0; ix < lines[iy].Length; ix++) {
                    _fields[ix][iy] = (Field)lines[iy][ix];

                    if (iy == y && _fields[ix][iy] == Field.OpenTile) {
                        x = Math.Min(x, ix);
                    }
                }
            }

            // init the rest of the fields (that are 0 right now)
            for (var ix = 0; ix < _fields.Length; ix++) {
                for (var iy = 0; iy < _fields[ix].Length; iy++) {
                    if (_fields[ix][iy] == 0) {
                        _fields[ix][iy] = Field.BlankSpace;
                    }
                }
            }
        }

        internal void MoveStepsForward(int steps) {
            for (var i = 0; i < steps; i++) {
                MoveStepForward();
            }
        }

        private void MoveStepForward() {
            if (_cube) {
                MoveStepForwardOnCube();
                return;
            }

            var newX = GetNextStepX(x);
            var newY = GetNextStepY(y);
            MoveToTile(newX, newY);
        }

        private int GetNextStepX(int originX) => (originX + facing.GetNextStepXPlus() + _fields.Length) % _fields.Length;
        private int GetNextStepY(int originY) => (originY + facing.GetNextStepYPlus() + _fields[0].Length) % _fields[0].Length;

        private void MoveToTile(int newX, int newY) {
            var field = _fields[newX][newY];
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
                    // wrap around
                    do {
                        newX = GetNextStepX(newX);
                        newY = GetNextStepY(newY);
                    } while (_fields[newX][newY] == Field.BlankSpace);

                    MoveToTile(newX, newY);
                    break;
                }
            }
        }

        private void MoveStepForwardOnCube() {
            var newX = x + facing.GetNextStepXPlus();
            var newY = y + facing.GetNextStepYPlus();

            // If the cube looks like this
            // ..#.       .##
            // ###.       .#
            // ..##       ##
            //            #
            // we need to know on which quadrant we land on to decide what to do
            // possibilities: x = -1..4, y=-1..3 (30 possibilities, but some like -1|-1 and 4|3 are not possible)
            // a cube has 12 edges, so 12 quadrants should be special, and 6 where we don't do anything

            var quadrantX = newX < 0 ? -1 : newX / _squareSize;
            var quadrantY = newY < 0 ? -1 : newY / _squareSize;

            if (newX >= 0 && newX < _mapWidth && newY >= 0 && newY < _mapHeight && _fields[newX][newY] != Field.BlankSpace) {
                // these are the faces of the cube that are visible and readily moveable
                MoveToTile(newX, newY);
                return;
            }

            if (newX < 0) {
                switch (quadrantY) {
                    case 1: {
                        // example - left outside to bottom right
                        var wrapX = (4 * _squareSize) - 1 - (newY % _squareSize);
                        var wrapY = (3 * _squareSize) - 1;
                        MoveToTile(wrapX, wrapY, Facing.Up);
                        return;
                    }
                    case 2: {
                        // input - left outside to top
                        var wrapX = _squareSize;
                        var wrapY = _squareSize - 1 - (newY % _squareSize);
                        MoveToTile(wrapX, wrapY, Facing.Right);
                        return;
                    }
                    case 3: {
                        // input - left outside to very top 
                        var wrapX = _squareSize + (newY % _squareSize);
                        var wrapY = 0;
                        MoveToTile(wrapX, wrapY, Facing.Down);
                        return;
                    }
                }

                throw new ArgumentException($"Quadrant {quadrantX}|{quadrantY} with facing {facing} is not implemented");
            }

            if (newX >= _mapWidth) {
                switch (quadrantY) {
                    case 0: {
                        // input - top right to middle
                        var wrapX = newX - _squareSize - 1;
                        var wrapY = (3 * _squareSize) - (newY % _squareSize) - 1;
                        MoveToTile(wrapX, wrapY, Facing.Left);
                        return;
                    }
                    case 2: {
                        // example - bottom left to top
                        var wrapX = newX - _squareSize - 1;
                        var wrapY = _squareSize - (newY % _squareSize) - 1;
                        MoveToTile(wrapX, wrapY, Facing.Left);
                        return;
                    }
                }

                throw new ArgumentException($"Quadrant {quadrantX}|{quadrantY} with facing {facing} is not implemented");
            }

            if (newY < 0) {
                switch (quadrantX) {
                    case 1: {
                        // top left to left / bottomest bottom left
                        var wrapX = 0;
                        var wrapY = (3 * _squareSize) + (newX % _squareSize);
                        MoveToTile(wrapX, wrapY, Facing.Right);
                        return;
                    }
                    case 2: {
                        if (_squareSize == 4) {
                            // top to left
                            var wrapX = _squareSize - (newX % _squareSize) - 1;
                            var wrapY = _squareSize;
                            MoveToTile(wrapX, wrapY, Facing.Down);
                        } else {
                            // top to  bottomest bottom
                            var wrapX = newX % _squareSize;
                            var wrapY = (4 * _squareSize) - 1;
                            MoveToTile(wrapX, wrapY, Facing.Up);
                        }
                        return;
                    }
                }

                throw new ArgumentException($"Quadrant {quadrantX}|{quadrantY} with facing {facing} is not implemented");
            }

            if (newY >= _mapHeight) {
                if (quadrantX == 0 && quadrantY == 4) {
                    // bottomest bottom to top right
                    var wrapX = (2 * _squareSize) + (newX % _squareSize);
                    const int wrapY = 0;
                    MoveToTile(wrapX, wrapY, Facing.Down);
                    return;
                }
                if (quadrantX == 2 && quadrantY == 3) {
                    // middle bottom to left
                    var wrapX = _squareSize - (newX % _squareSize) - 1;
                    var wrapY = newY - _squareSize - 1;
                    MoveToTile(wrapX, wrapY, Facing.Up);
                    return;
                }
                if (quadrantX == 3 && quadrantY == 3) {
                    // bottom right to middle left
                    var wrapX = 0;
                    var wrapY = (2 * _squareSize) - (newX % _squareSize) - 1;
                    MoveToTile(wrapX, wrapY, Facing.Right);
                    return;
                }
                throw new ArgumentException($"Quadrant {quadrantX}|{quadrantY} with facing {facing} is not implemented");
            }

            if (facing == Facing.Left && newY + _squareSize < _mapHeight && _fields[newX][newY + _squareSize] != Field.BlankSpace) {
                // right -> bottom
                var wrapX = (newX / _squareSize * _squareSize) + (newY % _squareSize);
                var wrapY = ((newY / _squareSize) + 1) * _squareSize;
                MoveToTile(wrapX, wrapY, Facing.Down);
                return;
            }

            if (facing == Facing.Left && newY - _squareSize >= 0 && _fields[newX][newY - _squareSize] != Field.BlankSpace) {
                // right -> up
                var wrapX = (((newX / _squareSize) + 1) * _squareSize) - (newY % _squareSize) - 1;
                var wrapY = (newY / _squareSize * _squareSize) - 1;
                MoveToTile(wrapX, wrapY, Facing.Up);
                return;
            }

            if (facing == Facing.Right && newY + _squareSize < _mapHeight && _fields[newX][newY + _squareSize] != Field.BlankSpace) {
                // right -> bottom
                var wrapX = (((newX / _squareSize) + 1) * _squareSize) - (newY % _squareSize) - 1;
                var wrapY = ((newY / _squareSize) + 1) * _squareSize;
                MoveToTile(wrapX, wrapY, Facing.Down);
                return;
            }

            if (facing == Facing.Right && newY - _squareSize >= 0 && _fields[newX][newY - _squareSize] != Field.BlankSpace) {
                // right -> up
                var wrapX = (newX / _squareSize * _squareSize) + (newY % _squareSize);
                var wrapY = (newY / _squareSize * _squareSize) - 1;
                MoveToTile(wrapX, wrapY, Facing.Up);
                return;
            }

            if (facing == Facing.Down && newX + _squareSize < _mapWidth && _fields[newX + _squareSize][newY] != Field.BlankSpace) {
                // down -> right
                var wrapX = ((newX / _squareSize) + 1) * _squareSize;
                var wrapY = (newY / _squareSize * _squareSize) + (newX % _squareSize) - 1;
                MoveToTile(wrapX, wrapY, Facing.Right);
                return;
            }

            if (facing == Facing.Down && newX - _squareSize >= 0 && _fields[newX - _squareSize][newY] != Field.BlankSpace) {
                // down -> left
                var wrapX = (newX / _squareSize * _squareSize) - 1;
                var wrapY = (newY / _squareSize * _squareSize) + (newX % _squareSize);
                MoveToTile(wrapX, wrapY, Facing.Left);
                return;
            }

            if (facing == Facing.Up && newX + _squareSize < _mapWidth && _fields[newX + _squareSize][newY] != Field.BlankSpace) {
                // up -> right
                var wrapX = ((newX / _squareSize) + 1) * _squareSize;
                var wrapY = (newY / _squareSize * _squareSize) + (newX % _squareSize);
                MoveToTile(wrapX, wrapY, Facing.Right);
                return;
            }

            if (facing == Facing.Up && newX - _squareSize >= 0 && _fields[newX - _squareSize][newY] != Field.BlankSpace) {
                // up -> left
                var wrapX = (newX / _squareSize * _squareSize) - 1;
                var wrapY = (((newY / _squareSize) + 1) * _squareSize) - (newX % _squareSize) - 1;
                MoveToTile(wrapX, wrapY, Facing.Left);
                return;
            }

            if (quadrantX == 3 && quadrantY == 0) {
                // top to bottom right
                var wrapX = _squareSize == 4
                    ? newX + _squareSize - 1
                    : newX - _squareSize - 1;
                var wrapY = (3 * _squareSize) - (newY % _squareSize) - 1;
                MoveToTile(wrapX, wrapY, Facing.Left);
                return;
            }
            
            if (quadrantX == 0 && quadrantY == 0) {
                if (_squareSize == 4) {
                    // top left to top
                    var wrapX = (3 * _squareSize) - (newX % _squareSize) - 1;
                    var wrapY = 0;
                    MoveToTile(wrapX, wrapY, Facing.Down);
                } else {
                    // top left to middle left
                    var wrapX = 0;
                    var wrapY = (3 * _squareSize) - (newY % _squareSize) - 1;
                    MoveToTile(wrapX, wrapY, Facing.Right);
                }
                return;
            }
            
            if (quadrantX == 0 && quadrantY == 2) {
                // left to middle bottom
                var wrapX = (3 * _squareSize) - (newX % _squareSize) - 1;
                var wrapY = _squareSize + newY - 1;
                MoveToTile(wrapX, wrapY, Facing.Up);
                return;
            }
            
            if (quadrantX == 2 && quadrantY == 2) {
                // middle to top left's left
                var wrapX = (3 * _squareSize) - 1;
                var wrapY = _squareSize - (newY % _squareSize) - 1;
                MoveToTile(wrapX, wrapY, Facing.Left);
                return;
            }

            throw new ArgumentException($"Quadrant {quadrantX}|{quadrantY} with facing {facing} is not implemented");
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
            for (var iy = 0; iy < _fields[0].Length; iy++) {
                for (var ix = 0; ix < _fields.Length; ix++) {
                    if (ix == x && iy == y) {
                        result += facing.GetChar();
                    } else {
                        result += (char)_fields[ix][iy];
                    }
                }

                result += "\r\n";
            }

            return result;
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