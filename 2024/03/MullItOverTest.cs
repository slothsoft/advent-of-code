using System.IO;
using System.Linq;
using NUnit.Framework;

namespace AoC.day3;

public class MullItOverTest {
    [Test]
    public void Example1_FindMuls() {
        var mullItOver = new MullItOver(File.ReadAllLines(@"03\example.txt"));

        var result = mullItOver.FindMuls().ToArray();
        Assert.NotNull(result);
        Assert.AreEqual(4,  result.Length);
        Assert.AreEqual("mul(2,4)",  result[0]);
        Assert.AreEqual("mul(5,5)",  result[1]);
        Assert.AreEqual("mul(11,8)",  result[2]);
        Assert.AreEqual("mul(8,5)",  result[3]);
    }
    
    [Test]
    public void Example1() {
        var example = new MullItOver(File.ReadAllLines(@"03\example.txt"));
        
        Assert.AreEqual(161,  example.CalculateSumOfMuls());       
    }

    [Test]
    public void Puzzle1() {
        var puzzle = new MullItOver(File.ReadAllLines(@"03\input.txt"));
        
        Assert.AreEqual(182780583,  puzzle.CalculateSumOfMuls());  
    }

    [Test]
    public void Example2() {
        var example = new MullItOver(["xmul(2,4)&mul[3,7]!^don't()_mul(5,5)+mul(32,64](mul(11,8)undo()?mul(8,5))"]);
        
        Assert.AreEqual(48,  example.CalculateSumOfMulsWithDoAndDont());   
    }

    [Test]
    public void Puzzle2() {
        var puzzle = new MullItOver(File.ReadAllLines(@"03\input.txt"));
        
        Assert.AreEqual(90772405,  puzzle.CalculateSumOfMulsWithDoAndDont());  
    }
}
