using System;
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

    public int ProblemDampener { get; set; } = 0;
    private IList<int[]> Input { get; } = new List<int[]>();

    private static IList<int[]> ParseInput(IEnumerable<string> input) {
        return input.Select(s => s.Split(" ").Select(n => n.ExtractDigitsAsInt()).ToArray()).ToList();
    }

    public long CalculateSafeReportsCount() {
        return Input.LongCount(IsReportSafe);
    }

    internal bool IsReportSafe(int[] report) {
        // theese really are a poor man's median
        var firstHalfMedian = report.Take(report.Length / 2).Order().Skip(report.Length / 4).First();
        var secondHalfMedian = report.Skip(report.Length / 2).Order().Skip(report.Length / 4).First();
        
        var rangeToCheck = firstHalfMedian > secondHalfMedian
            ? /* decreasing */ new Range<int>(-3, 0) // "to" is excluding
            : /* increasing */ new Range<int>(1, 4);
        var lastCheckedNumber = report[0];
        var reportIsSafe = true;
        var problemDampener = ProblemDampener;
            
        for (var i = 1; i < report.Length; i++) {
            if (rangeToCheck.Contains(report[i] - lastCheckedNumber)) {
                // this particular number is safe
                lastCheckedNumber = report[i];
            } else {
                if (problemDampener > 0) {
                    if (Math.Abs(report[i] - lastCheckedNumber) > 3) {
                        // the problem dampener cannot take this kind of errors D:
                        reportIsSafe = false;
                        Console.WriteLine(string.Join(",", report));
                        break;
                    } else {
                        // the problem dampener will take this problem
                        problemDampener--;
                        continue;
                    }
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
