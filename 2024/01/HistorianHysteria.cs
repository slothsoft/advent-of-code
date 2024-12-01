using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using NUnit.Framework;

namespace AoC.day1;

/// <summary>
/// <a href="https://adventofcode.com/2024/day/1">Day 1: Historian Hysteria</a>
/// </summary>
public class HistorianHysteria {
    public HistorianHysteria(IEnumerable<string> input) {
        ParseInput(input);
        _count = LeftList.Count;
        if (RightList.Count != _count) throw new Exception("Counts are not the same!");
    }

    internal IList<long> LeftList { get; } = new List<long>();
    internal IList<long> RightList { get; } = new List<long>();
    private long _count { get; }

    internal void ParseInput(IEnumerable<string> input) {
        foreach (var line in input) {
            var values = line.Split("   ");
            LeftList.Add(values[0].ExtractDigitsAsLong());
            RightList.Add(values[1].ExtractDigitsAsLong());
        }
    }

    public long CalculateTotalDistance() {
        var left = LeftList.Order().ToArray();
        var right = RightList.Order().ToArray();

        var result = 0L;
        for (var i = 0; i < _count; i++) {
            result += Math.Abs(left[i] - right[i]);
        }
        return result;
    }
    
    public long CalculateSimilarityScore() {
        var rightCount = RightList.GroupBy(v => v).ToDictionary(g => g.Key, g => g.Count());
        var result = 0L;
        foreach (var left in LeftList) {
            result += left * rightCount.GetValueOrDefault(left);
        }
        return result;
    }
}
