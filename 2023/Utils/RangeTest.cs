using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;

namespace AoC;

public class RangeTest {
    [Test]
    [TestCase(1, 5)]
    [TestCase(2L, 4L)]
    [TestCase(2.0, 2.0)]
    public void Constructor<TValue>(TValue from, TValue to)
        where TValue : IComparable<TValue> {
        var inverseRange = new Range<TValue>(to, from);

        Assert.AreEqual(from, inverseRange.From);
        Assert.AreEqual(true, inverseRange.FromIncluded);
        Assert.AreEqual(to, inverseRange.To);
        Assert.AreEqual(false, inverseRange.ToIncluded);
    }

    [Test]
    [TestCase(1, 5)]
    [TestCase(2L, 4L)]
    [TestCase(2.0, 2.0)]
    public void InverseConstructor<TValue>(TValue from, TValue to)
        where TValue : IComparable<TValue> {
        var inverseRange = new Range<TValue>(from, to);

        Assert.AreEqual(from, inverseRange.From);
        Assert.AreEqual(true, inverseRange.FromIncluded);
        Assert.AreEqual(to, inverseRange.To);
        Assert.AreEqual(false, inverseRange.ToIncluded);
    }

    [Test]
    [TestCase(1, 5, 3)]
    [TestCase(1, 5, 1)]
    [TestCase(2L, 4L, 3L)]
    [TestCase(2L, 4L, 2L)]
    [TestCase(2.0, 4.0, 3.0)]
    [TestCase(2.0, 3.0, 2.0)]
    public void ContainsTrue<TValue>(TValue from, TValue to, TValue value)
        where TValue : IComparable<TValue> {
        var range = new Range<TValue>(from, to);

        Assert.IsTrue(range.Contains(value));
    }

    [Test]
    [TestCase(1, 5, 0)]
    [TestCase(1, 5, 5)]
    [TestCase(1, 5, 10)]
    [TestCase(2L, 4L, 1L)]
    [TestCase(2L, 4L, 4L)]
    [TestCase(2L, 4L, 5L)]
    [TestCase(2.0, 4.0, 1.0)]
    [TestCase(2.0, 4.0, 4.0)]
    [TestCase(2.0, 3.0, 5.0)]
    public void ContainsFalse<TValue>(TValue from, TValue to, TValue value)
        where TValue : IComparable<TValue> {
        var range = new Range<TValue>(from, to);

        Assert.IsFalse(range.Contains(value));
    }

    private record IntersectionData<TValue>(Range<TValue> Range, Range<TValue> OtherRange, Range<TValue>? Intersection)
        where TValue : IComparable<TValue> {
        public TestCaseData ToTestData() {
            return new TestCaseData(Range, OtherRange, Intersection);
        }
    }

    private static readonly IntersectionData<int>[] IntersectionIntDatas = {
        new(new Range<int>(10, 20), new Range<int>(12, 18), new Range<int>(12, 18)),
        new(new Range<int>(10, 20), new Range<int>(5, 15), new Range<int>(10, 15)),
        new(new Range<int>(10, 20), new Range<int>(15, 25), new Range<int>(15, 20)), 
        new(new Range<int>(10, 20), new Range<int>(20, 30), null),
        new(new Range<int>(10, 20), new Range<int>(0, 5), null),
    };

    private static readonly IntersectionData<long>[] IntersectionLongDatas = {
        new(new Range<long>(10L, 20L), new Range<long>(12L, 18L), new Range<long>(12L, 18L)),
    };

    private static readonly IntersectionData<double>[] IntersectionDoubleDatas = {
        new(new Range<double>(10.0, 20.0), new Range<double>(5.0, 15.0), new Range<double>(10.0, 15.0)),
    };

    private static IEnumerable<TestCaseData> CreateIntersectionDatas() => IntersectionIntDatas.Select(d => d.ToTestData())
        .Concat(IntersectionLongDatas.Select(d => d.ToTestData()))
        .Concat(IntersectionDoubleDatas.Select(d => d.ToTestData()));

    [Test]
    [TestCaseSource(nameof(CreateIntersectionDatas))]
    public void TryIntersect<TValue>(Range<TValue> range, Range<TValue> otherRange, Range<TValue>? expectedIntersection)
        where TValue : IComparable<TValue> {
        var actualIntersection = range.TryIntersect(otherRange);

        Assert.AreEqual(expectedIntersection, actualIntersection);
    }
    
    [Test]
    [TestCaseSource(nameof(CreateIntersectionDatas))]
    public void TryIntersectReverse<TValue>(Range<TValue> range, Range<TValue> otherRange, Range<TValue>? expectedIntersection)
        where TValue : IComparable<TValue> {
        var actualIntersection = otherRange.TryIntersect(range);

        Assert.AreEqual(expectedIntersection, actualIntersection);
    }

    [Test]
    [TestCaseSource(nameof(CreateIntersectionDatas))]
    public void TryIntersectSingleValues<TValue>(Range<TValue> range, Range<TValue> otherRange, Range<TValue>? expectedIntersection)
        where TValue : IComparable<TValue> {
        var actualIntersection = range.TryIntersect(otherRange.From, otherRange.To);

        Assert.AreEqual(expectedIntersection, actualIntersection);
    }
}