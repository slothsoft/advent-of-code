using System.Collections.Generic;
using System.IO;
using NUnit.Framework;

namespace AoC._19;

public class NotEnoughMineralsTest {

    [Test]
    public void Example1Simulation1() {
        var simulation = new Simulation(new Blueprint {
            Id = 1,
            RobotCosts = new Inventory[] {
                // Each ore robot costs 4 ore.
                new Dictionary<Resource, int> {
                    { Resource.Ore, 4 },
                },
                // Each clay robot costs 2 ore.
                new Dictionary<Resource, int> {
                    { Resource.Ore, 2 },
                },
                // Each obsidian robot costs 3 ore and 14 clay.
                new Dictionary<Resource, int> {
                    { Resource.Ore, 3 },
                    { Resource.Clay, 14 },
                },
                // Each geode robot costs 2 ore and 7 obsidian.
                new Dictionary<Resource, int> {
                    { Resource.Ore, 2 },
                    { Resource.Obsidian, 7 },
                },
            }
        });

        Assert.AreEqual(9, simulation.Start());
    }

    [Test]
    public void Example1Simulation2() {
        var simulation = new Simulation(new Blueprint {
            Id = 2,
            RobotCosts = new Inventory[] {
                // Each ore robot costs 2 ore.
                new Dictionary<Resource, int> {
                    { Resource.Ore, 2 },
                },
                // Each clay robot costs 3 ore.
                new Dictionary<Resource, int> {
                    { Resource.Ore, 3 },
                },
                // Each obsidian robot costs 3 ore and 8 clay.
                new Dictionary<Resource, int> {
                    { Resource.Ore, 3 },
                    { Resource.Clay, 8 },
                },
                // Each geode robot costs 3 ore and 12 obsidian.
                new Dictionary<Resource, int> {
                    { Resource.Ore, 3 },
                    { Resource.Obsidian, 12 },
                },
            }
        });

        Assert.AreEqual(12, simulation.Start());
    }

    [Test]
    public void Example1() {
        var simulation = new NotEnoughMinerals(File.ReadAllLines(@"19\example.txt"));

        Assert.AreEqual(33, simulation.CalculateQualityLevels());
    }

    [Test]
    public void Puzzle1() {
        var simulation = new NotEnoughMinerals(File.ReadAllLines(@"19\input.txt"));
        int result = simulation.CalculateQualityLevels();
        Assert.AreEqual(1081, result);
        Assert.Pass("Puzzle 1: " + result);
    }

    [Test]
    public void Example2() {
        var simulation = new NotEnoughMinerals(File.ReadAllLines(@"19\example.txt")) {
            MaxMinute = 32,
            AllowedBlueprints = new[] { 1, 2, 3 },
        };

        Assert.AreEqual(56 * 62, simulation.CalculateProductOfGeodeCount());
    }

    [Test]
    public void Puzzle2() {
        var simulation = new NotEnoughMinerals(File.ReadAllLines(@"19\input.txt")) {
            MaxMinute = 32,
            AllowedBlueprints = new[] { 1, 2, 3 },
        };
        int result = simulation.CalculateProductOfGeodeCount();
        Assert.AreEqual(2415, result);
        Assert.Pass("Puzzle 2: " + result);
    }
}