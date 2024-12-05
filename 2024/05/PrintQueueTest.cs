using System.IO;
using NUnit.Framework;

namespace AoC.day5;

public class PrintQueueTest {
    [Test]
    public void Example1() {
        var example = new PrintQueue(File.ReadAllLines(@"05\example.txt"));
        
        Assert.AreEqual(143,  example.CalculateSumOfMiddlePageNumbers());       
    }

    [Test]
    public void Puzzle1() {
        var puzzle = new PrintQueue(File.ReadAllLines(@"05\input.txt"));
        
        Assert.AreEqual(6498,  puzzle.CalculateSumOfMiddlePageNumbers());  
    }

    [Test]
    public void Example2() {
        var example = new PrintQueue(File.ReadAllLines(@"05\example.txt"));
        
        Assert.AreEqual(123,  example.CalculateCorrectedSumOfMiddlePageNumbers());   
    }

    [Test]
    public void Puzzle2() {
        var puzzle = new PrintQueue(File.ReadAllLines(@"05\input.txt"));
        
        Assert.AreEqual(5017,  puzzle.CalculateCorrectedSumOfMiddlePageNumbers());  
    }
}
