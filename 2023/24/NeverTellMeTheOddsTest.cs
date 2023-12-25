using System.IO;
using NUnit.Framework;
using Point3D = AoC.day24.NeverTellMeTheOdds.Point3D;

namespace AoC.day24;

public class NeverTellMeTheOddsTest {
    // Examples from https://emedia.rmit.edu.au/learninglab/content/v7-intersecting-lines-3d
    [Test]
    // Example 1
    [TestCase("1, -4, 8 @  2, 1, -2", "5, 1, 8 @  1, -1, -3", "7, -1, 2")]
    // Example 2
    [TestCase("0, -4, 8 @  1, 1, -2", "2, -1, 3 @  2, 1, -2", null)]
    // Excercise
    [TestCase("2, 3, 3 @  2, 2, 1", "-2, 2, 0 @  2, -1, 2", "0, 1, 2")]
    // AoC Example I
    [TestCase("19, 13, 30 @ -2, 1, -2", "18, 19, 22 @ -1, -1, -2", "14.333, 15.333, 0")]
    public void Example1_Line3D_Intersect(string line1AsString, string line2AsString, string? expectedResultAsString) {
        var line1 = NeverTellMeTheOdds.ParseLine(line1AsString);
        var line2 = NeverTellMeTheOdds.ParseLine(line2AsString);

        Point3D? expectedResult = expectedResultAsString == null ? null : NeverTellMeTheOdds.ParsePoint(expectedResultAsString);

        Assert.AreEqual(expectedResult, line1.CalculateIntersection(line2));
        Assert.AreEqual(expectedResult, line2.CalculateIntersection(line1));
    }

    [Test]
    public void Example1() {
        var example = new NeverTellMeTheOdds(File.ReadAllLines(@"24\example.txt"), true);

        Assert.AreEqual(2, example.CalculateIntersections(7, 27));
    }

    [Test]
    public void Puzzle1() {
        var puzzle = new NeverTellMeTheOdds(File.ReadAllLines(@"24\input.txt"));

        // Assert.AreEqual(7, puzzle.CalculateIntersections());
    }

    [Test]
    public void Example2() {
        var example = new NeverTellMeTheOdds(File.ReadAllLines(@"24\example.txt"));

        // Assert.AreEqual(7, example.CalculateIntersections());
    }

    [Test]
    public void Puzzle2() {
        var puzzle = new NeverTellMeTheOdds(File.ReadAllLines(@"24\input.txt"));

        // Assert.AreEqual(7, puzzle.CalculateIntersections());
    }
}