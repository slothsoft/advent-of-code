using System;
using System.Collections.Generic;
using System.Linq;

namespace AoC.day3;

/// <summary>
/// <a href="https://adventofcode.com/2025/day/3">Day 3: Lobby</a>
/// </summary>
public class Lobby {
    internal record BatteryBank(params int[] Batteries) {
        
        internal BatteryBank(string input) : this(input.Select(b => int.Parse(b.ToString())).ToArray()) {}
        
        public long CalculateJoltage() {
            var firstDigit = Batteries.Take(Batteries.Length -1).Max();
            var firstDigitIndex = Array.IndexOf(Batteries, firstDigit);
            var secondDigit = Batteries.Skip(firstDigitIndex + 1).Max();
            return firstDigit * 10 + secondDigit;
        }
    }

    public Lobby(IEnumerable<string> input) {
        Input = ParseInput(input);
    }

    internal BatteryBank[] Input { get; }

    internal static BatteryBank[] ParseInput(IEnumerable<string> input) {
        return input.Select(s => new BatteryBank(s)).ToArray();
    }

    public long CalculateTotalJoltage() {
        return Input.Select(i => i.CalculateJoltage()).Sum();
    }
}

public static class LobbyExtensions {
    public static long Calculate(this int value) {
        return value + 1;
    }
}
