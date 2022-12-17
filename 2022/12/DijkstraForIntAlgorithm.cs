using System.Collections.Generic;

namespace AoC._15;

public class DijkstraForIntAlgorithm<TNode> : DijkstraAlgorithm<TNode, int>
    where TNode : notnull {
    class IntNodeManager : INodeManager {
        private readonly IntNodeManagerDelegate _delegate;

        internal IntNodeManager(IntNodeManagerDelegate newDelegate) {
            _delegate = newDelegate;
        }

        public int EmptyDistance => 0;
        public int AddDistances(int first, int second) => first + second;
        public int Difference(int first, int second) => first - second;

        public IEnumerable<KeyValuePair<TNode, int>> FindAccessibleNodes(TNode startNode) {
            return _delegate(startNode);
        }
    }

    public DijkstraForIntAlgorithm(IntNodeManagerDelegate nodeManager) : base(new IntNodeManager(nodeManager)) {
        MaxDistance = int.MaxValue;
    }

    public delegate IEnumerable<KeyValuePair<TNode, int>> IntNodeManagerDelegate(TNode startNode);
}