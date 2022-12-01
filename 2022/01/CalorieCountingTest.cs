using System.IO;
using NUnit.Framework;

namespace AoC;

public class CalorieCountingTest {

    private static readonly string[] ExampleInput = new[] {
        "1000",
        "2000",
        "3000", // 6000
        "",
        "4000", // 4000
        "",
        "5000",
        "6000", // 11000
        "",
        "7000",
        "8000",
        "9000", // 24000
        "",
        "10000" // 10000
    };
    
    [Test]
    public void Example1() {
        Assert.AreEqual(24000, CalorieCounting.Count(ExampleInput));
    }
    
    [Test]
    public void Puzzle1() {
        Assert.Pass("Total Calories: " + CalorieCounting.Count(File.ReadAllLines(@"01\input.txt")));
    }

    [Test]
    public void Example2() {
        Assert.AreEqual(45000, CalorieCounting.Count(ExampleInput, 3));
    }

    [Test]
    public void Puzzle2() {
        Assert.Pass("Top 3: " + CalorieCounting.Count(File.ReadAllLines(@"01\input.txt"), 3));
    }
}