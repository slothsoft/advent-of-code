using System.Collections.Generic;
using System.Linq;

namespace AoC;

/// <summary>
/// This is a modified Dijkstra algorithm.
/// </summary>
public class Puzzle2 {
    private record Edge(Portal FromPortal, Portal ToPortal);
    private record Distance(int Steps, int LevelDifference);
    private record PortalWithLevel(Portal Portal, int Level);

    private const int MaxLevel = 20;
    private const int MaxSteps = 10_000;

    private readonly DonutMaze _maze;
    private readonly IReadOnlyDictionary<Edge, Distance> _distances;

    public Puzzle2(string[] donutAsStrings, int ringSize) {
        _maze = new DonutMaze(donutAsStrings, ringSize);
        _distances = CalculateDistances();
    }

    private IReadOnlyDictionary<Edge, Distance> CalculateDistances() {
        var result = new Dictionary<Edge, Distance>();
        foreach (var portal in _maze.Portals) {
            CalculateDistances(result, new List<Point> {
                portal.Location
            });

            // link this portal to its partner
            var linkedPortal = _maze.Portals.SingleOrDefault(p => p != portal && p.Name == portal.Name);
            if (linkedPortal != null) {
                result.Add(new Edge(portal, linkedPortal), new Distance(1, portal.OuterPortal ? -1 : +1));
            }
        }

        return result;
    }

    private void CalculateDistances(Dictionary<Edge, Distance> results, List<Point> currentSteps) {
        var position = currentSteps[^1];
        var possibleNextPositions = _maze.FindPossibleNextPositions(position)
            .Where(p => !currentSteps.Contains(p)) // we will not backtrack
            .ToArray();

        if (possibleNextPositions.Length == 0) {
            return; // there is no way to go
        }

        if (possibleNextPositions.Length == 1) {
            // there is only one way to go, so go there
            CalculateDistances(results, currentSteps, possibleNextPositions[0]);
            return;
        }

        foreach (var possibleNextPosition in possibleNextPositions) {
            CalculateDistances(results, new List<Point>(currentSteps), possibleNextPosition);
        }
    }

    private void CalculateDistances(Dictionary<Edge, Distance> results, List<Point> currentSteps, Point nextPosition) {
        if (_maze.PortalConnections.ContainsKey(nextPosition)) {
            // the next step is a portal field
            var startPortal = _maze.FetchPortal(currentSteps.First());
            var endPortal = _maze.FetchPortal(nextPosition);
            results.Add(new Edge(startPortal, endPortal), new Distance(currentSteps.Count, 0));
        } else if (_maze.Maze[nextPosition.X][nextPosition.Y] == DonutMaze.End) {
            // the next step is THE end field
            var startPortal = _maze.FetchPortal(currentSteps.First());
            results.Add(new Edge(startPortal, _maze.FetchEndPortal()), new Distance(currentSteps.Count, 0));
        } else if (_maze.Maze[nextPosition.X][nextPosition.Y] == DonutMaze.Start) {
            // the next step is THE start field - we can ignore that
        } else {
            // the next step is a regular field
            currentSteps.Add(nextPosition);
            CalculateDistances(results, currentSteps);
        }
    }

    public int SolveReturnSteps() {
        var startPortal = new PortalWithLevel(_maze.FetchStartPortal(), 0);
        var endPortal = new PortalWithLevel(_maze.FetchEndPortal(), 0);
        return Dijkstra(startPortal, endPortal);
    }

    private int Dijkstra(PortalWithLevel startPortal, PortalWithLevel endPortal) {
        var solutions = new Dictionary<PortalWithLevel, int> {{startPortal, 0}};
        var portalsToHandle = new List<PortalWithLevel> {startPortal};

        while (portalsToHandle.Count > 0) {
            var fromPortal = portalsToHandle.First();
            portalsToHandle.Remove(fromPortal);
            var fromDistance = solutions[fromPortal];

            if (solutions.ContainsKey(endPortal) && solutions[endPortal] < fromDistance) {
                // we already found one solution, and it's smaller than the current one, sooo...
                continue;
            }

            if (fromDistance > MaxSteps) {
                // manual limits to return algorithm if no way could be found (or bugs happen)
                continue;
            }

            var toPortalAndDistance = _distances
                // remove end portal if not on level 0
                .Where(keyValue => keyValue.Key.ToPortal.Name != DonutMaze.EndPortal || fromPortal.Level == 0)
                // find portals that link to fromPortal
                .Where(keyValue => keyValue.Key.FromPortal == fromPortal.Portal)
                .ToDictionary(k => k.Key.ToPortal, v => v.Value);

            foreach (var portalDistance in toPortalAndDistance) {
                var totalSteps = fromDistance + portalDistance.Value.Steps;
                var totalLevel = fromPortal.Level + portalDistance.Value.LevelDifference;
                var toPortal = new PortalWithLevel(portalDistance.Key, totalLevel);

                if (totalLevel < 0) {
                    // algorithm tries to use an outer portal, which is not allowed
                    continue;
                }

                if (!solutions.ContainsKey(toPortal) || totalSteps < solutions[toPortal]) {
                    if (solutions.ContainsKey(toPortal)) {
                        // update the toPortal and all it points too
                        UpdateDistancesStartingWith(solutions, toPortal, solutions[toPortal] - totalSteps);
                    } else {
                        // it's a new distance, so just add it
                        solutions[toPortal] = totalSteps;
                    }
                    portalsToHandle.Add(toPortal);
                }
            }
        }

        return solutions.GetValueOrDefault(endPortal, -1);
    }

    private void UpdateDistancesStartingWith(Dictionary<PortalWithLevel, int> solutions, PortalWithLevel portal, int stepsToRemove) {
        var previousSteps = solutions[portal];
        solutions[portal] -= stepsToRemove;

        var targetPortals = _distances
            // get the connections starting from the above portal
            .Where(keyValue => keyValue.Key.FromPortal == portal.Portal)
            // but only the ones that go AWAY from the starting point
            .Where(keyValue => keyValue.Value.Steps > previousSteps)
            // and only the ones that were actually already calculated (else they are going to be calculated soon)
            .Where(keyValue => solutions.Any(s => s.Key.Portal == keyValue.Key.ToPortal && s.Key.Level == portal.Level + keyValue.Value.LevelDifference))
            // make portal IDs out of them
            .Select(keyValue => new PortalWithLevel(keyValue.Key.ToPortal, portal.Level + keyValue.Value.LevelDifference));
        
        foreach (var targetPortal in targetPortals) {
            UpdateDistancesStartingWith(solutions, targetPortal, stepsToRemove);
        }
    }
}