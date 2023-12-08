using System;
using NUnit.Framework;

namespace AoC;

public class ComparableExtensionsTest {
    [Test]
    [TestCase(1, 5, false)]
    [TestCase(1, 1, false)]
    [TestCase(1, 0, true)]
    [TestCase(1L, 5L, false)]
    [TestCase(1L, 1L, false)]
    [TestCase(1L, 0L, true)]
    [TestCase(1.1, 1.5, false)]
    [TestCase(1.1, 1.1, false)]
    [TestCase(1.1, 1.0, true)]
    public void IsGreaterThan<TValue>(TValue value, TValue other, bool expected)
        where TValue : IComparable<TValue> {
        Assert.AreEqual(expected, value.IsGreaterThan(other));
    }

    [Test]
    [TestCase(6, 2, false)]
    [TestCase(2, 2, false)]
    [TestCase(1, 2, true)]
    [TestCase(6L, 2L, false)]
    [TestCase(2L, 2L, false)]
    [TestCase(1L, 2L, true)]
    [TestCase(2.6, 2.2, false)]
    [TestCase(2.2, 2.2, false)]
    [TestCase(2.1, 2.2, true)]
    public void IsSmallerThan<TValue>(TValue value, TValue other, bool expected)
        where TValue : IComparable<TValue> {
        Assert.AreEqual(expected, value.IsSmallerThan(other));
    }
    
    [Test]
    [TestCase(3, 7, false)]
    [TestCase(3, 3, true)]
    [TestCase(3, 1, true)]
    [TestCase(3L, 7L, false)]
    [TestCase(3L, 3L, true)]
    [TestCase(3L, 1L, true)]
    [TestCase(3.3, 3.7, false)]
    [TestCase(3.3, 3.3, true)]
    [TestCase(3.3, 3.1, true)]
    public void IsGreaterOrEqualTo<TValue>(TValue value, TValue other, bool expected)
        where TValue : IComparable<TValue> {
        Assert.AreEqual(expected, value.IsGreaterOrEqualTo(other));
    }

    [Test]
    [TestCase(8, 3, false)]
    [TestCase(3, 3, true)]
    [TestCase(2, 3, true)]
    [TestCase(8L, 3L, false)]
    [TestCase(3L, 3L, true)]
    [TestCase(2L, 3L, true)]
    [TestCase(3.8, 3.3, false)]
    [TestCase(3.3, 3.3, true)]
    [TestCase(3.2, 3.3, true)]
    public void IsSmallerOrEqualTo<TValue>(TValue value, TValue other, bool expected)
        where TValue : IComparable<TValue> {
        Assert.AreEqual(expected, value.IsSmallerOrEqualTo(other));
    }
}