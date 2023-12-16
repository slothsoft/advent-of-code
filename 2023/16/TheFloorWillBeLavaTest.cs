using System.Collections.Generic;
using System.IO;
using System.Linq;
using NUnit.Framework;
using static AoC.TheFloorWillBeLava.Direction;
using Direction = AoC.TheFloorWillBeLava.Direction;

namespace AoC;

public class TheFloorWillBeLavaTest {
    [Test]
    [TestCase(".", 1, 0)]
    [TestCase("|", 0, -1)]
    [TestCase("-", 1, 0)]
    [TestCase("/", 0, -1)]
    [TestCase("\\", 0, 1)]
    public void Example1_MoveStep(string input, int expectedX, int expectedY) {
        var example = new TheFloorWillBeLava(new List<string> {
            $"{input}.", //
            "..", //
        });
        var rayOfLight = example.RaysOfLight.First();
        example.MoveStep(rayOfLight);
        
        Assert.AreEqual(expectedX, rayOfLight.X);
        Assert.AreEqual(expectedY, rayOfLight.Y);
    }
    
    [Test]
    [TestCaseSource(nameof(CreateMirrorTestCaseData))]
    public void Example1_MoveStepForMirrors(string input, Direction startPointFromInput, Direction expectedFinalDirection) {
        var example = new TheFloorWillBeLava(new List<string> {
            "...", //
            $".{input}.", //
            "...", //
        });
        var rayOfLight = example.RaysOfLight.Single();
        rayOfLight.X = 1 + startPointFromInput.XPlus;
        rayOfLight.Y = 1 + startPointFromInput.YPlus;
        rayOfLight.Direction = startPointFromInput.ToOppositeDirection();
        
        // moves the ray onto the mirror
        example.MoveStep();
        // moves the ray away from the mirror
        example.MoveStep();
        
        Assert.AreEqual(expectedFinalDirection, rayOfLight.Direction);
        Assert.AreEqual(1 + expectedFinalDirection.XPlus, rayOfLight.X);
        Assert.AreEqual(1 + expectedFinalDirection.YPlus, rayOfLight.Y);
    }

    private static IEnumerable<TestCaseData> CreateMirrorTestCaseData() {
        yield return new TestCaseData("/", North, West);
        yield return new TestCaseData("/", West, North);
        yield return new TestCaseData("/", East, South);
        yield return new TestCaseData("/", South, East);
        
        yield return new TestCaseData("\\", North, East);
        yield return new TestCaseData("\\", East, North);
        yield return new TestCaseData("\\", West, South);
        yield return new TestCaseData("\\", South, West);
    }
    
    [Test]
    [TestCaseSource(nameof(CreateSplitterTestCaseData))]
    public void Example1_MoveStepForSplitter(string input, Direction startPointFromInput, Direction[] expectedFinalDirections) {
        var example = new TheFloorWillBeLava(new List<string> {
            "...", //
            $".{input}.", //
            "...", //
        });
        var rayOfLight = example.RaysOfLight.Single();
        rayOfLight.X = 1 + startPointFromInput.XPlus;
        rayOfLight.Y = 1 + startPointFromInput.YPlus;
        rayOfLight.Direction = startPointFromInput.ToOppositeDirection();
        
        // moves the ray onto the splitter
        example.MoveStep();
        // moves the ray(s) away from the splitter
        example.MoveStep();
        
        Assert.AreEqual(expectedFinalDirections.Length, example.RaysOfLight.Count());
        
        for (var i = 0; i < expectedFinalDirections.Length; i++) {
            var expectedX = 1 + expectedFinalDirections[i].XPlus;
            var expectedY = 1 + expectedFinalDirections[i].YPlus;
            Assert.True(example.RaysOfLight.Any(r => r.X == expectedX && r.Y == expectedY), $"expected ({expectedX}, {expectedY}), but found {string.Join(", ", example.RaysOfLight)}");
        }
    }

    private static IEnumerable<TestCaseData> CreateSplitterTestCaseData() {
        yield return new TestCaseData("|", North, new[] { South });
        yield return new TestCaseData("|", East, new[] { North, South });
        yield return new TestCaseData("|", South, new[] { North });
        yield return new TestCaseData("|", West, new[] { North, South });
        
        yield return new TestCaseData("-", North, new[] { East, West });
        yield return new TestCaseData("-", East, new[] { West });
        yield return new TestCaseData("-", South, new[] { East, West });
        yield return new TestCaseData("-", West, new[] { East });
    }

    [Test]
    public void Example1() {
        var example = new TheFloorWillBeLava(File.ReadAllLines(@"16\example.txt"));
        var result = example.CalculateEnergizedTilesCount();
        
        Assert.AreEqual(46, result);
    }

    [Test]
    public void Puzzle1() {
        var example = new TheFloorWillBeLava(File.ReadAllLines(@"16\input.txt"));
        var result = example.CalculateEnergizedTilesCount();
        
        Assert.AreEqual(7498, result);
    }

    [Test]
    public void Example2() {
        var example = new TheFloorWillBeLava(File.ReadAllLines(@"16\example.txt"));
        var result = example.CalculateMaxEnergizedTilesCount();
        
        Assert.AreEqual(51, result);
    }

    [Test]
    public void Puzzle2() {
        var example = new TheFloorWillBeLava(File.ReadAllLines(@"16\input.txt"));
        var result = example.CalculateMaxEnergizedTilesCount();
        
        // 7903 too high
        Assert.AreEqual(7846, result);
    }
}