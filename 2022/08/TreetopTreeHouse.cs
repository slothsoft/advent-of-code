using System;
using System.Collections.Generic;
using System.Linq;

namespace AoC._08;

/// <summary>
/// <a href="https://adventofcode.com/2022/day/8">Day 8: Treetop Tree House</a>: The expedition comes across a peculiar patch of tall trees all planted carefully in a grid. The Elves explain that a previous expedition planted these trees as a reforestation effort. Now, they're curious if this would be a good location for a tree house.
/// First, determine whether there is enough tree cover here to keep a tree house hidden. To do this, you need to count the number of trees that are visible from outside the grid when looking directly along a row or column.
/// The Elves have already launched a quadcopter to generate a map with the height of each tree (your puzzle input). 
/// </summary>
public class TreetopTreeHouse {
    public static int FindVisibleTreesCount(string[] lines) {
        var result = 0;
        var resultAsMatrix = "";

        for (var row = 0; row < lines.Length; row++) {
            for (var col = 0; col < lines[row].Length; col++) {
                if (IsVisible(lines, row, col)) {
                    result++;
                    resultAsMatrix += ".";
                } else {
                    resultAsMatrix += "#";
                }
            }
            resultAsMatrix += "\n";
        }
        // Console.WriteLine(resultAsMatrix);
        return result;
    }

    private static bool IsVisible(string[] lines, int row, int col) {
        // visibility from the left
        if (IsVisibleInRow(lines[row], col)) return true;
        // visibility from the right
        if (IsVisibleInRow(lines[row].Reverse(), lines[row].Length - col - 1)) return true;
        
        // visibility from the top
        if (IsVisibleInRow(lines.Select(l => l[col]), row)) return true;
        // visibility from the bottom
        if (IsVisibleInRow(lines.Select(l => l[col]).Reverse(), lines.Length - row - 1)) return true;

        return false;
    }

    private static bool IsVisibleInRow(IEnumerable<char> line, int index) {
        var checkedIndex = 0;
        var maxValue = -1;

        foreach (var c in line) {
            if (checkedIndex == index) {
                // this is the field we actually need to check
                return int.Parse(char.ToString(c)) > maxValue;
            }
            maxValue = Math.Max(int.Parse(char.ToString(c)), maxValue);
            checkedIndex++;
        }
        return false;
    }
    
    public static int FindTreeHouseSpot(string[] lines) {
        var result = 0;

        for (var row = 0; row < lines.Length; row++) {
            for (var col = 0; col < lines[row].Length; col++) {
                result = Math.Max(CalculateScenicScore(lines, row, col), result);
            }
        }
        return result;
    }

    internal static int CalculateScenicScore(string[] lines, int row, int col) {
        if (col == 0 || row == 0 || col == lines.Length - 1 || row == lines[0].Length - 1)
            return 0;
        
        var result = 1;
        
        // scenic score to the left
        result *= CalculateScenicScoreInRow(lines[row].Substring(0, col + 1).Reverse());
        // scenic score to the right
        result *= CalculateScenicScoreInRow(lines[row].Substring(col));

        // scenic score to the top
        result *= CalculateScenicScoreInRow(lines.Take(row + 1).Select(l => l[col]).Reverse());
        // scenic score to the bottom
        result *= CalculateScenicScoreInRow(lines.Skip(row).Select(l => l[col]));

        return result;
    }

    internal static int CalculateScenicScoreInRow(IEnumerable<char> line) {
        int? spotValue = null;
        var result = 0;

        var lineAsString = string.Join("", line);
        
        foreach (var c in lineAsString) {
            if (spotValue == null) {
                // the first entry is where the tree house is
                spotValue = int.Parse(char.ToString(c));
            } else {
                // this is any tree after the tree house tree
                var treeValue = int.Parse(char.ToString(c));
                if (treeValue < spotValue) {
                    // the tree is visible \o/
                    result++;
                } else {
                    // the tree blocks all other trees
                    result++;
                    break;
                }
            }
        }
        return result;
    }
}