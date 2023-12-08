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
}