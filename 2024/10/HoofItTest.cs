using System.IO;
using NUnit.Framework;

namespace AoC.day10;

public class HoofItTest {
    private const string InputExample1 = @"10\example1.txt";
    private const string InputExample2A = @"10\example2A.txt";
    private const string InputExample2B = @"10\example2B.txt";
    private const string InputExample2C = @"10\example2C.txt";
    private const string InputExample3 = @"10\example3.txt";
    
    [Test]
    [TestCase(InputExample1, 1L)]
    [TestCase(InputExample2A, 2L)]
    [TestCase(InputExample2B, 4L)]
    [TestCase(InputExample2C, 3L)]
    [TestCase(InputExample3, 36L)]
    public void Example1(string input, long expectedTrailheadScore) {
        var example = new HoofIt(File.ReadAllLines(input));
        
        Assert.AreEqual(expectedTrailheadScore,  example.CalculateTrailheadScore());       
    }

    [Test]
    public void Puzzle1() {
        var puzzle = new HoofIt(File.ReadAllLines(@"10\input.txt"));
        
        Assert.AreEqual(514,  puzzle.CalculateTrailheadScore());  
    }

    [Test]
    public void Example2() {
        var example = new HoofIt(File.ReadAllLines(@"10\example3.txt"));
        
        Assert.AreEqual(81,  example.CalculateTrailheadScore());   
    }

    [Test]
    public void Puzzle2() {
        var puzzle = new HoofIt(File.ReadAllLines(@"10\input.txt"));
        
        Assert.AreEqual(1162,  puzzle.CalculateTrailheadScore());  
    }
}
