using System.IO;
using NUnit.Framework;

namespace AoC._15;

public class BeaconExclusionZoneTest {
    [Test]
    [TestCase(7, 8, 7, 8, 0)]
    [TestCase(2, 0, 2, 1, 1)]
    [TestCase(3, 4, 4, 5, 2)]
    [TestCase(8, 7, 2, 10, 9)]
    public void Example1TaxicabGeometry(int point1X, int point1Y, int point2X, int point2Y, int expectedDifference) {
        Assert.AreEqual(expectedDifference, new Point(point1X, point1Y) - new Point(point2X, point2Y));
        Assert.AreEqual(expectedDifference, new Point(point2X, point2Y) - new Point(point1X, point1Y));
    }
    
    // ....012345
    // .    #
    // 0   ###
    // 1  #####
    // 2 ###S###
    // 3  #####
    // 4   ###
    // 5    #
    
    [Test]
    [TestCase(-2, 0)]
    [TestCase(-1, 1)]
    [TestCase(0, 3)]
    [TestCase(1, 5)]
    [TestCase(2, 7)]
    [TestCase(3, 5)]
    [TestCase(4, 3)]
    [TestCase(5, 1)]
    [TestCase(6, 0)]
    public void Example1SimpleExampleFromCycle(int row, int expectedExlusions) {
        var zone = new BeaconExclusionZone();
        zone.AddCycle(new Cycle(new Point(1, 2), 3));
        
        Assert.AreEqual(expectedExlusions, zone.CalculateExclusionsInRow(row));
    }
    
    [Test]
    [TestCase(-2, 0)]
    [TestCase(-1, 1)]
    [TestCase(0, 3)]
    [TestCase(1, 5)]
    [TestCase(2, 7)]
    [TestCase(3, 5)]
    [TestCase(4, 2)] // the beacon is here
    [TestCase(5, 1)]
    [TestCase(6, 0)]
    public void Example1SimpleExampleFromString(int row, int expectedExlusions) {
        var zone = new BeaconExclusionZone();
        zone.AddCycle("Sensor at x=1, y=2: closest beacon is at x=2, y=4");
        
        Assert.AreEqual(expectedExlusions, zone.CalculateExclusionsInRow(row));
    }
    
    [Test]
    public void Example1() {
        var zone = new BeaconExclusionZone();
        zone.AddCycles(File.ReadAllLines(@"15\example.txt"));
        
        Assert.AreEqual(26, zone.CalculateExclusionsInRow(10));
    }

    [Test]
    public void Puzzle1() {
        var zone = new BeaconExclusionZone();
        zone.AddCycles(File.ReadAllLines(@"15\input.txt"));

        var result = zone.CalculateExclusionsInRow(2_000_000);
        Assert.AreEqual(4961647, result);
        Assert.Pass("Puzzle 1: " + result);
    }

    [Test]
    public void Example2() {
        var zone = new BeaconExclusionZone();
        zone.AddCycles(File.ReadAllLines(@"15\example.txt"));
        
        Assert.AreEqual(56_000_011, zone.CalculateTuningFrequency(20));
    }

    [Test]
    public void Puzzle2() {
        var zone = new BeaconExclusionZone();
        zone.AddCycles(File.ReadAllLines(@"15\input.txt"));

        var result = zone.CalculateTuningFrequency(4_000_000);
        Assert.AreEqual(12_274_327_017_867L, result);
        Assert.Pass("Puzzle 2: " + result);
    }
}