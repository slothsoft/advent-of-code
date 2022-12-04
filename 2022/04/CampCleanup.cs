using System;
using System.Collections.Generic;
using System.Linq;

namespace AoC;

/// <summary>
/// <a href="https://adventofcode.com/2022/day/4">Day 4: Camp Cleanup</a>: Space needs to be cleared
/// before the last supplies can be unloaded from the ships, and so several Elves have been assigned the
/// job of cleaning up sections of the camp. Every section has a unique ID number, and each Elf is assigned
/// a range of section IDs.
///
/// However, as some of the Elves compare their section assignments with each other, they've noticed that
/// many of the assignments overlap. To try to quickly find overlaps and reduce duplicated effort, the
/// Elves pair up and make a big list of the section assignments for each pair (your puzzle input).
/// </summary>
public static class CampCleanup {

    public static int CalculateContains(IEnumerable<string> lines) {
        return Calculate(lines, IsFullyContained);
    }
    
    private static int Calculate(IEnumerable<string> lines, Func<int[], int[], bool> predicate) {
        var result = 0;

        foreach (var line in lines) {
            var lineSplit = line.Split(",");
            var range1 = ToIntArray(lineSplit[0]);
            var range2 = ToIntArray(lineSplit[1]);

            if (predicate(range1, range2) || predicate(range2, range1)) {
                result++;
            }
        }
        return result;
    }
    
    private static int[] ToIntArray(string range) {
        return range.Split("-").Select(int.Parse).ToArray();
    }

    private static bool IsFullyContained(int[] biggerRange, int[] smallerRange) {
        return biggerRange[0] <= smallerRange[0] && biggerRange[1] >= smallerRange[1];
    }
    
    public static int CalculateOverlap(IEnumerable<string> lines) {
        return Calculate(lines, IsOverlapping);
    }
    
    private static bool IsOverlapping(int[] biggerRange, int[] smallerRange) {
        return biggerRange[0] <= smallerRange[1] && biggerRange[1] >= smallerRange[0];
    }
}