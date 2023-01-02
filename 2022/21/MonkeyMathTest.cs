using System.IO;
using NUnit.Framework;

namespace AoC._21;

public class MonkeyMathTest {

    [Test]
    public void Example1() {
        var monkeyMath = new MonkeyMath(File.ReadAllLines(@"21\example.txt"));

        var monkey = monkeyMath.Monkeys["hmdt"];
        Assert.AreEqual(32, monkey.Value);

        monkey = monkeyMath.Monkeys["zczc"];
        Assert.AreEqual(2, monkey.Value);
        
        monkey = monkeyMath.Monkeys["drzm"];
        Assert.AreEqual(30, monkey.Value);
        
        monkey = monkeyMath.Monkeys["sjmn"];
        Assert.AreEqual(150, monkey.Value);
        
        monkey = monkeyMath.Monkeys[MonkeyMath.MonkeyRoot];
        Assert.AreEqual(152, monkey.Value);
    }

    [Test]
    public void Puzzle1() {
        var monkeyMath = new MonkeyMath(File.ReadAllLines(@"21\input.txt"));

        var result = monkeyMath.Monkeys[MonkeyMath.MonkeyRoot].Value;
        Assert.AreEqual(159591692827554, result);
        Assert.Pass("Puzzle 1: " + result);
    }

    [Test]
    public void Example2() {
        var monkeyMath = new MonkeyMath(File.ReadAllLines(@"21\example.txt"), true);
        Assert.AreEqual("pppw", monkeyMath.MonkeyLeftOfRoot);
        Assert.AreEqual("sjmn", monkeyMath.MonkeyRightOfRoot);

        Assert.AreEqual(301, monkeyMath.FindHumanNumber(-5_000, 5_000));
    }
    
    [Test]
    public void Example2Wat() {
        // so 301 and 302 is correct too for some reason
        var monkeyMath = new MonkeyMath(File.ReadAllLines(@"21\example.txt"), true);
        
        monkeyMath.Monkeys[MonkeyMath.MonkeyHuman].CalculateValue = () => 301;
        Assert.AreEqual(1, monkeyMath.Monkeys[MonkeyMath.MonkeyRoot].Value);
        
        monkeyMath.Monkeys[MonkeyMath.MonkeyHuman].CalculateValue = () => 302;
        Assert.AreEqual(1, monkeyMath.Monkeys[MonkeyMath.MonkeyRoot].Value);
    }

    [Test]
    public void Puzzle2() {
        var monkeyMath = new MonkeyMath(File.ReadAllLines(@"21\input.txt"), true);

        var result = monkeyMath.FindHumanNumber(-200_000_000_000_000, 200_000_000_000_000);
        Assert.AreEqual(3509819803065, result); 
        Assert.Pass("Puzzle 2: " + result);
    }
    
    [Test]
    public void Puzzle2Wat() {
        var monkeyMath = new MonkeyMath(File.ReadAllLines(@"21\input.txt"), true);

        monkeyMath.Monkeys[MonkeyMath.MonkeyHuman].CalculateValue = () => 3_509_819_803_068;
        Assert.AreEqual(1, monkeyMath.Monkeys[MonkeyMath.MonkeyRoot].Value);
        
        monkeyMath.Monkeys[MonkeyMath.MonkeyHuman].CalculateValue = () => 3_509_819_803_067;
        Assert.AreEqual(1, monkeyMath.Monkeys[MonkeyMath.MonkeyRoot].Value);
        
        monkeyMath.Monkeys[MonkeyMath.MonkeyHuman].CalculateValue = () => 3_509_819_803_066;
        Assert.AreEqual(1, monkeyMath.Monkeys[MonkeyMath.MonkeyRoot].Value);
        
        monkeyMath.Monkeys[MonkeyMath.MonkeyHuman].CalculateValue = () => 3_509_819_803_065;
        Assert.AreEqual(1, monkeyMath.Monkeys[MonkeyMath.MonkeyRoot].Value);
    }
}