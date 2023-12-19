using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Newtonsoft.Json;

namespace AoC;

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
    }

    public interface IRule {
        string GoTo { get; }
        bool RatePart(IDictionary<string, int> part);
    }

    public record IfRule(string VariableName, Operator Operator, int Value, string GoTo) : IRule {
        public bool RatePart(IDictionary<string, int> part) => Operator.ApplyTo(part[VariableName], Value);
    }

    public record GoToRule(string GoTo) : IRule {
        public bool RatePart(IDictionary<string, int> part) => true;
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

    public long CalculateAcceptedCombinations() {
        var variableBorders = CalculateVariableBorders();

        var result = 0L;
        var lastX = 1L;

        // i have the feeling this loop-de-loop could be done much nicer / better / more generic
        foreach (var x in variableBorders[VARIABLE_X]) {
            var lastM = 1L;
            foreach (var m in variableBorders[VARIABLE_M]) {
                var lastA = 1L;
                foreach (var a in variableBorders[VARIABLE_A]) {
                    var lastS = 1L;
                    foreach (var s in variableBorders[VARIABLE_S]) {
                        if (RatePart(new Dictionary<string, int> {{VARIABLE_X, x}, {VARIABLE_M, m}, {VARIABLE_A, a}, {VARIABLE_S, s}})) {
                            // so now this part since the last variables is accepted, so add the possibilities to result
                            result += (x - lastX) * (m - lastM) * (a - lastA) * (s - lastS);
                        }
                        lastS = s;
                    }

                    lastA = a;
                }

                lastM = m;
            }

            lastX = x;
        }

        foreach (var variableSortedByBorder in variableBorders) {
            Console.WriteLine(variableSortedByBorder.Key + ": " + string.Join(", ", variableSortedByBorder.Value)
            );
        }

        return result;
    }

    private IDictionary<string, ISet<int>> CalculateVariableBorders(int min = 1, int max = 4000) {
        var dictionary = new Dictionary<string, ISet<int>>();
        foreach (var workflow in Workflows.Values) {
            foreach (var rule in workflow.Rules) {
                if (rule is IfRule ifRule) {
                    if (!dictionary.ContainsKey(ifRule.VariableName)) {
                        dictionary[ifRule.VariableName] = new SortedSet<int> {max};
                    }

                    // we add the value before the one that actually changes the result
                    var lowerBorder = ifRule.Operator.DisplayChar == '>' ? ifRule.Value : ifRule.Value - 1;
                    dictionary[ifRule.VariableName].Add(lowerBorder);
                }
            }
        }

        return dictionary;
    }
}