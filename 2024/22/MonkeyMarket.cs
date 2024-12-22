using System;
using System.Collections.Generic;
using System.Linq;

namespace AoC.day22;

/// <summary>
/// <a href="https://adventofcode.com/2024/day/22">Day 22: Monkey Market</a>
/// </summary>
public class MonkeyMarket {

    internal struct SecretNumber(long value) {

        private static readonly Func<long, long>[] Steps = [
            one => one * 64,
            two => two / 32,
            three => three * 2048,
        ];

        public long Value { get; } = value;

        public SecretNumber NextSecretNumber() {
            var result = Value;
            foreach (var step in Steps) {
                result = Prune(Mix(result, step(result)));
            }

            return new SecretNumber(result);
        }

        internal static long Mix(long currentValue, long nextValue) => currentValue ^ nextValue;
        internal static long Prune(long nextValue) => nextValue % 16777216;
    }

    internal struct Difference(long secretNumber, int difference) {
        public long SecretNumber { get; } = secretNumber;
        public int Value { get; } = difference;
    }

    public MonkeyMarket(IEnumerable<string> input) {
        Input = input.Select(long.Parse).ToArray();
    }

    private long[] Input { get; }

    public long GenerateSecretNumbersSum(long simulatedNumber) {
        return Input.Select(i => GenerateSecretNumbers(i, simulatedNumber).Last().SecretNumber).Sum();
    }

    internal static IEnumerable<Difference> GenerateSecretNumbers(long secretNumber, long simulatedNumber) {
        var result = new SecretNumber(secretNumber);
        for (var i = 0; i < simulatedNumber; i++) {
            var nextSecretNumber = result.NextSecretNumber();
            yield return new Difference(nextSecretNumber.Value, (int)((nextSecretNumber.Value % 10 - result.Value % 10)));
            result = nextSecretNumber;
        }
    }

    public long CalculateMaxBananas(long simulatedNumber) {
        var sellerDifferences = Input.Select(i => GenerateSecretNumbers(i, simulatedNumber).ToArray()).ToList();
        var differences = sellerDifferences.SelectMany(d => d).Select(d => d.Value).ToHashSet();
        var monkeyCommands = differences
            .SelectMany(first => differences.Select(second => (first, second)))
            .SelectMany(firstAndSecond => differences.Select(third => (firstAndSecond.first, firstAndSecond.second, third)))
            .SelectMany(firstToThird => differences.Select(fourth => new[] {firstToThird.first, firstToThird.second, firstToThird.third, fourth}));

        var result = 0L;
        foreach (var monkeyCommand in monkeyCommands) {
            var potentialResult = sellerDifferences.Select(d => CalculateBananas(d, monkeyCommand)).Sum();
            result = Math.Max(result, potentialResult);
        }
        return result;
    }

    internal static long CalculateBananas(long startNumber, int[] monkeyCommand) {
        var differences = GenerateSecretNumbers(startNumber, 2000).ToArray();
        return CalculateBananas(differences, monkeyCommand);
    }

    private static long CalculateBananas(Difference[] differences, int[] monkeyCommand) {
        // Console.WriteLine($"{differences[0].SecretNumber}: {differences[0].SecretNumber % 10}");
        for (var startIndex = 1; startIndex < differences.Length - monkeyCommand.Length; startIndex++) {
            // Console.WriteLine($"{differences[startIndex].SecretNumber}: {differences[startIndex].SecretNumber % 10} ({differences[startIndex].Value})");
            for (var index = 0; index < monkeyCommand.Length; index++) {
                if (differences[startIndex + index].Value != monkeyCommand[index]) {
                    // not the monkey command D:
                    break;
                }
                if (index == monkeyCommand.Length - 1) {
                    // the monkey command worked!!!
                    return (int)(differences[startIndex + index].SecretNumber % 10);
                } 
            }
        }
        return 0;
    }
}