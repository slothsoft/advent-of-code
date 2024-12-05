using System;
using System.Collections.Generic;
using System.Linq;

namespace AoC.day5;

/// <summary>
/// <a href="https://adventofcode.com/2024/day/5">Day 5: Print Queue</a>
/// </summary>
public class PrintQueue {
    internal interface IRule {
        bool IsApplied(long[] update);
        IEnumerable<long> AffectedPageNumbers();
    }
    
    internal record OrderRule(long Before, long After) : IRule {
        public bool IsApplied(long[] update) {
            var indexBefore = Array.IndexOf(update, Before);
            var indexAfter = Array.IndexOf(update, After);
            
            // at least one page is missing, so everything is fine
            if (indexBefore < 0 || indexAfter < 0) return true; 
            
            // both pages are present, so check actual order
            return indexBefore < indexAfter;
        }

        public IEnumerable<long> AffectedPageNumbers() {
            yield return Before;
            yield return After;
        }
    }

    private const string RuleSeparator = "|";
    private const string UpdateSeparator = ",";
    
    public PrintQueue(IEnumerable<string> input) {
        (Rules, Updates) = ParseInput(input);
    }

    private IRule[] Rules { get; }
    private IList<long[]> Updates { get; }

    private static (IRule[], IList<long[]>) ParseInput(IEnumerable<string> input) {
        var inputAsArray = input.ToArray();
        var rules = inputAsArray
            .Where(s => s.Contains(RuleSeparator))
            .Select(s => {
                var split = s.Split(RuleSeparator);
                return new OrderRule(split[0].ExtractDigitsAsLong(), split[1].ExtractDigitsAsLong());
            }).ToArray<IRule>();
        var updates = inputAsArray
            .Where(s => s.Contains(UpdateSeparator))
            .Select(s => s.ParseLongArray(UpdateSeparator[0]))
            .ToList();
        return (rules, updates);
    }

    public long CalculateSumOfMiddlePageNumbers() {
        return Updates
            .Where(update => Rules.All(rule => rule.IsApplied(update)))
            .Sum(update => update[update.Length / 2]);
    }
    
    public long CalculateCorrectedSumOfMiddlePageNumbers() {
        var brokenUpdates = Updates.Where(update => !Rules.All(rule => rule.IsApplied(update))).ToList();
        var pageNumberRules = Rules
            // create one rule for each affected page number
            .SelectMany(rule => rule.AffectedPageNumbers().Select(pageNumber => (pageNumber, rule)))
            // now make a dictionary
            .GroupBy(pr => pr.pageNumber)
            .ToDictionary(g => g.Key, g => g.Select(pr => pr.rule).ToArray());
        var middlePageNumbers = brokenUpdates
            .Select(update => {
                var middlePageIndex = update.Length / 2;
                var updateRules = update
                    // take only rules that have AT LEAST one of our numbers (Note: there are doubles)
                    .SelectMany(n => pageNumberRules[n]).Distinct()
                    // now rule out (haha) the rules that are for numbers not in the update
                    .Where(rule => rule.AffectedPageNumbers().All(update.Contains)).ToArray();
                // in updateRules, there is exactly one number that has {middlePageIndex} entries for before and after each => that is the middle number
                return update.Single(number => updateRules.OfType<OrderRule>().Count(r => r.Before == number) == middlePageIndex 
                                                && updateRules.OfType<OrderRule>().Count(r => r.After  == number) == middlePageIndex);
            });
        return middlePageNumbers.Sum();
    }
}
