using System.IO;
using System.Linq;
using NUnit.Framework;

namespace AoC.day2;

public class GiftShopTest {
    [Test]
    [TestCase(11, 22, new long[] {11, 22})]
    [TestCase(95, 115, new long[] {99})]
    [TestCase(998, 1012, new long[] {1010})]
    [TestCase(1188511880, 1188511890, new long[] {1188511885})]
    [TestCase(222220, 222224, new long[] {222222})]
    [TestCase(1698522, 1698528, new long[] { })]
    [TestCase(446443, 446449, new long[] {446446})]
    [TestCase(38593856, 38593862, new long[] {38593859})]
    public void Example1_IdRange_FindInvalidIds(long min, long max, long[] expected) {
        var range = new GiftShop.IdRange(min, max);
        var result = range.FindInvalidIds().ToArray();
        
        Assert.NotNull(result);
        Assert.AreEqual(expected, result);
    }
    
    [Test]
    public void Example1() {
        var example = new GiftShop(File.ReadAllLines(@"02\example.txt"));
        
        Assert.AreEqual(1227775554,  example.CalculatePart1());       
    }

    [Test]
    public void Puzzle1() {
        var puzzle = new GiftShop(File.ReadAllLines(@"02\input.txt"));
        
        Assert.AreEqual(13108371860,  puzzle.CalculatePart1());  
    }

    [Test]
    [TestCase(1, new long[] {1})]
    [TestCase(2, new long[] {2})]
    [TestCase(3, new long[] {3})]
    [TestCase(4, new long[] {2, 2})]
    [TestCase(6, new long[] {2, 3})]
    [TestCase(8, new long[] {2, 2, 2})]
    [TestCase(25, new long[] {5, 5})]
    public void Example2_GeneratePrimeFactorials(long number, long[] expected) {
        Assert.AreEqual(expected,  number.GeneratePrimeFactorials().ToArray());   
    }
    

    [Test]
    [TestCase("111", 1,new[] {"1", "1", "1"})]
    [TestCase("12", 2,new[] {"12"})]
    [TestCase("1234", 2,new[] {"12", "34"})]
    public void Example2_SplitIntoChunks(string value, int chunkSize, string[] expected) {
        Assert.AreEqual(expected,  value.SplitIntoChunks(chunkSize).ToArray());   
    }

    [Test]
    public void Example2() {
        var example = new GiftShop(File.ReadAllLines(@"02\example.txt"));
        
        Assert.AreEqual(4174379265,  example.CalculatePart2());   
    }
    
    [Test]
    public void Puzzle2_MaxLength() {
        var puzzle = new GiftShop(File.ReadAllLines(@"02\input.txt"));
        
        Assert.AreEqual(10,  puzzle.Input.Max(r => r.Max.ToString().Length));  
    }

    [Test]
    public void Puzzle2() {
        var puzzle = new GiftShop(File.ReadAllLines(@"02\input.txt"));
        
        Assert.AreEqual(22471660255,  puzzle.CalculatePart2());  
    }
}
