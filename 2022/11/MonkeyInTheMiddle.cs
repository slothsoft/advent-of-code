using System;
using System.Collections.Generic;
using System.Linq;

namespace AoC._11;

/// <summary>
/// <a href="https://adventofcode.com/2022/day/11">Day 11: Monkey in the Middle</a>: As you finally start
/// making your way upriver, you realize your pack is much lighter than you remember. Just then, one of
/// the items from your pack goes flying overhead. Monkeys are playing Keep Away with your missing things!
///
/// To get your stuff back, you need to be able to predict where the monkeys will throw your items. After
/// some careful observation, you realize the monkeys operate based on how worried you are about each item.
/// </summary>
public class MonkeyInTheMiddle {
    
    private class Monkey {
        
        public int Id { get; init; }
        public long InspectionsCount { get; private set; }
        public IList<long> Items { get; init; } = new List<long>();
        public Func<long, long> Operation { get; init; } = i => i;
        public int DivisibleBy { get; init; }
        public Func<long, int> ThrowToMonkey { get; init; } = _ => -1; // will throw exception

        public long Inspect(long monkeyItem) {
            InspectionsCount++;
            
            return Operation(monkeyItem);
        }
    }
    
    public Func<long, long> AfterInspectOperation { get; set; } = i => i;

    private readonly Monkey[] _monkeys;
    public IDictionary<int, long[]> MonkeyItems => _monkeys.ToDictionary(m => m.Id, m => m.Items.ToArray());
    public IDictionary<int, long> MonkeyInspections => _monkeys.ToDictionary(m => m.Id, m => m.InspectionsCount);
    public IDictionary<int, int> MonkeyDivisibleBy => _monkeys.ToDictionary(m => m.Id, m => m.DivisibleBy);
    
    public MonkeyInTheMiddle(string[] input) {
        _monkeys = ParseMonkeys(input);
    }

    private static Monkey[] ParseMonkeys(string[] lines) {
        var result = new List<Monkey>();
        
        for (var i = 0; i < lines.Length; i+= 7) {
            // Monkey 0:
            var id = int.Parse(lines[i].Split(" ")[1][..1]);
            // __Starting items: 79, 98
            var items = lines[i + 1].Split(":")[1].Split(",").Select(s => long.Parse(s.Trim())).ToList();
            // __Operation: new = old * 19
            var operation = ParseOperation(lines[i + 2]);
            // __Test: divisible by 23
            // ____If true: throw to monkey 2
            // ____If false: throw to monkey 3
            var divisibleBy = int.Parse(lines[i + 3].Split("by ")[1]);
            var throwToMonkey = ParseThrowToMonkey(lines[i + 3], lines[i + 4], lines[i + 5]);
            
            result.Add(new Monkey {
                Id = id,
                Items = items,
                Operation = operation,
                DivisibleBy = divisibleBy,
                ThrowToMonkey = throwToMonkey,
            });
        }
        return result.ToArray();
    }

    private static Func<long, long> ParseOperation(string operation) {
        if (operation.Contains("*")) {
            return ParseOperation(operation.Split("*")[1], (a, b) => a * b);
        }
        if (operation.Contains("+")) {
            return ParseOperation(operation.Split("+")[1], (a, b) => a + b);
        }
        throw new ArgumentException("Cannot parse operation: " + operation);
    }
    
    private static Func<long, long> ParseOperation(string value, Func<long, long, long> usedOperator) {
        if (value.Contains("old")) {
            // something like "old * old"
            return old => usedOperator(old, old);
        }
        var valueAsInt = int.Parse(value.Trim());
        return old => usedOperator(old, valueAsInt);
    }

    private static Func<long, int> ParseThrowToMonkey(string test, string ifTrue, string ifFalse) {
        var divisibleBy = int.Parse(test.Split("by ")[1]);
        var ifTrueMonkey = int.Parse(ifTrue.Split("monkey ")[1]);
        var ifFalseMonkey = int.Parse(ifFalse.Split("monkey ")[1]);
        return item => (item % divisibleBy == 0) ? ifTrueMonkey : ifFalseMonkey;
    }

    public void ExecuteRounds(int rounds) {
        for (var i = 0; i < rounds; i++) {
            ExecuteRound();
        }
    }
    
    public void ExecuteRound() {
        foreach (var monkey in _monkeys) {
            foreach (var monkeyItem in monkey.Items) {
                var newMonkeyItem = monkey.Inspect(monkeyItem);
                newMonkeyItem = AfterInspectOperation(newMonkeyItem);
                var throwToMonkey = monkey.ThrowToMonkey(newMonkeyItem);
                _monkeys.Single(m => m.Id == throwToMonkey).Items.Add(newMonkeyItem);
            }
            monkey.Items.Clear(); // monkey throws ALL items to other monkeys
        }
    }

    public long CalculateMonkeyBusiness() {
        return _monkeys.Select(m => m.InspectionsCount).OrderByDescending(i => i).Take(2).Aggregate(1L, (a, b) => a * b);
    }

}