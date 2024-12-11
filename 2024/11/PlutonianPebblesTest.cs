using System.IO;
using System.Linq;
using NUnit.Framework;

namespace AoC.day11;

public class PlutonianPebblesTest {

    private const string InputExampleA = "0 1 10 99 999";
    private const string InputExampleB = "125 17";
    
    [Test]
    public void Example1A() {
        var example = new PlutonianPebbles(InputExampleA);
        
        Assert.AreEqual("1 2024 1 0 9 9 2021976".ParseLongArray().Order().ToArray(),  example.CalculateBlinks().Order().ToArray());       
    }
    
    [Test]
    [TestCase(1, "253000 1 7")]
    [TestCase(2, "253 0 2024 14168")]
    [TestCase(3, "512072 1 20 24 28676032")]
    [TestCase(4, "512 72 2024 2 0 2 4 2867 6032")]
    [TestCase(5, "1036288 7 2 20 24 4048 1 4048 8096 28 67 60 32")]
    [TestCase(6, "2097446912 14168 4048 2 0 2 4 40 48 2024 40 48 80 96 2 8 6 7 6 0 3 2")]
    public void Example1B(int blinks, string expectedStones) {
        var example = new PlutonianPebbles(InputExampleB);
        
        Assert.AreEqual(expectedStones.ParseLongArray().Order().ToArray(),  example.CalculateBlinks(blinks).Order().ToArray());       
    }
    
    [Test]
    [TestCase(6, 22)]
    [TestCase(25, 55312)]
    public void Example1(int blinks, int expectedStonesCount) {
        var example = new PlutonianPebbles(InputExampleB);
        
        Assert.AreEqual(expectedStonesCount,  example.CalculateBlinkCount(blinks));      
    }

    [Test]
    public void Puzzle1() {
        var puzzle = new PlutonianPebbles(File.ReadAllLines(@"11\input.txt").Single());
        
        Assert.AreEqual(197157,  puzzle.CalculateBlinkCount(25));  
    }

    [Test]
    public void Puzzle2() {
        var puzzle = new PlutonianPebbles(File.ReadAllLines(@"11\input.txt").Single());
        
        Assert.AreEqual(234_430_066_982_597,  puzzle.CalculateBlinkCount(75));  
    }
}
