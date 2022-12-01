using System;
using System.Collections.Generic;
using System.Linq;

namespace AoC;

/// <summary>
/// <a href="https://adventofcode.com/2022/day/1">Day 1: Calorie Counting</a>: Find the Elf carrying the most Calories.
/// How many total Calories is that Elf carrying?
/// </summary>
public static class CalorieCounting {
    public static int Count(IEnumerable<string> lines, int countTop = 1) {
        var maxValues = new List<int>();
        var currentValue = 0;
        foreach (var line in lines) {
            if (line.Equals("")) {
                maxValues.Add(currentValue);
                currentValue = 0;
            } else {
                currentValue += Int32.Parse(line);
            }
        }
        maxValues.Add(currentValue);

        return maxValues.OrderByDescending(i => i).Take(countTop).Sum();
    }
}