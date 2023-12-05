using System.IO;
using NUnit.Framework;

namespace AoC;

public class SeedFertilizerTest {

    [Test]
    public void Example1_ParseAlmanac() {
        var (almanac, seeds) = SeedFertilizer.ParseAlmanac(File.ReadAllLines(@"05\example.txt"));
        Assert.AreEqual(new [] {79, 14, 55, 13}, seeds);
        foreach (var almanacMap in almanac.AlmanacMaps)
        {
            Assert.NotNull(almanacMap);
        }
    }
    
    [Test]
    [TestCase(79,81,81,81,74,78,78,82)]
    [TestCase(14,14,53,49,42,42,43,43)]
    [TestCase(55,57,57,53,46,82,82,86)]
    [TestCase(13,13,52,41,34,34,35,35)]
    public void Example1_AlmanacMap(long seed, long soil, long fertilizer, long water, long light, long temperature, long humidity, long location) {
        var (almanac, _) = SeedFertilizer.ParseAlmanac(File.ReadAllLines(@"05\example.txt"));
        
        Assert.AreEqual(soil, almanac.SeedToSoilMap[seed]);
        Assert.AreEqual(fertilizer, almanac.SoilToFertilizerMap[soil]);
        Assert.AreEqual(water, almanac.FertilizerToWaterMap[fertilizer]);
        Assert.AreEqual(light, almanac.WaterToLightMap[water]);
        Assert.AreEqual(temperature, almanac.LightToTemperatureMap[light]);
        Assert.AreEqual(humidity, almanac.TemperatureToHumidityMap[temperature]);
        Assert.AreEqual(location, almanac.HumidityToLocationMap[humidity]);
    }
    
    [Test]
    [TestCase(79,82)]
    [TestCase(14,43)]
    [TestCase(55,86)]
    [TestCase(13,35)]
    public void Example1_GetSeedLocation(long seed, long location) {
        var (almanac, _) = SeedFertilizer.ParseAlmanac(File.ReadAllLines(@"05\example.txt"));
        
        Assert.AreEqual(location, almanac.GetSeedLocation(seed));
    }
    
    [Test]
    public void Example1() {
        var example = new SeedFertilizer(File.ReadAllLines(@"05\example.txt"));
        
        Assert.AreEqual(35, example.GetLowestSeedLocation());
    }
    
    [Test]
    public void Puzzle1() {
        var puzzle = new SeedFertilizer(File.ReadAllLines(@"05\input.txt"));
        
        Assert.AreEqual(251346198, puzzle.GetLowestSeedLocation());
    }
    
    [Test]
    public void Example2() {
        var example = new SeedFertilizer(File.ReadAllLines(@"05\example.txt"));
    
        Assert.AreEqual(46, example.GetLowestSeedLocationForRange());
    }
    
    [Test]
    public void Puzzle2() {
        var puzzle = new SeedFertilizer(File.ReadAllLines(@"05\input.txt"));
        
        Assert.AreEqual(251346198, puzzle.GetLowestSeedLocationForRange());
    }
}