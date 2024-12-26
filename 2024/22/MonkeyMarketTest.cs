using System;
using System.IO;
using System.Linq;
using NUnit.Framework;

namespace AoC.day22;

public class MonkeyMarketTest {

    [Test]
    public void Example1_Mix() {
        Assert.AreEqual(37,  MonkeyMarket.SecretNumber.Mix(42, 15));
    }
    
    [Test]
    public void Example1_Prune() {
        Assert.AreEqual(16113920,  MonkeyMarket.SecretNumber.Prune(100000000));
    }

    [Test]
    public void Example1_NextSecretNumber() {
        var secretNumber = new MonkeyMarket.SecretNumber(123);
        
        var nextValues = new long[] {
            15887950,
            16495136,
            527345,
            704524,
            1553684,
            12683156,
            11100544,
            12249484,
            7753432,
            5908254,
        };

        for (var i = 0; i < nextValues.Length; i++) {
            secretNumber = secretNumber.NextSecretNumber();
            Assert.AreEqual(nextValues[i],  secretNumber.Value, $"Step {i+1} is wrong!");
        }
    }
    
    [Test]
    [TestCase(1, 8685429L)]
    [TestCase(10, 4700978L)]
    [TestCase(100, 15273692L)]
    [TestCase(2024, 8667524L)]
    public void Example1_GenerateSecretNumbers(long secretNumber, long expected) {
        Assert.AreEqual(expected,  MonkeyMarket.GenerateSecretNumbers(secretNumber, 2000).Last().SecretNumber);       
    }
    
    [Test]
    public void Example1() {
        var example = new MonkeyMarket(File.ReadAllLines(@"22\example1.txt"));
        
        Assert.AreEqual(37327623,  example.GenerateSecretNumbersSum(2000));       
    }

    [Test]
    public void Puzzle1() {
        var puzzle = new MonkeyMarket(File.ReadAllLines(@"22\input.txt"));
        
        Assert.AreEqual(16_999_668_565,  puzzle.GenerateSecretNumbersSum(2000));  
    }
    
    [Test]
    [TestCase(123, new[] {-1,-1,0,2},6)]
    [TestCase(1, new[] {-2,1,-1,3}, 7)]
    [TestCase(2, new[] {-2,1,-1,3},7)]
    [TestCase(3, new[] {-2,1,-1,3},0)]
    [TestCase(2024, new[] {-2,1,-1,3},9)]
    public void Example2_CalculateBananas(int startNumber, int[] monkeyCommand, long expectedPrice) {
        Assert.AreEqual(expectedPrice,  MonkeyMarket.CalculateBananas(startNumber, monkeyCommand));   
        Assert.AreEqual(expectedPrice,  MonkeyMarket.CalculateNormalizedBananas(startNumber, monkeyCommand));   
    }

    
    [Test]
    public void Example2() {
        var example = new MonkeyMarket(File.ReadAllLines(@"22\example2.txt"));
        
        Assert.AreEqual(23,  example.CalculateMaxNormalizedBananas(2000));   
    }
    
    [Test]
    public void Example2_Normalized() {
        var example = new MonkeyMarket(File.ReadAllLines(@"22\example2.txt"));
        
        Assert.AreEqual(23,  example.CalculateMaxNormalizedBananas(2000));   
    }


    [Test]
    public void Puzzle2() {
        var puzzle = new MonkeyMarket(File.ReadAllLines(@"22\input.txt"));
        
        // this took only an hour :D
        Assert.AreEqual(1898,  puzzle.CalculateMaxBananas(2000));  
    }
}
