using System.IO;
using NUnit.Framework;

namespace AoC.day9;

public class MirageMaintenanceTest {
    [Test]
    [TestCase(new[] {0L, 3, 6, 9, 12, 15}, 18L)]
    [TestCase(new[] {1L, 3, 6, 10, 15, 21}, 28L)]
    [TestCase(new[] {10L, 13, 16, 21, 30, 45}, 68L)]
    // 10 10 16 32 62 110 180 276 402 562 760 1000 1286 1622 2012 2460 2970 3546 4192 4912 5710   => 6590
    //   0  6 16 30 48  70   96 126  160 198 240 286  336  390  448  510  576  646  720  798      => 880
    //    6  10 14 18  22  26  30  34  38  42   46  50   54   58  62    66   70   74   78         => 82
    //      4  4  4  ...
    [TestCase(new[] {10L, 10, 16, 32, 62, 110, 180, 276, 402, 562, 760, 1000, 1286, 1622, 2012, 2460, 2970, 3546, 4192, 4912, 5710}, 6590L)]
    public void Example1_Extrapolate(long[] input, long expectedValue) {
        Assert.AreEqual(expectedValue, MirageMaintenance.Extrapolate(input));
    }

    [Test]
    public void Example1() {
        var example = new MirageMaintenance(File.ReadAllLines(@"09\example.txt"));

        Assert.AreEqual(114, example.SumExtrapolatedValues());
    }

    [Test]
    public void Puzzle1() {
        var puzzle = new MirageMaintenance(File.ReadAllLines(@"09\input.txt"));

        Assert.AreEqual(2175229206L, puzzle.SumExtrapolatedValues());
    }

    [Test]
    [TestCase(new[] {0L, 3, 6, 9, 12, 15}, -3L)]
    [TestCase(new[] {1L, 3, 6, 10, 15, 21}, 0L)]
    [TestCase(new[] {10L, 13, 16, 21, 30, 45}, 5L)]
    public void Example1_Revextrapolate(long[] input, long expectedValue) {
        Assert.AreEqual(expectedValue, MirageMaintenance.Revextrapolate(input));
    }

    [Test]
    public void Example2() {
        var example = new MirageMaintenance(File.ReadAllLines(@"09\example.txt"));

        Assert.AreEqual(2, example.SumRevextrapolatedValues());
    }
    
    [Test]
    public void Puzzle2() {
        var puzzle = new MirageMaintenance(File.ReadAllLines(@"09\input.txt"));

        Assert.AreEqual(2175229206L, puzzle.SumRevextrapolatedValues());
    }
}