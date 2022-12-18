using System;
using System.IO;
using NUnit.Framework;

namespace AoC._17;

public class PyroclasticFlowTest {
    [Test]
    public void Example1SmallSteps() {
        var pyroclasticFlow = new PyroclasticFlow(File.ReadAllText(@"17\example.txt"));

        // The first rock begins falling
        pyroclasticFlow.PerformStep();
        Assert.AreEqual(@"|..@@@@.|
|.......|
|.......|
|.......|
+-------+", pyroclasticFlow.Stringify());

        // Jet of gas pushes rock right:
        pyroclasticFlow.PerformJetPatternStep();
        Assert.AreEqual(@"|...@@@@|
|.......|
|.......|
|.......|
+-------+", pyroclasticFlow.Stringify());

        // Rock falls 1 unit:
        pyroclasticFlow.PerformGravityStep();
        Assert.AreEqual(@"|...@@@@|
|.......|
|.......|
+-------+", pyroclasticFlow.Stringify());

        // Jet of gas pushes rock right, but nothing happens:
        pyroclasticFlow.PerformJetPatternStep();
        Assert.AreEqual(@"|...@@@@|
|.......|
|.......|
+-------+", pyroclasticFlow.Stringify());

        // Rock falls 1 unit:
        pyroclasticFlow.PerformGravityStep();
        Assert.AreEqual(@"|...@@@@|
|.......|
+-------+", pyroclasticFlow.Stringify());

        // Jet of gas pushes rock right, but nothing happens:
        pyroclasticFlow.PerformJetPatternStep();
        Assert.AreEqual(@"|...@@@@|
|.......|
+-------+", pyroclasticFlow.Stringify());

        // Rock falls 1 unit:
        pyroclasticFlow.PerformGravityStep();
        Assert.AreEqual(@"|...@@@@|
+-------+", pyroclasticFlow.Stringify());

        // Jet of gas pushes rock left:
        pyroclasticFlow.PerformJetPatternStep();
        Assert.AreEqual(@"|..@@@@.|
+-------+", pyroclasticFlow.Stringify());

        // Rock falls 1 unit, causing it to come to rest:
        pyroclasticFlow.PerformGravityStep();
        Assert.AreEqual(@"|..####.|
+-------+", pyroclasticFlow.Stringify());

        // A new rock begins falling:
        pyroclasticFlow.PerformStep();
        Assert.AreEqual(-3, pyroclasticFlow.Stone?.Y);
        Assert.AreEqual(@"|...@...|
|..@@@..|
|...@...|
|.......|
|.......|
|.......|
|..####.|
+-------+", pyroclasticFlow.Stringify());

        // Jet of gas pushes rock left:
        pyroclasticFlow.PerformJetPatternStep();
        Assert.AreEqual(-3, pyroclasticFlow.Stone?.Y);
        Assert.AreEqual(@"|..@....|
|.@@@...|
|..@....|
|.......|
|.......|
|.......|
|..####.|
+-------+", pyroclasticFlow.Stringify());

        // Rock falls 1 unit:
        pyroclasticFlow.PerformGravityStep();
        Assert.AreEqual(-2, pyroclasticFlow.Stone?.Y);
        Assert.AreEqual(@"|..@....|
|.@@@...|
|..@....|
|.......|
|.......|
|..####.|
+-------+", pyroclasticFlow.Stringify());

        // Jet of gas pushes rock right:
        pyroclasticFlow.PerformJetPatternStep();
        Assert.AreEqual(-2, pyroclasticFlow.Stone?.Y);
        Assert.AreEqual(@"|...@...|
|..@@@..|
|...@...|
|.......|
|.......|
|..####.|
+-------+", pyroclasticFlow.Stringify());

        // Rock falls 1 unit:
        pyroclasticFlow.PerformGravityStep();
        Assert.AreEqual(-1, pyroclasticFlow.Stone?.Y);
        Assert.AreEqual(@"|...@...|
|..@@@..|
|...@...|
|.......|
|..####.|
+-------+", pyroclasticFlow.Stringify());

        // Jet of gas pushes rock left:
        pyroclasticFlow.PerformJetPatternStep();
        Assert.AreEqual(-1, pyroclasticFlow.Stone?.Y);
        Assert.AreEqual(@"|..@....|
|.@@@...|
|..@....|
|.......|
|..####.|
+-------+", pyroclasticFlow.Stringify());

        // Rock falls 1 unit:
        pyroclasticFlow.PerformGravityStep();
        Assert.AreEqual(0, pyroclasticFlow.Stone?.Y);
        Assert.AreEqual(@"|..@....|
|.@@@...|
|..@....|
|..####.|
+-------+", pyroclasticFlow.Stringify());

        // Jet of gas pushes rock right:
        pyroclasticFlow.PerformJetPatternStep();
        Assert.AreEqual(0, pyroclasticFlow.Stone?.Y);
        Assert.AreEqual(@"|...@...|
|..@@@..|
|...@...|
|..####.|
+-------+", pyroclasticFlow.Stringify());

        // Rock falls 1 unit, causing it to come to rest:
        pyroclasticFlow.PerformGravityStep();
        Assert.IsNull(pyroclasticFlow.Stone);
        Assert.AreEqual(@"|...#...|
|..###..|
|...#...|
|..####.|
+-------+", pyroclasticFlow.Stringify());

        // A new rock begins falling:
        pyroclasticFlow.PerformStep();
        Assert.AreEqual(@"|....@..|
|....@..|
|..@@@..|
|.......|
|.......|
|.......|
|...#...|
|..###..|
|...#...|
|..####.|
+-------+", pyroclasticFlow.Stringify());
    }

    [Test]
    public void Example1BigSteps() {
        var pyroclasticFlow = new PyroclasticFlow(File.ReadAllText(@"17\example.txt"));

        // Rock 1
        pyroclasticFlow.ReleaseRock();
        Assert.AreEqual(@"|...@...|
|..@@@..|
|...@...|
|.......|
|.......|
|.......|
|..####.|
+-------+", pyroclasticFlow.Stringify());

        // Rock 2
        pyroclasticFlow.ReleaseRock();
        Assert.AreEqual(@"|....@..|
|....@..|
|..@@@..|
|.......|
|.......|
|.......|
|...#...|
|..###..|
|...#...|
|..####.|
+-------+", pyroclasticFlow.Stringify());

        // Rock 3
        pyroclasticFlow.ReleaseRock();
        Assert.AreEqual(@"|..@....|
|..@....|
|..@....|
|..@....|
|.......|
|.......|
|.......|
|..#....|
|..#....|
|####...|
|..###..|
|...#...|
|..####.|
+-------+", pyroclasticFlow.Stringify());

        // Rock 4
        pyroclasticFlow.ReleaseRock();
        Assert.AreEqual(@"|..@@...|
|..@@...|
|.......|
|.......|
|.......|
|....#..|
|..#.#..|
|..#.#..|
|#####..|
|..###..|
|...#...|
|..####.|
+-------+", pyroclasticFlow.Stringify());

        // Rock 5
        pyroclasticFlow.ReleaseRock();
        Assert.AreEqual(@"|..@@@@.|
|.......|
|.......|
|.......|
|....##.|
|....##.|
|....#..|
|..#.#..|
|..#.#..|
|#####..|
|..###..|
|...#...|
|..####.|
+-------+", pyroclasticFlow.Stringify());

        // Rock 6
        pyroclasticFlow.ReleaseRock();
        Assert.AreEqual(@"|...@...|
|..@@@..|
|...@...|
|.......|
|.......|
|.......|
|.####..|
|....##.|
|....##.|
|....#..|
|..#.#..|
|..#.#..|
|#####..|
|..###..|
|...#...|
|..####.|
+-------+", pyroclasticFlow.Stringify());

        // Rock 7
        pyroclasticFlow.ReleaseRock();
        Assert.AreEqual(@"|....@..|
|....@..|
|..@@@..|
|.......|
|.......|
|.......|
|..#....|
|.###...|
|..#....|
|.####..|
|....##.|
|....##.|
|....#..|
|..#.#..|
|..#.#..|
|#####..|
|..###..|
|...#...|
|..####.|
+-------+", pyroclasticFlow.Stringify());

        // Rock 8
        pyroclasticFlow.ReleaseRock();
        Assert.AreEqual(@"|..@....|
|..@....|
|..@....|
|..@....|
|.......|
|.......|
|.......|
|.....#.|
|.....#.|
|..####.|
|.###...|
|..#....|
|.####..|
|....##.|
|....##.|
|....#..|
|..#.#..|
|..#.#..|
|#####..|
|..###..|
|...#...|
|..####.|
+-------+", pyroclasticFlow.Stringify());

        // Rock 9
        pyroclasticFlow.ReleaseRock();
        Assert.AreEqual(@"|..@@...|
|..@@...|
|.......|
|.......|
|.......|
|....#..|
|....#..|
|....##.|
|....##.|
|..####.|
|.###...|
|..#....|
|.####..|
|....##.|
|....##.|
|....#..|
|..#.#..|
|..#.#..|
|#####..|
|..###..|
|...#...|
|..####.|
+-------+", pyroclasticFlow.Stringify());

        // Rock 10
        pyroclasticFlow.ReleaseRock();
        Assert.AreEqual(@"|..@@@@.|
|.......|
|.......|
|.......|
|....#..|
|....#..|
|....##.|
|##..##.|
|######.|
|.###...|
|..#....|
|.####..|
|....##.|
|....##.|
|....#..|
|..#.#..|
|..#.#..|
|#####..|
|..###..|
|...#...|
|..####.|
+-------+", pyroclasticFlow.Stringify());
    }

    [Test]
    public void Example1() {
        var pyroclasticFlow = new PyroclasticFlow(File.ReadAllText(@"17\example.txt"));
        pyroclasticFlow.ReleaseRocks(2022);

        Assert.AreEqual(3068, pyroclasticFlow.Height);
    }

    [Test]
    public void Puzzle1() {
        var pyroclasticFlow = new PyroclasticFlow(File.ReadAllText(@"17\input.txt"));
        pyroclasticFlow.ReleaseRocks(2022);
        var result = pyroclasticFlow.Height;
        Assert.AreEqual(3069, result);
        Assert.Pass("Puzzle 1: " + result);
    }

    [Test]
    public void Example2Log() {
        // With console log:
        // - pattern repeats every 2597
        // - (so 6 times in 10_000)
        // - the first 306 lines do not repeat
        var pyroclasticFlow = new PyroclasticFlow(File.ReadAllText(@"17\input.txt"));
        pyroclasticFlow.ReleaseRocks(10_000);
        Console.WriteLine(pyroclasticFlow.Stringify());
    }

    [Test]
    public void Example2() {
        // startOfRepetition = 26 lines
        // repetitionLength = 4293 lines
        // startOfRepetition = 15 rocks
        // repetitionLength = 2975 rocks
        var pyroclasticFlow = new PyroclasticFlow(File.ReadAllText(@"17\example.txt"));
        var result = pyroclasticFlow.CalculateRocksHeight(1_000_000_000_000);
        Assert.AreEqual(1_514_285_714_288, result);
    }

    [Test]
    public void Puzzle2() {
        // startOfRepetition = 308 lines
        // repetitionLength = 2597 lines, 1705 rocks
        var pyroclasticFlow = new PyroclasticFlow(File.ReadAllText(@"17\input.txt"));
        var result = pyroclasticFlow.CalculateRocksHeight(1_000_000_000_000);
        // Assert.AreEqual(1_514_285_714_288, result);
        Assert.Pass("Puzzle 2: " + result);
    }
}