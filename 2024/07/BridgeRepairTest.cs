using System.IO;
using NUnit.Framework;

namespace AoC.day7;

public class BridgeRepairTest {
    [Test]
    public void Example1() {
        var example = new BridgeRepair(File.ReadAllLines(@"07\example.txt"));
        
        Assert.AreEqual(3749,  example.CalculateCalibrationResult());       
    }

    [Test]
    public void Puzzle1() {
        var puzzle = new BridgeRepair(File.ReadAllLines(@"07\input.txt"));
        
        Assert.AreEqual(7,  puzzle.CalculateCalibrationResult());  
    }

    [Test]
    public void Example2() {
        var example = new BridgeRepair(File.ReadAllLines(@"07\example.txt"));
        
        Assert.AreEqual(7,  example.CalculateCalibrationResult());   
    }

    [Test]
    public void Puzzle2() {
        var puzzle = new BridgeRepair(File.ReadAllLines(@"07\input.txt"));
        
        Assert.AreEqual(7,  puzzle.CalculateCalibrationResult());  
    }
}
