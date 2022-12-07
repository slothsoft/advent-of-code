using NUnit.Framework;

namespace AoC._07;

public class NoSpaceLeftOnDeviceTest {

    [Test]
    public void Example1()
    {
        var device = new NoSpaceLeftOnDevice(System.IO.File.ReadAllLines(@"07\example.txt"));
        Assert.AreEqual(95437, device.TraverseSmallFolders());
    }

    [Test]
    public void Puzzle1() {
        var device = new NoSpaceLeftOnDevice(System.IO.File.ReadAllLines(@"07\input.txt"));
        var result = device.TraverseSmallFolders();
        Assert.AreEqual(1501149, result);
        Assert.Pass("Puzzle 1: " + result);
    }


    [Test]
    public void Example2() {
        var device = new NoSpaceLeftOnDevice(System.IO.File.ReadAllLines(@"07\example.txt"));
        Assert.AreEqual(24933642, device.FindDirectoryToDelete());
    }

    [Test]
    public void Puzzle2() {
        var device = new NoSpaceLeftOnDevice(System.IO.File.ReadAllLines(@"07\input.txt"));
        var result = device.FindDirectoryToDelete();
        Assert.AreEqual(10096985, result);
        Assert.Pass("Puzzle 2: " + result);
    }
}