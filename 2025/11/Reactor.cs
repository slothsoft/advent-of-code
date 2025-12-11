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
        return CalculatePaths("you").Count();
    }
    
    private IEnumerable<ISet<string>> CalculatePaths(string from, string to = "out", ISet<string>? checkedDevices = null) {
        checkedDevices ??= new HashSet<string>();
        checkedDevices.Add(from);

        if (from == to) {
            yield return checkedDevices;
            yield break;
        }

        if (from == "out") {
            // there are no ways starting with out
            yield break;
        }

        var fromDevice = Input.Single(d => d.Name == from);
        foreach (var output in fromDevice.Outputs.Where(o => !checkedDevices.Contains(o))) {
            foreach (var path in CalculatePaths(output, to, new HashSet<string>(checkedDevices))) {
                yield return path;
            }
        }
    }
    
    public long CalculatePathsWithDevicesCount() {
        return CalculatePathsWithDevicesCount("fft", "dac");
    }

    private long CalculatePathsWithDevicesCount(string device1, string device2) {
        // since the devices are connected in a single direction everything that comes after device1 can't be part of the solution
        var svrToDevice1 = CalculatePaths("svr", device1, CalculateFollowers(device1)).LongCount();
        Debug.WriteLine($"svrToDevice1 = {svrToDevice1}");
        
        // we similarly remove the followers of device2 from the equation (we don't need to do anything for the devices before device1,
        // since we can't go back anyway
        var device1ToDevice2 = CalculatePaths(device1, device2, CalculateFollowers(device2)).LongCount();
        Debug.WriteLine($"device1ToDevice2 = {device1ToDevice2}");

        // and finally the last step of the way
        var device2ToOut = CalculatePaths(device2, "out").LongCount();
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