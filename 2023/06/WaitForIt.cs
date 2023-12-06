using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace AoC;

/// <summary>
/// <a href="https://adventofcode.com/2023/day/6">Day 6: Wait For It</a>: Determine
/// the number of ways you could beat the record in each race. What do you get if you multiply these numbers together?
/// </summary>
public class WaitForIt {
    public record Race(int Id, long Time, long Distance) {
        internal long CalculateDistance(long timeToHold) {
            var speed = timeToHold;
            return speed * (Time - timeToHold);
        }
    }

    public WaitForIt(IEnumerable<string> input, bool ignoreSpaces = false) {
        Races = ParseRaces(input, ignoreSpaces);
    }

    public Race[] Races { get; }

    public static Race[] ParseRaces(IEnumerable<string> input, bool ignoreSpaces = false) {
        var inputAsArray = input.ToArray();
        var times = ParseIntArray(inputAsArray[0], ignoreSpaces);
        var distances = ParseIntArray(inputAsArray[1], ignoreSpaces);
        if (times.Length != distances.Length) {
            throw new ArgumentException($"Something went wrong parsing races: times={string.Join(", ", times)}, distances={string.Join(", ", distances)}");
        }

        return times.Select((t, i) => new Race(i, t, distances[i])).ToArray();
    }

    private static long[] ParseIntArray(string input, bool ignoreSpaces) {
        return Regex.Replace(input.Split(':')[1], @"\s+", ignoreSpaces ? "" : " ").ParseLongArray();
    }

    public long CalculateMarginOfError() {
        var result = 1L;
        foreach (var race in Races) {
            result *= CalculateWaysToWin(race).Count();
        }

        return result;
    }
    
    internal IEnumerable<long> CalculateWaysToWin(Race race) {
        for (var i = 1L; i < race.Time; i++) {
            var distance = race.CalculateDistance(i);
            if (distance > race.Distance) {
                yield return i;
            }
        }
    }
}