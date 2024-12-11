using System;
using System.Collections.Generic;

namespace AoC.day10;

/// <summary>
/// <a href="https://adventofcode.com/2024/day/10">Day 10: Hoof It</a>
/// </summary>
public class HoofIt {
    internal struct Direction(string name, Func<int, int> modifyX, Func<int, int> modifyY) {
        public (int X, int Y) Modify(int oldX, int oldY) => (
            modifyX(oldX), 
            modifyY(oldY)
        );
        public override string ToString() => name;
    }
    
    public HoofIt(IEnumerable<string> input) {
        Input = input.ParseMatrix(c => c == '.' ? -1 : int.Parse(c.ToString()));
        AllDirections = [
            new Direction("North", x => x, y => y - 1),
            new Direction("South", x => x, y => y + 1),
            new Direction("West", x => x - 1, y => y),
            new Direction("East", x => x + 1, y => y),
        ];
    }

    internal int[][] Input { get; }
    internal Direction[]AllDirections { get; }

    public long CalculateTrailheadScore() {
        var result = 0L;
        var alreadyCounted = new HashSet<(int X, int Y)>();
        
        for (var x = 0; x < Input.Length; x++) {
            for (var y = 0; y < Input[x].Length; y++) {
                if (Input[x][y] == 0) {
                    alreadyCounted.Clear();
                    result += CalculateTrailheadScore(x, y, alreadyCounted);
                }
            }
        }

        return result;
    }
    
    internal long CalculateTrailheadScore(int x, int y, ISet<(int X, int Y)> alreadyCounted, int currentValue = 0) {
        var result = 0L;
        foreach (var direction in AllDirections) {
            var newPoint = direction.Modify(x, y);
            if (IsPointOnMap(newPoint.X, newPoint.Y) && Input[newPoint.X][newPoint.Y] == currentValue + 1) {
                if (currentValue + 1 == 9) {
                    // if (!alreadyCounted.Contains(newPoint)) {
                        // last point of trail, so return 1
                        result += 1;
                        alreadyCounted.Add(newPoint);
                    // }
                } else {
                    // middle point of the trail, so check all surrounding points
                    result += CalculateTrailheadScore(newPoint.X, newPoint.Y, alreadyCounted, currentValue + 1);
                }
            }
        }
        return result;
    }
    
    private bool IsPointOnMap(int x, int y) => IsCoordOnMap(x, Input.Length) && IsCoordOnMap(y, Input[0].Length);
    private static bool IsCoordOnMap(int coord, int length) => coord >= 0 && coord < length;
}
