using System.Collections.Generic;
using System.Linq;

namespace AoC.day4;

/// <summary>
/// <a href="https://adventofcode.com/2025/day/4">Day 4: Printing Department</a>
/// </summary>
public class PrintingDepartment {
    public PrintingDepartment(IEnumerable<string> input) {
        Input = input.ParseBoolMatrix('@', '.');
    }

    internal bool[][] Input { get; }

    public int CalculateAccessibleRollsCount() {
        return CalculateAccessibleRollsCoords().Count();
    }
    
    private IEnumerable<(int X, int Y)> CalculateAccessibleRollsCoords() {
        for (var x = 0; x < Input.Length; x++) {
            for (var y = 0; y < Input[x].Length; y++) {
                // if we have no roll of paper, we don't need to check anything else
                if (!Input[x][y]) continue;
                
                if (FindSurroundingCoords(x, y).Count(p => Input[p.X][p.Y]) < 4) {
                    yield return (x, y);
                }
            }
        }
    }

    private IEnumerable<(int X, int Y)> FindSurroundingCoords(int x, int y) {
        for (var xPlus = -1; xPlus <= 1; xPlus++) {
            for (var yPlus = -1; yPlus <= 1; yPlus++) {
                // this is the point x | y
                if (xPlus == 0 && yPlus == 0) continue;
                
                var resultX = x + xPlus;
                if (resultX < 0 || resultX >= Input.Length) continue;
                
                var resultY = y + yPlus;
                if (resultY < 0 || resultY >= Input[resultX].Length) continue;

                yield return (resultX, resultY);
            }
        }
    }
    
    public long CalculateRemoveableRollsCount() {
        var result = 0L;
        int lastCount;
        
        do {
            var accessible = CalculateAccessibleRollsCoords().ToArray();
            lastCount = accessible.Length;
            result += lastCount;
            
            foreach (var point in accessible) {
                Input[point.X][point.Y] = false;
            }
        } while (lastCount > 0);

        return result;
    }
}