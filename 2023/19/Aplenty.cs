using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;

namespace AoC.day19;

/// <summary>
/// <a href="https://adventofcode.com/2023/day/19">Day 19: Aplenty</a>:
/// Sort through all of the parts you've been given; what do you get if you add together all of the rating numbers for all of the parts
/// that ultimately get accepted?
/// </summary>
public class Aplenty {
    public record Workflow(string DisplayName, params IRule[] Rules) {
        public string RatePart(IDictionary<string, int> part) {
            foreach (var rule in Rules) {
                if (rule.RatePart(part)) {
                    return rule.GoTo;
                }
            }

            throw new ArgumentException($"Workflow {DisplayName} is broken - no rule fits for {part}");
        }

        public IEnumerable<(IDictionary<string, Range<long>> Range, string GoTo)> RatePart(IDictionary<string, Range<long>> ranges) {
            var currentRanges = ranges;
            foreach (var rule in Rules) {
                var rangeSplit = rule.RatePart(currentRanges);
                if (rangeSplit.TrueRange != null) {
                    yield return (rangeSplit.TrueRange, rule.GoTo);
                }

                if (rangeSplit.FalseRange != null) {
                    currentRanges = rangeSplit.FalseRange;
                } else {
                    // we have no range left for coming rules D: 
                    break;
                }
            }
        }

        // cq{a<289:R,a<499:A,m>826:A,A}
        public override string ToString() => DisplayName + "{" + string.Join(", ", Rules.Select(r => r.ToString()) + "}");
    }

    public interface IRule {
        string GoTo { get; }
        bool RatePart(IDictionary<string, int> part);
        (IDictionary<string, Range<long>>? TrueRange, IDictionary<string, Range<long>>? FalseRange) RatePart(IDictionary<string, Range<long>> ranges);
    }

    public record IfRule(string VariableName, Operator Operator, int Value, string GoTo) : IRule {
        public bool RatePart(IDictionary<string, int> part) => Operator.ApplyTo(part[VariableName], Value);

        public (IDictionary<string, Range<long>>? TrueRange, IDictionary<string, Range<long>>? FalseRange) RatePart(IDictionary<string, Range<long>> ranges) {
            if (Operator.lessThan.Equals(Operator)) {
                if (ranges[VariableName].Contains(Value)) {
                    // simple split
                    var trueRange = ranges.ToDictionary(r => r.Key, r => r.Value);
                    trueRange[VariableName] = new Range<long>(ranges[VariableName].From, Value - 1);
                    var falseRange = ranges.ToDictionary(r => r.Key, r => r.Value);
                    falseRange[VariableName] = new Range<long>(Value, ranges[VariableName].To);
                    return (trueRange, falseRange);
                }

                // the value is either completely before the range or completely after
                if (ranges[VariableName].To < Value) {
                    return (ranges, null);
                }

                return (null, ranges);
            }

            if (ranges[VariableName].Contains(Value)) {
                // simple split
                var falseRange = ranges.ToDictionary(r => r.Key, r => r.Value);
                falseRange[VariableName] = new Range<long>(ranges[VariableName].From, Value);
                var trueRange = ranges.ToDictionary(r => r.Key, r => r.Value);
                trueRange[VariableName] = new Range<long>(Value + 1, ranges[VariableName].To);
                return (trueRange, falseRange);
            }

            // the value is either completely before the range or completely after
            if (ranges[VariableName].From > Value) {
                return (ranges, null);
            }

            return (null, ranges);
        }

        public override string ToString() => VariableName + " " + Operator.DisplayChar + " " + Value;
    }

    public record GoToRule(string GoTo) : IRule {
        public bool RatePart(IDictionary<string, int> part) => true;

        public (IDictionary<string, Range<long>>? TrueRange, IDictionary<string, Range<long>>? FalseRange) RatePart(IDictionary<string, Range<long>> ranges) =>
            (ranges, null);

        public override string ToString() => GoTo;
    }

    public struct Operator {
        public static Operator lessThan = new('<', (a, b) => a < b);
        public static Operator greaterThan = new('>', (a, b) => a > b);
        public static Operator[] all = {lessThan, greaterThan};

        public static Operator ForDisplayChar(char displayChar) {
            foreach (var op in all) {
                if (displayChar == op.DisplayChar) {
                    return op;
                }
            }

            throw new ArgumentException("Do not know display char " + displayChar);
        }

        private Func<int, int, bool> _apply;

        public Operator(char displayChar, Func<int, int, bool> apply) {
            DisplayChar = displayChar;
            _apply = apply;
        }

        public char DisplayChar { get; }
        public bool ApplyTo(int a, int b) => _apply(a, b);
        public override string ToString() => $"Operator({DisplayChar})";
    }

    internal const string VARIABLE_X = "x";
    internal const string VARIABLE_M = "m";
    internal const string VARIABLE_A = "a";
    internal const string VARIABLE_S = "s";

    internal const string WORKFLOW_START = "in";
    internal const string WORKFLOW_ACCEPTED = "A";
    internal const string WORKFLOW_REJECTED = "R";

    public Aplenty(IEnumerable<string> input) {
        var inputAsArray = input.ToArray();
        Workflows = ParseWorkflows(inputAsArray);
        Parts = ParseParts(inputAsArray).ToList();
    }

    public IDictionary<string, Workflow> Workflows { get; }
    public IList<IDictionary<string, int>> Parts { get; }

    private static IDictionary<string, Workflow> ParseWorkflows(IEnumerable<string> input) {
        var result = new List<Workflow>();
        foreach (var line in input) {
            if (line.Trim().Equals(string.Empty)) {
                break;
            }

            result.Add(ParseWorkflow(line));
        }

        return result.ToDictionary(r => r.DisplayName, r => r);
    }

    private IEnumerable<IDictionary<string, int>>? ParseParts(IEnumerable<string> input) {
        var parseParts = false;
        foreach (var line in input) {
            if (line.Trim().Equals(string.Empty)) {
                parseParts = true;
            } else if (parseParts) {
                // {x=787,m=2655,a=1222,s=2876}
                yield return JsonConvert.DeserializeObject<Dictionary<string, int>>(line.Replace('=', ':'));
            }
        }
    }

    internal static Workflow ParseWorkflow(string input) {
        // "ex{x>10:one,m<20:two,a>30:R,A}"
        var split = input.Split('{');
        return new Workflow(split[0], split[1][..^1].Split(",").Select(ParseRule).ToArray());
    }

    private static IRule ParseRule(string input) {
        // "x>10:one" or "A"
        var split = input.Split(':');
        if (split.Length > 1) {
            // "x>10:one"
            return new IfRule(split[0][0].ToString(), Operator.ForDisplayChar(split[0][1]), int.Parse(split[0][2..]), split[1]);
        }

        // "A"
        return new GoToRule(input);
    }

    internal bool RatePart(IDictionary<string, int> part) {
        var workflow = WORKFLOW_START;
        while (true) {
            workflow = Workflows[workflow].RatePart(part);

            if (WORKFLOW_ACCEPTED.Equals(workflow)) {
                return true;
            }

            if (WORKFLOW_REJECTED.Equals(workflow)) {
                return false;
            }
        }
    }

    public long CalculateRatingNumber() {
        var result = 0L;
        foreach (var part in Parts) {
            if (RatePart(part)) {
                result += part.Values.Sum();
            }
        }

        return result;
    }

    // if we split all the ranges for all the rules we get about 250 ranges per letter
    // which makes still 3_906_250_000 parts to check
    // -> so only split ranges if necessary

    public long CalculateAcceptedCombinations() {
        var ranges = Parts.SelectMany(p => p.Keys).Distinct().ToDictionary(p => p, _ => new Range<long>(1, 4000));
        return RatePartForRanges(ranges, WORKFLOW_START);
    }

    private long RatePartForRanges(IDictionary<string, Range<long>> ranges, string currentWorkflow) {
        if (WORKFLOW_ACCEPTED.Equals(currentWorkflow)) {
            return CalculatePartCount(ranges);
        }

        if (WORKFLOW_REJECTED.Equals(currentWorkflow)) {
            return 0L;
        }

        var result = 0L;
        var workflow = Workflows[currentWorkflow];
        var newRangesToGoTo = workflow.RatePart(ranges);
        foreach (var (newRanges, goTo) in newRangesToGoTo) {
            result += RatePartForRanges(newRanges, goTo);
        }

        return result;
    }

    internal static long CalculatePartCount(IDictionary<string, Range<long>> ranges) {
        return ranges.Values.Select(r => r.To - r.From + 1).Aggregate((a, b) => a * b);
    }
}