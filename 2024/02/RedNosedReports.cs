﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace AoC.day2;

/// <summary>
/// <a href="https://adventofcode.com/2024/day/2">Day 2: Red-Nosed Reports</a>
/// </summary>
public class RedNosedReports {
    public RedNosedReports(IEnumerable<string> input) {
        Input = ParseInput(input);
    }

    public bool ProblemDampener { get; set; }
    private IList<int[]> Input { get; } = new List<int[]>();

    private static IList<int[]> ParseInput(IEnumerable<string> input) {
        return input.Select(s => s.Split(" ").Select(n => n.ExtractDigitsAsInt()).ToArray()).ToList();
    }

    public long CalculateSafeReportsCount() {
        return Input.LongCount(i => IsReportSafe(i, ProblemDampener));
    }

    internal bool IsReportSafe(int[] report) => IsReportSafe(report, ProblemDampener);
        
    private bool IsReportSafe(IReadOnlyList<int> report, bool problemDampener) {
        // theese really are a poor man's median
        var firstHalfMedian = report.Take(report.Count / 2).Order().Skip(report.Count / 4).First();
        var secondHalfMedian = report.Skip(report.Count / 2).Order().Skip(report.Count / 4).First();
        
        var rangeToCheck = firstHalfMedian > secondHalfMedian
            ? /* decreasing */ new Range<int>(-3, 0) // "to" is excluding
            : /* increasing */ new Range<int>(1, 4);
        var lastCheckedNumber = report[0];
        var reportIsSafe = true;
            
        for (var i = 1; i < report.Count; i++) {
            if (rangeToCheck.Contains(report[i] - lastCheckedNumber)) {
                // this particular number is safe
                lastCheckedNumber = report[i];
            } else {
                if (problemDampener) {
                    // the problem dampener might take this problem
                    return
                        IsReportSafe(report.Where((_, index) => index != i).ToArray(), false) ||
                        IsReportSafe(report.Where((_, index) => index != i - 1).ToArray(), false) ||
                        IsReportSafe(report.Where((_, index) => index != i + 1).ToArray(), false);
                }
                // the number is not safe, so we can stop checking
                reportIsSafe = false;
                Console.WriteLine(string.Join(",", report));
                break;
            }
        }

        return reportIsSafe;
    }
}
