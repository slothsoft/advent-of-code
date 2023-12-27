using System;
using System.IO;
using System.Linq;
using NUnit.Framework;
using Point2D = AoC.day24.NeverTellMeTheOdds.Point2D;
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
    public void Example1_Line3D_Intersect(string line1AsString, string line2AsString, string? expectedResultAsString) {
        var line1 = NeverTellMeTheOdds.ParseLine(line1AsString);
        var line2 = NeverTellMeTheOdds.ParseLine(line2AsString);

        Point3D? expectedResult = expectedResultAsString == null ? null : NeverTellMeTheOdds.ParsePoint(expectedResultAsString);

        Assert.AreEqual(expectedResult, line1.CalculateIntersection(line2));
        Assert.AreEqual(expectedResult, line2.CalculateIntersection(line1));
    }

    [Test]
    // AoC Example I
    [TestCase("19, 13, 30 @ -2, 1, -2", "18, 19, 22 @ -1, -1, -2", "14.333, 15.333, 0")]
    // AoC Example II
    [TestCase("19, 13, 30 @ -2, 1, -2", "20, 25, 34 @ -2, -2, -4", "11.667, 16.667, 0")]
    // AoC Example III
    [TestCase("19, 13, 30 @ -2, 1, -2", "12, 31, 28 @ -1, -2, -1", "6.2, 19.4, 0")]
    // AoC Example V
    [TestCase("18, 19, 22 @ -1, -1, -2", "20, 25, 34 @ -2, -2, -4", null)]
    // AoC Example VI
    [TestCase("18, 19, 22 @ -1, -1, -2", "12, 31, 28 @ -1, -2, -1", "-6, -5, 0")]
    // AoC Example VIII
    [TestCase("20, 25, 34 @ -2, -2, -4", "12, 31, 28 @ -1, -2, -1", "-2, 3, 0")]
    public void Example1_Line2D_Intersect(string line1AsString, string line2AsString, string? expectedResultAsString) {
        var line1 = NeverTellMeTheOdds.ParseLine(line1AsString).To2D();
        var line2 = NeverTellMeTheOdds.ParseLine(line2AsString).To2D();

        var intersection1 = line1.CalculateIntersection(line2);

        const double delta = 0.001;

        if (expectedResultAsString == null) {
            Assert.Null(intersection1);
        } else {
            var expectedResult = NeverTellMeTheOdds.ParsePoint(expectedResultAsString!).To2D();

            Assert.NotNull(intersection1);
            Assert.AreEqual(expectedResult.X, intersection1!.Value.X, delta);
            Assert.AreEqual(expectedResult.Y, intersection1.Value.Y, delta);
        }

        var intersection2 = line2.CalculateIntersection(line1);
        Assert.AreEqual(intersection1?.X ?? 0.0, intersection2?.X ?? 0.0, delta);
        Assert.AreEqual(intersection1?.Y ?? 0.0, intersection2?.Y ?? 0.0, delta);
    }

    [Test]
    public void Example1() {
        var example = new NeverTellMeTheOdds(File.ReadAllLines(@"24\example.txt"), true);

        Assert.AreEqual(2, example.CalculateIntersections(7, 27));
    }

    [Test]
    public void Puzzle1() {
        var puzzle = new NeverTellMeTheOdds(File.ReadAllLines(@"24\input.txt"), true);

        Assert.AreEqual(17867, puzzle.CalculateIntersections(200000000000000, 400000000000000));
    }

    [Test]
    public void Example2() {
        var example = new NeverTellMeTheOdds(File.ReadAllLines(@"24\example.txt"));

        var rockThrow = example.CalculateRockThrow();
        Assert.NotNull(rockThrow);
        
        Assert.AreEqual(24, rockThrow!.ZeroCoordinates.X);      
        Assert.AreEqual(13, rockThrow.ZeroCoordinates.Y);
        Assert.AreEqual(10, rockThrow.ZeroCoordinates.Z);    
        
        Assert.AreEqual(-3, rockThrow.VelocityCoordinates.X); 
        Assert.AreEqual(1, rockThrow.VelocityCoordinates.Y); 
        Assert.AreEqual(2, rockThrow.VelocityCoordinates.Z); 
        
        Assert.AreEqual(47, rockThrow.ZeroCoordinates.GetCoordinates().Select(rockThrow.ZeroCoordinates.Get).Sum());    
    }

    [Test]
    public void Puzzle2() {
        var puzzle = new NeverTellMeTheOdds(File.ReadAllLines(@"24\input.txt"));

        // Assert.AreEqual(7, puzzle.CalculateIntersections());
    }
}