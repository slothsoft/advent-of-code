using System.IO;
using NUnit.Framework;

namespace AoC;

public class ClumsyCrucibleTest {

    [Test]
    public void Example1() {
        var clumsyCrucible = new ClumsyCrucible(File.ReadAllLines(@"17\example.txt"));

        Assert.AreEqual(102, clumsyCrucible.CalculateHeatLoss());
    }

    [Test]
    public void Puzzle1() {
        var clumsyCrucible = new ClumsyCrucible(File.ReadAllLines(@"17\input.txt"));

        Assert.AreEqual(1023, clumsyCrucible.CalculateHeatLoss());
    }

    [Test]
    public void Example2() {
        var clumsyCrucible = new ClumsyCrucible(File.ReadAllLines(@"17\example.txt")) {
            MinDistance = 4,
            MaxDistance = 10,
        };

        Assert.AreEqual(94, clumsyCrucible.CalculateHeatLoss());
    }

    [Test]
    public void Puzzle2() {
        var clumsyCrucible = new ClumsyCrucible(File.ReadAllLines(@"17\input.txt")) {
            MinDistance = 4,
            MaxDistance = 10,
        };

        Assert.AreEqual(94, clumsyCrucible.CalculateHeatLoss());
    }
}