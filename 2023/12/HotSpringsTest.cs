using System.Collections.Generic;
using System.IO;
using System.Linq;
using NUnit.Framework;

namespace AoC;

public class HotSpringsTest {
    [Test]
    [TestCase("???.###", new[] {1, 1, 3}, 1)]
    [TestCase(".??..??...?##.", new[] {1, 1, 3}, 4)]
    [TestCase("?#?#?#?#?#?#?#?", new[] {1, 3, 1, 6}, 1)]
    [TestCase("????.#...#...", new[] {4, 1, 1}, 1)]
    [TestCase("????.######..#####.", new[] {1, 6, 5}, 4)]
    [TestCase("?###????????", new[] {3, 2, 1}, 10)]
    public void Example1_CalculatePossibleArrangementsCountManyUnknown(string input, int[] groups, int expectedCount) {
        Assert.AreEqual(expectedCount, HotSprings.CalculatePossibleArrangementsCount(input, new List<int>(groups), new Dictionary<string, long>()));
    }

    [Test]
    public void Example1() {
        var example = new HotSprings(File.ReadAllLines(@"12\example.txt"));

        Assert.AreEqual(21, example.CalculatePossibleArrangementsCount());
    }

    [Test]
    public void Puzzle1() {
        var example = new HotSprings(File.ReadAllLines(@"12\input.txt"));

        Assert.AreEqual(7922, example.CalculatePossibleArrangementsCount());
    }

    [Test]
    [TestCase("???.### 1,1,3", 1)]
    [TestCase(".??..??...?##. 1,1,3", 16384)]
    [TestCase("?#?#?#?#?#?#?#? 1,3,1,6", 1)]
    [TestCase("????.#...#... 4,1,1", 16)]
    [TestCase("????.######..#####. 1,6,5", 2500)]
    [TestCase("?###???????? 3,2,1", 506250)]
    public void Example2_Unfold(string input, int possibleArrangementCount) {
        var example = new HotSprings(new []{ input });
        example.Unfold(5);

        Assert.AreEqual(possibleArrangementCount, example.CalculatePossibleArrangementsCount());
    }

    // Note: I couldn't find the bug in my algorithm, so I had to try to find test cases that didn't work
    [Test]
    [TestCase(".?????...? 1,1,1", 7)]
    [TestCase("#????...? 1,1,1", 4)]
    [TestCase(".????...? 1,1,1", 3)]
    [TestCase("#???...? 1,1,1", 2)]
    [TestCase("#??...? 1,1,1", 1)]
    [TestCase(".???...? 1,1,1", 1)]
    [TestCase("#?...? 1,1,1", 0)]
    [TestCase(".??...? 1,1,1", 0)]
    [TestCase("#????????.#?#?????? 2,1,1,5,1", 36)]
    [TestCase("???##?###????? 1,2,3,4", 2)]
    [TestCase("?#?????##????#?? 1,9", 3)]
    [TestCase("?.?.??#?...????? 1,2,1", 28)]
    [TestCase(".#.#???..??#???#?? 1,1,1,1,1,4", 4)]
    [TestCase("?#??#??#..#?#???. 1,4,1,1,2", 1)]
    [TestCase("??????##????# 1,7,2", 3)]
    public void Example2_CalculatePossibleArrangementsCount(string input, int expectedCount) {
        var example = new HotSprings(new[] { input });

        Assert.AreEqual(expectedCount, example.CalculatePossibleArrangementsCount());
    }
    
    [Test]
    public void Example2() {
        var example = new HotSprings(File.ReadAllLines(@"12\example.txt"));
        example.Unfold(5);

        Assert.AreEqual(525152, example.CalculatePossibleArrangementsCount());
    }

    [Test]
    public void Puzzle2() {
        var example = new HotSprings(File.ReadAllLines(@"12\input.txt"));
        example.Unfold(5);

        // 1193780037 for row 2
        Assert.AreEqual(18093821750095L, example.CalculatePossibleArrangementsCount());
    }
}