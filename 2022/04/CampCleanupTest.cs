using System.IO;
using NUnit.Framework;

namespace AoC;

public class CampCleanupTest {

    private static readonly string[] ExampleInput = {
        "2-4,6-8",
        "2-3,4-5",
        "5-7,7-9",
        "2-8,3-7",
        "6-6,4-6",
        "2-6,4-8"
    };
    
    [Test]
    public void Example1() {
        Assert.AreEqual(2, CampCleanup.CalculateContains(ExampleInput));
    }
    
    [Test]
    public void Puzzle1() {
        var result = CampCleanup.CalculateContains(File.ReadAllLines(@"04\input.txt"));
        Assert.AreEqual(483, result);
        Assert.Pass("Puzzle 1: " + result);
    }

    [Test]
    public void Example2() {
        Assert.AreEqual(4, CampCleanup.CalculateOverlap(ExampleInput));
    }
    
    [Test]
    public void Puzzle2() {
        var result = CampCleanup.CalculateOverlap(File.ReadAllLines(@"04\input.txt"));
        Assert.AreEqual(874, result);
        Assert.Pass("Puzzle 2: " + result);
    }
}