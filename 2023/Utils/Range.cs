using System;
using System.Collections.Generic;

namespace AoC;

public readonly struct Range<TValue>
    where TValue : IComparable<TValue> {
    
    public Range(TValue from, TValue to) {
        if (from.IsSmallerOrEqualTo(to)) {
            From = from;
            To = to;
        } else {
            From = to;
            To = from;
        }
    }

    public TValue From { get; }
    public bool FromIncluded => true;
    public TValue To { get; }
    public bool ToIncluded => false;

    public bool Contains(TValue value) {
        return From.IsSmallerOrEqualTo(value) && To.IsGreaterThan(value);
    }

    public Range<TValue>? TryIntersect(Range<TValue> range) => TryIntersect(range.From, range.To); 
    
    public Range<TValue>? TryIntersect(TValue rangeFrom, TValue rangeTo) {
        var fromMax = rangeFrom.Max(From);
        var toMin = rangeTo.Min(To);
        
        if (fromMax.IsGreaterOrEqualTo(toMin)) {
            return null;
        }
            
        return new Range<TValue>(fromMax, toMin);
    }
    
    public IEnumerable<Range<TValue>> SplitAt(TValue value) {
        if (!Contains(value)) {
            // value is not in this range
            yield return this;
        } else {
            yield return new Range<TValue>(From, value);
            yield return new Range<TValue>(value, To);
        }
    }
    
    public override string ToString() => $"Range: {From} -> {To}";
}