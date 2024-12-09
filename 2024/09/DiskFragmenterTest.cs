using System.IO;
using System.Linq;
using NUnit.Framework;

namespace AoC.day9;

public class DiskFragmenterTest {

    private const string InputExample1A = "2333133121414131402";
    private const string InputExample1B = "12345";
    
    [Test]
    [TestCase(InputExample1B, "0..111....22222")]
    [TestCase(InputExample1A, "00...111...2...333.44.5555.6666.777.888899")]
    public void Example1_ParseInput(string input, string expectedDiskMap) {
        var result = input.ParseDiskMap();
        
        Assert.NotNull(result);
        Assert.AreEqual(expectedDiskMap,  result.Stringify());
    }
    
    [Test]
    [TestCase(InputExample1B, "022111222......")]
    [TestCase(InputExample1A, "0099811188827773336446555566..............")]
    public void Example1_Fragment(string input, string expectedDiskMap) {
        var result = input.ParseDiskMap();
        result.Fragment();
        
        Assert.NotNull(result);
        Assert.AreEqual(expectedDiskMap,  result.Stringify());
    }
    
    [Test]
    public void Example1() {
        var example = new DiskFragmenter(InputExample1A);
        example.Input.Fragment();
        
        Assert.AreEqual(1928,  example.CalculateFileSystemChecksum(true));       
    }

    [Test]
    public void Puzzle1() {
        var puzzle = new DiskFragmenter(File.ReadAllLines(@"09\input.txt").Single());
        puzzle.Input.Fragment();
        
        Assert.AreEqual(6_385_338_159_127,  puzzle.CalculateFileSystemChecksum(true));  
    }
    
    [Test]
    [TestCase(InputExample1A, "00992111777.44.333....5555.6666.....8888..")]
    public void Example1_Compact(string input, string expectedDiskMap) {
        var result = input.ParseDiskMap();
        result.Compact();
        
        Assert.NotNull(result);
        Assert.AreEqual(expectedDiskMap,  result.Stringify());
    }

    [Test]
    public void Example2() {
        var example = new DiskFragmenter(InputExample1A);
        example.Input.Compact();
        
        Assert.AreEqual(2858,  example.CalculateFileSystemChecksum(false));   
    }

    [Test]
    public void Puzzle2() {
        var puzzle = new DiskFragmenter(File.ReadAllLines(@"09\input.txt").Single());
        puzzle.Input.Compact();
        
        Assert.AreEqual(6_415_163_624_282,  puzzle.CalculateFileSystemChecksum(false));  
    }
}
