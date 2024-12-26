using System;
using System.Collections.Generic;
using System.Linq;

namespace AoC.day25;

/// <summary>
/// <a href="https://adventofcode.com/2024/day/25">Day 25: Code Chronicle</a>
/// </summary>
public class CodeChronicle {
    internal record Key(int[] Heights);
    internal record Lock(int[] Heights);

    public CodeChronicle(IEnumerable<string> input) {
        (Keys, Locks) = ParseInput(input);
    }

    internal Key[] Keys { get; }
    internal Lock[] Locks { get; }

    internal static (Key[], Lock[]) ParseInput(IEnumerable<string> input) {
        var inputAsArray = input.ToArray();
        var keys = new List<Key>();
        var locks = new List<Lock>();
        var start = 0;

        while (start <= inputAsArray.Length) {
            var keyOrLock = inputAsArray.Skip(start).Take(7).ParseBoolMatrix('#', '.');

            var heights = keyOrLock.Select(a => a.Count(b => b)).ToArray();
            if (keyOrLock.Select(a => a[0]).Count(b => b) == keyOrLock.Length) {
                // the entire first row is true, so its a lock
                locks.Add(new Lock(heights));
            } else {
                // the entire first row is NOT true, so its a key
                keys.Add(new Key(heights));
            }
            start += 7 + 1;
        }


        return (keys.ToArray(), locks.ToArray());
    }

    public long CalculateFittingKeyLocks(int availableSpace) {
        return Keys
            .SelectMany(key => Locks.Select(luck => (key, luck)))
            .Count(keyLuck => keyLuck.Item1.Heights.Select((k, i) => k + keyLuck.Item2.Heights[i]).All(h => h <= availableSpace));
    }
}