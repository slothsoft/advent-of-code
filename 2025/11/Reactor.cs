using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace AoC.day11;

/// <summary>
/// <a href="https://adventofcode.com/2025/day/11">Day 11: Reactor</a>
/// </summary>
public class Reactor {
    internal record Device(string Name, params string[] Outputs) {
    }

    public Reactor(IEnumerable<string> input) {
        Input = ParseInput(input);
    }

    internal Device[] Input { get; }

    internal static Device[] ParseInput(IEnumerable<string> input) {
        return input.Select(s => {
            var split = s.Split(":");
            return new Device(split[0], split[1].Trim().Split(" ").ToArray());
        }).ToArray();
    }

    public long CalculatePathsCount() {
        return CalculatePathCount("you");
    }
    
    private long CalculatePathCount(string from, string to = "out", IDictionary<string, long>? checkedDevices = null) {
        checkedDevices ??= new Dictionary<string, long>();

        if (checkedDevices.TryGetValue(from, out var cachedCount)) {
            return cachedCount;
        }

        if (from == to) {
            return 1;
        }

        var fromDevice = Input.Single(d => d.Name == from);
        var result = 0L;
        
        foreach (var output in fromDevice.Outputs) {
            result += CalculatePathCount(output, to, checkedDevices);
        }

        checkedDevices[from] = result;
        return result;
    }
    
    public long CalculatePathsWithDevicesCount() {
        return CalculatePathsWithDevicesCount("fft", "dac");
    }

    private long CalculatePathsWithDevicesCount(string device1, string device2) {
        // since the devices are connected in a single direction everything that comes after device1 can't be part of the solution
        var svrToDevice1 = CalculatePathCount("svr", device1, CalculateFollowers(device1).ToDictionary(d => d, _ => 0L));
        Debug.WriteLine($"svrToDevice1 = {svrToDevice1}");
        
        // we similarly remove the followers of device2 from the equation (we don't need to do anything for the devices before device1,
        // since we can't go back anyway
        var device1ToDevice2 = CalculatePathCount(device1, device2, CalculateFollowers(device2).ToDictionary(d => d, _ => 0L));
        Debug.WriteLine($"device1ToDevice2 = {device1ToDevice2}");

        // and finally the last step of the way
        var device2ToOut = CalculatePathCount(device2, "out");
        Debug.WriteLine($"device2ToOut = {device2ToOut}");
        
        // now multiply and hope none of these values is zero
        return svrToDevice1 * device1ToDevice2 * device2ToOut;
    }
    
    private ISet<string> CalculateFollowers(string deviceName, ISet<string>? followers = null) {
        followers ??= new HashSet<string>();
        
        var device = Input.SingleOrDefault(d => d.Name == deviceName);
        if (device == null) {
            // this is "out"
            return followers;
        }
        
        foreach (var output in device.Outputs) {
            if (followers.Contains(output)) {
                continue;
            }
            followers.Add(output);
            CalculateFollowers(output, followers);
        }

        return followers;
    }
}