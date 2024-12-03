using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace AoC.day3;

/// <summary>
/// <a href="https://adventofcode.com/2024/day/3">Day 3: Mull It Over</a>
/// </summary>
public class MullItOver {

    private const string MulRegex = @"mul\(\d{1,3},\d{1,3}\)";
    private const string DoRegex = @"do\(\)";
    private const string DontRegex = @"don't\(\)";
    
    public MullItOver(IEnumerable<string> input) {
        Input = input.ToArray();
    }

    internal string[] Input { get; }

    public long CalculateSumOfMuls() {
        return FindMuls().Sum(DoMul);
    }
    
    private static long DoMul(string mul) {
        var split = mul.Split(",");
        var number1 = split[0].ExtractDigitsAsLong();
        var number2 = split[1].ExtractDigitsAsLong();
        return number1 * number2;
    }

    internal IEnumerable<string> FindMuls() => FindByRegex(MulRegex).Select(mi => mi.Value);
    
    private IEnumerable<(string Value, long Index)> FindByRegex(string regexAsString) {
        var regex = new Regex(regexAsString);
        var previousLineLengths = 0L;
        foreach (var line in Input) {
            var matchingMuls = regex.Matches(line);
            for (var i = 0; i < matchingMuls.Count; i++) {
                yield return (matchingMuls[i].Value, previousLineLengths + matchingMuls[i].Index);
            }
            previousLineLengths += line.Length;
        }
    }

    public long CalculateSumOfMulsWithDoAndDont() {
        var muls = FindByRegex(MulRegex).ToDictionary(m => m.Index, m => m.Value);
        var dos = FindByRegex(DoRegex).ToDictionary(d => d.Index, d => d.Value);
        var donts = FindByRegex(DontRegex).ToDictionary(n => n.Index, n => n.Value);

        var maxIndex = Math.Max(Math.Max(muls.Keys.Max(), dos.Keys.Max()), donts.Keys.Max());
        var result = 0L;
        var doMul = true;
        
        for (var index = 0L; index <= maxIndex; index++) {
            if (dos.ContainsKey(index)) doMul = true;
            if (donts.ContainsKey(index)) doMul = false;

            if (muls.TryGetValue(index, out var mul)) {
                if (doMul) {
                    result += DoMul(mul);
                }
            }
        }

        return result;
    }
}
