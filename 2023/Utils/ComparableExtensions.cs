using System;

namespace AoC;

public static class ComparableExtensions {
    
    public static bool IsGreaterOrEqualTo<TValue>(this TValue value, TValue other)
        where TValue : IComparable<TValue> {
        return value.CompareTo(other) >= 0;
    }
    
    public static bool IsGreaterThan<TValue>(this TValue value, TValue other)
        where TValue : IComparable<TValue> {
        return value.CompareTo(other) > 0;
    }
    
    public static bool IsSmallerOrEqualTo<TValue>(this TValue value, TValue other)
        where TValue : IComparable<TValue> {
        return value.CompareTo(other) <= 0;
    }
    
    public static bool IsSmallerThan<TValue>(this TValue value, TValue other)
        where TValue : IComparable<TValue> {
        return value.CompareTo(other) < 0;
    }
    
    public static TValue Max<TValue>(this TValue value, params TValue[] others)
        where TValue : IComparable<TValue> {
        var result = value;
        foreach (var other in others) {
            if (other.IsGreaterThan(result)) {
                result = other;
            }
        }
        
        return result;
    }
    
    public static TValue Min<TValue>(this TValue value, params TValue[] others)
        where TValue : IComparable<TValue> {
        var result = value;
        foreach (var other in others) {
            if (other.IsSmallerThan(result)) {
                result = other;
            }
        }
        
        return result;
    }
}