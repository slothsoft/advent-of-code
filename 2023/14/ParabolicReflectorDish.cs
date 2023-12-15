using System.Collections.Generic;
using System.Linq;

namespace AoC;

/// <summary>
/// <a href="https://adventofcode.com/2023/day/14">Day 14: Parabolic Reflector Dish</a>: Tilt the platform so that the rounded rocks all roll north. Afterward,
/// what is the total load on the north support beams?
/// </summary>
public class ParabolicReflectorDish {
    public const char Rock = 'O';
    public const char RockWall = '#';
    public const char Ground = '.';

    public ParabolicReflectorDish(IEnumerable<string> input) {
        Input = input.ParseCharMatrix();
    }

    public char[][] Input { get; }

    public void TiltNorth() {
        for (var y = 1; y < Input[0].Length; y++) {
            for (var x = 0; x < Input.Length; x++) {
                if (Input[x][y] == Rock) {
                    for (var newY = y; newY >= 0; newY--) {
                        if (newY == 0 || Input[x][newY - 1] != Ground) {
                            // the value above is blocked, so put stone here
                            if (y != newY) {
                                // except if it is the same value as before
                                Input[x][newY] = Rock;
                                Input[x][y] = Ground;
                            }

                            break;
                        }
                    }
                }
            }
        }
    }

    public long CalculateLoad() {
        var result = 0L;
        for (var y = 0; y < Input[0].Length; y++) {
            var rockCount = Input.Count(i => i[y] == Rock);
            result += rockCount * (Input[0].Length - y);
        }

        return result;
    }

    private int CalculateCalculateRockCount(int x) {
        return Input[x].Count(c => c == Rock);
    }

    private static long CalculateGaußianFormula(int n) {
        // to calculate n + ... + 3 + 2 + 1
        return (n * (n + 1)) / 2;
    }
}