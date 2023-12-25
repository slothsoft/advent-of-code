using System.Collections.Generic;
using System.IO;
using System.Linq;
using NUnit.Framework;
using Point = AoC.day22.SandSlabs.Point;
using Slab = AoC.day22.SandSlabs.Slab;

namespace AoC.day22;

public class SandSlabsTest {
    [Test]
    public void Example1_FallDown() {
        var example = new SandSlabs(File.ReadAllLines(@"22\example.txt"));
        
        var index = 0;
        Assert.AreEqual(1,  example.Slabs[index].StartPoint.Z);     
        Assert.AreEqual(1,  example.Slabs[index].EndPoint.Z);
        index++;
        Assert.AreEqual(2,  example.Slabs[index].StartPoint.Z);     
        Assert.AreEqual(2,  example.Slabs[index].EndPoint.Z);
        index++;
        Assert.AreEqual(3,  example.Slabs[index].StartPoint.Z);     
        Assert.AreEqual(3,  example.Slabs[index].EndPoint.Z);
        index++;
        Assert.AreEqual(4,  example.Slabs[index].StartPoint.Z);     
        Assert.AreEqual(4,  example.Slabs[index].EndPoint.Z);
        index++;
        Assert.AreEqual(5,  example.Slabs[index].StartPoint.Z);     
        Assert.AreEqual(5,  example.Slabs[index].EndPoint.Z);
        index++;
        Assert.AreEqual(6,  example.Slabs[index].StartPoint.Z);     
        Assert.AreEqual(6,  example.Slabs[index].EndPoint.Z);
        index++;
        Assert.AreEqual(8,  example.Slabs[index].StartPoint.Z);     
        Assert.AreEqual(9,  example.Slabs[index].EndPoint.Z);
        
        example.FallDown();

        index = 0;
        Assert.AreEqual(1,  example.Slabs[index].StartPoint.Z);     
        Assert.AreEqual(1,  example.Slabs[index].EndPoint.Z);
        index++;
        Assert.AreEqual(2,  example.Slabs[index].StartPoint.Z);     
        Assert.AreEqual(2,  example.Slabs[index].EndPoint.Z);
        index++;
        Assert.AreEqual(2,  example.Slabs[index].StartPoint.Z);     
        Assert.AreEqual(2,  example.Slabs[index].EndPoint.Z);
        index++;
        Assert.AreEqual(3,  example.Slabs[index].StartPoint.Z);     
        Assert.AreEqual(3,  example.Slabs[index].EndPoint.Z);
        index++;
        Assert.AreEqual(3,  example.Slabs[index].StartPoint.Z);     
        Assert.AreEqual(3,  example.Slabs[index].EndPoint.Z);
        index++;
        Assert.AreEqual(4,  example.Slabs[index].StartPoint.Z);     
        Assert.AreEqual(4,  example.Slabs[index].EndPoint.Z);
        index++;
        Assert.AreEqual(5,  example.Slabs[index].StartPoint.Z);     
        Assert.AreEqual(6,  example.Slabs[index].EndPoint.Z);
    }

    [Test]
    [TestCaseSource(nameof(CreateIntersectionTestSource))]
    public void Example1_Intersection(Point slab1PointA, Point slab1PointB, Point slab2PointA, Point slab2PointB,Point resultingSlabPointA, Point resultingSlabPointB) {
        var slab1 = new Slab(slab1PointA, slab1PointB);
        var slab2 = new Slab(slab2PointA, slab2PointB);
        
        var resultingSlab = new Slab(resultingSlabPointA, resultingSlabPointB);

        var intersection = slab1.Intersection(slab2);
        Assert.NotNull(intersection);
        Assert.True(intersection!.EqualsPoints(resultingSlab), $"Expected {resultingSlab}, but was {intersection}");
    }

    private static IEnumerable<TestCaseData> CreateIntersectionTestSource() {
        // this is always the same test case of the 2D array 10|5-25|15 intersecting 20|10-30|25 in 20|10-25|15
        return SandSlabs.coordinates.SelectMany(coordinate => {
            var otherCoordinates = SandSlabs.coordinates.Where(c => c != coordinate).ToArray();
            
            var rect1 = new List<Point[]>();

            var point1 = new Point {values = {[otherCoordinates[0]] = 10, [otherCoordinates[1]] = 5}};
            var point2 = new Point {values = {[otherCoordinates[0]] = 25, [otherCoordinates[1]] = 15}};
            rect1.Add(new [] { point1, point2 });
            rect1.Add(new [] { point2, point1 });
            
            var point3 = new Point {values = {[otherCoordinates[0]] = 10, [otherCoordinates[1]] = 15}};
            var point4 = new Point {values = {[otherCoordinates[0]] = 25, [otherCoordinates[1]] = 5}};
            rect1.Add(new [] { point3, point4 });
            rect1.Add(new [] { point4, point3 });

            var rect2 = new List<Point[]>();
            
            point1 = new Point {values = {[otherCoordinates[0]] = 20, [otherCoordinates[1]] = 10}};
            point2 = new Point {values = {[otherCoordinates[0]] = 30, [otherCoordinates[1]] = 25}};
            rect2.Add(new [] { point1, point2 });
            rect2.Add(new [] { point2, point1 });
            
            point3 = new Point {values = {[otherCoordinates[0]] = 20, [otherCoordinates[1]] = 30}};
            point4 = new Point {values = {[otherCoordinates[0]] = 25, [otherCoordinates[1]] = 10}};
            rect2.Add(new [] { point3, point4 });
            rect2.Add(new [] { point4, point3 });

            var resultPoint1 = new Point {values = {[otherCoordinates[0]] = 20, [otherCoordinates[1]] = 10}};
            var resultPoint2 = new Point {values = {[otherCoordinates[0]] = 25, [otherCoordinates[1]] = 15}};
            
            return rect1.SelectMany(p => rect2.Select(q => new TestCaseData(p[0], p[1], q[0], q[1], resultPoint1, resultPoint2)));
        });
    }
    
    [Test]
    public void Example1() {
        var example = new SandSlabs(File.ReadAllLines(@"22\example.txt"));
        
        Assert.AreEqual(5,  example.CalculateDisintegrateableSlabs().Count());       
    }

    [Test]
    public void Puzzle1() {
        var puzzle = new SandSlabs(File.ReadAllLines(@"22\input.txt"));
        
        Assert.AreEqual(492,  puzzle.CalculateDisintegrateableSlabs().Count());  
    }

    [Test]
    public void Example2() {
        var example = new SandSlabs(File.ReadAllLines(@"22\example.txt"));
        
        Assert.AreEqual(7,  example.CalculateEntireFallenBricks());   
    }

    [Test]
    public void Puzzle2() {
        var puzzle = new SandSlabs(File.ReadAllLines(@"22\input.txt"));
        
        Assert.AreEqual(86556,  puzzle.CalculateEntireFallenBricks());  
    }
}
