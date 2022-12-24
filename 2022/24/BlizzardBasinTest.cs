using System.IO;
using NUnit.Framework;

namespace AoC._24;

public class BlizzardBasinTest {
    [Test]
    public void Example1ASteps() {
        var blizzardBasin = new BlizzardBasin(File.ReadAllLines(@"24\exampleA.txt"));

        Assert.AreEqual(@"#.#####
#.....#
#>....#
#.....#
#...v.#
#.....#
#####.#
", blizzardBasin.map.ToString());

        blizzardBasin.map.ExecuteNextMinute();
        
        Assert.AreEqual(@"#.#####
#.....#
#.>...#
#.....#
#.....#
#...v.#
#####.#
", blizzardBasin.map.ToString());
        
        blizzardBasin.map.ExecuteNextMinute();
        
        Assert.AreEqual(@"#.#####
#...v.#
#..>..#
#.....#
#.....#
#.....#
#####.#
", blizzardBasin.map.ToString());
        
        blizzardBasin.map.ExecuteNextMinute();
        
        Assert.AreEqual(@"#.#####
#.....#
#...2.#
#.....#
#.....#
#.....#
#####.#
", blizzardBasin.map.ToString());
        
        blizzardBasin.map.ExecuteNextMinute();
        
        Assert.AreEqual(@"#.#####
#.....#
#....>#
#...v.#
#.....#
#.....#
#####.#
", blizzardBasin.map.ToString());
        
        blizzardBasin.map.ExecuteNextMinute();
        
        Assert.AreEqual(@"#.#####
#.....#
#>....#
#.....#
#...v.#
#.....#
#####.#
", blizzardBasin.map.ToString());
    }

    [Test]
    public void Example1BSteps() {
        var blizzardBasin = new BlizzardBasin(File.ReadAllLines(@"24\exampleB.txt"));

        Assert.AreEqual(@"#.######
#>>.<^<#
#.<..<<#
#>v.><>#
#<^v^^>#
######.#
", blizzardBasin.map.ToString());

    }
    
    [Test]
    public void Example1B() {
        var blizzardBasin = new BlizzardBasin(File.ReadAllLines(@"24\exampleB.txt"));

        Assert.AreEqual(18, blizzardBasin.CalculateQuickestPath());
    }

    [Test]
    public void Puzzle1() {
        var blizzardBasin = new BlizzardBasin(File.ReadAllLines(@"24\input.txt"));

        var result = blizzardBasin.CalculateQuickestPath();
        Assert.AreEqual(314, result);
        Assert.Pass("Puzzle 1: " + result);
    }

    [Test]
    public void Example2() {
        var blizzardBasin = new BlizzardBasin(File.ReadAllLines(@"24\exampleB.txt"));

        Assert.AreEqual(54, blizzardBasin.CalculateQuickestPathBackAndForth());
    }

    [Test]
    public void Puzzle2() {
        var blizzardBasin = new BlizzardBasin(File.ReadAllLines(@"24\input.txt"));

        var result = blizzardBasin.CalculateQuickestPathBackAndForth();
        //Assert.AreEqual(314, result);
        Assert.Pass("Puzzle 1: " + result);
    }
}