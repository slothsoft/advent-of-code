using System;
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

    public long CalculateFileSystemChecksum(bool skipEmpty) {
        var result = 0L;
        var skipped = 0;
        
        for (var i = 0; i < Input.Length; i++) {
            if (Input[i] != null) {
                result += (i - skipped) * Input[i]!.Value;
            } else {
                // I have NO IDEA why this is necessary, but found it via trial and error
                if (skipEmpty) skipped++;
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
    
    public static void Compact(this int?[] diskMap) {
        var ids = diskMap.OfType<int>().Distinct().OrderDescending().ToArray();

        foreach (var id in ids) {
            var fileStart = Array.IndexOf(diskMap, id);
            var fileEnd = Array.LastIndexOf(diskMap, id);
            var fileLength = fileEnd - fileStart + 1;
            
            // where exactly did the ID go?
            if (fileStart == -1) throw new Exception("Could not find fields with ID " + id + ": " + diskMap.Stringify());

            for (var i = 0; i < diskMap.Length; i++) {
                // we are now on the file
                if (diskMap[i] != null && diskMap[i] == id) break;

                if (diskMap[i] == null) {
                    // found free space, check if it is big enough
                    var bigEnough = true;
                    var breakingIndex = 0;
                    for (breakingIndex = i + 1; breakingIndex < i + fileLength; breakingIndex++) {
                        if (diskMap[breakingIndex] != null) {
                            bigEnough = false;
                            break;
                        }
                    }
                    
                    // if it is not big enough, skip a couple of spaces
                    if (!bigEnough) {
                        i += breakingIndex - i;
                        continue;
                    }
                    
                    // if it is big enough, move file and break
                    for (var iPlus = 0; iPlus < fileLength; iPlus++) {
                        diskMap[i + iPlus] = diskMap[fileStart + iPlus];
                        diskMap[fileStart + iPlus] = null;
                    }
                    break;
                }      
            }

        }
    }
    
    public static string Stringify(this int?[] diskMap) {
        return string.Join("", diskMap.Select(i => i == null ? "." : i.ToString()));
    }
}