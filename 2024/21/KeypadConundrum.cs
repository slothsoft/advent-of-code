using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AoC.day21;

/// <summary>
/// <a href="https://adventofcode.com/2024/day/21">Day 21: Keypad Conundrum</a>
/// </summary>
public class KeypadConundrum {
    internal static char activationButton = 'A';
    internal record Keypad {
        internal Keypad(string keypadAsString) {
            Keys = keypadAsString.Split("$n").ParseMatrix(c => c == ' ' ? (char?) null : c);
            (int x, int y)? activationPosition = null;
            for (var x = 0; x < Keys.Length; x++) {
                for (var y = 0; y < Keys[x].Length; y++) {
                    if (Keys[x][y] != null) {
                        KeyPositions[Keys[x][y]!.Value] = (x, y);
                    }
                    if (Keys[x][y] == 'A') {
                        activationPosition = (x, y);
                    }
                }
            }

            ActivationPosition = activationPosition ?? throw new Exception("Could not find initial position!");
        }
        
        internal char?[][] Keys { get; }
        internal (int x, int y) ActivationPosition { get; }
        internal IDictionary<char, (int x, int y)> KeyPositions { get; } = new Dictionary<char, (int x, int y)>();
    }

    internal static Keypad numericKeypad = new("789$n456$n123$n 0" + activationButton);
    internal static Keypad directionalKeypad = new(" ^" + activationButton + "$n<v>");
    
    internal struct Direction(char value, Func<int, int> modifyX, Func<int, int> modifyY) {
        public (int X, int Y) Modify(int oldX, int oldY) => (
            modifyX(oldX), 
            modifyY(oldY)
        );
        public override string ToString() => value.ToString();
    }
    internal static Direction north = new('^', x => x, y => y - 1);
    internal static Direction south = new('v', x => x, y => y + 1);
    internal static Direction east = new('>', x => x + 1, y => y);
    internal static Direction west = new('<', x => x - 1, y => y);
    internal static Direction[] allDirections = [north, south, east, west];

    private IDictionary<char, Direction[]> _directionPriority = new Dictionary<char, Direction[]>();
    
    public KeypadConundrum(IEnumerable<string> input) {
        Input = input.ToArray();
    }

    internal string[] Input { get; }

    public long CalculateComplexityOfInput() {
        return 7;
    }
    
    internal static long CalculateComplexity(string code) {
        var result =  
            // what the cool robot (-40°C) needs to press
            CalculateShortestPath(directionalKeypad, 
            // what the highly radiated robot needs to press
            CalculateShortestPath(directionalKeypad, 
            // what the depressurized robot needs to press
            CalculateShortestPath(numericKeypad, code)));
        return code.ExtractDigitsAsLong() * result.Length;
    }
    
    internal static string CalculateShortestPath(Keypad keypad, string code) {
        var currentPosition = keypad.ActivationPosition;
        var result = new StringBuilder();
        foreach (var c in code) {
            currentPosition = MoveTo(result, currentPosition, keypad.KeyPositions[c]);
            result.Append('A');
        }

        return result.ToString();
    }

    private static (int x, int y) MoveTo(StringBuilder result, (int x, int y) currentPosition, (int x, int y) targetPosition) {
        // the order of north / south / east / west does not matter for the shortest part
        while (currentPosition.x != targetPosition.x || currentPosition.y != targetPosition.y) {
            while (currentPosition.x > targetPosition.x) {
                result.Append(west);
                currentPosition.x--;
            }
            while (currentPosition.y > targetPosition.y) {
                result.Append(north);
                currentPosition.y--;
            }
            while (currentPosition.y < targetPosition.y) {
                result.Append(south);
                currentPosition.y++;
            }
            while (currentPosition.x < targetPosition.x) {
                result.Append(east);
                currentPosition.x++;
            }
        }
        return currentPosition;
    }
}

public static class KeypadConundrumExtensions {
    public static long Calculate(this int value) {
        return value + 1;
    }
}
