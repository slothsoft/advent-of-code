using System;
using System.Collections.Generic;
using System.Linq;

namespace AoC._21;

/// <summary>
/// <a href="https://adventofcode.com/2022/day/21">Day 21: Monkey Math</a>
/// </summary>
public class MonkeyMath {
    public record Monkey(string Name, Func<long> CalculateValue) {
        public long Value => CalculateValue();
        public Func<long> CalculateValue { get; set; } = CalculateValue;
    }

    public const string MonkeyHuman = "humn";
    public const string MonkeyRoot = "root";

    private readonly bool _secondRiddle;
    public readonly IDictionary<string, Monkey> Monkeys;
    public readonly string MonkeyLeftOfRoot;
    public readonly string MonkeyRightOfRoot;

    public MonkeyMath(string[] lines, bool secondRiddle = false) {
        _secondRiddle = secondRiddle;
        Monkeys = lines.Select(ParseMonkey).ToDictionary(m => m.Name, m => m);

        var split = lines.Single(l => l.StartsWith("root")).Split(" ");
        MonkeyLeftOfRoot = split[1];
        MonkeyRightOfRoot = split[^1];
    }

    private Monkey ParseMonkey(string monkeyString) {
        var colonIndex = monkeyString.IndexOf(':');
        var monkeyName = monkeyString.Substring(0, colonIndex);
        var monkeyOperation = monkeyString.Substring(colonIndex + 2);

        if (monkeyOperation.Contains(' ')) {
            // since there is a space, the monkey has an operation

            if (monkeyName.Equals(MonkeyRoot) && _secondRiddle) {
                // fix the wrong operation
                monkeyOperation = monkeyOperation.Replace("+", "=");
            }

            return new Monkey(monkeyName, CreateMonkeyOperation(monkeyOperation));
        }

        // if there is no space, it should only be a number
        return new Monkey(monkeyName, () => long.Parse(monkeyOperation));
    }

    private Func<long> CreateMonkeyOperation(string monkeyOperation) {
        var split = monkeyOperation.Split(" ");
        var monkey1 = split[0];
        var monkey2 = split[2];

        return split[1] switch {
            "+" => () => Monkeys[monkey1].Value + Monkeys[monkey2].Value,
            "-" => () => Monkeys[monkey1].Value - Monkeys[monkey2].Value,
            "*" => () => Monkeys[monkey1].Value * Monkeys[monkey2].Value,
            "/" => () => Monkeys[monkey1].Value / Monkeys[monkey2].Value,
            "=" => () => Monkeys[monkey1].Value == Monkeys[monkey2].Value ? 1 : 0,
            _ => () => throw new ArgumentException("Do not know operator " + split[1]),
        };
    }

    public long FindHumanNumber(long start, long end) {
        Monkeys[MonkeyHuman].CalculateValue = () => start;
        var leftStart = Monkeys[MonkeyLeftOfRoot].Value;
        var rightStart = Monkeys[MonkeyRightOfRoot].Value;

        Monkeys[MonkeyHuman].CalculateValue = () => end;
        var leftEnd = Monkeys[MonkeyLeftOfRoot].Value;
        var rightEnd = Monkeys[MonkeyRightOfRoot].Value;

        if (leftStart == leftEnd) {
            // we make sure that the value for the first value is smaller than the other one 
            return rightStart < rightEnd
                ? ApproachHumanNumber(MonkeyRightOfRoot, leftStart, start, end)
                : ApproachHumanNumber(MonkeyRightOfRoot, leftStart, end, start);
        }

        if (rightStart == rightEnd) {
            // we make sure that the value for the first value is smaller than the other one 
            return leftStart < leftEnd
                ? ApproachHumanNumber(MonkeyLeftOfRoot, rightStart, start, end)
                : ApproachHumanNumber(MonkeyLeftOfRoot, rightStart, end, start);
        }

        throw new ArgumentException("We expect that the value of one of the monkeys doesn't change.");
    }

    private long ApproachHumanNumber(string monkey, long targetValue, long startHumanNumber, long endHumanNumber) {
        // startHumanNumber is smaller than targetValue and endHumanNumber is bigger
        long middleHumanNumber;
        long middleValue;

        do {
            middleHumanNumber = (startHumanNumber + endHumanNumber) / 2;
            Monkeys[MonkeyHuman].CalculateValue = () => middleHumanNumber;
            middleValue = Monkeys[monkey].Value;

            if (middleValue < targetValue) {
                // if middle value is still smaller, it can be the new start
                startHumanNumber = middleHumanNumber;
            } else if (middleValue > targetValue) {
                // if middle value is still bigger, it can be the new end
                endHumanNumber = middleHumanNumber;
            }
        } while (middleValue != targetValue);

        // see if we can use a smaller number
        do {
            var possibleHumanNumber = middleHumanNumber - 1;
            Monkeys[MonkeyHuman].CalculateValue = () => possibleHumanNumber;
            var possibleValue = Monkeys[monkey].Value;
            if (possibleValue == targetValue) {
                middleHumanNumber = possibleHumanNumber;
            } else {
                break;
            }
        } while (true);

        return middleHumanNumber;
    }
}