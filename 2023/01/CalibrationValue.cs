using System.Collections.Generic;
using System.Linq;

namespace AoC;

/// <summary>
/// <a href="https://adventofcode.com/2023/day/1">Day 1: Trebuchet?!</a>: What is the sum of all of the calibration values?
/// </summary>
public static class CalibrationValue {
    private static readonly IDictionary<string, int> WordToDigit = new Dictionary<string, int> {
        {"one", 1},
        {"two", 2},
        {"three", 3},
        {"four", 4},
        {"five", 5},
        {"six", 6},
        {"seven", 7},
        {"eight", 8},
        {"nine", 9},
    };

    public static int Calculate(IEnumerable<string> input) {
        return input.Select(CalculateSingle).Sum();
    }

    internal static int CalculateSingle(string input) {
        var withoutRubbish = input.ExtractDigitsAsString();
        return int.Parse(withoutRubbish.First().ToString() + withoutRubbish.Last());
    }

    public static int CalculateWithDigitAsStrings(IEnumerable<string> input) {
        return input.Select(CalculateSingleWithDigitAsStrings).Sum();
    }

    internal static int CalculateSingleWithDigitAsStrings(string input) {
        var firstDigit = FindFirstDigit(input, 0, +1);
        var lastDigit = FindFirstDigit(input, input.Length - 1, -1);
        return (firstDigit * 10) + lastDigit;
    }
    
    private static int FindFirstDigit(string input, int startIndex, int increment) {
        var index = startIndex;
        while (true) {
            if (char.IsDigit(input[index])) {
                return int.Parse(input[index].ToString());
            }
            
            foreach (var (word, digit) in WordToDigit) {
                if (input[index..].StartsWith(word)) {
                    return digit;
                }
            }

            index += increment;
        }
    }
}