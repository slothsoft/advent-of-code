using System;
using System.Collections.Generic;
using System.Linq;

namespace AoC;

/// <summary>
/// <a href="https://adventofcode.com/2023/day/8">Day 8: Haunted Wasteland</a>: Starting at AAA, follow the left/right instructions. How many steps are required to reach ZZZ?
/// </summary>
public class HauntedWasteland {

    public struct Next {
        public Next(string left, string right) {
            Left = left;
            Right = right;
        }
        public string Left { get; }
        public string Right { get; }

        public string Direction(char c) {
            switch (c) {
                case 'L': return Left;
                case 'R': return Right;
                default: throw new ArgumentException($"Do not know direction {c}");
            }
        }
    }

    private readonly string _instructions;
    private readonly IDictionary<string, Next> _map;

    public HauntedWasteland(IEnumerable<string> input) {
        (_instructions, _map) = ParseInput(input);
    }

    public static (string Instructions, IDictionary<string, Next> Map) ParseInput(IEnumerable<string> input) {
        var inputEnumerator = input.GetEnumerator();
        try {
            inputEnumerator.MoveNext();
            var instructions = inputEnumerator.Current;

            inputEnumerator.MoveNext();
            inputEnumerator.MoveNext();
            var result = new Dictionary<string, Next>();

            do {
                var keyValues = inputEnumerator.Current.Replace('(', ' ').Replace(')', ' ').Split("=");
                var values = keyValues[1].Split(',');
                result.Add(keyValues[0].Trim(), new Next(values[0].Trim(), values[1].Trim()));
            } while (inputEnumerator.MoveNext());

            return (instructions, result);
        } finally {
            inputEnumerator.Dispose();
        }
    }

    public long CalculateStepsTo(string start = "AAA", string finish = "ZZZ") {
        var result = 0L;
        var current = start;
        
        while (current != finish) {
            current = _map[current].Direction(_instructions[(int)(result % _instructions.Length)]);
            result++;
        }

        return result;
    }
    public long CalculateGhostStepsTo(char startEndsWith = 'A', char finishEndsWith = 'Z') {
        var finishes = _map.Keys.Where(n => n[2] == finishEndsWith).ToArray();
        var startsWithes = _map.Keys.Where(n => n[2] == startEndsWith).ToArray();
        var result = 1L;
        
        foreach (var startsWith in startsWithes) {
            var current = startsWith;
            var resultPart = 0L;
            
            while (!finishes.Contains(current)) {
                current = _map[current].Direction(_instructions[(int)(resultPart % _instructions.Length)]);
                resultPart++;
            }
            
            var divisor = FindGreatestCommonDivisor(resultPart, result);
            if (divisor == 0) {
                divisor = 1;
            }
            result *= resultPart / divisor;
        }

        return result;
    }
    
    private static long FindGreatestCommonDivisor(long a, long b) {
        if (a == 0) {
            return b;
        }

        if (b == 0) {
            return a;
        }

        if (a < b) {
            return FindGreatestCommonDivisor(b, a);
        }

        return FindGreatestCommonDivisor(b, a % b);
    }
}