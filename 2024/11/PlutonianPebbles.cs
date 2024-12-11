using System.Collections.Generic;
using System.Linq;

namespace AoC.day11;

/// <summary>
/// <a href="https://adventofcode.com/2024/day/11">Day 11: Plutonian Pebbles</a>
/// </summary>
public class PlutonianPebbles {
    public PlutonianPebbles(string input) {
        Input = input.ParseLongArray().ToList();
    }

    internal IList<long> Input { get; }
    
    public IList<long> CalculateBlinks(int count = 1) {
        return DoCalculateBlinks(count).SelectMany(kv => Enumerable.Repeat(kv.Key, (int) kv.Value)).ToList();
    }

    public long CalculateBlinkCount(int count = 1) {
        return DoCalculateBlinks(count).Values.Sum(v => v);
    }
    
    private IDictionary<long, long> DoCalculateBlinks(int count = 1) {
        var stones = Input.ToDictionary(s => s, _ => 1L);
        
        for (var blink = 0; blink < count; blink++) {
            var newStones = new Dictionary<long, long>();
            foreach (var (stone, number) in stones.ToList()) {
                if (stone == 0) {
                    // rule 1 - replace 0 by 1
                    var stoneOne = 1;
                    newStones[stoneOne] = newStones.TryGetValue(stoneOne, out var newStone) ? newStone + number : number;
                } else if (stone.ToString().Length % 2 == 0) {
                    // rule 2 - even stones separate
                    var stoneAsString = stone.ToString();
                    var firstStone = long.Parse(stoneAsString[(stoneAsString.Length / 2)..]);
                    var secondStone = long.Parse(stoneAsString[..(stoneAsString.Length / 2)]);
                    newStones[firstStone] = newStones.TryGetValue(firstStone, out var newStone1) ? newStone1 + number : number;
                    newStones[secondStone] = newStones.TryGetValue(secondStone, out var newStone2) ? newStone2 + number : number;
                } else {
                    // rules 3 - multiply by 2024
                    var multiplyStone = stone * 2024;
                    newStones[multiplyStone] = newStones.TryGetValue(multiplyStone, out var newStone) ? newStone + number : number;
                }
            }
            stones = newStones;
        }
        return stones;
    }
}

public static class PlutonianPebblesExtensions {
    public static string Stringify(this IEnumerable<long> stones) {
        return string.Join(" ", stones);
    }
}
