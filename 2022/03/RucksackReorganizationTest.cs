using System.IO;
using NUnit.Framework;

namespace AoC;

public class RucksackReorganizationTest {

    private static readonly string[] ExampleInput = {
        "vJrwpWtwJgWrhcsFMMfFFhFp",
        "jqHRNqRjqzjGDLGLrsFMfFZSrLrFZsSL",
        "PmmdzqPrVvPwwTWBwg",
        "wMqvLMZHhHMvwLHjbvcjnnSBnvTQFn",
        "ttgJtRGJQctTZtZT",
        "CrZsJsPPZsGzwwsLwLmpwMDw",
    };
    
    [Test]
    public void Example1() {
        Assert.AreEqual(157, RucksackReorganization.CalculateMisplacedItemPriority(ExampleInput));
    }
    
    [Test]
    public void Puzzle1() {
        var result = RucksackReorganization.CalculateMisplacedItemPriority(File.ReadAllLines(@"03\input.txt"));
        Assert.AreEqual(8153, result);
        Assert.Pass("Misplaced Item Priority: " + result);
    }

    [Test]
    public void Example2() {
        Assert.AreEqual(70, RucksackReorganization.FindBadgePriority(ExampleInput));
    }
    
    [Test]
    public void Puzzle2() {
        var result = RucksackReorganization.FindBadgePriority(File.ReadAllLines(@"03\input.txt"));
        Assert.AreEqual(2342, result);
        Assert.Pass("Badge Priority: " + result);
    }
}