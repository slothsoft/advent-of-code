using System;
using System.Collections.Generic;
using System.Linq;

namespace AoC.day5;

/// <summary>
/// <a href="https://adventofcode.com/2025/day/5">Day 5: Cafeteria</a>
/// </summary>
public class Cafeteria {
    internal record FreshRange(long Min, long Max) {
        public long Length => Max - Min + 1;
        
        internal FreshRange(string input) : this(long.Parse(input[..input.IndexOf(RANGE_SEPARATOR)]),
            long.Parse(input[(input.IndexOf(RANGE_SEPARATOR) + 1)..])) {
        }

        public bool Contains(long value) {
            return value >= Min && value <= Max;
        }
        
        public bool Contains(FreshRange value) {
            return Contains(value.Max) && Contains(value.Min);
        }
    }

    public Cafeteria(IEnumerable<string> input) {
        (FreshRanges, AvailableIngredients) = ParseInput(input);
    }

    private const char RANGE_SEPARATOR = '-';

    internal FreshRange[] FreshRanges { get; }
    internal long[] AvailableIngredients { get; }

    internal static (FreshRange[], long[]) ParseInput(IEnumerable<string> input) {
        var inputAsArray = input.Where(i => !string.IsNullOrWhiteSpace(i)).ToArray();
        var freshRanges = inputAsArray.Where(i => i.Contains(RANGE_SEPARATOR)).Select(s => new FreshRange(s)).ToArray();
        var availableIngredients = inputAsArray.Where(i => !i.Contains(RANGE_SEPARATOR)).Select(long.Parse).ToArray();
        return (freshRanges, availableIngredients);
    }

    public long CalculateFreshAvailableIngredientsCount() {
        long result = 0;
        foreach (var availableIngredient in AvailableIngredients) {
            foreach (var freshRange in FreshRanges) {
                if (freshRange.Contains(availableIngredient)) {
                    result++;
                    break;
                }
            }
        }

        return result;
    }

    public long CalculateFreshIngredientsCount() {
        var nonOverlappingRanges = new List<FreshRange>();
        foreach (var freshRange in FreshRanges) {
            nonOverlappingRanges.AddNonOverlappingRange(freshRange);
        }

        return nonOverlappingRanges.Sum(r => r.Length);
    }
}

internal static class CafeteriaExtensions {
    internal static void AddNonOverlappingRange(this IList<Cafeteria.FreshRange> list, Cafeteria.FreshRange freshRange) {
        // two ranges can have these relations:
        // distinct: (----)     {~~~~~~~}
        // intersect: (----{≈≈≈≈≈≈≈≈≈≈)~~~~}
        // contains: (----{≈≈≈≈≈≈}---)
        // expand: {~~~~~(≈≈≈≈≈≈)~~~}
        
        foreach (var existingRange in list) {
            if (existingRange.Contains(freshRange)) {
                // contains: we can ignore freshRange; it cannot intersect anything else, so we are finished
                return;
            } 
            if (existingRange.Contains(freshRange.Min) || existingRange.Contains(freshRange.Max)){
                // intersect: make one range containing both; it cannot intersect anything else, so we are finished
                list.Remove(existingRange);
                list.AddNonOverlappingRange(new Cafeteria.FreshRange(Math.Min(existingRange.Min, freshRange.Min), Math.Max(existingRange.Max, freshRange.Max)));
                return;
            } 
            if (freshRange.Contains(existingRange.Min) || freshRange.Contains(existingRange.Max)){
                // expand: we can ignore existingRange; it cannot intersect anything else, so we are finished
                list.Remove(existingRange);
                list.AddNonOverlappingRange(freshRange);
                return;
            } 
            // distinct: check other ranges
        }
        // distinct to ALL: just add this range now
        list.Add(freshRange);
    }
}
