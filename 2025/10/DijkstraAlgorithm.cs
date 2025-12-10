using System;
using System.Collections.Generic;
using System.Linq;

namespace AoC;

/// <summary>
/// Copy of the 2019-20 Dijkstra algorithm (because that's why I outsourced this class).
/// </summary>
public class DijkstraAlgorithm<TNode, TDistance>
    where TNode : notnull
    where TDistance : IComparable<TDistance> {
    public interface INodeManager {
        TDistance EmptyDistance { get; }
        IEqualityComparer<TNode>? EqualityComparer { get; }
        IEnumerable<KeyValuePair<TNode, TDistance>> FindAccessibleNodes(TNode startNode);
        TDistance AddDistances(TDistance first, TDistance second);
        TDistance Difference(TDistance first, TDistance second);
    }

    private readonly INodeManager _nodeManager;

    public DijkstraAlgorithm(INodeManager nodeManager) {
        _nodeManager = nodeManager;
    }

    public TDistance? MaxDistance { get; set; }

    public TDistance? Solve(TNode startNode, TNode endNode) => Solve(startNode, node => endNode.Equals(node));

    public TDistance? Solve(TNode startNode, Func<TNode, bool> isEndNode) {
        var solutions = new Dictionary<TNode, TDistance>(_nodeManager.EqualityComparer) {{startNode, _nodeManager.EmptyDistance}};
        var nodesToHandle = new List<TNode> {startNode};
        var currentEndNodeDistance = MaxDistance;

        while (nodesToHandle.Count > 0) {
            var fromNode = nodesToHandle.First();
            nodesToHandle.Remove(fromNode);
            var fromDistance = solutions[fromNode];

            if (currentEndNodeDistance != null && currentEndNodeDistance.CompareTo(fromDistance) < 0) {
                // we already found one solution, and it's smaller than the current one, sooo...
                continue;
            }

            if (MaxDistance != null && fromDistance.CompareTo(MaxDistance) > 0) {
                // manual limits to return algorithm if no way could be found (or bugs happen)
                continue;
            }

            var toNodeAndDistance = _nodeManager.FindAccessibleNodes(fromNode);

            foreach (var nodeDistance in toNodeAndDistance) {
                var totalDistance = _nodeManager.AddDistances(fromDistance, nodeDistance.Value);

                if (!solutions.ContainsKey(nodeDistance.Key)) {
                    // it's a new distance, so just add it
                    solutions[nodeDistance.Key] = totalDistance;
                    nodesToHandle.Add(nodeDistance.Key);
                    if (isEndNode(nodeDistance.Key)) {
                        currentEndNodeDistance = totalDistance;
                    }
                } else if (totalDistance.CompareTo(solutions[nodeDistance.Key]) < 0) {
                    // update the toNode and all it points too
                    UpdateDistancesStartingWith(solutions, nodeDistance.Key, _nodeManager.Difference(solutions[nodeDistance.Key], totalDistance));
                    nodesToHandle.Add(nodeDistance.Key);
                    if (isEndNode(nodeDistance.Key)) {
                        currentEndNodeDistance = totalDistance;
                    }
                }
            }
        }

        var result = solutions.Where(kv => isEndNode(kv.Key)).OrderBy(kv => kv.Value).FirstOrDefault();

        if (result.Key is not null) {
        }

        return result.Value;
    }

    private void UpdateDistancesStartingWith(IDictionary<TNode, TDistance> solutions, TNode node, TDistance distanceToRemove) {
        var previousDistance = solutions[node];
        solutions[node] = _nodeManager.Difference(previousDistance, distanceToRemove);

        var targetNodes = _nodeManager.FindAccessibleNodes(node)
            // but only the ones that go AWAY from the starting point
            .Where(keyValue => keyValue.Value.CompareTo(previousDistance) > 0)
            // and only the ones that were actually already calculated (else they are going to be calculated soon)
            .Where(keyValue => solutions.Any(s => s.Key.Equals(keyValue.Key)))
            // make nodes out of them
            .Select(keyValue => keyValue.Key);

        foreach (var targetNode in targetNodes) {
            UpdateDistancesStartingWith(solutions, targetNode, distanceToRemove);
        }
    }
}