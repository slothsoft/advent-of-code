using System.Linq;
using NUnit.Framework;

namespace AoC;

public class WaitForItTest {
    private static readonly string[] ExampleInput = {
        "Time:      7  15   30",
        "Distance:  9  40  200"
    };
    private static readonly string[] PuzzleInput = {
        "Time:        41     77     70     96",
        "Distance:   249   1362   1127   1011",
    };

    [Test]
    public void Example1_ParseRaces() {
        var races = WaitForIt.ParseRaces(ExampleInput);
        Assert.AreEqual(3, races.Length);

        var race = races[0];
        Assert.AreEqual(7, race.Time);
        Assert.AreEqual(9, race.Distance);
        
        race = races[1];
        Assert.AreEqual(15, race.Time);
        Assert.AreEqual(40, race.Distance);
        
        race = races[2];
        Assert.AreEqual(30, race.Time);
        Assert.AreEqual(200, race.Distance);
    }
    
    [Test]
    [TestCase(0, new[] {2, 3, 4, 5})]
    [TestCase(1, new[] {4, 5, 6, 7, 8, 9, 10, 11})]
    [TestCase(2, new[] {11, 12, 13, 14, 15, 16, 17, 18, 19})]
    public void Example1_CalculateWaysToWin(int raceId, int[] expectedWaysToWin) {
        var algorithm = new WaitForIt(ExampleInput);
        Assert.AreEqual(expectedWaysToWin, algorithm.CalculateWaysToWin(algorithm.Races.Single(r => r.Id == raceId)).ToArray());
    }
    
    [Test]
    public void Example1() {
        var algorithm = new WaitForIt(ExampleInput);
        Assert.AreEqual(288, algorithm.CalculateMarginOfError());
    }
    
    [Test]
    public void Puzzle1() {
        var algorithm = new WaitForIt(PuzzleInput);
        
        Assert.AreEqual(771628, algorithm.CalculateMarginOfError());
    }
    
    [Test]
    public void Example2_ParseRaces() {
        var races = WaitForIt.ParseRaces(ExampleInput, true);
        Assert.AreEqual(1, races.Length);

        var race = races[0];
        Assert.AreEqual(71530, race.Time);
        Assert.AreEqual(940200, race.Distance);
    }
    
    [Test]
    public void Example2_CalculateWaysToWin() {
        var algorithm = new WaitForIt(ExampleInput, true);

        var waysToWin = algorithm.CalculateWaysToWin(algorithm.Races.Single()).ToArray();
        Assert.AreEqual(14, waysToWin.First());
        Assert.AreEqual(71516, waysToWin.Last());
    }
    
    [Test]
    public void Example2() {
        var algorithm = new WaitForIt(ExampleInput, true);
        Assert.AreEqual(71503, algorithm.CalculateMarginOfError());
    }
    
    [Test]
    public void Puzzle2() {
        var algorithm = new WaitForIt(PuzzleInput, true);
        
        Assert.AreEqual(27363861, algorithm.CalculateMarginOfError());
    }

}