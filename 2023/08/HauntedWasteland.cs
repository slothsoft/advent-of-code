using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;

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
        var result = 0L;
        var current = _map.Keys.Where(n => n[2] == startEndsWith).ToArray();
        
        while (current.Any(n => n[2] != finishEndsWith)) {
            for (var i = 0; i < current.Length; i++) {
                current[i] = _map[current[i]].Direction(_instructions[(int)(result % _instructions.Length)]);
            }
            result++;
        }

        return result;
    }
}