using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace AoC;

/// <summary>
/// <a href="https://adventofcode.com/2020/day/3">Day 3: Toboggan Trajectory</a>: Starting at the top-left corner of your map and following a slope of
/// right 3 and down 1, how many trees would you encounter?
/// </summary>
public class TobogganTrajectory {
    public bool[][] TreeMatrix { get; }

    public TobogganTrajectory(IEnumerable<string> lines) {
        TreeMatrix = lines.ParseBoolMatrix('#', '.');
    }

    public int CountTreesForSlope(int xPlus, int yPlus) {
        var result = 0;
        var x = xPlus;
        var y = yPlus;
        while (y < TreeMatrix[0].Length) {
            if (TreeMatrix[x][y]) {
                result++;
            }

            x = (x + xPlus) % TreeMatrix.Length;
            y += yPlus;
        }

        return result;
    }

    public long CountTreesForAllSlopes() {
        var result = 1;
        result *= CountTreesForSlope(1, 1);
        result *= CountTreesForSlope(3, 1);
        result *= CountTreesForSlope(5, 1);
        result *= CountTreesForSlope(7, 1);
        result *= CountTreesForSlope(1, 2);
        return result;
    }
}