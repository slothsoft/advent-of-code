   using System;
   using System.Collections.Generic;
using System.Linq;

namespace AoC.day7;

/// <summary>
/// <a href="https://adventofcode.com/2024/day/7">Day 7: Bridge Repair</a>
/// </summary>
public class BridgeRepair {
    
    internal struct Operator(string Name, Func<long, long, long> apply) {
        public long Apply(long a, long b) => apply(a, b);
        public override string ToString() => Name;
    }
    internal record Equation(long TestValue, long[] Numbers);

    public BridgeRepair(IEnumerable<string> input) {
        Operators = [
            new Operator("+", (a,b) => a + b),
            new Operator("*", (a,b) => a * b),
        ];
        Input = ParseInput(input);
    }

    internal Equation[] Input { get; }
    internal Operator[] Operators { get; }

    internal static Equation[] ParseInput(IEnumerable<string> input) {
        return input.Select(s => {
            var split = s.Split(": ");
            return new Equation(split[0].ExtractDigitsAsLong(), split[1].ParseLongArray());
        }).ToArray();
    }

    public long CalculateCalibrationResult() {
        return Input.Where(e => CanBeTrue(e)).Sum(e => e.TestValue);
    }

    private bool CanBeTrue(Equation equation, long? currentValue = null, long currentOperatorIndex = 0) {
        if (currentOperatorIndex >= equation.Numbers.Length - 1) {
            // there are only operators after the [0th, 1st, 2nd... (L - 2)th] element, not after (L - 1), the last element
            return currentValue == equation.TestValue;
        }

        currentValue ??= equation.Numbers[0];

        // if there are still operators missing, check iteratively (?) if any work
        foreach (var op in Operators) {
            if (CanBeTrue(equation, op.Apply(currentValue.Value, equation.Numbers[currentOperatorIndex + 1]), currentOperatorIndex + 1)) {
                return true;
            }
        }

        return false;
    }
}
