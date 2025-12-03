using System.IO;
using NUnit.Framework;

namespace AoC.day3;

public class LobbyTest {
    [Test]
    [TestCase("987654321111111", 98)]
    [TestCase("811111111111119", 89)]
    [TestCase("234234234234278", 78)]
    [TestCase("818181911112111", 92)]
    public void Example1_ParseInput(string input, long expected) {
        var batteryBank = new Lobby.BatteryBank(input);
        Assert.AreEqual(expected,  batteryBank.CalculateJoltage());
    }
    
    [Test]
    public void Example1() {
        var example = new Lobby(File.ReadAllLines(@"03\example.txt"));
        
        Assert.AreEqual(357,  example.CalculateTotalJoltage());       
    }

    [Test]
    public void Puzzle1() {
        var puzzle = new Lobby(File.ReadAllLines(@"03\input.txt"));
        
        Assert.AreEqual(17031,  puzzle.CalculateTotalJoltage());  
    }

    [Test]
    public void Example2() {
        var example = new Lobby(File.ReadAllLines(@"03\example.txt"));
        
        Assert.AreEqual(7,  example.CalculateTotalJoltage());   
    }

    [Test]
    public void Puzzle2() {
        var puzzle = new Lobby(File.ReadAllLines(@"03\input.txt"));
        
        Assert.AreEqual(7,  puzzle.CalculateTotalJoltage());  
    }
}
