using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

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
    }

    private static Valve CreateValve(string line) {
        // line: Valve AA has flow rate=0; tunnels lead to valves DD, II, BB
        // OR: Valve HH has flow rate=22; tunnel leads to valve GG
        var lineSplit = Regex.Split(line).ToArray();
        return new Valve(lineSplit[2], int.Parse(lineSplit[4]), lineSplit[6].Split(", ").ToDictionary(s => s, s => 1));
    }

    public int CalculateMaxPressure(int minutes) {
        return Solve(StartValve, new VolcanoState(minutes, _valves)).Select(s => s.PressureReleased).Max();
    }

    private static IEnumerable<VolcanoState> Solve(string valveName, VolcanoState state) {
        var startValve = state.Valves.Single(v => v.Name == valveName);

        if (state.OpenValves.Count == state.Valves.Count) {
            // all valves where opened, so wait the time out
            state.PassMinutes(state.MinutesLeft);
            return new[] {state};
        }

        if (!state.OpenValves.ContainsKey(valveName) && startValve.FlowRate > 0) {
            // there is a valve here that is not open - open it
            state.OpenValve(valveName);

            if (state.MinutesLeft < 0) {
                return new[] {state};
            }
        }

        var possibleNextValves = startValve.LeadsToWithDistance.Where(l => state.MinutesLeft - l.Value > 0).ToArray();

        if (possibleNextValves.Length == 0) {
            // there is no way to go
            state.PassMinutes(state.MinutesLeft);
            return new[] {state};
        }

        if (possibleNextValves.Length == 1) {
            // there is only one way to go, so go there
            state.PassMinutes(possibleNextValves[0].Value);
            return Solve(possibleNextValves[0].Key, state);
        }

        // there are many ways to go, so split up
        return possibleNextValves.SelectMany(possibleNextValve => {
            var copyOfState = state.Copy();
            copyOfState.PassMinutes(possibleNextValve.Value);
            return Solve(possibleNextValve.Key, copyOfState);
        });
    }

    private record VolcanoState(IDictionary<string, int> OpenValves, int MinutesLeft, int PressureReleased, IList<Valve> Valves) {
        internal int MinutesLeft { get; private set; } = MinutesLeft;
        internal int PressureReleased { get; private set; } = PressureReleased;

        internal VolcanoState(int minutesLeft, IList<Valve> valves) : this(new Dictionary<string, int>(), minutesLeft, 0, valves) {
        }

        internal void OpenValve(string valveName) {
            PassMinute();

            var valve = Valves.Single(v => v.Name == valveName);
            OpenValves[valveName] = valve.FlowRate;
            Valves.RedirectValve(valve);
            Valves.Remove(valve);
        }

        internal void PassMinutes(int minutes) {
            for (var i = 0; i < minutes; i++) {
                PassMinute();
            }
        }

        internal void PassMinute() {
            MinutesLeft--;
            foreach (var openValve in OpenValves) {
                PressureReleased += openValve.Value;
            }
        }

        public VolcanoState Copy() {
            return this with {
                OpenValves = new Dictionary<string, int>(OpenValves),
                Valves = Valves.Copy(),
            };
        }
    }
}

internal record Valve(string Name, int FlowRate, IDictionary<string, int> LeadsToWithDistance) {
    internal Valve Copy() {
        return this with {
            LeadsToWithDistance = new Dictionary<string, int>(LeadsToWithDistance)
        };
    }
}

internal static class ProboscideaVolcaniumExtensions {
    internal static IList<Valve> Copy(this IEnumerable<Valve> valves) {
        return valves.Select(v => v.Copy()).ToList();
    }

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
}