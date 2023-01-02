using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using AoC._15;

namespace AoC._16;

/// <summary>
/// <a href="https://adventofcode.com/2022/day/16">Day 16: Proboscidea Volcanium</a>
/// </summary>
public class ProboscideaVolcanium {
    private static readonly Regex Regex = new("(Valve | has flow rate=|; tunnels lead to valves |; tunnel leads to valve )");
    internal const string StartValve = "AA";

    private readonly IList<Valve> _valves = new List<Valve>();

    public ProboscideaVolcanium(string[] lines) {
        foreach (var line in lines) {
            _valves.Add(CreateValve(line));
        }

        _valves.Normalize();
        _valves.CalculateAllCombinations();
    }

    private static Valve CreateValve(string line) {
        // line: Valve AA has flow rate=0; tunnels lead to valves DD, II, BB
        // OR: Valve HH has flow rate=22; tunnel leads to valve GG
        var lineSplit = Regex.Split(line).ToArray();
        return new Valve(lineSplit[2], int.Parse(lineSplit[4]), lineSplit[6].Split(", ").ToDictionary(s => s, s => 1));
    }


    public int CalculateMaxPressure(int minutes) {
        return CalculateMaxPressure(_valves, minutes, _valves.Single(v => v.Name == StartValve));
    }

    private static int CalculateMaxPressure(IList<Valve> stillOpenValves, int minutesLeft, Valve currentValve) {
        // This is a modified depth first search https://en.wikipedia.org/wiki/Depth-first_search
        var result = 0;
        foreach (var openValve in stillOpenValves) {
            var newMinutesLeft = minutesLeft - currentValve.AllShortestPaths[openValve.Name] - 1;
            if (newMinutesLeft > 0) {
                var entireFlowRate = newMinutesLeft * openValve.FlowRate + CalculateMaxPressure(stillOpenValves.Where(v => v != openValve).ToArray(), newMinutesLeft, openValve);
                if (result < entireFlowRate) {
                    result = entireFlowRate;
                }
            }
        }

        return result;
    }

    public double CalculateMaxPressureWithElephant(int minutes) {
        var startValve = _valves.Single(v => v.Name == StartValve);
        return CalculateMaxPressureWithElephant(_valves, new[] {minutes, minutes}, new[] {startValve, startValve});
    }

    static int CalculateMaxPressureWithElephant(IList<Valve> stillOpenValves, int[] minutesLeft, Valve[] currentValves) {
        var result = 0;
        var elephantOrI = minutesLeft[0] > minutesLeft[1] ? 0 : 1;

        var currentValve = currentValves[elephantOrI];
        foreach (var openValve in stillOpenValves) {
            var newMinuteLeft = minutesLeft[elephantOrI] - currentValve.AllShortestPaths[openValve.Name] - 1;
            if (newMinuteLeft > 0) {
                var newMinutesLeft = new[] {newMinuteLeft, minutesLeft[1 - elephantOrI]};
                var newCurrentValves = new[] {openValve, currentValves[1 - elephantOrI]};
                var entireFlowRate = newMinuteLeft * openValve.FlowRate + CalculateMaxPressureWithElephant(stillOpenValves.Where(v => v != openValve).ToArray(), newMinutesLeft, newCurrentValves);
                if (result < entireFlowRate) {
                    result = entireFlowRate;
                }
            }
        }

        return result;
    }
}

internal record Valve(string Name, int FlowRate, IDictionary<string, int> LeadsToWithDistance) {
    public IDictionary<string, int> AllShortestPaths { get; set; } = new Dictionary<string, int>();
}

internal static class ProboscideaVolcaniumExtensions {
    internal static void Normalize(this ICollection<Valve> valves) {
        var uselessValves = valves.Where(v => v.FlowRate == 0 && v.Name != ProboscideaVolcanium.StartValve).ToArray();
        foreach (var uselessValve in uselessValves) {
            valves.RedirectValve(uselessValve);
        }

        // now remove the useless valves
        foreach (var uselessValve in uselessValves) {
            valves.Remove(uselessValve);
        }
    }

    internal static void RedirectValve(this ICollection<Valve> valves, Valve uselessValve) {
        foreach (var leadsTo in uselessValve.LeadsToWithDistance) {
            var leadsToValve = valves.Single(v => v.Name == leadsTo.Key);
            var distanceToUselessValve = leadsToValve.LeadsToWithDistance[uselessValve.Name];
            leadsToValve.LeadsToWithDistance.Remove(uselessValve.Name);

            foreach (var otherLeadsTo in uselessValve.LeadsToWithDistance.Where(e => e.Key != leadsTo.Key)) {
                var newDistance = otherLeadsTo.Value + distanceToUselessValve;
                if (!leadsToValve.LeadsToWithDistance.ContainsKey(otherLeadsTo.Key) || newDistance < leadsToValve.LeadsToWithDistance[otherLeadsTo.Key]) {
                    // add a new connection if none is present or the present one is too long
                    leadsToValve.LeadsToWithDistance[otherLeadsTo.Key] = newDistance;
                }
            }
        }
    }

    internal static void CalculateAllCombinations(this IList<Valve> valves) {
        foreach (var valve in valves) {
            var dijkstra = new DijkstraForIntAlgorithm<string>(name => valves.Single(v => v.Name == name).LeadsToWithDistance);
            valve.AllShortestPaths = dijkstra.SolveForAll(valve.Name, "ZZ");
        }
    }
}