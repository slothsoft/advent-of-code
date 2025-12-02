using System;
using System.Collections.Generic;
using System.Linq;

namespace AoC.day2;

/// <summary>
/// <a href="https://adventofcode.com/2025/day/2">Day 2: Gift Shop</a>
/// </summary>
public class GiftShop {
    internal record IdRange(long Min, long Max) {
        // so if a number repeats 6 times, does it also repeat 2 and 3 times?
        // 12.12.12.12.12.12 => 121212.121212 / 1212.1212.1212
        internal IEnumerable<long> FindInvalidIds(int maxRepeats = 2) {
            for (var id = Min; id <= Max; id++) {
                var idAsString = id.ToString();
                var factorials = idAsString.Length.GeneratePrimeFactorials().Where(f => f <= maxRepeats).Distinct().ToArray();

                foreach (var factorial in factorials) {
                    var chunks = idAsString.SplitIntoChunks(idAsString.Length / factorial).ToArray();
                    var differentChunks = chunks.Distinct().Count();
                    if (chunks.Length > 1 && differentChunks == 1) {
                        yield return id;
                        break;
                    }
                }
            }
        }
    }

    public GiftShop(IEnumerable<string> input) {
        Input = ParseInput(input.Single(i => !string.IsNullOrWhiteSpace(i)));
    }

    internal IdRange[] Input { get; }

    internal static IdRange[] ParseInput(string input) {
        return input.Split(',')
            .Select(s => s.Split('-'))
            .Select(a => new IdRange(long.Parse(a[0]), long.Parse(a[1])))
            .ToArray();
    }

    public long CalculatePart1() {
        return Input.SelectMany(i => i.FindInvalidIds()).Sum();
    }

    public long CalculatePart2() {
        return Input.SelectMany(i => i.FindInvalidIds(int.MaxValue)).Sum();
    }
}

public static class GiftShopExtensions {
    public static IEnumerable<int> GeneratePrimeFactorials(this int number) => ((long)number).GeneratePrimeFactorials().Select(v => (int)v);
        
    public static IEnumerable<long> GeneratePrimeFactorials(this long number) {
        var currentNumber = number;
        var maxValue = (long) Math.Sqrt(currentNumber);
        for (var div = 2L; div <= maxValue; div++) {
            while (currentNumber % div == 0) {
                yield return div;
                currentNumber /= div;
                maxValue = currentNumber;
            }
        }

        if (currentNumber == number) {
            // this means nothing we ever did created a clean division, so just add the number itself
            yield return number;
        }
    }
    
    public static IEnumerable<string> SplitIntoChunks(this string value, int chunkSize)
    {
        return Enumerable.Range(0, value.Length / chunkSize).Select(i => value.Substring(i * chunkSize, chunkSize));
    }
}