using System;
using System.Collections.Generic;

namespace AoC;

public class DijkstraForIntAlgorithm<TNode> : DijkstraAlgorithm<TNode, int>
    where TNode : notnull {

    public interface IIntNodeManager {
        IEqualityComparer<TNode>? EqualityComparer { get; }
        IEnumerable<KeyValuePair<TNode, int>> FindAccessibleNodes(TNode startNode);
    }
    
    private record FuncIntNodeManager(Func<TNode, IEnumerable<KeyValuePair<TNode, int>>> Function, IEqualityComparer<TNode>? EqualityComparer = null) : IIntNodeManager {
       public IEnumerable<KeyValuePair<TNode, int>> FindAccessibleNodes(TNode startNode) => Function(startNode);
    }

    private class IntNodeManager : INodeManager {

        private readonly IIntNodeManager _delegate;
        
        internal IntNodeManager(IIntNodeManager newDelegate) {
            _delegate = newDelegate;
        }
        
        public int EmptyDistance => 0;
        public IEqualityComparer<TNode>? EqualityComparer => _delegate.EqualityComparer;
        public int AddDistances(int first, int second) => first + second;
        public int Difference(int first, int second) => first - second;
        
        public IEnumerable<KeyValuePair<TNode, int>> FindAccessibleNodes(TNode startNode) {
            return _delegate.FindAccessibleNodes(startNode);
        }
    }
    
    public DijkstraForIntAlgorithm(Func<TNode, IEnumerable<KeyValuePair<TNode, int>>> nodeManager, IEqualityComparer<TNode>? equalityComparer = null) 
        : this(new FuncIntNodeManager(nodeManager, equalityComparer)) {
    }
    
    public DijkstraForIntAlgorithm(IIntNodeManager nodeManager) : base(new IntNodeManager(nodeManager)) {
        MaxDistance = int.MaxValue;
    }
}