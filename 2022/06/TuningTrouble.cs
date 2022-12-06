using System.Linq;

namespace AoC._06;

/// <summary>
/// <a href="https://adventofcode.com/2022/day/6">Day 6: Tuning Trouble</a>: The preparations are finally complete;
/// you and the Elves leave camp on foot and begin to make your way toward the star fruit grove.
///
/// As you move through the dense undergrowth, one of the Elves gives you a handheld device. He says that it has
/// many fancy features, but the most important one to set up right now is the communication system.
/// </summary>
public static class TuningTrouble
{
    public static long Tune(string input, int markerLength = 4) {
        for (var starting = 0; starting < input.Length - markerLength + 1; starting++) {
            if (input.Substring(starting, markerLength).Distinct().Count() == markerLength)
                return starting + markerLength;
        }
        return -1;
    }
}