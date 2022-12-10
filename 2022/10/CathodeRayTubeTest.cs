using System.IO;
using NUnit.Framework;

namespace AoC._10;

public class CathodeRayTubeTest {

    private static readonly int[] TestedCycles = {20, 60, 100, 140, 180, 220};
    
    [Test]
    public void Example1A() {
        var result = CathodeRayTube.ExecuteProgram(File.ReadAllLines(@"10\example1.txt"), 1, 2, 3, 4, 5, 6);
        
        Assert.AreEqual(1, result[1]);
        Assert.AreEqual(1, result[2]);
        Assert.AreEqual(1, result[3]);
        Assert.AreEqual(4, result[4]);
        Assert.AreEqual(4, result[5]);
        Assert.AreEqual(-1, result[6]);
    }
    
    [Test]
    public void Example1B() {
        var result = CathodeRayTube.ExecuteProgram(File.ReadAllLines(@"10\example2.txt"), TestedCycles);
        
        Assert.AreEqual(21, result[20]);
        Assert.AreEqual(19, result[60]);
        Assert.AreEqual(18, result[100]);
        Assert.AreEqual(21, result[140]);
        Assert.AreEqual(16 , result[180]);
        Assert.AreEqual(18, result[220]);
        
        Assert.AreEqual(13140, result.CalculateSignalStrength());
    }
    
    [Test]
    public void Puzzle1() {
        var result = CathodeRayTube.ExecuteProgram(File.ReadAllLines(@"10\input.txt"), TestedCycles);
        var signalStrength = result.CalculateSignalStrength();
        Assert.AreEqual(13480, signalStrength);
        Assert.Pass("Puzzle 1: " + signalStrength);
    }
    
    [Test]
    public void Example2() {
        var result = CathodeRayTube.RenderProgram(File.ReadAllLines(@"10\example2.txt"));
        
        Assert.AreEqual(@"##..##..##..##..##..##..##..##..##..##..
###...###...###...###...###...###...###.
####....####....####....####....####....
#####.....#####.....#####.....#####.....
######......######......######......####
#######.......#######.......#######.....", result);
    }
    
    [Test]
    public void Puzzle2() {
        var result = CathodeRayTube.RenderProgram(File.ReadAllLines(@"10\input.txt"));
        Assert.AreEqual(@"####..##....##.###...##...##..####.#..#.
#....#..#....#.#..#.#..#.#..#.#....#.#..
###..#.......#.###..#....#....###..##...
#....#.##....#.#..#.#.##.#....#....#.#..
#....#..#.#..#.#..#.#..#.#..#.#....#.#..
####..###..##..###...###..##..#....#..#.", result);
        Assert.Pass("Puzzle 2: \n" + result);
    }
}