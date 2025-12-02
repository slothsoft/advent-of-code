using System.Linq;
using NUnit.Framework;

namespace AoC.day2;

public class DanisTest {
    [Test]
    [TestCase(12_12_12_12, true)]
    [TestCase(1234_1234, true)]
    [TestCase(12_12_12, false)]
    [TestCase(1111111111, true)]
    [TestCase(1234, false)]
    public void Part1(long value, bool expected) {
        Assert.AreEqual(expected,  value.IsInvalidId());  
    }
    
    [Test]
    [TestCase(12_12_12_12, true)]
    [TestCase(1234_1234, true)]
    [TestCase(12_12_12, true)]
    [TestCase(1111111111, true)]
    [TestCase(1234, false)]
    public void Part2(long value, bool expected) {
        Assert.AreEqual(expected,  value.IsInvalidId(int.MaxValue));  
    }
}

public static class DanisTestExtensions {
    public static bool IsInvalidId(this long value, int maxRepeats = 2) {
        return new GiftShop.IdRange(value, value).FindInvalidIds(maxRepeats).SingleOrDefault() == value;
    }
}