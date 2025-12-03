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
        
        public long CalculateJoltage(int batteryCount) {
            var result = 0L;
            var currentBatteries = Batteries;
            
            for (var battery = 0; battery < batteryCount; battery++) {
                var digit = currentBatteries.Take(currentBatteries.Length - batteryCount + battery + 1).Max();
                var digitIndex = Array.IndexOf(currentBatteries, digit);
                currentBatteries = currentBatteries.Skip(digitIndex + 1).ToArray();

                result = result * 10 + digit;
            }
            
            return result;
        }
    }

    public Lobby(IEnumerable<string> input) {
        Input = ParseInput(input);
    }

    internal BatteryBank[] Input { get; }

    internal static BatteryBank[] ParseInput(IEnumerable<string> input) {
        return input.Select(s => new BatteryBank(s)).ToArray();
    }

    public long CalculateTotalJoltage(int batteryCount) {
        return Input.Select(i => i.CalculateJoltage(batteryCount)).Sum();
    }
}