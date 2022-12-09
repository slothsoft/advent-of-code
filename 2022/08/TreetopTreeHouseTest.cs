using System.IO;
using NUnit.Framework;

namespace AoC._08;

public class TreetopTreeHouseTest {
    
    [Test]
    public void Example1() {
        Assert.AreEqual(21, TreetopTreeHouse.FindVisibleTreesCount(File.ReadAllLines(@"08\example.txt")));
    }
    
    [Test]
    public void Puzzle1() {
        var result = TreetopTreeHouse.FindVisibleTreesCount(File.ReadAllLines(@"08\input.txt"));
        Assert.AreEqual(1812, result);
        Assert.Pass("Puzzle 1: " + result);
    }

    [Test]
    public void Example2ScenicScoreA() {
        Assert.AreEqual(1, TreetopTreeHouse.CalculateScenicScoreInRow("53"));
        Assert.AreEqual(1, TreetopTreeHouse.CalculateScenicScoreInRow("552"));
        Assert.AreEqual(2, TreetopTreeHouse.CalculateScenicScoreInRow("512"));
        Assert.AreEqual(2, TreetopTreeHouse.CalculateScenicScoreInRow("5353"));
        
        Assert.AreEqual(0, TreetopTreeHouse.CalculateScenicScore(File.ReadAllLines(@"08\example.txt"), 0, 2));
        Assert.AreEqual(0, TreetopTreeHouse.CalculateScenicScore(File.ReadAllLines(@"08\example.txt"), 1, 0));
        Assert.AreEqual(4, TreetopTreeHouse.CalculateScenicScore(File.ReadAllLines(@"08\example.txt"), 1, 2));
    }
    
    [Test]
    public void Example2ScenicScoreB() {
        Assert.AreEqual(2, TreetopTreeHouse.CalculateScenicScoreInRow("5353"));
        Assert.AreEqual(2, TreetopTreeHouse.CalculateScenicScoreInRow("533"));
        Assert.AreEqual(1, TreetopTreeHouse.CalculateScenicScoreInRow("53"));
        Assert.AreEqual(2, TreetopTreeHouse.CalculateScenicScoreInRow("549"));

        Assert.AreEqual(0, TreetopTreeHouse.CalculateScenicScore(File.ReadAllLines(@"08\example.txt"), 0, 2));
        Assert.AreEqual(0, TreetopTreeHouse.CalculateScenicScore(File.ReadAllLines(@"08\example.txt"), 3, 0));
        Assert.AreEqual(8, TreetopTreeHouse.CalculateScenicScore(File.ReadAllLines(@"08\example.txt"), 3, 2));
    }

    
    [Test]
    public void Example2() {
        Assert.AreEqual(8, TreetopTreeHouse.FindTreeHouseSpot(File.ReadAllLines(@"08\example.txt")));
    }
    
    [Test]
    public void Puzzle2() {
        var result = TreetopTreeHouse.FindTreeHouseSpot(File.ReadAllLines(@"08\input.txt"));
        Assert.AreEqual("315495", result);
        Assert.Pass("Puzzle 2: " + result);
    }
}