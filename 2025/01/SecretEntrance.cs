using System;
using System.Collections.Generic;
using System.Linq;

namespace AoC.day1;

/// <summary>
/// <a href="https://adventofcode.com/2025/day/1">Day 1: Secret Entrance</a>
/// </summary>
public class SecretEntrance {
    internal record DialMove(Direction Direction, int Value) {
        internal int Execute(int currentValue) {
            return Direction.Execute(currentValue, Value);
        }
    }
    internal record Direction(char Name, Func<int, int, int> Execute);

    internal const int MaxSafeNumber = 100;
    internal static readonly Direction DirectionLeft = new('L', (original, value) => original - value);

    internal static readonly Direction DirectionRight = new('R', (original, value) => original + value);

    public SecretEntrance(IEnumerable<string> input) {
        Input = ParseInput(input);
    }

    internal DialMove[] Input { get; }

    internal static DialMove[] ParseInput(IEnumerable<string> input) {
        return input
            .Where(s => !string.IsNullOrWhiteSpace(s))
            .Select(s => new DialMove(ParseDirection(s[0]), int.Parse(s[1..])))
            .ToArray();
    }
    
    private static Direction ParseDirection(char input) {
        return input == DirectionLeft.Name ? DirectionLeft : DirectionRight;
    }

    public int Calculate(int startValue = 50) {
        var currentValue = startValue;
        var result = 0;
        
        foreach (var dialMove in Input) {
            currentValue = dialMove.Execute(currentValue) % MaxSafeNumber;
            if (currentValue == 0) {
                result++;
            }
        }
        return result;
    }
    
    public int Calculate0x434C49434B(int startValue = 50) {
        var currentValue = startValue;
        var result = 0;
        
        foreach (var dialMove in Input) {
            // if movement is more than 100, then we HAVE to pass zero
            result += dialMove.Value / MaxSafeNumber;
            
            var nextValue = dialMove.Execute(currentValue);
            
            // if we move LEFT and do not start at zero, we pass zero if returning value is negative
            if (nextValue < 0 && currentValue > 0) {
                result++;
            }
            while (nextValue < 0) {
                nextValue += MaxSafeNumber;
            }
            
            // if we move RIGHT than everything over 100 passes zero
            if (nextValue > MaxSafeNumber) {
                result++;
            }
            while (nextValue >= MaxSafeNumber) {
                nextValue -= MaxSafeNumber;
            }
            
            // we land directly on zero
            if (nextValue == 0) {
                result++;
            }
            
            Console.WriteLine(dialMove.Direction.Name.ToString() + dialMove.Value + " => " + nextValue + " (" + result + ")");
            currentValue = nextValue % MaxSafeNumber;
        }
        return result;
    }
}
