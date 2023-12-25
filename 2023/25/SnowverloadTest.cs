using System.IO;
using NUnit.Framework;

namespace AoC.day25;

public class SnowverloadTest {
    [Test]
    [TestCase("jqt", "nvd", "jqt -> nvd")]
    [TestCase("cmg", "ntq", "cmg -> bvb -> ntq")]
    public void Example1_FindPath(string from, string to, string expected) {
        var example = new Snowverload(File.ReadAllLines(@"25\example.txt"));

        var path = example.FindPath(from, to);
        Assert.NotNull(path);
        Assert.AreEqual(expected, string.Join(" -> ", path!));
    }

    [Test]
    public void Example1_CalculateBottlenecks() {
        var example = new Snowverload(File.ReadAllLines(@"25\example.txt"));

        var bottlenecks = example.CalculateBottlenecks();
        Assert.NotNull(bottlenecks);
        Assert.AreEqual(new[] {"bvb/cmg", "hfx/pzl", "jqt/nvd"}, bottlenecks, string.Join(", ", bottlenecks));
    }

    [Test]
    public void Example1() {
        var example = new Snowverload(File.ReadAllLines(@"25\example.txt"));

        Assert.AreEqual(54, example.Calculate());
    }

    [Test]
    public void Puzzle1() {
        var puzzle = new Snowverload(File.ReadAllLines(@"25\input.txt"));

        Assert.AreEqual(606062, puzzle.Calculate());
    }
}