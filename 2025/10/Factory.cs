using System;
using System.Collections.Generic;
using System.Linq;

namespace AoC.day10;

/// <summary>
/// <a href="https://adventofcode.com/2025/day/10">Day 10: Factory</a>
/// </summary>
public class Factory {
    internal record Machine(bool[] IndicatorLights, ICollection<int[]> ButtonWirings) {
    }

    public Factory(IEnumerable<string> input) {
        Input = ParseInput(input);
    }

    internal Machine[] Input { get; }

    internal static Machine[] ParseInput(IEnumerable<string> input) {
        return input.Select(ParseInputLine).ToArray();
    }
    
    internal static Machine ParseInputLine(string input) {
        var parts = input.Split(" ");
        var indicatorLights = parts.Single(p => p.StartsWith('[')).StripBrackets().Select(c => c == '#').ToArray();
        var buttonWirings = parts.Where(p => p.StartsWith('(')).Select(p => p.StripBrackets().ParseIntArray(',')).ToList();
        return new Machine(indicatorLights, buttonWirings);
    }

    public long CalculateIndicatorLights() {
        return Input.Select(CalculateIndicatorLightsForMachine).Sum();
    }
    
    internal static long CalculateIndicatorLightsForInputLine(string input) => CalculateIndicatorLightsForMachine(ParseInputLine(input));
    
    internal static long CalculateIndicatorLightsForMachine(Machine input) {
        var algorithm = new DijkstraForIntAlgorithm<bool[]>(
            indicatorLights => input.ButtonWirings.Select(buttonWiring => {
                var newLights = (bool[]) indicatorLights.Clone();
                foreach (var button in buttonWiring) {
                    newLights[button] = !newLights[button];
                }
                return newLights;
            }).ToDictionary(l => l, l => 1),
           new ArrayEqualityComparer<bool>()
        );
        return algorithm.Solve(new bool[input.IndicatorLights.Length], node => input.IndicatorLights.SequenceEqual(node));
    }

    private class ArrayEqualityComparer<TValue> : IEqualityComparer<TValue[]> {

        public bool Equals(TValue[]? x, TValue[]? y) => x!.SequenceEqual(y!);
        public int GetHashCode(TValue[] obj) => obj.Select(e => e?.GetHashCode() ?? 13).Aggregate((x, y) => x ^ y);
    }
}

public static class FactoryExtensions {
    public static string StripBrackets(this string value) {
        return value[1..^1];
    }
}
