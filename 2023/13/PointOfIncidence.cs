using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualBasic.CompilerServices;

namespace AoC;

/// <summary>
/// <a href="https://adventofcode.com/2023/day/13">Day 13: Point of Incidence</a>: Find the line of reflection in each of the patterns in your notes.
/// What number do you get after summarizing all of your notes?
/// </summary>
public class PointOfIncidence {
    private IList<string[]> _patterns;

    public PointOfIncidence(IEnumerable<string> input) {
        _patterns = ParsePatterns(input);
    }

    public int AllowedDifferences { get; set; }
    
    private static IList<string[]> ParsePatterns(IEnumerable<string> input) {
        var result = new List<string[]>();
        var inputAsArray = input.ToArray();
        var lastEmptyLine = 0;

        for (var i = 0; i < inputAsArray.Length; i++) {
            if (inputAsArray[i].Length == 0) {
                result.Add(inputAsArray.Skip(lastEmptyLine).Take(i - lastEmptyLine).Select(s => s.Trim()).ToArray());
                lastEmptyLine = i + 1;
            }
        }

        result.Add(inputAsArray.Skip(lastEmptyLine).ToArray());

        return result;
    }

    public long CalculateMirrorNumber() {
        var result = 0L;

        foreach (var pattern in _patterns) {
            var columnMirrorNumber = CalculateColumnMirrorNumber(pattern);
            var rowMirrorNumber = CalculateRowMirrorNumber(pattern);

            if (columnMirrorNumber == 0 && rowMirrorNumber == 0) {
                throw new ArgumentException("Found no mirror axis for pattern: \n" + string.Join("\n", pattern));
            }

            if (columnMirrorNumber > 0 && rowMirrorNumber > 0) {
                throw new ArgumentException("Found too many mirror axises for pattern: \n" + string.Join("\n", pattern));
            }

            result += columnMirrorNumber + 100 * rowMirrorNumber;
        }

        return result;
    }

    private long CalculateColumnMirrorNumber(string[] pattern) {
        for (var i = 0; i < pattern[0].Length - 1; i++) {
            if (GetColumnsDifferences(pattern, i) == AllowedDifferences) {
                return i + 1;
            }
        }

        // we found no mirror axis
        return 0;
    }

    private int GetColumnsDifferences(string[] pattern, int leftColumn) {
        var differences = 0;
        
        for (var i = 0; i < pattern.Length; i++) {
            if (!pattern[i][leftColumn].Equals(pattern[i][leftColumn + 1])) {
                differences++;
            }
        }

        if (differences > AllowedDifferences + 1) {
            return differences;
        }

        for (var j = 1; j < pattern[0].Length; j++) {
            var leftIndex = leftColumn - j;
            var rightIndex = leftColumn + j + 1;
            if (leftIndex < 0 || rightIndex >= pattern[0].Length) {
                // we have arrived at the end of the end of the pattern, so it mirrors with x differences
                break;
            }

            for (var i = 0; i < pattern.Length; i++) {
                if (!pattern[i][leftIndex].Equals(pattern[i][rightIndex])) {
                    differences++;
                }
            }
        }

        return differences;
    }

    private long CalculateRowMirrorNumber(string[] pattern) {
        for (var i = 0; i < pattern.Length - 1; i++) {
            if (GetRowsDifferences(pattern, i) == AllowedDifferences) {
                return i + 1;
            }
        }

        // we found no mirror axis
        return 0;
    }

    private int GetRowsDifferences(string[] pattern, int topRow) {
        var differences = 0;
        
        for (var i = 0; i < pattern[topRow].Length; i++) {
            if (!pattern[topRow][i].Equals(pattern[topRow + 1][i])) {
                differences++;
            }
        }

        if (differences > AllowedDifferences + 1) {
            return differences;
        }
        
        for (var j = 1; j < pattern.Length; j++) {
            var topIndex = topRow - j;
            var bottomIndex = topRow + j + 1;
            if (topIndex < 0 || bottomIndex >= pattern.Length) {
                // we have arrived at the end of the end of the pattern, so it mirrors with x differences
                break;
            }

            for (var i = 0; i < pattern[topRow].Length; i++) {
                if (!pattern[topIndex][i].Equals(pattern[bottomIndex][i])) {
                    differences++;
                }
            }
        }

        return differences;
    }


}