using System.Linq;
using NUnit.Framework;

namespace AoC._07;

/// <summary>
/// <a href="https://adventofcode.com/2022/day/7">Day 7: No Space Left On Device</a>: You can hear birds chirping and
/// raindrops hitting leaves as the expedition proceeds. Occasionally, you can even hear much louder sounds
/// in the distance; how big do the animals get out here, anyway?
/// 
/// The device the Elves gave you has problems with more than just its communication system.
/// </summary>
public class NoSpaceLeftOnDevice
{
    private const string CommandPrefix = "$ ";
    private const string DirectoryPrefix = "dir ";

    private readonly FileSystem _fileSystem = new();

    public NoSpaceLeftOnDevice(string[] input)
    {
        InitFileSystem(input);
    }

    private void InitFileSystem(string[] input)
    {
        for (var i = 1; i < input.Length;)
        {
            Assert.IsTrue(input[i].StartsWith(CommandPrefix), "Input line should be a command: " + input[i]);
            var commandSplit = input[i].Substring(CommandPrefix.Length).Split(" ");
            var command = commandSplit[0];
            var commandArgument = commandSplit.Length > 1 ? commandSplit[1] : null;
            var commandResult = input.Skip(i + 1).TakeWhile(l => !l.StartsWith(CommandPrefix)).ToArray();
            ApplyCommand(command, commandArgument, commandResult);
            i += 1 + commandResult.Length;
        }
    }

    private void ApplyCommand(string command, string? argument, string[] results)
    {
        switch (command)
        {
            case "ls":
                _fileSystem.CurrentFolder.AddChildren(results.Select(ParseLocation));
                break;
            case "cd":
                _fileSystem.ChooseDirectory(argument!);
                break;
        }
    }

    private ILocation ParseLocation(string location)
    {
        // "dir a" OR "14848514 b.txt"
        if (location.StartsWith(DirectoryPrefix))
        {
            return new Directory(location.Substring(DirectoryPrefix.Length));
        }
        var fileSplit = location.Split(" ");
        return new File(fileSplit[1], int.Parse(fileSplit[0]));
    }

    public int TraverseSmallFolders()
    {
        return _fileSystem.Root.Traverse().Where(l => l is {IsDirectory: true, Size: < 100000}).Select(l => l.Size).Sum();
    }

    public int FindDirectoryToDelete()
    {
        const int totalSize = 70000000;
        const int toBeFreedTotal = 30000000;
        var currentlyFree = totalSize - _fileSystem.Root.Size;
        var yetToBeFreed = toBeFreedTotal - currentlyFree;
        return _fileSystem.Root.Traverse().Where(l => l.IsDirectory && l.Size >= yetToBeFreed).OrderBy(l => l.Size).First().Size;
    }
}