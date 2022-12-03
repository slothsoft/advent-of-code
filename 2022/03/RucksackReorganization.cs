using System.Collections.Generic;
using System.Linq;

namespace AoC;

/// <summary>
/// <a href="https://adventofcode.com/2022/day/3">Day 3: Rucksack Reorganization</a>: The Elves have made a list of all
/// of the items currently in each rucksack (your puzzle input), but they need your help finding the errors. Every item
/// type is identified by a single lowercase or uppercase letter (that is, a and A refer to different types of items).
/// </summary>
public static class RucksackReorganization {

    public static int CalculateMisplacedItemPriority(IEnumerable<string> lines) {
        var result = 0;

        foreach (var line in lines) {
            var compartment1 = line[..(line.Length / 2)];
            var compartment2 = line[(line.Length / 2)..];

            foreach (var letter in line) {
                if (compartment1.Contains(letter) && compartment2.Contains(letter)) {
                    // found the wrong item!
                    result += CalculateLetterPriority(letter);
                    break;
                }
            }
        }

        return result;
    }

    private static int CalculateLetterPriority(char letter) {
        return char.IsUpper(letter) ? letter - 'A' + 27 : letter - 'a' + 1;
    }
    
    public static int FindBadgePriority(IEnumerable<string> lines) {
        var result = 0;

        var linesAsArray = lines.ToArray();
        for (var i = 0; i < linesAsArray.Length; i+= 3) {
            var rucksack1 = linesAsArray[i];
            var rucksack2 = linesAsArray[i + 1];
            var rucksack3 = linesAsArray[i + 2];
            
            foreach (var letter in rucksack1 + rucksack2 + rucksack3) {
                if (rucksack1.Contains(letter) && rucksack2.Contains(letter) && rucksack3.Contains(letter)) {
                    // found the badge!
                    result += CalculateLetterPriority(letter);
                    break;
                }
            }
        }
        return result;
    }
}