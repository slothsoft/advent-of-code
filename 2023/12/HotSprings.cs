using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AoC;

/// <summary>
/// <a href="https://adventofcode.com/2023/day/12">Day 12: Hot Springs</a>: For each row, count all of the different arrangements of operational and broken
/// springs that meet the given criteria. What is the sum of those counts?
/// </summary>
public class HotSprings {
    public const char Operational = '.';
    public const char Damaged = '#';
    public const char Unknown = '?';

    private IList<string> _input;
    private IList<int[]> _groups;

    public HotSprings(IEnumerable<string> input) {
        (_input, _groups) = ParseInputGroups(input);
    }

    private static (IList<string>, IList<int[]>) ParseInputGroups(IEnumerable<string> input) {
        var result = new List<string>();
        var groups = new List<int[]>();

        foreach (var value in input) {
            var split = value.Split(" ");
            result.Add(split[0]);
            groups.Add(split[1].ParseIntArray(','));
        }

        return (result, groups);
    }

    public long CalculatePossibleArrangementsCount() {
        var result = 0L;
        var inputLengthCache = new Dictionary<string, long>();
        for (var i = 0; i < _groups.Count; i++) {
            result += CalculatePossibleArrangementsCount(_input[i], _groups[i], inputLengthCache);
        }

        return result;
    }

    internal static long CalculatePossibleArrangementsCount(string input, int[] groups, IDictionary<string, long> inputLength) {
        do {
            // we can ignore leading and trailing operationals, since they don't make up groups
            if (input.StartsWith(Operational)) {
                input = input.Trim(Operational);
            }

            var key = input + ' ' + string.Join(",", groups);

            // the key is completely unique (hopefully), so if it exists return that result
            if (inputLength.TryGetValue(key, out var count)) {
                return count;
            }

            // there are no more groups
            if (groups.Length == 0) {
                return inputLength[key] = (input.Contains(Damaged) ? 0 : 1);
            }

            // if there are no characters left and no groups this is a valid solution
            if (input.Length == 0) {
                return inputLength[key] = 0;
            }

            // there are not enough characters to fullfill all the groups
            if (input.Length < groups.Sum() + groups.Length - 1) {
                return inputLength[key] = 0;
            }

            // if the input starts a group, validate with the group array
            if (input.StartsWith(Damaged)) {
                // the input is too small to accommodate the group
                if (input.Length < groups[0]) {
                    return inputLength[key] = 0;
                }

                // if there is at least one group, we need to check that we can actually fulfill it
                var inputStart = input[0..groups[0]];

                // if we have any . in there the group is not valid; same if the char after is another #
                if (!inputStart.Contains(Operational)) {
                    // so even if there are ? in this part, there is only one way to fill them in
                    if (input.Length <= groups[0]) {
                        // there is nothing after this group, so just return 1 if there are no more groups
                        return inputLength[key] = (groups.Length == 1 ? 1L : 0L);
                    }

                    if (input[groups[0]] != Damaged) {
                        input = input[(groups[0] + 1)..];
                        groups = groups.Skip(1).ToArray();
                        continue;
                    }
                }

                return inputLength[key] = 0;
            }

            // if the input starts with ?, we need to test the rest of the string with . and #
            if (input.StartsWith(Unknown)) {
                return CalculatePossibleArrangementsCount(Operational + input[1..], groups, inputLength) +
                       CalculatePossibleArrangementsCount(Damaged + input[1..], groups, inputLength);
            }
        } while (true);
    }

    public void Unfold(int number) {
        for (var i = 0; i < _input.Count; i++) {
            var newInput = new StringBuilder();
            for (var n = 0; n < number; n++) {
                if (n > 0) {
                    newInput.Append(Unknown);
                }

                newInput.Append(_input[i]);
            }

            _input[i] = newInput.ToString();

            var newGroup = new List<int>();
            for (var n = 0; n < number; n++) {
                newGroup.AddRange(_groups[i]);
            }

            _groups[i] = newGroup.ToArray();
        }
    }
}