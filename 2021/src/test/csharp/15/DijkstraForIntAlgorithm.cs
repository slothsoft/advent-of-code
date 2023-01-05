using System;
using System.Collections.Generic;

namespace AoC._15;

public class DijkstraForIntAlgorithm<TNode> : DijkstraAlgorithm<TNode, int>
    where TNode : notnull {

    public interface IIntNodeManager {
        IEnumerable<KeyValuePair<TNode, int>> FindAccessibleNodes(TNode startNode);
    }

    class IntNodeManager : INodeManager {

        private readonly IIntNodeManager _delegate;
        
        internal IntNodeManager(IIntNodeManager newDelegate) {
            _delegate = newDelegate;
        }
        
        public int EmptyDistance => 0;
        public int AddDistances(int first, int second) => first + second;
        public int Difference(int first, int second) => first - second;
        
        public IEnumerable<KeyValuePair<TNode, int>> FindAccessibleNodes(TNode startNode) {
            return _delegate.FindAccessibleNodes(startNode);
        }
    }
    
    public DijkstraForIntAlgorithm(IIntNodeManager nodeManager) : base(new IntNodeManager(nodeManager)) {
        MaxDistance = int.MaxValue;
    }
}