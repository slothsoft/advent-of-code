using System;
using System.Collections.Generic;
using System.Linq;

namespace AoC.day6;

/// <summary>
/// <a href="https://adventofcode.com/2025/day/6">Day 6: Trash Compactor</a>
/// </summary>
public class TrashCompactor {
    public TrashCompactor(IEnumerable<string> input, bool cephalopodMath = false) {
        (Input, Operators) = cephalopodMath ? ParseCephalopodInput(input) : ParseHumanInput(input);
    }

    internal IList<int[]> Input { get; }
    internal char[] Operators { get; }

    private static (IList<int[]>, char[]) ParseCephalopodInput(IEnumerable<string> input) {
        var inputAsArray = input.Where(i => !string.IsNullOrWhiteSpace(i)).ToArray();
        var operators = inputAsArray.Last().SplitCephalopodMath().Select(s => s.Single()).ToArray();
        var index = 0;
        var currentNumbers = new List<int>();
        var cephalopodNumbers = new List<int[]>();
        
        while (index < inputAsArray[0].Length) {
            var digits = inputAsArray.Take(inputAsArray.Length - 1).Select(s => s[index]).ToArray();

            if (digits.All(d => d == ' ')) {
                // we are in the separator column between problems
                cephalopodNumbers.Add(currentNumbers.ToArray());
                currentNumbers.Clear();
            } else {
                currentNumbers.Add(int.Parse(new string(digits).Trim()));
            } 
            index++;
        }
        
        cephalopodNumbers.Add(currentNumbers.ToArray());
        return (cephalopodNumbers, operators);
    }
    
    internal static (IList<int[]>, char[]) ParseHumanInput(IEnumerable<string> input) {
        var inputAsArray = input.Where(i => !string.IsNullOrWhiteSpace(i)).ToArray();
        var numbersInRows = inputAsArray
            .Take(inputAsArray.Length - 1)
            .Select(r => r.SplitCephalopodMath().Select(int.Parse).ToArray())
            .ToList();
        var numbersInColumns = Enumerable.Range(0, numbersInRows[0].Length).Select(i => numbersInRows.Select(n => n[i]).ToArray()).ToList();
        var operators = inputAsArray.Last().SplitCephalopodMath().Select(s => s.Single()).ToArray();
        return (numbersInColumns, operators);
    }
    
    public long CalculateResult() {
        var result = 0L;

        for (var index = 0; index < Operators.Length; index++) {
            var subResult = CalculateResult(index);
            Console.WriteLine($"{index}: {subResult}");
            result += subResult;
        }
        
        return result;
    }
    
    private long CalculateResult(int index) {
        var result = NeutralElement(index);
        var op = Operator(index);
        
        foreach (var value in Input[index]) {
            result = op(result, value);
        }

        return result;
    }
    
    private Func<long, long, long> Operator(int index) {
        var op = Operators[index];
        return op == '*' ? (a, b) => a * b : (a, b) => a + b;
    }
    
    private long NeutralElement(int index) {
        var op = Operators[index];
        return op == '*' ? 1 : 0;
    }
}

public static class TrashCompactorExtensions {
    public static IEnumerable<string> SplitCephalopodMath(this string input) {
        return input.Split(' ').Where(i => !string.IsNullOrWhiteSpace(i));
    }
}
