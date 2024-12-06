using System;
using System.Collections.Generic;
using System.Linq;
using AoC.day4;

namespace AoC.day6;

/// <summary>
/// <a href="https://adventofcode.com/2024/day/6">Day 6: Guard Gallivant</a>
/// </summary>
public class GuardGallivant {
    internal struct Direction(string name, Func<int, int> modifyX, Func<int, int> modifyY) {
        public (int X, int Y) Modify(int oldX, int oldY) => (
            modifyX(oldX), 
            modifyY(oldY)
        );
        public override string ToString() => name;
    }

    private static readonly Direction North = new Direction("North", x => x, y => y - 1);
    private static readonly Direction South = new Direction("South", x => x, y => y + 1);
    private static readonly Direction West = new Direction( "West", x => x - 1, y => y);
    private static readonly Direction East = new Direction("East", x => x + 1, y => y); 
    private static readonly Direction[] TurnRightDirections = [North, East, South, West]; 

    public GuardGallivant(IEnumerable<string> input) {
        (Map, StartPosition) = ParseInput(input);
    }

    internal bool[][] Map { get; }
    internal (int X, int Y) StartPosition { get; }
    internal Direction StartDirection { get; } = North;

    internal static (bool[][], (int, int)) ParseInput(IEnumerable<string> input) {
        var inputAsArray = input.ToArray();
        var map = inputAsArray.ParseMatrix(c => {
            return c switch {
                '#' => true,
                '.' => false,
                '^' => false,
                _ => throw new ArgumentException("Cannot parse: " + c),
            };
        });
        (int X, int Y) startPosition = (-1, -1);
        for (var y = 0; y < map.Length; y++) {
            var index = inputAsArray[y].IndexOf('^');
            if (index >= 0) {
                startPosition = (index, y);
            }
        }
        return (map, startPosition);
    }

    public long CalculateDistinctPositions() {
        var positions = new HashSet<(int, int)>();
         WalkTheGuard(coords => {
            positions.Add((coords.x, coords.y));
            return /* continue */ true;
         });
         return positions.Count;
    }
    
    private void WalkTheGuard(Func<(int x, int y, Direction direction), bool> doOnPosition, (int x, int y)? additionalObstruction = null) {
        var (currentX, currentY) = StartPosition;
        var currentDirection = StartDirection;
        
        // while the guard is still on the map
        while (IsOnMap(currentX, currentY)) {
            // we are on a specific map position: remember that
            if (!doOnPosition((currentX, currentY, currentDirection))) {
                break;
            }
            
            // try to figure out where to go next
            var (nextX, nextY) = currentDirection.Modify(currentX, currentY);
            if (!IsOnMap(nextX, nextY)) {
                // guard will step of map, so break and continue does the same
                break;
            }
            
            if (Map[nextX][nextY] || (additionalObstruction != null && additionalObstruction.Value.x == nextX && additionalObstruction.Value.y == nextY)) {
                // guard would run into an object, so we rotate instead 
                var index = Array.IndexOf(TurnRightDirections, currentDirection);
                currentDirection = TurnRightDirections[(index + 1) % TurnRightDirections.Length];
            } else {
                // everything is okay with that position
                (currentX, currentY) = (nextX, nextY);
            }
        }
    }

    private bool IsOnMap(int x, int y) {
        return x >= 0 && x < Map.Length && y >= 0 && y < Map[0].Length;
    }
    
    public long CalculateLoopDeLoopsCount() {
        // BRUTE FORCE AAAARG!!!
        var result = 0L;
        for (var x = 0; x < Map.Length; x++) {
            for (var y = 0; y < Map[0].Length; y++) {
                if (ContainsLoop((x, y))) {
                    Console.WriteLine($"{x}, {y}");
                    result++;
                }
            }
        }
        return result;
    }
    
    internal bool ContainsLoop((int x, int y) additionalObstruction) {
        var positions = new HashSet<(int, int, Direction)>();
        var containsLoop = false;
        WalkTheGuard(coords => {
            if (!positions.Add(coords)) {
                // we have a loop!
                containsLoop = true;
                return /* continue */ false;
            }
            return /* continue */ true;
        }, additionalObstruction);
        return containsLoop;
    }
}
