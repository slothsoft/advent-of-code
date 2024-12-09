using System.Collections.Generic;
using System.Linq;

namespace AoC.day9;

/// <summary>
/// <a href="https://adventofcode.com/2024/day/9">Day 9: Disk Fragmenter</a>
/// </summary>
public class DiskFragmenter {
    public DiskFragmenter(string input) {
        Input = input.ParseDiskMap();
    }

    internal int?[] Input { get; }

    public long CalculateFileSystemChecksum() {
        var result = 0L;
        var skipped = 0;
        
        for (var i = 0; i < Input.Length; i++) {
            if (Input[i] != null) {
                result += (i - skipped) * Input[i]!.Value;
            } else {
                skipped++;
            }
        }

        return result;
    }
}

public static class DiskFragmenterExtensions {
    public static int?[] ParseDiskMap(this string input) {
        var result = new List<int?>();
        var id = 0;
        var isFile = true;
        
        foreach (var c in input) {
            var length = int.Parse(c.ToString());
            for (var i = 0; i < length; i++) {
                result.Add(isFile ? id : null);
            }

            if (isFile) id++;
            isFile = !isFile;
        }
        
        return result.ToArray();
    }
    
    public static void Fragment(this int?[] diskMap) {
        var firstIndex = 0;
        var lastIndex = diskMap.Length - 1;

        while (firstIndex < lastIndex) {
            if (diskMap[firstIndex] != null) {
                // there already is a file, so we don't do nothing
                firstIndex++;
                continue;
            }
            
            // free space! let's find the last file part and move it
            while (diskMap[lastIndex] == null) lastIndex--;

            diskMap[firstIndex] = diskMap[lastIndex];
            diskMap[lastIndex] = null;
            
            firstIndex++;
            lastIndex--;
        }
    }
    
    public static string Stringify(this int?[] diskMap) {
        return string.Join("", diskMap.Select(i => i == null ? "." : i.ToString()));
    }
}