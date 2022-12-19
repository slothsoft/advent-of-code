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

public class Inventory {
    readonly int[] _resources;

    public Inventory() : this(new int[(int)Resource.Geode + 1]) {
    }

    Inventory(int[] resources) {
        _resources = resources;
    }

    public bool Has(IDictionary<Resource, int> resources) {
        return resources.All(e => Has(e.Key, e.Value));
    }

    public bool Has(Resource resource, int count) {
        return _resources[(int)resource] >= count;
    }

    public void Decrement(IDictionary<Resource, int> resources) {
        foreach (var resource in resources) {
            Increment(resource.Key, -resource.Value);
        }
    }

    public void Increment(Resource resource, int count) {
        _resources[(int)resource] += count;
    }

    public int Get(Resource resource) {
        return _resources[(int)resource];
    }

    public Inventory Copy() {
        int[] copyOfResources = new int[_resources.Length];
        Array.Copy(_resources, copyOfResources, _resources.Length);
        return new Inventory(copyOfResources);
    }
}

public class Robot {
    public Resource Collecting { get; }

    public Robot(Resource collecting) {
        Collecting = collecting;
    }

    public void Work(Inventory inventory) {
        inventory.Increment(Collecting, 1);
    }

    public override string ToString() {
        return "Robot " + Collecting;
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
            var robots = new List<Robot> {
                new(Resource.Ore),
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

    void WorkOnAllResources(Result result, int currentMinute, Inventory inventory, IList<Robot> robots) {
        // There already is a solution - and we can't reach it
        int maximumCurrentGeodes = (_maxMinute - currentMinute + 1) * robots.Count(r => r.Collecting == Resource.Geode);
        int maximumFutureGeodes = (_maxMinute - currentMinute) * (_maxMinute - currentMinute) / 2;
        if (inventory.Get(Resource.Geode) + maximumCurrentGeodes + maximumFutureGeodes < result.Geodes) {
            return;
        }

        foreach (var resource in Resources) {
            // if the existing robots will not reach this robot's cost in the runtime of the simulation
            if (!AreRobotsCollectingForRobot(currentMinute, inventory, robots, resource)) {
                continue;
            }

            // do not build robots after the maximum for their resource is reached
            if (resource != Resource.Geode && robots.Count(r => r.Collecting == resource) >= _blueprint.GetMaxResourceCount(resource)) {
                continue;
            }

            // copy everything and split into new work threads
            Work(result, currentMinute, inventory.Copy(), new List<Robot>(robots), resource);
        }
    }

    bool AreRobotsCollectingForRobot(int currentMinute, Inventory inventory, IList<Robot> robots, Resource robotToBuild) {
        var robotCost = _blueprint.GetRobotCost(robotToBuild);
        foreach (var resource in robotCost.Keys) {
            int count = robots.Count(r => r.Collecting == resource);
            if (inventory.Get(resource) + ((_maxMinute - currentMinute - 1) * count) < robotCost[resource]) {
                return false;
            }
        }

        return true;
    }

    void Work(Result result, int currentMinute, Inventory inventory, IList<Robot> robots, Resource nextRobotType) {
        // check if we can build the next robot type
        Robot? nextRobot = null;
        var nextRobotCost = _blueprint.GetRobotCost(nextRobotType);
        if (inventory.Has(nextRobotCost)) {
            nextRobot = new Robot(nextRobotType);
            inventory.Decrement(nextRobotCost);
        }

        // let  the robots do their thing
        foreach (var robot in robots) {
            robot.Work(inventory);
        }

        // stop the recursiveness
        if (currentMinute >= _maxMinute) {
            // but we do not count this as a result if there are no geodes
            if (inventory.Has(Resource.Geode, 1)) {
                result.Geodes = Math.Max(inventory.Get(Resource.Geode), result.Geodes);
            }

            return;
        }

        // so we still have work to do

        if (nextRobot != null) {
            // since the next robot was build, we need to split up to search for the best next robot
            robots.Add(nextRobot);
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