using System;
using System.Collections.Generic;
using System.Linq;

namespace AoC._18;

/// <summary>
/// <a href="https://adventofcode.com/2021/day/18">Day 18: Snailfish</a>: You descend into the ocean trench
/// and encounter some snailfish. They say they saw the sleigh keys! They'll even tell you which direction
/// the keys went if you help one of the smaller snailfish with his math homework.
/// </summary>
public static class Snailfish {

    internal static bool EnableLogging = false;
    
    public interface ISnailfishNumber {
        SnailfishPair? Parent { get; set; }
        int Magnitude { get; }
        string Stringify();
        IEnumerable<ISnailfishNumber> Traverse();
    }

    public class SnailfishNumber : ISnailfishNumber {
        public int Value { get; set; }
        public SnailfishPair? Parent { get; set; }
        public int Magnitude => Value;
        public string Stringify() => Value.ToString();
        public override string ToString() => Stringify();
        public SnailfishNumber(int value) {
            Value = value;
        }

        public IEnumerable<ISnailfishNumber> Traverse() {
            yield return this;
        }
    }

    public class SnailfishPair : ISnailfishNumber {
        public SnailfishPair? Parent { get; set; }
        internal int Nesting => Parent?.Nesting + 1 ?? 0;
        public int Magnitude => 3 * Left.Magnitude + 2 * Right.Magnitude;
        public ISnailfishNumber Left { get; set; }
        public ISnailfishNumber Right { get; set; }

        public SnailfishPair(ISnailfishNumber left, ISnailfishNumber right) {
            Left = left;
            Left.Parent = this;
            Right = right;
            Right.Parent = this;
        }

        public string Stringify() => $"[{Left.Stringify()},{Right.Stringify()}]";
        public override string ToString() => Stringify();

        public IEnumerable<ISnailfishNumber> Traverse() {
            foreach (var lefty in Left.Traverse()) {
                yield return lefty;
            }
            yield return this;
            
            foreach (var righty in Right.Traverse()) {
                yield return righty;
            }
        }
    }

    public static ISnailfishNumber[] ParseNumbers(string[] lines) {
        return lines.Select(ParseNumber).ToArray();
    }

    public static ISnailfishNumber ParseNumber(string line) {
        if (line.StartsWith("[")) {
            // this line contains another pair
            var elements = SplitList(line.Substring(1, line.Length - 2))
                .Where(l => l.Length > 0)
                .Select(ParseNumber).ToArray();
            return new SnailfishPair(elements[0], elements[1]);
        }

        try {
            // this line contains a single number
            return new SnailfishNumber(int.Parse(line));
        } catch (FormatException e) {
            throw new FormatException(e.Message + $" ({line})", e.InnerException);
        }
    }

    private static IEnumerable<string> SplitList(string line) {
        var bracketCounter = 0;
        var currentString = "";

        for (var i = 0; i < line.Length; i++) {
            switch (line[i]) {
                case '[':
                    currentString += line[i];
                    bracketCounter++;
                    break;
                case ']':
                    currentString += line[i];
                    bracketCounter--;
                    break;
                case ',' when bracketCounter == 0:
                    yield return currentString;
                    currentString = "";
                    break;
                default:
                    currentString += line[i];
                    break;
            }
        }

        if (currentString != "") {
            yield return currentString;
        }
    }

    // Extension methods for the snailfish numbers

    public static ISnailfishNumber Add(this ISnailfishNumber left, ISnailfishNumber right) {
        var result = new SnailfishPair(left, right);
        result.Reduce();
        return result;
    }

    public static ISnailfishNumber Sum(this ISnailfishNumber[] numbers) {
        var result = numbers[0];
        for (var i = 1; i < numbers.Length; i++) {
            result = result.Add(numbers[i]);
        }

        return result;
    }

    public static void Reduce(this ISnailfishNumber number) {
        bool somethingHappened;
        do {
            if (EnableLogging)
                Console.WriteLine(number.Stringify());
            somethingHappened = number.Explode();
            if (somethingHappened) {
                continue;
            }
            somethingHappened = number.Split();
        } while (somethingHappened);
    }

    private static bool Explode(this ISnailfishNumber number) {
        foreach (var child in number.Traverse().OfType<SnailfishPair>()) {
            var somethingHappened = ExplodePropertyIfNecessary(number, child, p => p.Left, (p, v) => p.Left = v);
            if (somethingHappened) {
                return true;
            }
            somethingHappened = ExplodePropertyIfNecessary(number, child, p => p.Right, (p, v) => p.Right = v);
            if (somethingHappened) {
                return true;
            }
        }
        return false;
    }

    private static bool ExplodePropertyIfNecessary(ISnailfishNumber root, SnailfishPair parent, Func<SnailfishPair, ISnailfishNumber> getter,
        Action<SnailfishPair, ISnailfishNumber> setter) {
        var child = getter(parent);

        if (child is SnailfishPair {Nesting: >= 4, Left: SnailfishNumber, Right: SnailfishNumber} childPair) {
            var newChild = new SnailfishNumber(0) {
                Parent = parent
            };
            setter(parent, newChild);

            var left = root.GetLeftNumber(getter(parent));
            if (left != null) {
                left.Value += ((SnailfishNumber) childPair.Left).Value;
            }

            var right = root.GetRightNumber(getter(parent));
            if (right != null) {
                right.Value += ((SnailfishNumber) childPair.Right).Value;
            }

            return true;
        }

        return false;
    }

    private static SnailfishNumber? GetLeftNumber(this ISnailfishNumber root, ISnailfishNumber number) {
        var allNumbers = root.Traverse().OfType<SnailfishNumber>().ToArray<ISnailfishNumber>();
        var indexOfNumber = Array.IndexOf(allNumbers, number);
        return indexOfNumber > 0 ? (SnailfishNumber) allNumbers[indexOfNumber - 1] : null;
    }

    private static SnailfishNumber? GetRightNumber(this ISnailfishNumber root, ISnailfishNumber number) {
        var allNumbers = root.Traverse().OfType<SnailfishNumber>().ToArray<ISnailfishNumber>();
        var indexOfNumber = Array.IndexOf(allNumbers, number);
        return indexOfNumber < allNumbers.Length - 1 ? (SnailfishNumber) allNumbers[indexOfNumber + 1] : null;
    }

    private static bool Split(this ISnailfishNumber number) {
        foreach (var snailfishNumber in number.Traverse().OfType<SnailfishPair>()) {
            var somethingHappened = SplitPropertyIfNecessary(snailfishNumber, p => p.Left, (p, v) => p.Left = v);
            if (somethingHappened) {
                return true;
            }
            somethingHappened = SplitPropertyIfNecessary(snailfishNumber, p => p.Right, (p, v) => p.Right = v);
            if (somethingHappened) {
                return true;
            }
        }
        return false;
    }

    private static bool SplitPropertyIfNecessary(SnailfishPair parent, Func<SnailfishPair, ISnailfishNumber> getter,
        Action<SnailfishPair, ISnailfishNumber> setter) {
        var child = getter(parent);

        if (child is SnailfishNumber {Value: > 9} childNumber) {
            setter(parent, new SnailfishPair(
                new SnailfishNumber((int) Math.Floor(childNumber.Value / 2.0)),
                new SnailfishNumber((int) Math.Ceiling(childNumber.Value / 2.0))
            ) {
                Parent = parent
            });
            return true;
        }

        return false;
    }

    public static int CalculateLargestMagnitude(this ISnailfishNumber[] numbers) {
        var result = 0;
        for (var i = 0; i < numbers.Length; i++) {
            for (var j = 0; j < numbers.Length; j++) {
                if (i == j) {
                    continue;
                }

                var copyOfNumberI = ParseNumber(numbers[i].Stringify());
                var copyOfNumberJ = ParseNumber(numbers[j].Stringify());
                var magnitude = copyOfNumberI.Add(copyOfNumberJ).Magnitude;
                result = Math.Max(result, magnitude);
            }
        }

        return result;
    }
}