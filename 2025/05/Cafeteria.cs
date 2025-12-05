using System.Collections.Generic;
using System.Linq;

namespace AoC.day5;

/// <summary>
/// <a href="https://adventofcode.com/2025/day/5">Day 5: Cafeteria</a>
/// </summary>
public class Cafeteria {
    internal record FreshRange(long Min, long Max) {
        internal FreshRange(string input) : this(long.Parse(input[..input.IndexOf(RANGE_SEPARATOR)]),
            long.Parse(input[(input.IndexOf(RANGE_SEPARATOR) + 1)..])) {
        }
        public bool Contains(long value) {
            return value >= Min && value <= Max;
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
        long result = 0;
        var nonOverlappingRanges = new List<FreshRange>();
        

        return result;
    }
    
    private static IEnumerable<long> EnumerableRange(long min, long max) {
        for (var i = min; i <= max; i++) {
            yield return i;
        }
    }
}