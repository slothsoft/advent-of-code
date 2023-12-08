using System.IO;
using NUnit.Framework;

namespace AoC;

public class HauntedWastelandTest {

    [Test]
    public void Example1_ParseInput1() {
        var (instructions, map) = HauntedWasteland.ParseInput(File.ReadAllLines(@"08\example1.txt"));
        
        Assert.AreEqual("LLR", instructions);
        
        Assert.NotNull(map);
        Assert.AreEqual(3, map.Count);
        Assert.AreEqual(new HauntedWasteland.Next("BBB", "BBB"), map["AAA"]);
        Assert.AreEqual(new HauntedWasteland.Next("AAA", "ZZZ"), map["BBB"]);
        Assert.AreEqual(new HauntedWasteland.Next("ZZZ", "ZZZ"), map["ZZZ"]);
    }
    
    [Test]
    public void Example1() {
        var example = new HauntedWasteland(File.ReadAllLines(@"08\example1.txt"));
        
        Assert.AreEqual(6, example.CalculateStepsTo());
    }
    
    [Test]
    public void Puzzle1() {
        var example = new HauntedWasteland(File.ReadAllLines(@"08\input.txt"));
        
        Assert.AreEqual(19667, example.CalculateStepsTo());
    }
    
    [Test]
    public void Example2() {
        var example = new HauntedWasteland(File.ReadAllLines(@"08\example2.txt"));
        
        Assert.AreEqual(6, example.CalculateGhostStepsTo());
    }
    
    [Test]
    public void Puzzle2() {
        // var example = new HauntedWasteland(File.ReadAllLines(@"08\input.txt"));
        //
        // Assert.AreEqual(6, example.CalculateGhostStepsTo());
    }
}