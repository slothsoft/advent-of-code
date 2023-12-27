using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Numerics;
using AoC.day25;

namespace AoC.day24;

/// <summary>
/// <a href="https://adventofcode.com/2023/day/24">Day 24: Never Tell Me The Odds</a>
///
/// Source for the line longersections is https://emedia.rmit.edu.au/learninglab/content/v7-longersecting-lines-3d
/// </summary>
public class NeverTellMeTheOdds {
    public interface ILine {
        IPoint? CalculateIntersection(ILine other);
        IPoint ZeroPoint { get; }
        IPoint VelocityPoint { get; }
    }

    public interface IPoint {
        double Get(int coordinate);
        int[] GetCoordinates();
    }

    // x=x0+at; y=y0+bt; z=z0+ct (the first part is Zero; the second part that multiplies with t is Velocity)
    public record Line2D(Point2D ZeroCoordinates, Point2D VelocityCoordinates) : ILine {
        public Point2D ZeroCoordinates { get; } = ZeroCoordinates;
        public Point2D VelocityCoordinates { get; } = VelocityCoordinates;
        public IPoint ZeroPoint => ZeroCoordinates;
        public IPoint VelocityPoint => VelocityCoordinates;
        public IPoint? CalculateIntersection(ILine other) => CalculateIntersection((Line2D)other);

        public Point2D? CalculateIntersection(Line2D other) {
            // (ZeroX - other.ZeroX) == (s * other.VelocityX - t * VelocityX)
            var equations = CalculateEquations(other);

            // we multiply X with other.VelocityY and Y with -other.VelocityX, so that when we add these lines, zero will be the result
            const int sOrT = 1;
            const int tOrS = 2;
            var xTimesVelocityY = equations[Point2D.COORDINATE_X].Select(n => n * equations[Point2D.COORDINATE_Y][sOrT]).ToArray();
            var yTimesVelocityX = equations[Point2D.COORDINATE_Y].Select(n => n * -equations[Point2D.COORDINATE_X][sOrT]).ToArray();
            for (var i = 0; i < xTimesVelocityY.Length; i++) {
                xTimesVelocityY[i] += yTimesVelocityX[i];
            }

            if (xTimesVelocityY[sOrT] != 0) {
                throw new ArgumentException(
                    $"Something went wrong multiplying and adding the equations for X and Y! expected 0, but was {xTimesVelocityY[sOrT]}");
            }

            // so now we have (ZeroX - other.ZeroX) == (t * VelocityX), so we can calculate T
            var t = xTimesVelocityY[0] / xTimesVelocityY[tOrS];

            // now we just need to calculate the intersection point
            var intersection = CalculatePoint(t);
            if (double.IsInfinity(intersection.X) || double.IsInfinity(intersection.Y)) {
                return null;
            }

            return intersection;
        }

        private Point2D CalculatePoint(double n) {
            return new Point2D(Point2D.coordinates.Select(coordinate => ZeroCoordinates.Get(coordinate) + VelocityCoordinates.Get(coordinate) * n).ToArray());
        }

        private IList<double[]> CalculateEquations(Line2D other) {
            return Point2D.coordinates.Select(coordinate => new[] {
                ZeroCoordinates.Get(coordinate) - other.ZeroCoordinates.Get(coordinate), other.VelocityCoordinates.Get(coordinate),
                -VelocityCoordinates.Get(coordinate)
            }).ToList();
        }
    }

    public readonly struct Point2D : IPoint {
        public const int COORDINATE_X = 0;
        public const int COORDINATE_Y = 1;
        public static readonly int[] coordinates = {COORDINATE_X, COORDINATE_Y};

        public Point2D(double[] newCoordinates) {
            _coordinates = newCoordinates;
        }

        public Point2D(double x, double y) {
            _coordinates = new[] {x, y};
        }

        private readonly double[] _coordinates;
        public double X { get => _coordinates[COORDINATE_X]; }
        public double Y { get => _coordinates[COORDINATE_Y]; }

        public double Get(int coordinate) {
            return _coordinates[coordinate];
        }

        public int[] GetCoordinates() {
            return coordinates;
        }

        public Point2D Copy() {
            return new Point2D(X, Y);
        }

        public Point2D With(int coordinate, double value) {
            return new Point2D(X, Y) {_coordinates = {[coordinate] = value}};
        }

        public override string ToString() => $"({X}|{Y})";

        public bool Equals(Point2D other) => _coordinates.SequenceEqual(other._coordinates);
        public override bool Equals(object? obj) => obj is Point2D other && Equals(other);
        public override int GetHashCode() => _coordinates.Select(n => n.GetHashCode()).Aggregate((a, b) => a * (13 + b));
    }

    // x=x0+at; y=y0+bt; z=z0+ct (the first part is Zero; the second part that multiplies with t is Velocity)
    public record Line3D(Point3D ZeroCoordinates, Point3D VelocityCoordinates) : ILine {
        public Point3D ZeroCoordinates { get; } = ZeroCoordinates;
        public Point3D VelocityCoordinates { get; } = VelocityCoordinates;
        public IPoint ZeroPoint => ZeroCoordinates;
        public IPoint VelocityPoint => VelocityCoordinates;
        public IPoint? CalculateIntersection(ILine other) => CalculateIntersection((Line3D)other);

        public Point3D? CalculateIntersection(Line3D other) {
            // (ZeroX - other.ZeroX) == (s * other.VelocityX - t * VelocityX)
            var equations = CalculateEquations(other);

            // we multiply X with other.VelocityY and Y with -other.VelocityX, so that when we add these lines, zero will be the result
            const int sOrT = 1;
            const int tOrS = 2;
            var xTimesVelocityY = equations[Point3D.COORDINATE_X].Select(n => n * equations[Point3D.COORDINATE_Y][sOrT]).ToArray();
            var yTimesVelocityX = equations[Point3D.COORDINATE_Y].Select(n => n * -equations[Point3D.COORDINATE_X][sOrT]).ToArray();
            for (var i = 0; i < xTimesVelocityY.Length; i++) {
                xTimesVelocityY[i] += yTimesVelocityX[i];
            }

            if (xTimesVelocityY[sOrT] != 0) {
                throw new ArgumentException(
                    $"Something went wrong multiplying and adding the equations for X and Y! expected 0, but was {xTimesVelocityY[sOrT]}");
            }

            // so now we have (ZeroX - other.ZeroX) == (t * VelocityX), so we can calculate T
            var t = xTimesVelocityY[0] / xTimesVelocityY[tOrS];

            // and we can calculate S by using the other equation
            var s = (yTimesVelocityX[0] - yTimesVelocityX[2] * t) / yTimesVelocityX[1];

            // now we just need to calculate the intersection point
            var intersection1 = CalculatePoint(t);
            var intersection2 = other.CalculatePoint(s);
            if (!intersection1.Equals(intersection2)) {
                // not really an intersection, because Z didn't work out
                return null;
            }

            return intersection1;
        }

        private Point3D CalculatePoint(double n) {
            return new Point3D(Point3D.coordinates.Select(coordinate => ZeroCoordinates.Get(coordinate) + VelocityCoordinates.Get(coordinate) * n).ToArray());
        }

        private IList<double[]> CalculateEquations(Line3D other) {
            // for all coordinates is (ZeroX - other.ZeroX) == (s * other.VelocityX - t * VelocityX)
            // (and we need to find s and t)
            return Point3D.coordinates.Select(coordinate => new[] {
                ZeroCoordinates.Get(coordinate) - other.ZeroCoordinates.Get(coordinate), // ZeroX - other.ZeroX
                other.VelocityCoordinates.Get(coordinate), // s * other.VelocityX
                -VelocityCoordinates.Get(coordinate) // t * VelocityX
            }).ToList();
        }

        public Line2D To2D() {
            return new Line2D(ZeroCoordinates.To2D(), VelocityCoordinates.To2D());
        }
    }

    public readonly struct Point3D : IPoint {
        public const int COORDINATE_X = 0;
        public const int COORDINATE_Y = 1;
        public const int COORDINATE_Z = 2;
        public static readonly int[] coordinates = {COORDINATE_X, COORDINATE_Y, COORDINATE_Z};

        public Point3D(double[] newCoordinates) {
            _coordinates = newCoordinates;
        }

        public Point3D(double x, double y, double z) {
            _coordinates = new[] {x, y, z};
        }

        private readonly double[] _coordinates;
        public double X { get => _coordinates[COORDINATE_X]; }
        public double Y { get => _coordinates[COORDINATE_Y]; }
        public double Z { get => _coordinates[COORDINATE_Z]; set => _coordinates[COORDINATE_Z] = value; }

        public double Get(int coordinate) {
            return _coordinates[coordinate];
        }

        public int[] GetCoordinates() {
            return coordinates;
        }

        public Point3D Copy() {
            return new Point3D(X, Y, Z);
        }

        public Point3D With(int coordinate, double value) {
            return new Point3D(X, Y, Z) {_coordinates = {[coordinate] = value}};
        }

        public Point2D To2D() {
            return new Point2D(X, Y);
        }

        public override string ToString() => $"({X}|{Y}|{Z})";

        public bool Equals(Point3D other) => _coordinates.SequenceEqual(other._coordinates);
        public override bool Equals(object? obj) => obj is Point3D other && Equals(other);
        public override int GetHashCode() => _coordinates.Select(n => n.GetHashCode()).Aggregate((a, b) => a * (13 + b));
    }

    public NeverTellMeTheOdds(IEnumerable<string> input, bool ignoreZ = false) {
        Input = input
            .Select(ParseLine)
            .Select(line => (ILine)(ignoreZ ? line.To2D() : line))
            .ToArray();
    }

    internal ILine[] Input { get; }

    internal static Line3D ParseLine(string input) {
        var split = input.Split(" @ ");
        return new Line3D(ParsePoint(split[0]), ParsePoint(split[1]));
    }

    internal static Point3D ParsePoint(string input) {
        return new Point3D(input.Split(", ").Select(s => double.Parse(s.Trim(), NumberFormatInfo.InvariantInfo)).ToArray());
    }

    public long CalculateIntersections(double min, double max) {
        var result = 0L;
        for (var i = 0; i < Input.Length; i++) {
            for (var o = i + 1; o < Input.Length; o++) {
                var input = Input[i];
                var other = Input[o];

                if (input != other) {
                    var intersection = input.CalculateIntersection(other);
                    if (intersection == null) {
                        continue; // they don't intersect at all
                    }

                    if (!intersection.GetCoordinates().All(c => intersection.Get(c) >= min && intersection.Get(c) <= max)) {
                        continue; // not in range!
                    }

                    if (IsInPast(input, intersection)) {
                        continue; // was in past for input
                    }

                    if (IsInPast(other, intersection)) {
                        continue; // was in past for other
                    }

                    Console.WriteLine($"Found intersection between {i} and {o}: {intersection}");
                    result++;
                }
            }
        }

        return result;
    }

    private static bool IsInPast(ILine line, IPoint point) {
        // calculate s or t; it has to be positive
        const int x = 0;
        var sOrT = (point.Get(x) - line.ZeroPoint.Get(x)) / line.VelocityPoint.Get(x);
        return sOrT < 0;
    }

    public Line3D? CalculateRockThrow() {
        // okay we need a solution for (Zero) + (Velocity * t) = (ZeroHail0) + (VelocityHail0 * s); (ZeroHail1) + (VelocityHail1 * r); (ZeroHail2) + (VelocityHail2 * q) ...
        // only... t == s, then t == r, then t == q, then ...
        // so we have three things we want (Zero, Velocity, t), so we need 3 Hails to calculate them (?)
        // I    (ZeroStone) + (VelocityStone * s) = (ZeroHail0) + (VelocityHail0 * s)
        // II   (ZeroStone) + (VelocityStone * r) = (ZeroHail1) + (VelocityHail1 * r)
        // III  (ZeroStone) + (VelocityStone * q) = (ZeroHail2) + (VelocityHail2 * q)
        // ...
        // okay, so each formula adds one variable, so we have one too many?
        //
        // I a) ZeroStone - ZeroHail0 = (VelocityHail0 - VelocityStone) * s
        // -> maybe s is zero for some hail storm? guess we could try out all 300 hails for that? 
        //
        // Vector Multiplication ( https://physics.info/vector-multiplication/ )
        //   "Geometrically, the cross product of two vectors is the area of the parallelogram between them.
        //    Since two identical vectors produce a degenerate parallelogram with no area, the cross product of any vector with itself is zero: A × A = 0"
        // 
        // -> so if we calculate with (ZeroStone - ZeroHail0) or (VelocityHail0 - VelocityStone) we get 0 on one side of the equation (but 2nd one removes s)
        // I b)   (ZeroStone - ZeroHail0) × (VelocityHail0 - VelocityStone) = 0
        // II b)  (ZeroStone - ZeroHail1) × (VelocityHail1 - VelocityStone) = 0
        // III b) (ZeroStone - ZeroHail2) × (VelocityHail2 - VelocityStone) = 0
        // 3 formulas, 2 variables, and we can probably remove any ambiguity with the fact that t, s, r, ... must be positive (because negative is in the past)
        // 
        // Cross Product ( https://en.wikipedia.org/wiki/Cross_product )
        // I b)   0 = ZeroY * VelocityZ - ZeroZ * VelocityY
        //        0 = ZeroZ * VelocityX - ZeroX * VelocityZ
        //        0 = ZeroX * VelocityY - ZeroY * VelocityX
        //        Zero = (ZeroStone - ZeroHail0)
        //        Velocity = (VelocityHail0 - VelocityStone)
        // does that help?
        //
        // both inputs have hails were the X/Y/Z are equal; could we use them for the specific coordinate?
        // A      (ZeroStone) + (VelocityStone * s) = (ZeroHail0) + (VelocityHail * s)
        // B      (ZeroStone) + (VelocityStone * r) = (ZeroHail1) + (VelocityHail * r)
        // A - B          (VelocityStone * (s - r)) = (ZeroHail0 - ZeroHail1) + (VelocityHail * (s - r))
        // -VelocityHail    (ZeroHail0 - ZeroHail1) = (VelocityStone - VelocityHail) * (s - r)
        // [needed]                                          X                          X   X
        // × (VelocityStone - VelocityHail)       0 = (ZeroHail0 - ZeroHail1) × (VelocityStone - VelocityHail)
        // OR with different VelocityHails
        //                                        0 = (ZeroHail0 - ZeroHail1) × (VelocityStone - (VelocityHail0 - VelocityHail1))
        // and this also means that (ZeroHail0 - ZeroHail1) and (VelocityStone - (VelocityHail0 - VelocityHail1)) are the same vector
        // which means that the coordinates of one are the same as the others multiplied by m (with m = s - r see above)
        //
        // Use https://www.wolframalpha.com/widgets/view.jsp?id=54af80f0c43c8717d710f39be0642aaa and the first two hails of example:
        // - (19 - 18) = m * (x - (-2 + 1))
        // - (13 - 19) = m * (y - (1 + 1))
        // - (30 - 22) = m * (z - (-2 + 2))
        // => m = -1, x = -2, y = 8, z = -8
        // OR m =  1, x =  0, y =-4, z =  8
        //
        // soooo....   (ZeroHail0 - ZeroHail1) = m * (VelocityStone - (VelocityHail0 - VelocityHail1))
        //               ZeroHail0 - ZeroHail1 = m * VelocityStone - m * VelocityHail0 + m * VelocityHail1      | + m * VelocityHail0 - m * VelocityHail1
        // m * (VelocityHail0 - VelocityHail1) + ZeroHail0 - ZeroHail1 = m * VelocityStone                      | /m
        // VelocityHail0 - VelocityHail1 + (ZeroHail0 - ZeroHail1) / m = VelocityStone
        //
        // This last equation can only be solved for integer VelocityStone if m is an divisor of (ZeroHail0 - ZeroHail1) (or the negative of these divisors)
        // If we use enough additional hails we should be able to find m pretty quickly

        var hailIndex = 0;
        ICollection<long>? possibleDivisors = null;
        do {
            var hail0 = Input[hailIndex++];
            var hail1 = Input[hailIndex++];

            var possibleDivisorsForX = CalculateDivisors((long)(hail0.ZeroPoint.Get(Point3D.COORDINATE_X) - hail1.ZeroPoint.Get(Point3D.COORDINATE_X)));
            possibleDivisors = possibleDivisors?.Intersect(possibleDivisorsForX).ToArray() ?? possibleDivisorsForX;
            possibleDivisors = possibleDivisors
                .Intersect(CalculateDivisors((long)(hail0.ZeroPoint.Get(Point3D.COORDINATE_Y) - hail1.ZeroPoint.Get(Point3D.COORDINATE_Y)))).ToArray();
            possibleDivisors = possibleDivisors
                .Intersect(CalculateDivisors((long)(hail0.ZeroPoint.Get(Point3D.COORDINATE_Z) - hail1.ZeroPoint.Get(Point3D.COORDINATE_Z)))).ToArray();
        } while (possibleDivisors.Count > 2 && hailIndex + 1 < Input.Length);

        Console.WriteLine("Possible divisors: " + string.Join(", ", possibleDivisors));

        // by testing the algorithm that far, both example and input now return 1 and -1 as results for m; let's see if we can eliminate one or the other later
        // 
        // now we calculate the Velocity Vector for all remaining divisors using this formular (see above):
        // VelocityHail0 - VelocityHail1 + (ZeroHail0 - ZeroHail1) / m = VelocityStone
        var h0 = (Line3D)Input[0];
        var h1 = (Line3D)Input[1];
        var possibleVelocityVectors = possibleDivisors
            .Select(m => new Point3D(
                h0.VelocityCoordinates.X - h1.VelocityCoordinates.X + (h0.ZeroCoordinates.X - h1.ZeroCoordinates.X) / m,
                h0.VelocityCoordinates.Y - h1.VelocityCoordinates.Y + (h0.ZeroCoordinates.Y - h1.ZeroCoordinates.Y) / m,
                h0.VelocityCoordinates.Z - h1.VelocityCoordinates.Z + (h0.ZeroCoordinates.Z - h1.ZeroCoordinates.Z) / m
            )).ToArray();

        Console.WriteLine("Velocity vectors: " + string.Join(", ", possibleVelocityVectors));

        // how do we get to the stone zero vector from here? We could use this         
        // A - B   (VelocityStone * m) = (ZeroHail0 - ZeroHail1) + (VelocityHail * m)
        //           VelocityStone = (ZeroHail0 - ZeroHail1) / m + VelocityHail

        var possibleZeroVectors = possibleDivisors
            .Select((m, i) => new Point3D(
                ((h0.ZeroCoordinates.X - h1.ZeroCoordinates.X) / m) + possibleVelocityVectors[i].X,
                ((h0.ZeroCoordinates.Y - h1.ZeroCoordinates.Y) / m) + possibleVelocityVectors[i].Y,
                ((h0.ZeroCoordinates.Z - h1.ZeroCoordinates.Z) / m) + possibleVelocityVectors[i].Z
            )).ToArray();

        Console.WriteLine("Zero vectors: " + string.Join(", ", possibleZeroVectors));

        var possibleLines = possibleVelocityVectors
            .Select((velocity, i) => new Line3D(possibleZeroVectors[i], velocity))
            .ToList();

        // so we have (possibly) multiple lines, we can probably just fill in hails in this formular and see if they match
        // I    (ZeroStone) + (VelocityStone * s) = (ZeroHail0) + (VelocityHail0 * s)
        //      (ZeroStone) + (VelocityStone * s) - (ZeroHail0) - (VelocityHail0 * s) = 0
        //                ZeroStone - ZeroHail0 + s * (VelocityStone - VelocityHail0) = 0
        //    s * (VelocityStone - VelocityHail0) = ZeroHail0 - ZeroStone
        //                                      s = (ZeroHail0 - ZeroStone) / (VelocityStone - VelocityHail0)

        hailIndex = 0;
        while (possibleLines.Count > 1) {
            var hail = Input[hailIndex++];
            possibleLines.RemoveAll(
                line => {
                    var s = Point3D.coordinates.Select(c => (hail.ZeroPoint.Get(c) - line.ZeroPoint.Get(c)) / (line.VelocityPoint.Get(c) - hail.VelocityPoint.Get(c))).ToArray();
                    if (s.Distinct().Count() > 1) {
                        Console.WriteLine("Remove line " + possibleLines.IndexOf(line) + " because it had different s: " + string.Join(", ", s));
                        return true;
                    }
                    if (s.Distinct().Single() >= 0) {
                        Console.WriteLine("Remove line " + possibleLines.IndexOf(line) + " because it had negative s: " + s.Distinct().Single());
                        return true;
                    }

                    return false;
                }
            );
        }

        return possibleLines.SingleOrDefault();
    }

    private static ICollection<long> CalculateDivisors(long n) {
        if (n <= 0) {
            n = Math.Abs(n);
        }

        var divisors = new List<long> {n};
        for (long i = 1; i <= Math.Sqrt(n); i++) {
            if (n % i == 0) {
                divisors.Add(i);
                divisors.Add(n / i);
            }
        }

        return divisors.Distinct().SelectMany(d => new[] {d, -d}).OrderBy(d => d).ToArray();
    }
}