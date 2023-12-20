using System.IO;
using NUnit.Framework;

namespace AoC.day{day};

public class {ClassName}Test {
    [Test]
    public void Example1_ParseInput() {
        var result = {ClassName}.ParseInput(new[] { "1", "2", "3" });
        
        Assert.NotNull(result);
        Assert.AreEqual(3,  result.Length);
        Assert.AreEqual("1",  result[0].Property);
        Assert.AreEqual("2",  result[1].Property);
        Assert.AreEqual("3",  result[2].Property);
    }
    
    [Test]
    public void Example1() {
        var example = new {ClassName}(File.ReadAllLines(@"{dayWithZero}\example.txt"));
        
        Assert.AreEqual(7,  example.Calculate());       
    }

    [Test]
    public void Puzzle1() {
        var puzzle = new {ClassName}(File.ReadAllLines(@"{dayWithZero}\input.txt"));
        
        Assert.AreEqual(7,  puzzle.Calculate());  
    }

    [Test]
    public void Example2() {
        var example = new {ClassName}(File.ReadAllLines(@"{dayWithZero}\example.txt"));
        
        Assert.AreEqual(7,  example.Calculate());   
    }

    [Test]
    public void Puzzle2() {
        var puzzle = new {ClassName}(File.ReadAllLines(@"{dayWithZero}\input.txt"));
        
        Assert.AreEqual(7,  puzzle.Calculate());  
    }
}