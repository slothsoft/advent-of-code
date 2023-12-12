using System.IO;
using System.Linq;
using NUnit.Framework;

namespace AoC;

public class HotSpringsTest {
    [Test]
    [TestCase("#.#.###", new[] {1, 1, 3})]
    [TestCase(".#...#....###.", new[] {1, 1, 3})]
    [TestCase(".#.###.#.######", new[] {1, 3, 1, 6})]
    [TestCase("####.#...#...", new[] {4, 1, 1})]
    [TestCase("#....######..#####.", new[] {1, 6, 5})]
    [TestCase(".###.##....#", new[] {3, 2, 1})]
    public void Example1_CalculateGroups(string input, int[] expectedGroups) {
        Assert.AreEqual(expectedGroups, HotSprings.CalculateGroups(input));
    }

    [Test]
    [TestCase("#.#.###", new[] {1, 1, 3})]
    [TestCase(".#...#....###.", new[] {1, 1, 3})]
    [TestCase(".#.###.#.######", new[] {1, 3, 1, 6})]
    [TestCase("####.#...#...", new[] {4, 1, 1})]
    [TestCase("#....######..#####.", new[] {1, 6, 5})]
    [TestCase(".###.##....#", new[] {3, 2, 1})]
    public void Example1_CalculatePossibleArrangementsNoUnknown(string input, int[] groups) {
        Assert.AreEqual(input, HotSprings.CalculatePossibleArrangements(input, groups).Single());
    }

    [Test]
    [TestCase("?###????????", new[] {3, 2, 1},
        ".###.##.#...",
        ".###.##..#..",
        ".###.##...#.",
        ".###.##....#",
        ".###..##.#..",
        ".###..##..#.",
        ".###..##...#",
        ".###...##.#.",
        ".###...##..#",
        ".###....##.#")]
    public void Example1_CalculatePossibleArrangementsManyUnknown(string input, int[] groups, params string[] expectedArrangements) {
        Assert.AreEqual(expectedArrangements.OrderBy(s => s), HotSprings.CalculatePossibleArrangements(input, groups).OrderBy(s => s));
    }

    [Test]
    [TestCase("???.###", new[] {1, 1, 3}, 1)]
    [TestCase(".??..??...?##.", new[] {1, 1, 3}, 4)]
    [TestCase("?#?#?#?#?#?#?#?", new[] {1, 3, 1, 6}, 1)]
    [TestCase("????.#...#...", new[] {4, 1, 1}, 1)]
    [TestCase("????.######..#####.", new[] {1, 6, 5}, 4)]
    [TestCase("?###????????", new[] {3, 2, 1}, 10)]
    public void Example1_CalculatePossibleArrangementsCountManyUnknown(string input, int[] groups, int expectedCount) {
        Assert.AreEqual(expectedCount, HotSprings.CalculatePossibleArrangements(input, groups).Count());
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

        Assert.AreEqual(525152, example.CalculatePossibleArrangementsCount());
    }
}