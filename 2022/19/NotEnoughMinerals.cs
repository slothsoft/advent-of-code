using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace AoC._19;

/// <summary>
/// <a href="https://adventofcode.com/2022/day/19">Day 19: Not Enough Minerals</a>
/// </summary>
public class NotEnoughMinerals {
    static readonly Regex Regex =
        new("(Blueprint |: Each ore robot costs |\\. Each clay robot costs |\\. Each obsidian robot costs |\\. Each geode robot costs |\\.)");

    readonly Blueprint[] _blueprints;

    public NotEnoughMinerals(string[] lines) {
        _blueprints = lines.Select(ParseBlueprint).ToArray();
    }

    public int MaxMinute { get; init; } = 24;
    public int[]? AllowedBlueprints { get; init; }

    static Blueprint ParseBlueprint(string line) {
        // Blueprint 1: Each ore robot costs 4 ore.
        //     Each clay robot costs 2 ore.
        //     Each obsidian robot costs 3 ore and 14 clay.
        //     Each geode robot costs 2 ore and 7 obsidian.
        string[] split = Regex.Split(line);
        return new Blueprint {
            Id = int.Parse(split[2]),
            RobotCosts = new[] {
                ParseCosts(split[4]),
                ParseCosts(split[6]),
                ParseCosts(split[8]),
                ParseCosts(split[10]),
            }
        };
    }

    static IDictionary<Resource, int> ParseCosts(string costs) {
        // 2 ore, 3 ore and 14 clay
        return costs.Split(" and ").Select(s => {
            string[] numberAndResource = s.Trim().Split(" ");
            return new {
                Number = int.Parse(numberAndResource[0]),
                Resource = (Resource)Enum.Parse(typeof(Resource), char.ToUpper(numberAndResource[1][0]) + numberAndResource[1][1..])
            };
        }).ToDictionary(e => e.Resource, e => e.Number);
    }

    public int CalculateQualityLevels() {
        return SimulateMaxGeodeCount().Sum(e => e.Key * e.Value);
    }

    public int CalculateProductOfGeodeCount() {
        return SimulateMaxGeodeCount().Values.Aggregate((a, b) => a * b);
    }

    IDictionary<int, int> SimulateMaxGeodeCount() {
        var result = new Dictionary<int, int>();
        foreach (var blueprint in _blueprints) {
            if (AllowedBlueprints == null || AllowedBlueprints.Contains(blueprint.Id)) {
                var simulation = new Simulation(blueprint, MaxMinute);
                int simulationResult = simulation.Start();
                result[blueprint.Id] = simulationResult;
            }
        }

        return result;
    }
}

public enum Resource {
    Ore,
    Clay,
    Obsidian,
    Geode,
}

public struct Inventory {
    int oreCount;
    int clayCount;
    int obsidianCount;
    int geodeCount;

    public int this[Resource resource] {
        get {
            return resource switch {
                Resource.Ore => oreCount,
                Resource.Clay => clayCount,
                Resource.Obsidian => obsidianCount,
                Resource.Geode => geodeCount,
                _ => throw new NotImplementedException(),
            };
        }
        set {
            switch (resource) {
                case Resource.Ore:
                    oreCount = value;
                    break;
                case Resource.Clay:
                    clayCount = value;
                    break;
                case Resource.Obsidian:
                    obsidianCount = value;
                    break;
                case Resource.Geode:
                    geodeCount = value;
                    break;
                default:
                    throw new NotImplementedException();
            }
        }
    }

    public bool Has(in Inventory resources) {
        return oreCount >= resources.oreCount
            && clayCount >= resources.clayCount
            && obsidianCount >= resources.obsidianCount
            && geodeCount >= resources.geodeCount;
    }

    public bool Has(IDictionary<Resource, int> resources) {
        foreach (var (resource, count) in resources) {
            if (!Has(resource, count)) {
                return false;
            }
        }
        return true;
    }

    public bool Has(Resource resource, int count) {
        return this[resource] >= count;
    }

    public void Decrement(IDictionary<Resource, int> resources) {
        foreach (var resource in resources) {
            this[resource.Key] -= resource.Value;
        }
    }

    public void Increment(in Inventory resources) {
        oreCount += resources.oreCount;
        clayCount += resources.clayCount;
        obsidianCount += resources.obsidianCount;
        geodeCount += resources.geodeCount;
    }

}

public record Blueprint {
    public int Id { get; init; }
    public IDictionary<Resource, int>[] RobotCosts { get; init; } = Array.Empty<IDictionary<Resource, int>>();

    int[]? _maxResourceCount;

    public IDictionary<Resource, int> GetRobotCost(Resource resource) {
        return RobotCosts[(int)resource];
    }

    public int GetMaxResourceCount(Resource resource) {
        if (_maxResourceCount == null) {
            _maxResourceCount = new int[RobotCosts.Length];
            foreach (var r in Simulation.Resources) {
                _maxResourceCount[(int)r] = RobotCosts.Where(c => c.ContainsKey(r)).Select(c => c[r]).DefaultIfEmpty().Max();
            }
        }

        return _maxResourceCount[(int)resource];
    }
}

public class Simulation {
    internal static readonly Resource[] Resources = { Resource.Ore, Resource.Clay, Resource.Obsidian, Resource.Geode };

    record Result {
        public int Geodes { get; set; }
    }

    readonly Blueprint _blueprint;
    readonly int _maxMinute;

    public Simulation(Blueprint blueprint, int maxMinute = 24) {
        _blueprint = blueprint;
        _maxMinute = maxMinute;
    }

    public int Start() {
        try {
            var inventory = new Inventory();
            var robots = new Inventory {
                [Resource.Ore] = 1,
            };
            var result = new Result();
            WorkOnAllResources(result, 1, inventory, robots);

            if (result.Geodes == 0) {
                Console.WriteLine($"Blueprint {_blueprint.Id} did not return any result");
            }
            return result.Geodes;
        } catch (Exception) {
            Console.WriteLine($"Blueprint {_blueprint} was broken somehow");
            throw;
        }
    }

    void WorkOnAllResources(Result result, int currentMinute, in Inventory inventory, in Inventory robots) {
        // There already is a solution - and we can't reach it
        int maximumCurrentGeodes = (_maxMinute - currentMinute + 1) * robots[Resource.Geode];
        int maximumFutureGeodes = (_maxMinute - currentMinute) * (_maxMinute - currentMinute) / 2;
        if (inventory[Resource.Geode] + maximumCurrentGeodes + maximumFutureGeodes < result.Geodes) {
            return;
        }

        foreach (var resource in Resources) {
            // if the existing robots will not reach this robot's cost in the runtime of the simulation
            if (!AreRobotsCollectingForRobot(currentMinute, inventory, robots, resource)) {
                continue;
            }

            // do not build robots after the maximum for their resource is reached
            if (resource != Resource.Geode && robots[resource] >= _blueprint.GetMaxResourceCount(resource)) {
                continue;
            }

            // copy everything and split into new work threads
            Work(result, currentMinute, inventory, robots, resource);
        }
    }

    bool AreRobotsCollectingForRobot(int currentMinute, in Inventory inventory, in Inventory robots, Resource robotToBuild) {
        var robotCost = _blueprint.GetRobotCost(robotToBuild);
        foreach (var resource in robotCost.Keys) {
            int count = robots[resource];
            if (inventory[resource] + ((_maxMinute - currentMinute - 1) * count) < robotCost[resource]) {
                return false;
            }
        }

        return true;
    }

    void Work(Result result, int currentMinute, Inventory inventory, Inventory robots, Resource nextRobotType) {
        // check if we can build the next robot type
        bool hasBuiltRobot = false;
        var nextRobotCost = _blueprint.GetRobotCost(nextRobotType);
        if (inventory.Has(nextRobotCost)) {
            hasBuiltRobot = true;
            inventory.Decrement(nextRobotCost);
        }

        // let  the robots do their thing
        inventory.Increment(robots);

        // stop the recursiveness
        if (currentMinute >= _maxMinute) {
            // but we do not count this as a result if there are no geodes
            result.Geodes = Math.Max(inventory[Resource.Geode], result.Geodes);
            return;
        }

        // so we still have work to do

        if (hasBuiltRobot) {
            // since the next robot was build, we need to split up to search for the best next robot
            robots[nextRobotType]++;
            if (currentMinute >= _maxMinute - 1) {
                // we do not need to split up any more
                Work(result, currentMinute + 1, inventory, robots, nextRobotType);
                return;
            }

            WorkOnAllResources(result, currentMinute + 1, inventory, robots);
            return;
        }

        // the next robot was not build yet, because the resources are missing; just work another minute
        Work(result, currentMinute + 1, inventory, robots, nextRobotType);
    }
}