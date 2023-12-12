using System;
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
        for (var i = 0; i < _groups.Count; i++) {
            result += CalculatePossibleArrangements(_input[i], _groups[i]).Count();
        }

        return result;
    }

    internal static IEnumerable<string> CalculatePossibleArrangements(string input, int[] groups) {
        if (input.Contains(Unknown)) {
            // we check if we need to search farther
            if (IsStillPossible(input, groups)) {
                var index = input.IndexOf(Unknown);
                var possibleArrangements = CalculatePossibleArrangements(ReplaceAt(input, index, Damaged), groups)
                    .Concat(CalculatePossibleArrangements(ReplaceAt(input, index, Operational), groups));
                foreach (var possibleArrangement in possibleArrangements) {
                    yield return possibleArrangement;
                }
            }
        } else {
            if (CalculateGroups(input).SequenceEqual(groups)) {
                yield return input;
            }
        }
    }


    private static string ReplaceAt(string input, int index, char newChar)
    {
        var chars = input.ToCharArray();
        chars[index] = newChar;
        return new string(chars);
    }

    private static bool IsStillPossible(string input, int[] groups) {
        var currentGroupCount = 0;
        var groupIndex = 0;
        
        foreach (var value in input) {
            if (value == Damaged) {
                currentGroupCount++;
            } else if (value == Operational) {
                if (currentGroupCount > 0) {
                    if (groupIndex >= groups.Length || currentGroupCount != groups[groupIndex]) {
                        return false;
                    }
                    
                    currentGroupCount = 0;
                    groupIndex++;
                }
            } else if (value == Unknown) {
                return true;
            }
        }
        
        if (currentGroupCount > 0) {
            if (groupIndex >= groups.Length || currentGroupCount != groups[groupIndex]) {
                return false;
            }
        }
        
        return true;
    }
    
    internal static int[] CalculateGroups(string input) {
        var result = new List<int>();
        var currentGroupCount = 0;
        foreach (var value in input) {
            if (value == Damaged) {
                currentGroupCount++;
            } else if (value == Operational) {
                if (currentGroupCount > 0) {
                    result.Add(currentGroupCount);
                    currentGroupCount = 0;
                }
            }
        }

        if (currentGroupCount > 0) {
            result.Add(currentGroupCount);
        }
        
        return result.ToArray();
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