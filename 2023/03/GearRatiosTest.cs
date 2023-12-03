using System.IO;
using System.Linq;
using NUnit.Framework;

namespace AoC;

public class GearRatiosTest {
    [Test]
    [TestCase(0, 0, '4')]
    [TestCase(3, 7, '.')]
    [TestCase(3, 8, '$')]
    [TestCase(9, 0, '.')]
    [TestCase(9, 9, '.')]
    public void Example1_ParseEngineSchematic_Symbols(int x, int y, char expectedValue) {
        var engineSchmatic = GearRatios.ParseEngineSchematic(File.ReadAllLines(@"03\example.txt"));

        Assert.NotNull(engineSchmatic);
        Assert.NotNull(engineSchmatic.Symbols);
        Assert.AreEqual(expectedValue, engineSchmatic.Symbols[x][y]);
    }

    [Test]
    [TestCase(467, 0, 0)]
    [TestCase(35, 2, 2)]
    [TestCase(58, 7, 5)]
    [TestCase(664, 1, 9)]
    [TestCase(598, 5, 9)]
    public void Example1_ParseEngineSchematic_PartNumbers(int numberValue, int expectedX, int expectedY) {
        var engineSchmatic = GearRatios.ParseEngineSchematic(File.ReadAllLines(@"03\example.txt"));

        Assert.NotNull(engineSchmatic);
        Assert.NotNull(engineSchmatic.PartNumbers);

        var partNumber = engineSchmatic.PartNumbers.SingleOrDefault(n => n.Value == numberValue);
        Assert.NotNull(partNumber, $"Could not find part number with value {numberValue}, but found {string.Join(", ", engineSchmatic.PartNumbers.Select(n => n.Value))}");
        Assert.AreEqual(expectedX, partNumber!.X);
        Assert.AreEqual(expectedY, partNumber.Y);
    }
    
    [Test]
    [TestCase(467, true)]
    [TestCase(35, true)]
    [TestCase(58, false)]
    [TestCase(664, true)]
    [TestCase(598, true)]
    public void Example1_IsNextToSymbol(int numberValue, bool expectedIsNextToSymbol) {
        var engineSchmatic = GearRatios.ParseEngineSchematic(File.ReadAllLines(@"03\example.txt"));

        Assert.NotNull(engineSchmatic);

        var partNumber = engineSchmatic.PartNumbers.SingleOrDefault(n => n.Value == numberValue);
        Assert.NotNull(partNumber, $"Could not find part number with value {numberValue}, but found {string.Join(", ", engineSchmatic.PartNumbers.Select(n => n.Value))}");
        Assert.AreEqual(expectedIsNextToSymbol, partNumber!.IsNextToSymbol(engineSchmatic.Symbols));
    }

    [Test]
    public void Example1() {
        var engineSchematic = GearRatios.ParseEngineSchematic(File.ReadAllLines(@"03\example.txt"));
    
        Assert.AreEqual(4361, engineSchematic.FetchPartNumbersNextToSymbols().Sum(g => g.Value));
    }
    
    [Test]
    public void Puzzle1() {
        var engineSchematic = GearRatios.ParseEngineSchematic(File.ReadAllLines(@"03\input.txt"));
    
        Assert.AreEqual(527364, engineSchematic.FetchPartNumbersNextToSymbols().Sum(g => g.Value));
    }
    
    [Test]
    [TestCase(3, 1)]
    [TestCase(3, 4)]
    [TestCase(5, 8)]
    public void Example2_FetchAsterixCoordinates(int expectedX, int expectedY) {
        var engineSchmatic = GearRatios.ParseEngineSchematic(File.ReadAllLines(@"03\example.txt"));

        Assert.NotNull(engineSchmatic);
        Assert.AreEqual(3, engineSchmatic.FetchAsterixCoordinates().Count());

        var asterixCoordinate = engineSchmatic.FetchAsterixCoordinates().SingleOrDefault(c => c.X == expectedX && c.Y == expectedY);
        Assert.NotNull(asterixCoordinate, $"Could not find coordinate with value {expectedX} | {expectedY}, but found {string.Join(", ", engineSchmatic.FetchAsterixCoordinates().Select(c => c.X + "|" + c.Y))}");
    }
    
    [Test]
    [TestCase(3, 1)]
    [TestCase(5, 8)]
    public void Example2_FetchGearCoordinates(int expectedX, int expectedY) {
        var engineSchmatic = GearRatios.ParseEngineSchematic(File.ReadAllLines(@"03\example.txt"));

        Assert.NotNull(engineSchmatic);
        Assert.AreEqual(2, engineSchmatic.FetchGearCoordinates().Count());

        var asterixCoordinate = engineSchmatic.FetchGearCoordinates().SingleOrDefault(c => c.X == expectedX && c.Y == expectedY);
        Assert.NotNull(asterixCoordinate, $"Could not find coordinate with value {expectedX} | {expectedY}, but found {string.Join(", ", engineSchmatic.FetchAsterixCoordinates().Select(c => c.X + "|" + c.Y))}");
    }
    
    [Test]
    public void Example2() {
        var engineSchematic = GearRatios.ParseEngineSchematic(File.ReadAllLines(@"03\example.txt"));
    
        Assert.AreEqual(467835, engineSchematic.FetchGearRatios().Sum());
    }
    
    [Test]
    public void Puzzle2() {
    var engineSchematic = GearRatios.ParseEngineSchematic(File.ReadAllLines(@"03\input.txt"));
    
    Assert.AreEqual(79026871, engineSchematic.FetchGearRatios().Sum());
    }
}