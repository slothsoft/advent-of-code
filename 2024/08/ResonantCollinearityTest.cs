using System.IO;
using NUnit.Framework;
using static AoC.day8.ResonantCollinearity;

namespace AoC.day8;

public class ResonantCollinearityTest {
    [Test]
    public void Example1_ParseInput() {
        var result = ParseInput(File.ReadAllLines(@"08\example.txt"));
        
        Assert.NotNull(result);
        Assert.AreEqual(7,  result.Length);

        var index = 0;
        Assert.AreEqual(new Node('0', new Point(8, 1)),  result[index++]);
        Assert.AreEqual(new Node('0', new Point(5, 2)),  result[index++]);
        Assert.AreEqual(new Node('0', new Point(7, 3)),  result[index++]);
        Assert.AreEqual(new Node('0', new Point(4, 4)),  result[index++]);
        Assert.AreEqual(new Node('A', new Point(6, 5)),  result[index++]);
    }
    
    [Test]
    public void Example1() {
        var example = new ResonantCollinearity(File.ReadAllLines(@"08\example.txt"));
        
        Assert.AreEqual(14,  example.CalculateAntinodesCount());       
    }

    [Test]
    public void Puzzle1() {
        var puzzle = new ResonantCollinearity(File.ReadAllLines(@"08\input.txt"));
        
        Assert.AreEqual(261,  puzzle.CalculateAntinodesCount());  
    }

    [Test]
    public void Example2A() {
        var example = new ResonantCollinearity(File.ReadAllLines(@"08\example.txt"));
        
        Assert.AreEqual(34,  example.CalculateResonantHarmonicsCount());   
    }
    
    [Test]
    public void Example2B() {
        var example = new ResonantCollinearity(File.ReadAllLines(@"08\example2.txt"));
        
        Assert.AreEqual(9,  example.CalculateResonantHarmonicsCount());   
    }

    [Test]
    public void Puzzle2() {
        var puzzle = new ResonantCollinearity(File.ReadAllLines(@"08\input.txt"));
        
        Assert.AreEqual(898,  puzzle.CalculateResonantHarmonicsCount());  
    }
}
