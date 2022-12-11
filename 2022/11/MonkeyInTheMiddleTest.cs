using System;
using System.IO;
using System.Linq;
using NUnit.Framework;

namespace AoC._11;

public class MonkeyInTheMiddleTest {

    [Test]
    public void Example1A() {
        var monkeyInTheMiddle = new MonkeyInTheMiddle(File.ReadAllLines(@"11\example.txt"));
        monkeyInTheMiddle.AfterInspectOperation = item => item / 3;

        monkeyInTheMiddle.ExecuteRound();

        var result = monkeyInTheMiddle.MonkeyItems;
        Assert.AreEqual(new[] {20, 23, 27, 26}, result[0]);
        Assert.AreEqual(new[] {2080, 25, 167, 207, 401, 1046}, result[1]);
        Assert.AreEqual(Array.Empty<int>(), result[2]);
        Assert.AreEqual(Array.Empty<int>(), result[3]);
        
        monkeyInTheMiddle.ExecuteRound(); // round 2

        result = monkeyInTheMiddle.MonkeyItems;
        Assert.AreEqual(new[] {695, 10, 71, 135, 350}, result[0]);
        Assert.AreEqual(new[] {43, 49, 58, 55, 362}, result[1]);
        Assert.AreEqual(Array.Empty<int>(), result[2]);
        Assert.AreEqual(Array.Empty<int>(), result[3]);
        
        monkeyInTheMiddle.ExecuteRound(); // round 3

        result = monkeyInTheMiddle.MonkeyItems;
        Assert.AreEqual(new[] {16, 18, 21, 20, 122}, result[0]);
        Assert.AreEqual(new[] {1468, 22, 150, 286, 739}, result[1]);
        Assert.AreEqual(Array.Empty<int>(), result[2]);
        Assert.AreEqual(Array.Empty<int>(), result[3]);
        
        monkeyInTheMiddle.ExecuteRound(); // round 4

        result = monkeyInTheMiddle.MonkeyItems;
        Assert.AreEqual(new[] {491, 9, 52, 97, 248, 34}, result[0]);
        Assert.AreEqual(new[] {39, 45, 43, 258}, result[1]);
        Assert.AreEqual(Array.Empty<int>(), result[2]);
        Assert.AreEqual(Array.Empty<int>(), result[3]);
        
        monkeyInTheMiddle.ExecuteRound(); // round 5

        result = monkeyInTheMiddle.MonkeyItems;
        Assert.AreEqual(new[] {15, 17, 16, 88, 1037}, result[0]);
        Assert.AreEqual(new[] {20, 110, 205, 524, 72}, result[1]);
        Assert.AreEqual(Array.Empty<int>(), result[2]);
        Assert.AreEqual(Array.Empty<int>(), result[3]);
        
        monkeyInTheMiddle.ExecuteRound(); // round 6

        result = monkeyInTheMiddle.MonkeyItems;
        Assert.AreEqual(new[] {8, 70, 176, 26, 34}, result[0]);
        Assert.AreEqual(new[] {481, 32, 36, 186, 2190}, result[1]);
        Assert.AreEqual(Array.Empty<int>(), result[2]);
        Assert.AreEqual(Array.Empty<int>(), result[3]);
        
        monkeyInTheMiddle.ExecuteRound(); // round 7

        result = monkeyInTheMiddle.MonkeyItems;
        Assert.AreEqual(new[] {162, 12, 14, 64, 732, 17}, result[0]);
        Assert.AreEqual(new[] {148, 372, 55, 72}, result[1]);
        Assert.AreEqual(Array.Empty<int>(), result[2]);
        Assert.AreEqual(Array.Empty<int>(), result[3]);
        
        monkeyInTheMiddle.ExecuteRound(); // round 8

        result = monkeyInTheMiddle.MonkeyItems;
        Assert.AreEqual(new[] {51, 126, 20, 26, 136}, result[0]);
        Assert.AreEqual(new[] {343, 26, 30, 1546, 36}, result[1]);
        Assert.AreEqual(Array.Empty<int>(), result[2]);
        Assert.AreEqual(Array.Empty<int>(), result[3]);
        
        monkeyInTheMiddle.ExecuteRound(); // round 9

        result = monkeyInTheMiddle.MonkeyItems;
        Assert.AreEqual(new[] {116, 10, 12, 517, 14}, result[0]);
        Assert.AreEqual(new[] {108, 267, 43, 55, 288}, result[1]);
        Assert.AreEqual(Array.Empty<int>(), result[2]);
        Assert.AreEqual(Array.Empty<int>(), result[3]);
        
        monkeyInTheMiddle.ExecuteRound(); // round 10

        result = monkeyInTheMiddle.MonkeyItems;
        Assert.AreEqual(new[] {91, 16, 20, 98}, result[0]);
        Assert.AreEqual(new[] {481, 245, 22, 26, 1092, 30}, result[1]);
        Assert.AreEqual(Array.Empty<int>(), result[2]);
        Assert.AreEqual(Array.Empty<int>(), result[3]);
        
        monkeyInTheMiddle.ExecuteRound(); 
        monkeyInTheMiddle.ExecuteRound(); 
        monkeyInTheMiddle.ExecuteRound(); 
        monkeyInTheMiddle.ExecuteRound(); 
        monkeyInTheMiddle.ExecuteRound(); // round 15

        result = monkeyInTheMiddle.MonkeyItems;
        Assert.AreEqual(new[] {83, 44, 8, 184, 9, 20, 26, 102}, result[0]);
        Assert.AreEqual(new[] {110, 36}, result[1]);
        Assert.AreEqual(Array.Empty<int>(), result[2]);
        Assert.AreEqual(Array.Empty<int>(), result[3]);
        
        monkeyInTheMiddle.ExecuteRound(); 
        monkeyInTheMiddle.ExecuteRound(); 
        monkeyInTheMiddle.ExecuteRound(); 
        monkeyInTheMiddle.ExecuteRound(); 
        monkeyInTheMiddle.ExecuteRound(); // round 20

        result = monkeyInTheMiddle.MonkeyItems;
        Assert.AreEqual(new[] {10, 12, 14, 26, 34}, result[0]);
        Assert.AreEqual(new[] {245, 93, 53, 199, 115}, result[1]);
        Assert.AreEqual(Array.Empty<int>(), result[2]);
        Assert.AreEqual(Array.Empty<int>(), result[3]);
        
        var monkeyBusiness = monkeyInTheMiddle.CalculateMonkeyBusiness();
        Assert.AreEqual(10605, monkeyBusiness);
    }
    
    [Test]
    public void Example1B() {
        var monkeyInTheMiddle = new MonkeyInTheMiddle(File.ReadAllLines(@"11\example.txt"));
        monkeyInTheMiddle.AfterInspectOperation = item => item / 3;

        monkeyInTheMiddle.ExecuteRounds(20);

        var result = monkeyInTheMiddle.MonkeyItems;
        Assert.AreEqual(new[] {10, 12, 14, 26, 34}, result[0]);
        Assert.AreEqual(new[] {245, 93, 53, 199, 115}, result[1]);
        Assert.AreEqual(Array.Empty<int>(), result[2]);
        Assert.AreEqual(Array.Empty<int>(), result[3]);
        
        var monkeyBusiness = monkeyInTheMiddle.CalculateMonkeyBusiness();
        Assert.AreEqual(10605, monkeyBusiness);
    }
    
    [Test]
    public void Puzzle1() {
        var monkeyInTheMiddle = new MonkeyInTheMiddle(File.ReadAllLines(@"11\input.txt"));
        monkeyInTheMiddle.AfterInspectOperation = item => item / 3;

        monkeyInTheMiddle.ExecuteRounds(20);
        
        var monkeyBusiness = monkeyInTheMiddle.CalculateMonkeyBusiness();
        Assert.AreEqual(61005, monkeyBusiness);
        Assert.Pass("Puzzle 1: " + monkeyBusiness);
    }
    
    [Test]
    public void Example2A() {
        var monkeyInTheMiddle = new MonkeyInTheMiddle(File.ReadAllLines(@"11\example.txt"));
        
        // we can calculate the items "modulo product-of-all-divisibleBys" without changing the result
        var monkeysModulo = monkeyInTheMiddle.MonkeyDivisibleBy.Values.Aggregate(1, (a, b) => a * b);
        monkeyInTheMiddle.AfterInspectOperation = item => item % monkeysModulo;
        
        monkeyInTheMiddle.ExecuteRound();
        
        var result = monkeyInTheMiddle.MonkeyInspections;
        Assert.AreEqual(2, result[0]);
        Assert.AreEqual(4, result[1]);
        Assert.AreEqual(3, result[2]);
        Assert.AreEqual(6, result[3]);
        
        monkeyInTheMiddle.ExecuteRounds(19);
        
        result = monkeyInTheMiddle.MonkeyInspections;
        Assert.AreEqual(99, result[0]);
        Assert.AreEqual(97, result[1]);
        Assert.AreEqual(8, result[2]);
        Assert.AreEqual(103, result[3]);
        
        monkeyInTheMiddle.ExecuteRounds(980);
        
        result = monkeyInTheMiddle.MonkeyInspections;
        Assert.AreEqual(5204, result[0]);
        Assert.AreEqual(4792, result[1]);
        Assert.AreEqual(199, result[2]);
        Assert.AreEqual(5192, result[3]);
        
        monkeyInTheMiddle.ExecuteRounds(9000);
        
        result = monkeyInTheMiddle.MonkeyInspections;
        Assert.AreEqual(52166, result[0]);
        Assert.AreEqual(47830, result[1]);
        Assert.AreEqual(1938, result[2]);
        Assert.AreEqual(52013, result[3]);
        
        var monkeyBusiness = monkeyInTheMiddle.CalculateMonkeyBusiness();
        Assert.AreEqual(2713310158L, monkeyBusiness);
    }
    
    [Test]
    public void Example2B() {
        var monkeyInTheMiddle = new MonkeyInTheMiddle(File.ReadAllLines(@"11\example.txt"));
        
        // we can calculate the items "modulo product-of-all-divisibleBys" without changing the result
        var monkeysModulo = monkeyInTheMiddle.MonkeyDivisibleBy.Values.Aggregate(1, (a, b) => a * b);
        monkeyInTheMiddle.AfterInspectOperation = item => item % monkeysModulo;

        monkeyInTheMiddle.ExecuteRounds(10000);
        
        var monkeyBusiness = monkeyInTheMiddle.CalculateMonkeyBusiness();
        Assert.AreEqual(2713310158L, monkeyBusiness);
    }
    
    [Test]
    public void Puzzle2() {
        var monkeyInTheMiddle = new MonkeyInTheMiddle(File.ReadAllLines(@"11\input.txt"));

        // we can calculate the items "modulo product-of-all-divisibleBys" without changing the result
        var monkeysModulo = monkeyInTheMiddle.MonkeyDivisibleBy.Values.Aggregate(1, (a, b) => a * b);
        monkeyInTheMiddle.AfterInspectOperation = item => item % monkeysModulo;
        
        monkeyInTheMiddle.ExecuteRounds(10000);
        
        var monkeyBusiness = monkeyInTheMiddle.CalculateMonkeyBusiness();
        Assert.AreEqual(20567144694L, monkeyBusiness);
        Assert.Pass("Puzzle 2: " + monkeyBusiness);
    }
}