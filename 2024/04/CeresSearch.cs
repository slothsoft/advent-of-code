using System;
using System.Collections.Generic;

namespace AoC.day4;

/// <summary>
/// <a href="https://adventofcode.com/2024/day/4">Day 4: Ceres Search</a>
/// </summary>
public class CeresSearch {

    private const int InvalidCoord = -1;
    
    internal struct Direction(CeresSearch parent, string name, Func<int, int> modifyX, Func<int, int> modifyY) {
        public (int X, int Y) Modify(int oldX, int oldY) => (
            Sanitize(modifyX(oldX), parent.Input.Length), 
            Sanitize(modifyY(oldY), parent.Input[0].Length)
        );
        private static int Sanitize(int coord, int length) => coord < 0 || coord >= length ? InvalidCoord : coord;
        public override string ToString() => name;
    }

    public CeresSearch(IEnumerable<string> input) {
        Input = input.ParseCharMatrix();

        directions = [
            new Direction(this, "North", x => x, y => y - 1),
            new Direction(this, "South", x => x, y => y + 1),
            new Direction(this, "West", x => x - 1, y => y),
            new Direction(this, "East", x => x + 1, y => y),

            new Direction(this, "NE", x => x + 1, y => y - 1),
            new Direction(this, "SE", x => x + 1, y => y + 1),
            new Direction(this, "NW", x => x - 1, y => y - 1),
            new Direction(this, "SW", x => x - 1, y => y - 1),
        ];
    }

    internal char[][] Input { get; }
    private Direction[] directions { get; }

    public long CalculateXmasCount() {
        long result = 0;
        for (var x = 0; x < Input.Length; x++) {
            for (var y = 0; y < Input[x].Length; y++) {
                result += CalculateWordCount(x, y, "XMAS");
            }
        }
        return result;
    }
    
    private long CalculateWordCount(int x, int y, string word) {
        long result = 0;
        foreach (var direction in directions) {
            var foundWord = false;
            var index = 0;
            var (currentX, currentY) = (x, y);
            
            while (Input[currentX][currentY] == word[index]) {
                index++;
                if (index >= word.Length) {
                    foundWord = true;
                    break;
                }   
                
                (currentX, currentY) = direction.Modify(currentX, currentY);
                if (currentX == InvalidCoord || currentY == InvalidCoord) break;
            }

            if (foundWord) {
                result++;
                Console.WriteLine($"({x}, {y}): {direction}");
            }
        }
        return result;
    }
}
