using System;
using System.Collections.Generic;
using System.Linq;

namespace AoC.day1;

/// <summary>
/// <a href="https://adventofcode.com/2025/day/1">Day 1: Secret Entrance</a>
/// </summary>
public class SecretEntrance {
    private record DialMove(Direction Direction, int Value) {
        internal int Execute(int currentValue) {
            return Direction.Execute(currentValue, Value);
        }
    }
    private record Direction(char Name, Func<int, int, int> Execute);

    private const int MAX_SAFE_NUMBER = 100;
    private static readonly Direction DirectionLeft = new('L', (original, value) => original - value);

    private static readonly Direction DirectionRight = new('R', (original, value) => original + value);

    public SecretEntrance(IEnumerable<string> input) {
        Input = ParseInput(input);
    }

    private DialMove[] Input { get; }

    private static DialMove[] ParseInput(IEnumerable<string> input) {
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
            currentValue = dialMove.Execute(currentValue) % MAX_SAFE_NUMBER;
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
            var fullHundred = dialMove.Value / MAX_SAFE_NUMBER;
            result += fullHundred;
            
            var nextValue = dialMove.Execute(currentValue);
            
            // we ignore full hundreds
            var minusOrPlusOne = dialMove.Direction.Execute(0, 1);
            nextValue -=  minusOrPlusOne * fullHundred * MAX_SAFE_NUMBER;
            
            // we land DIRECTLY on zero
            if (nextValue == 0) {
                result++;
            }

            // if we move LEFT and do not start at zero, we pass zero if returning value is negative
            if (nextValue < 0 && currentValue > 0) {
                result++;
            }
            
            // if we move RIGHT than everything over 100 passes zero
            if (nextValue >= MAX_SAFE_NUMBER) {
                result++;
            }
            
            currentValue = (nextValue + MAX_SAFE_NUMBER) % MAX_SAFE_NUMBER;
        }
        return result;
    }
}
