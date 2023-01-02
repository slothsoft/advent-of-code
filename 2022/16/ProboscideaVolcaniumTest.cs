using System.IO;
using NUnit.Framework;

namespace AoC._16;

public class ProboscideaVolcaniumTest {
    [Test]
    public void Example1() {
        var volcanium = new ProboscideaVolcanium(File.ReadAllLines(@"16\example.txt"));
        
        Assert.AreEqual(1651, volcanium.CalculateMaxPressure(30));
    }

    [Test]
    public void Puzzle1() {
        var volcanium = new ProboscideaVolcanium(File.ReadAllLines(@"16\input.txt"));
        var result = volcanium.CalculateMaxPressure(30);
        Assert.AreEqual(2250, result);
        Assert.Pass("Puzzle 1: " + result);
    }

    [Test]
    public void Example2() {
        var volcanium = new ProboscideaVolcanium(File.ReadAllLines(@"16\example.txt"));
        
        Assert.AreEqual(1707, volcanium.CalculateMaxPressureWithElephant(26));
    }

    [Test]
    public void Puzzle2() {
        var volcanium = new ProboscideaVolcanium(File.ReadAllLines(@"16\input.txt"));
        var result = volcanium.CalculateMaxPressureWithElephant(26);
        Assert.AreEqual(3015, result);
        Assert.Pass("Puzzle 2: " + result);
    }
}