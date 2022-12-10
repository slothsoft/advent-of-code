using System;
using System.Collections.Generic;
using System.Linq;
using AoC.algorithm.backtrack;
using AoC2.algorithm.dijkstra;

namespace AoC;

/// <summary>
/// This is a modified Dijkstra algorithm.
/// </summary>
public class Puzzle2 : BacktrackAlgorithm<Point>.IPositionManager, DijkstraAlgorithm<PortalWithLevel, Distance>.INodeManager {
    private record Edge(Portal FromPortal, Portal ToPortal);

    public int? MaxSteps { get; set; }

    private readonly DonutMaze _maze;
    private readonly IReadOnlyDictionary<Edge, Distance> _distances;

    public Puzzle2(string[] donutAsStrings, int ringSize) {
        _maze = new DonutMaze(donutAsStrings, ringSize);
        _distances = CalculateDistances();
    }

    private IReadOnlyDictionary<Edge, Distance> CalculateDistances() {
        var backtrack = new BacktrackAlgorithm<Point>(this);
        var result = new Dictionary<Edge, Distance>();
        
        foreach (var portal in _maze.Portals) {
            // link this portal to all possible reachable portals
            backtrack.Solve(portal.Location, nextPoint => _maze.Portals.Any(p => p.Location == nextPoint))
                // map to the correct value
                .ForEach(a =>
                    result.Add(new Edge(portal, _maze.FetchPortal(a[^1])),
                        new Distance(a.Length - 1, 0)));
            
            // link this portal to its partner
            var linkedPortal = _maze.Portals.SingleOrDefault(p => p != portal && p.Name == portal.Name);
            if (linkedPortal != null) {
                result.Add(new Edge(portal, linkedPortal), new Distance(1, portal.OuterPortal ? -1 : +1));
            }
        }

        return result;
    }

    public IEnumerable<Point> FindPossibleNextPositions(Point position) {
        return _maze.FindPossibleNextPositions(position).ToList();
    }

    public int SolveReturnSteps() {
        var startPortal = new PortalWithLevel(_maze.FetchStartPortal(), 0);
        var endPortal = new PortalWithLevel(_maze.FetchEndPortal(), 0);

        var dijkstra = new DijkstraAlgorithm<PortalWithLevel, Distance>(this) {
            MaxDistance = MaxSteps == null ? null : new Distance(MaxSteps.Value, 0)
        };
        return dijkstra.Solve(startPortal, endPortal)?.Steps ?? -1;
    }

    public Distance EmptyDistance => new(0, 0);

    public IEnumerable<KeyValuePair<PortalWithLevel, Distance>> FindAccessibleNodes(PortalWithLevel fromPortal) {
        return _distances
            // remove outer portals if on level 0
            .Where(keyValue => fromPortal.Level + keyValue.Value.LevelDifference >= 0)
            // remove end portal if not on level 0
            .Where(keyValue => keyValue.Key.ToPortal.Name != DonutMaze.EndPortal || fromPortal.Level == 0)
            // find portals that link to fromPortal
            .Where(keyValue => keyValue.Key.FromPortal == fromPortal.Portal)
            // now map to the correct type
            .ToDictionary(k => new PortalWithLevel(k.Key.ToPortal, fromPortal.Level + k.Value.LevelDifference), k => k.Value);
    }

    public Distance AddDistances(Distance first, Distance second) {
        return new Distance(first.Steps + second.Steps, first.LevelDifference + second.LevelDifference);
    }

    public Distance Difference(Distance first, Distance second) {
        return new Distance(first.Steps - second.Steps, first.LevelDifference - second.LevelDifference);
    }
}

public record Distance(int Steps, int LevelDifference) : IComparable<Distance> {
    public int CompareTo(Distance? other) {
        if (ReferenceEquals(this, other)) return 0;
        if (ReferenceEquals(null, other)) return 1;
        return Steps.CompareTo(other.Steps);
    }
}

public record PortalWithLevel(Portal Portal, int Level) {
}