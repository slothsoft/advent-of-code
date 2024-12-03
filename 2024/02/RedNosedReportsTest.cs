using System.IO;
using System.Linq;
using NUnit.Framework;

namespace AoC.day2;

public class RedNosedReportsTest {
    [Test]
    public void Example1() {
        var example = new RedNosedReports(File.ReadAllLines(@"02\example.txt"));
        
        Assert.AreEqual(2,  example.CalculateSafeReportsCount());       
    }

    [Test]
    public void Puzzle1() {
        var puzzle = new RedNosedReports(File.ReadAllLines(@"02\input.txt"));
        
        Assert.AreEqual(686,  puzzle.CalculateSafeReportsCount());  
    }

    [Test]
    public void Example2() {
        var example = new RedNosedReports(File.ReadAllLines(@"02\example.txt")) {
            ProblemDampener = true,
        };
        
        Assert.AreEqual(4,  example.CalculateSafeReportsCount());   
    }

    [Test]
    public void Puzzle2() {
        var puzzle = new RedNosedReports(File.ReadAllLines(@"02\input.txt")) {
            ProblemDampener = true,
        };
        
        Assert.AreEqual(717,  puzzle.CalculateSafeReportsCount());  
    }
    
    [Test]
    [TestCase("43,44,45,44,46,44", false)]
    [TestCase("79,82,84,83,85,92", false)]
    [TestCase("7,6,4,2,1", true)]
    [TestCase("1,2,7,8,9", false)]
    [TestCase("9,7,6,2,1", false)]
    [TestCase("1,3,2,4,5", true)]
    [TestCase("8,6,4,4,1", true)]
    [TestCase("1,3,6,7,9", true)]
    public void Puzzle2_IsReportSafe(string reportLine, bool expectedIsSafe) {
        var puzzle = new RedNosedReports([]) {
            ProblemDampener = true,
        };

        var report = reportLine.Split(",").Select(s => s.ExtractDigitsAsInt()).ToArray();
        Assert.AreEqual(expectedIsSafe,  puzzle.IsReportSafe(report));  
    }
}
