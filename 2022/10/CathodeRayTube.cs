using System;
using System.Collections.Generic;
using System.Linq;

namespace AoC._10;

/// <summary>
/// <a href="https://adventofcode.com/2022/day/10">Day 10: Cathode-Ray Tube</a>: Start by figuring out the signal
/// being sent by the CPU. The CPU has a single register, X, which starts with the value 1.
/// </summary>
public static class CathodeRayTube {
    private interface ICommand {
        public static ICommand Parse(string line) {
            if (line.StartsWith("noop"))
                return new NoopCommand();
            if (line.StartsWith("addx"))
                return new AddXCommand(int.Parse(line.Split(" ")[1]));
            throw new ArgumentException("Cannot parse line: " + line);
        }

        internal int CyclesNeeded { get; }

        int Execute(int value);
    }

    private class NoopCommand : ICommand {
        public int CyclesNeeded => 1;

        public int Execute(int value) {
            return value; // noop
        }
    }

    private class AddXCommand : ICommand {
        private readonly int _x;

        public AddXCommand(int x) {
            _x = x;
        }

        public int CyclesNeeded => 2;

        public int Execute(int value) {
            return value + _x;
        }
    }

    public static IReadOnlyDictionary<int, int> ExecuteProgram(string[] lines, params int[] testedCycles) {
        var result = new Dictionary<int, int>();

        var value = 1;
        var currentCycle = 0;
        var testedCycleIndex = 0;

        for (var i = 0; i < lines.Length; i++) {
            var command = ICommand.Parse(lines[i]);

            while (testedCycles[testedCycleIndex] <= currentCycle + command.CyclesNeeded) {
                // the command will execute only after the next tested cycle, so get the value NOW
                result[testedCycles[testedCycleIndex]] = value;
                testedCycleIndex++;

                if (testedCycleIndex >= testedCycles.Length) {
                    // are done with testing
                    return result;
                }
            }

            // execute the command
            currentCycle += command.CyclesNeeded;
            value = command.Execute(value);
        }

        // now all commands where executed - if there are still tested cycles, they will all have the same value
        for (var i = testedCycleIndex; i < testedCycles.Length; i++) {
            result[testedCycles[i]] = value;
        }

        return result;
    }

    internal static int CalculateSignalStrength(this IReadOnlyDictionary<int, int> values) {
        return values.Select(e => e.Key * e.Value).Sum();
    }

    public static string RenderProgram(string[] lines) {
        const int cyclesPerLine = 40;
        const int maxCycle = 6 * cyclesPerLine;
        var values = ExecuteProgram(lines, Enumerable.Range(1, maxCycle).ToArray());
        var result = "";

        for (var i = 0; i < maxCycle; i++) {
            var pixel = i % cyclesPerLine;
            
            if (i > 0 && pixel == 0) {
                // start new line
                result += "\r\n";
            }

            if (Math.Abs(values[i + 1] - pixel) <= 1) {
                // lit pixel
                result += '#';
            } else {
                // dark pixel D:
                result += '.';
            }
        }

        return result;
    }
}