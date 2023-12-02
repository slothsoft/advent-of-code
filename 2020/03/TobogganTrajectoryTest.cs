using System.IO;
using NUnit.Framework;

namespace AoC;

public class TobogganTrajectoryTest {
    [Test]
    public void Example1() {
        var algorithm = new TobogganTrajectory(File.ReadAllLines(@"03\example.txt"));

        Assert.AreEqual(7, algorithm.CountTreesForSlope(3, 1));
    }

    [Test]
    public void Puzzle1() {
        var algorithm = new TobogganTrajectory(File.ReadAllLines(@"03\input.txt"));

        Assert.AreEqual(257, algorithm.CountTreesForSlope(3, 1));
    }

    [Test]
    public void Example2() {
        var algorithm = new TobogganTrajectory(File.ReadAllLines(@"03\example.txt"));

        Assert.AreEqual(336, algorithm.CountTreesForAllSlopes());
    }

    [Test]
    public void Puzzle2() {
        var algorithm = new TobogganTrajectory(File.ReadAllLines(@"03\input.txt"));

        Assert.AreEqual(1744787392, algorithm.CountTreesForAllSlopes());
    }
}