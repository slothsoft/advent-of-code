using System;
using System.Linq;

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
        private readonly Field[][] _fields;
        private readonly int _cubeSize;
        private readonly bool _cube;

        internal int X = int.MaxValue;
        internal int Y;
        internal Facing Facing = Facing.Right;

        public Map(string[] lines, bool cube) {
            _cube = cube;

            var maxY = lines.Length - 2;
            var maxX = lines.Take(lines.Length - 2).Max(l => l.Length);

            // init arrays
            _fields = new Field[maxX][];
            for (var x = 0; x < _fields.Length; x++) {
                _fields[x] = new Field[maxY];
            }

            // set field values
            for (var y = 0; y < lines.Length - 2; y++) {
                for (var x = 0; x < lines[y].Length; x++) {
                    _fields[x][y] = (Field) lines[y][x];

                    if (y == Y && _fields[x][y] == Field.OpenTile) {
                        X = Math.Min(X, x);
                    }
                }
            }

            // init the rest of the fields (that are 0 right now)
            for (var x = 0; x < _fields.Length; x++) {
                for (var y = 0; y < _fields[x].Length; y++) {
                    if (_fields[x][y] == 0) {
                        _fields[x][y] = Field.BlankSpace;
                    }
                }
            }
            
            _cubeSize = _fields.Length / 4;
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

            var newX = GetNextStepX(X);
            var newY = GetNextStepY(Y);
            MoveToTile(newX, newY);
        }

        private int GetNextStepX(int x) => (x + Facing.GetNextStepXPlus() + _fields.Length) % _fields.Length;
        private int GetNextStepY(int y) => (y + Facing.GetNextStepYPlus() + _fields[0].Length) % _fields[0].Length;

        private void MoveToTile(int newX, int newY) {
            var field = _fields[newX][newY];
            switch (field) {
                case Field.OpenTile: {
                    X = newX;
                    Y = newY;
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
            var newX = X + Facing.GetNextStepXPlus();
            var newY = Y + Facing.GetNextStepYPlus();

            // If the cube looks like this
            // ..#.  
            // ###.
            // ..##
            // we need to know on which quadrant we land on to decide what to do
            // possibilities: x = -1..4, y=-1..3 (30 possibilities, but some like -1|-1 and 4|3 are not possible)
            // a cube has 12 edges, so 12 quadrants should be special, and 6 where we don't do anything

            var quadrantX = newX / _cubeSize;
            var quadrantY = newY / _cubeSize;

            if ((quadrantX == 0 && quadrantY == 1) || (quadrantX == 1 && quadrantY == 1) || (quadrantX == 2 && quadrantY == 1) ||
                (quadrantX == 2 && quadrantY == 0) || (quadrantX == 2 && quadrantY == 2) || (quadrantX == 3 && quadrantY == 2)) {
                // these are the faces of the cube that are visible and readily moveable
                MoveToTile(newX, newY);
                return;
            } 
            if (quadrantX == 0 && quadrantY == 0) {
                // the top left empty space
                
            } 
            if (quadrantX == 1 && quadrantY == 0) {
                // the middle top empty space - connects down to right
                if (Facing == Facing.Up) {
                    var wrapX = _cubeSize * 2;
                    var wrapY = newX - _cubeSize;
                    MoveToTile(wrapX, wrapY, Facing.Right);
                    return;
                } 
                if (Facing == Facing.Left) {
                    var wrapX = newY + _cubeSize;
                    var wrapY = _cubeSize;
                    MoveToTile(wrapX, wrapY, Facing.Down);
                    return;
                }
            }
            if (quadrantX == 3 && quadrantY == 1) {
                // the middle right empty space - connects left to down
                if (Facing == Facing.Right) {
                    var wrapX = _cubeSize * 4 - 1 - (newY - _cubeSize);
                    var wrapY = _cubeSize * 2;
                    MoveToTile(wrapX, wrapY, Facing.Down);
                    return;
                } 
                if (Facing == Facing.Up) {
                    var wrapX = _cubeSize * 3 - 1;
                    var wrapY = _cubeSize  + (_cubeSize * 4 - 1 - newX);
                    MoveToTile(wrapX, wrapY, Facing.Left);
                    return;
                }
            }
            if (quadrantX == 2 && quadrantY == 3) {
                // the space below the left bottom rectangle - connects up to the rectangle on the far left
                if (Facing == Facing.Down) {
                    var wrapX = _cubeSize - 1 - (newX - 2 * _cubeSize);
                    var wrapY = _cubeSize * 2 - 1;
                    MoveToTile(wrapX, wrapY, Facing.Up);
                    return;
                }
            }
            if (quadrantX == 0 && quadrantY == 2) {
                // the space below the left most rectangle - connects up to the rectangle on the bottom left
                if (Facing == Facing.Down) {
                    var wrapX = 3 * _cubeSize - newX - 1;
                    var wrapY = _cubeSize * 3 - 1;
                    MoveToTile(wrapX, wrapY, Facing.Up);
                    return;
                }
            }

            throw new ArgumentException($"Quadrant {quadrantX}|{quadrantY} with facing {Facing} is not implemented");
        }

        private void MoveToTile(int newX, int newY, Facing newFacing) {
            MoveToTile(newX, newY);
            if (X == newX && Y == newY) {
                Facing = newFacing;
            }
        }

        internal void TurnRight() {
            TurnLeft();
            TurnLeft();
            TurnLeft();
        }

        internal void TurnLeft() {
            Facing = Facing.GetLeftTurn();
        }

        public override string ToString() {
            var result = "";
            for (var y = 0; y < _fields[0].Length; y++) {
                for (var x = 0; x < _fields.Length; x++) {
                    if (x == X && y == Y) {
                        result += Facing.GetChar();
                    } else {
                        result += (char) _fields[x][y];
                    }
                }

                result += "\r\n";
            }

            return result;
        }

        public int CalculatePassword() {
            var row = Y + 1;
            var col = X + 1;
            return 1000 * row + 4 * col + (int) Facing;
        }

        internal void ChangePosition(int newX, int newY, Facing facing) {
            X = newX;
            Y = newY;
            Facing = facing;
        }
    }

    internal readonly Map Fields;
    private readonly string _defaultMovements;

    public MonkeyMap(string[] lines, bool cube = false) {
        Fields = new Map(lines, cube);
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
        Fields.MoveStepsForward(int.Parse(numberAsString));

        if (movements.Length > numberAsString.Length) {
            TurnAround(movements[numberAsString.Length..]);
        }
    }

    private void TurnAround(string movements) {
        if (movements[0] == 'L') {
            Fields.TurnLeft();
        } else {
            Fields.TurnRight();
        }

        if (movements.Length > 1) {
            MoveStepsForward(movements[1..]);
        }
    }

    public int CalculatePassword() {
        return Fields.CalculatePassword();
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