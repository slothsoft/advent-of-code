using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.VisualStudio.TestPlatform.ObjectModel;
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
    [TestCaseSource(nameof(GetSeedSoilFertilizerWaterLightTemperatureHumidityLocationData))]
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

    private static IEnumerable<TestCaseData> GetSeedSoilFertilizerWaterLightTemperatureHumidityLocationData() {
        yield return new TestCaseData(79, 81, 81, 81, 74, 78, 78, 82);
        yield return new TestCaseData(14,14,53,49,42,42,43,43);
        yield return new TestCaseData(55,57,57,53,46,82,82,86);
        yield return new TestCaseData(13,13,52,41,34,34,35,35);
        yield return new TestCaseData(82,84,84,84,77,45,46,46);
    }
    
    [Test]
    [TestCaseSource(nameof(GetSeedLocationData))]
    public void Example1_GetSeedLocation(long seed, long location) {
        var (almanac, _) = SeedFertilizer.ParseAlmanac(File.ReadAllLines(@"05\example.txt"));
        
        Assert.AreEqual(location, almanac.GetSeedLocation(seed));
    }
    
    private static IEnumerable<TestCaseData> GetSeedLocationData() {
        return GetSeedSoilFertilizerWaterLightTemperatureHumidityLocationData().Select(d => new TestCaseData(d.Arguments.First(), d.Arguments.Last()));
    }
    
    [Test]
    public void Example1() {
        var example = new SeedFertilizer(File.ReadAllLines(@"05\example.txt"));
        
        Assert.AreEqual(35, example.GetLowestSeedLocation());
    }
    
    [Test]
    public void Puzzle1() {
        var puzzle = new SeedFertilizer(File.ReadAllLines(@"05\input.txt"));
        
        Assert.AreEqual(251_346_198, puzzle.GetLowestSeedLocation());
    }
    
    [Test]
    [TestCaseSource(nameof(GetSeedLocationData))]
    public void Example2_GetMinSeedLocation(long seed, long location) {
        var (almanac, _) = SeedFertilizer.ParseAlmanac(File.ReadAllLines(@"05\example.txt"));
        
        Assert.AreEqual(location, almanac.GetMinSeedLocation(seed, seed + 1));
    }

    [Test]
    public void Example2() {
        var example = new SeedFertilizer(File.ReadAllLines(@"05\example.txt"));
    
        Assert.AreEqual(46, example.GetLowestSeedLocationForRange());
    }
    
    // [Test]
    // public void Puzzle2() {
    //     var puzzle = new SeedFertilizer(File.ReadAllLines(@"05\input.txt"));
    //     
    //     Assert.AreEqual(251346198, puzzle.GetLowestSeedLocationForRange());
    // }
}