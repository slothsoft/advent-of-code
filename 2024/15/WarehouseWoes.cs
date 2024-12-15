using System;
using System.Collections.Generic;
using System.Linq;

namespace AoC.day15;

/// <summary>
/// <a href="https://adventofcode.com/2024/day/15">Day 15: Warehouse Woes</a>
/// </summary>
public class WarehouseWoes {
    internal enum CanMoveResult {
        CanMoveInto,
        CanMove,
        CanNeverMove,
    }

    internal interface ITile {
        public char MapChar { get; }
        CanMoveResult CanMoveInto(ITile[][] map, int fromX, int fromY, Direction direction);
        (int ToX, int ToY) MoveInto(ITile[][] map, int fromX, int fromY, Direction direction);
    }
    
    internal record Wall : ITile {
        public virtual char MapChar => '#';
        public CanMoveResult CanMoveInto(ITile[][] map, int fromX, int fromY, Direction direction) => CanMoveResult.CanNeverMove;
        public (int ToX, int ToY) MoveInto(ITile[][] map, int fromX, int fromY, Direction direction) => throw new Exception("Cannot move wall!");
        public override string ToString() => "Wall";
    }
    
    internal record Box : ITile {
        public char MapChar => 'O';
        public CanMoveResult CanMoveInto(ITile[][] map, int fromX, int fromY, Direction direction) {
            var (newX, newY) = direction.Modify(fromX, fromY);
            return map[newX][newY].CanMoveInto(map, newX, newY, direction);
        }

        public (int ToX, int ToY) MoveInto(ITile[][] map, int fromX, int fromY, Direction direction) {
            var (newX, newY) = direction.Modify(fromX, fromY);
            var (resultX, resultY) = map[newX][newY].MoveInto(map, newX, newY, direction);
            map[newX][newY] = this;
            map[fromX][fromY] = TileNothing;
            return (resultX, resultY);
        }

        public override string ToString() => "Box";
    }
    
    internal record Nothing : ITile {
        public char MapChar => '.';
        public CanMoveResult CanMoveInto(ITile[][] map, int fromX, int fromY, Direction direction) => CanMoveResult.CanMoveInto;
        public (int ToX, int ToY) MoveInto(ITile[][] map, int fromX, int fromY, Direction direction) => (fromX, fromY);
        public override string ToString() => "Nothing";
    }
    
    internal record RogueRobot {
        public int X { get; set; }
        public int Y { get; set; }
        public char MapChar => '@';
        
        public bool CanMove(ITile[][] map, Direction direction) {
            var (newX, newY) = direction.Modify(X, Y);
            var targetTile = map[newX][newY];
            return targetTile.CanMoveInto(map, newX, newY, direction) != CanMoveResult.CanNeverMove;
        }

        public (int ToX, int ToY) Move(ITile[][] map, Direction direction) {
            var (newX, newY) = direction.Modify(X, Y);
            map[newX][newY].MoveInto(map, newX, newY, direction);
            (X, Y) = (newX, newY);
            return (X, Y);
        }
        
        public override string ToString() => "RogueRobot";
    }

    internal struct Direction(string name, Func<int, int> modifyX, Func<int, int> modifyY) {
        public (int X, int Y) Modify(int oldX, int oldY) => (
            modifyX(oldX), 
            modifyY(oldY)
        );
        public override string ToString() => name;
    }

    internal static IDictionary<char, Direction> AllDirections { get; } = new[] {
        new Direction("^", x => x, y => y - 1),
        new Direction("v", x => x, y => y + 1),
        new Direction("<", x => x - 1, y => y),
        new Direction(">", x => x + 1, y => y),
    }.ToDictionary(d => d.ToString()[0], d => d);
    
    private static readonly ITile TileNothing = new Nothing();
    private static readonly ITile TileBox = new Box();
    private static readonly ITile TileWall = new Wall();
    
    public WarehouseWoes(IEnumerable<string> input) {
        (Map, Robot, Movements) = ParseInput(input);
    }

    internal ITile[][] Map { get; }
    internal RogueRobot Robot { get; }
    internal Direction[] Movements { get; }

    internal static (ITile[][], RogueRobot, Direction[]) ParseInput(IEnumerable<string> input) {
        var inputAsArray = input.ToArray();
        var separatorLine = Array.IndexOf(inputAsArray, string.Empty);

        var tileRobot = new RogueRobot();
        var allTiles = new[] {TileBox, TileNothing, TileWall}.ToDictionary(t => t.MapChar, t => t);
        var map = inputAsArray.Take(separatorLine).ParseMatrix((x, y, c) => {
            if (c == tileRobot.MapChar) {
                // there can only be ONE!!!
                tileRobot.X = x;
                tileRobot.Y = y;
                return TileNothing;
            }
            return allTiles[c];
        });
        var movements = inputAsArray.Skip(separatorLine).SelectMany(i => i).Select(i => AllDirections[i]).ToArray();

        return (map, tileRobot, movements);
    }

    public long MoveAndCalculateGpsCoordsSum() {
        MoveAll();
        return CalculateGpsCoords().Sum();
    }

    public IEnumerable<long> CalculateGpsCoords() {
        for (var x = 0; x < Map.Length; x++) {
            for (var y = 0; y < Map.Length; y++) {
                if (Map[x][y] is Box) {
                    yield return 100 * y + x;
                }
            }
        }
    }
    
    internal void MoveAll() {
        foreach (var direction in Movements) {
            Move(direction);
        }
    }
    
    internal (int ToX, int ToY) Move(Direction direction) {
        if (Robot.CanMove(Map, direction)) {
            return Robot.Move(Map, direction);
        }
        return (Robot.X, Robot.Y);
    }
    
    public string Stringify() {
        return Map.StringifyMatrix((x, y, t) => Robot.X == x && Robot.Y == y ? Robot.MapChar : t.MapChar);
    }

}
