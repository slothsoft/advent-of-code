using System;

namespace AoC._25;

/// <summary>
/// <a href="https://adventofcode.com/2022/day/25">Day 25: Full of Hot Air</a>
/// </summary>
public class FullOfHotAir {
    private readonly string[] _lines;
    public FullOfHotAir(string[] lines) {
        _lines = lines;
    }

    public string CalculateSumInSnafu() {
        return CalculateSumInDecimal().ConvertToSnafu();
    }
    
    public long CalculateSumInDecimal() {
        var result = 0L;
        foreach (var line in _lines) {
            result += line.ConvertFromSnafu();
        }

        return result;
    }
}

internal enum SnafuDigits {
    MinusTwo='=',
    MinusOne='-',
    Zero='0',
    One='1',
    Two='2',
}

internal static class SnafuExtensions {
    private static readonly long[] DigitValues = {-2, -1, 0, 1, 2 };
    private static readonly SnafuDigits[] Digits = { SnafuDigits.MinusTwo, SnafuDigits.MinusOne, SnafuDigits.Zero, SnafuDigits.One, SnafuDigits.Two };
    public static long ConvertFromSnafu(this string value) {
        var result = 0L;
        for (var i = 0; i < value.Length; i++) {
            var index = Array.IndexOf(Digits, (SnafuDigits) value[i]);
            result += (long) Math.Pow(5, value.Length - i - 1) * DigitValues[index];
        }

        return result;
    }
    
    public static string ConvertToSnafu(this long value) {
        var result = "";
        var highestPower = CalculateHighestPower(value) + 1;
        for (var p = highestPower; p >= 0; p--) {
            var fivePower = Math.Pow(5, p);
            var digit = (long) Math.Round(value / fivePower);
            var index = Array.IndexOf(DigitValues, digit);
            if (index < 0) {
                throw new ArgumentException($"Digit {digit} ({value} / {fivePower}) could not be found!");
            }
            result += (char) Digits[index];
            value -= (long)fivePower * digit;
        }

        return result.StartsWith("0") ? result.Substring(1) : result;
    }
    
    private static long CalculateHighestPower(this long value) {
        var result = 0;
        var resultDiff = value;
        while (true) {
            var possibleResult = result + 1;
            var possibleDiff = Math.Abs((long) Math.Pow(5, possibleResult) - value);

            if (possibleDiff > resultDiff) {
                return result;
            }

            result = possibleResult;
            resultDiff = possibleDiff;
        }
    }
}