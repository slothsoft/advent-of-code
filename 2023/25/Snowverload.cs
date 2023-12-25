using System;
using System.Collections.Generic;
using System.Linq;

namespace AoC.day25;

/// <summary>
/// <a href="https://adventofcode.com/2023/day/25">Day 25: Snowverload</a>
/// </summary>
public class Snowverload {

    private static readonly Random Rnd = new();
    
    public Snowverload(IEnumerable<string> input) {
        Input = ParseInput(input);
    }

    internal IDictionary<string, ISet<string>> Input { get; }

    private static IDictionary<string, ISet<string>> ParseInput(IEnumerable<string> input) {
        var result = new Dictionary<string, ISet<string>>();

        foreach (var line in input) {
            var keyValues = line.Split(": ");
            var values = keyValues[1].Split(' ');

            result.GetOrCreate(keyValues[0], () => new HashSet<string>()).AddRange(values);
            foreach (var value in values) {
                result.GetOrCreate(value, () => new HashSet<string>()).Add(keyValues[0]);
            }
        }

        return result;
    }

    internal long Calculate(int number = 3) {
        var bottlenecks = CalculateBottlenecks(number);
        bottlenecks.ForEach(SnipBottleneck);
        var multiplier1 = CountGroupSize(FromKey(bottlenecks[0])[0]);
        var multiplier2 = CountGroupSize(FromKey(bottlenecks[0])[1]);
        return multiplier1 * multiplier2;
    }

    internal string[] CalculateBottlenecks(int number = 3) {
        var componentCounter = new Dictionary<string, int>();
        for (var i = 0; i < 250; i++) {
            var from = FetchRandomComponent();
            var to = FetchRandomComponent();
            var path = FindPath(from, to);
            if (path == null) {
                // we couldn't find a path, so try again
                i--;
                continue;
            }
            // about 50% of the paths should go through the bottlenecks
            // so the most-used components should be them --^
            for (var p = 0; p < path.Length - 1; p++) {
                var key = CreateKey(path[p], path[p + 1]);
                componentCounter[key] = componentCounter.GetValueOrDefault(key) + 1;
            }
        }
        
        return componentCounter.OrderByDescending(kv => kv.Value).Take(number).Select(kv => kv.Key).ToArray();
    }

    private string FetchRandomComponent() {
        return Input.Keys.Skip(Rnd.Next(Input.Count)).Take(1).Single();
    }

    internal string[]? FindPath(string from, string to) {
        // we know all components are connected, so just taking random neighbors will probably find us a way 
        // special case A: the components are in different groups -> since we know the groups are connected by three wires, the
        //                 path could go back and forth and back and is than in the correct group
        // special case B: the components are in the same group -> as above the path might go back and forth and back, and the path
        //                 will never be found, this might happen if we just move along an unlucky path
        var previous = new Dictionary<string, string> {{from, from}};
        var componentsToHandle = new List<string> {from};
        var handledComponents = new HashSet<string> {from};

        while (componentsToHandle.Count > 0) {
            componentsToHandle = componentsToHandle.SelectMany(component => {
                var nextComponents = Input[component].Where(c => !handledComponents.Contains(c)).ToList();
                nextComponents.ForEach(nextComponent => {
                    previous[nextComponent] = component;
                    handledComponents.Add(nextComponent);
                });
                return nextComponents;
            }).ToList();
        }

        if (!previous.ContainsKey(to)) {
            return null; // the unlucky path
        }

        var path = new List<string> {from};
        var component = to;
        while (component != from) {
            path.Insert(1, component);
            component = previous[component];
        }
        return path.ToArray();
    }
    
    private void SnipBottleneck(string bottleneck) {
        var components = FromKey(bottleneck);
        Input[components[0]].Remove(components[1]);
        Input[components[1]].Remove(components[0]);
    }
    
    private static string CreateKey(string component1, string component2) {
        if (string.Compare(component1, component2, StringComparison.Ordinal) > 0) {
            (component1, component2) = (component2, component1);
        }
        return component1 + "/" + component2;
    }
    
    private static string[] FromKey(string key) {
        return key.Split("/");
    }
    
    private long CountGroupSize(string from) {      
        var componentsToHandle = new List<string> {from};
        var handledComponents = new HashSet<string> {from};

        while (componentsToHandle.Count > 0) {
            componentsToHandle = componentsToHandle.SelectMany(component => {
                var nextComponents = Input[component].Where(c => !handledComponents.Contains(c)).ToList();
                handledComponents.AddRange(nextComponents);
                return nextComponents;
            }).ToList();
        }

        return handledComponents.Count;
    }

}

internal static class SnowverloadExtensions {
    public static TValue GetOrCreate<TKey, TValue>(this IDictionary<TKey, TValue> dict, TKey key, Func<TValue> valueCreator) {
        if (!dict.ContainsKey(key)) {
            dict[key] = valueCreator();
        }

        return dict[key];
    }

    public static void AddRange<TElement>(this ISet<TElement> set, IEnumerable<TElement> values) {
        foreach (var value in values) {
            set.Add(value);
        }
    }

    public static void ForEach<TElement>(this IEnumerable<TElement> elements, Action<TElement> action) {
        foreach (var element in elements) {
            action(element);
        }
    }
}