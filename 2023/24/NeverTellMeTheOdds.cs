using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Numerics;
using AoC.day25;
using NUnit.Framework;

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
        // m = s - r        (ZeroHail0 - ZeroHail1) = m * (VelocityStone - VelocityHail) 
        // /m + VelocityHail          VelocityStone = (ZeroHail0 - ZeroHail1) / m + VelocityHail 
        //
        // This last equation can only be solved for integer VelocityStone if m is an divisor of (ZeroHail0 - ZeroHail1) (or the negative of these divisors)
        // If we use enough additional hails we should be able to find m pretty quickly
        // (m = 0 might also work, but then two hailstorms would intersect with the stone at the same time)
        // => does NOT work, because only ONE coordinate matches
        //
        // C      (ZeroStone) + (VelocityStone * s) = (ZeroHail0) + (VelocityHail0 * s)                                | - (VelocityStone * s) - (ZeroHail0)
        //                (ZeroStone) - (ZeroHail0) = (VelocityHail0 * s) - (VelocityStone * s)      
        //                    ZeroStone - ZeroHail0 = s * (VelocityHail0 - VelocityStone)                              | / (VelocityHail0 - VelocityStone)
        // C(X)                                   s = (ZeroStoneX - ZeroHail0X) / (VelocityHail0X - VelocityStoneX)         
        // C(Y)                                   s = (ZeroStoneY - ZeroHail0Y) / (VelocityHail0Y - VelocityStoneY)           
        // C(X) - C(Y)                            0 = (ZeroStoneX - ZeroHail0X) / (VelocityHail0X - VelocityStoneX) - (ZeroStoneY - ZeroHail0Y) / (VelocityHail0Y - VelocityStoneY)
        //                                        0 = (VelocityHail0Y - VelocityStoneY) * (ZeroStoneX - ZeroHail0X) - (VelocityHail0X - VelocityStoneX) * (ZeroStoneY - ZeroHail0Y)
        // [needed]                                                            X               X                                                X               X  
        //                                        0 = (VelocityHail0Y - VelocityStoneY) * (ZeroStoneX - ZeroHail0X) - (VelocityHail0X - VelocityStoneX) * (ZeroStoneY - ZeroHail0Y)
        //                                        0 = VelocityHail0Y * (ZeroStoneX - ZeroHail0X) - VelocityStoneY * (ZeroStoneX - ZeroHail0X)  - VelocityHail0X * (ZeroStoneY - ZeroHail0Y) + VelocityStoneX * (ZeroStoneY - ZeroHail0Y)
        // WOPRKS                                 0 = VelocityHail0Y * (ZeroStoneX - ZeroHail0X)                - VelocityStoneY * (ZeroStoneX - ZeroHail0X)                  - VelocityHail0X * (ZeroStoneY - ZeroHail0Y)                + VelocityStoneX * (ZeroStoneY - ZeroHail0Y)
        // [needed below]                                                 v                                              v              v               v                                            v                                            v              v             v  
        // E                                      0 = VelocityHail0Y * ZeroStoneX - VelocityHail0Y * ZeroHail0X - VelocityStoneY * ZeroStoneX + VelocityStoneY *  ZeroHail0X  - VelocityHail0X * ZeroStoneY + VelocityHail0X * ZeroHail0Y + VelocityStoneX * ZeroStoneY - VelocityStoneX * ZeroHail0Y
        // F                                      0 = VelocityHail1Y * ZeroStoneX - VelocityHail1Y * ZeroHail1X - VelocityStoneY * ZeroStoneX + VelocityStoneY *  ZeroHail1X  - VelocityHail1X * ZeroStoneY + VelocityHail1X * ZeroHail1Y + VelocityStoneX * ZeroStoneY - VelocityStoneX * ZeroHail1Y
        // E - F                                  0 = ZeroStoneX (VelocityHail0Y - VelocityHail1Y)                               + VelocityStoneY * (ZeroHail0X - ZeroHail1X) - ZeroStoneY * (VelocityHail0X - VelocityHail1X)                           - VelocityStoneX * (ZeroHail0Y - ZeroHail1Y)
        //                                                           - VelocityHail0Y * ZeroHail0X + VelocityHail1Y * ZeroHail1X                                                                 + VelocityHail0X * ZeroHail0Y - VelocityHail1X * ZeroHail1Y             
        //                                                                                      
        // That is... a linear formula with 4 unknown? and we can easily create as many as we need:

        const int four = 4;
        var formulaWith4Unknown = new double[four, 5];
        for (var i = 0; i < four; i++) {
            var col = 0;
            // ZeroStoneX (VelocityHail0Y - VelocityHail1Y)
            formulaWith4Unknown[i, col++] = Input[i].VelocityPoint.Get(Point3D.COORDINATE_Y) - Input[i + 1].VelocityPoint.Get(Point3D.COORDINATE_Y);
            // VelocityStoneY * (ZeroHail0X - ZeroHail1X)
            formulaWith4Unknown[i, col++] = - Input[i].ZeroPoint.Get(Point3D.COORDINATE_X) + Input[i + 1].ZeroPoint.Get(Point3D.COORDINATE_X);
            // - ZeroStoneY * (VelocityHail0X - VelocityHail1X)
            formulaWith4Unknown[i, col++] = - Input[i].VelocityPoint.Get(Point3D.COORDINATE_X) + Input[i + 1].VelocityPoint.Get(Point3D.COORDINATE_X);
            // - VelocityStoneX * (ZeroHail0Y - ZeroHail1Y)
            formulaWith4Unknown[i, col++] = Input[i].ZeroPoint.Get(Point3D.COORDINATE_Y) - Input[i + 1].ZeroPoint.Get(Point3D.COORDINATE_Y);
            // VelocityHail0Y * ZeroHail0X
            formulaWith4Unknown[i, col++] = Input[i].VelocityPoint.Get(Point3D.COORDINATE_Y) * Input[i].ZeroPoint.Get(Point3D.COORDINATE_X)
                                          //  - VelocityHail1Y * ZeroHail1X   
                                          - Input[i + 1].VelocityPoint.Get(Point3D.COORDINATE_Y) * Input[i + 1].ZeroPoint.Get(Point3D.COORDINATE_X)
                                          // - VelocityHail0X * ZeroHail0Y 
                                          - Input[i].VelocityPoint.Get(Point3D.COORDINATE_X) * Input[i].ZeroPoint.Get(Point3D.COORDINATE_Y)
                                          // VelocityHail1X * ZeroHail1Y    
                                          + Input[i + 1].VelocityPoint.Get(Point3D.COORDINATE_X) * Input[i + 1].ZeroPoint.Get(Point3D.COORDINATE_Y);
        }

        var solvable = SolveGaußianElimination(formulaWith4Unknown);
        if (!solvable) {
            Console.WriteLine("Formula for X and Y is not solveable");
            return null;
        }
        
        // so now we have the X and Y coordinates completely solved
        var row = 0;
        var zeroStoneX = Math.Round(formulaWith4Unknown[row++, four]);
        var velocityStoneY = Math.Round(formulaWith4Unknown[row++, four]);
        var zeroStoneY = Math.Round(formulaWith4Unknown[row++, four]);
        var velocityStoneX = Math.Round(formulaWith4Unknown[row++, four]);
        
        // we should now be able to calculate t (s, r, q, ...) pretty easily
        // (ZeroStone) + (VelocityStone * t) = (ZeroHail0) + (VelocityHail * t)        | - (VelocityStone * t) - (ZeroHail0)
        //             ZeroStone - ZeroHail0 = VelocityHail * t - VelocityStone * t    | / (VelocityHail - VelocityStone)
        //                                 t = (ZeroStone - ZeroHail0) / (VelocityHail - VelocityStone)
        var t1 = (zeroStoneX - Input[0].ZeroPoint.Get(Point3D.COORDINATE_X)) / (Input[0].VelocityPoint.Get(Point3D.COORDINATE_X) - velocityStoneX);
        var t2 = (zeroStoneY - Input[0].ZeroPoint.Get(Point3D.COORDINATE_Y)) / (Input[0].VelocityPoint.Get(Point3D.COORDINATE_Y) - velocityStoneY);
        
        // Using the Cross Product (see above and https://en.wikipedia.org/wiki/Cross_product) we might be able to calculate the Y values
        // I b)   0 = - ZeroZ * VelocityY + ZeroY * VelocityZ 
        //        0 =   ZeroZ * VelocityX - ZeroX * VelocityZ
        var formulaWith2Unknown = new double[2, 3];
        // - ZeroZ * VelocityY
        formulaWith2Unknown[0, 0] = -velocityStoneY;
        // ZeroY * VelocityZ
        formulaWith2Unknown[0, 1] = - zeroStoneY;
        // ZeroZ * VelocityX
        formulaWith2Unknown[1, 0] = velocityStoneX;
        // - ZeroX * VelocityZ
        formulaWith2Unknown[1, 1] = zeroStoneX;

        solvable = SolveGaußianElimination(formulaWith2Unknown);
        if (!solvable) {
            Console.WriteLine("Formula for Z is not solveable");
            return null;
        }
        
            
       // Zero: 159153037374407 228139153674672 170451316297300
        // Velocity: -245 -75 -221
        return new Line3D(new Point3D(zeroStoneX, zeroStoneY, 170451316297300), new Point3D(velocityStoneX, velocityStoneY, -221));
    }

    /// <summary>Computes the solution of a linear equation system.
    /// Explanation see https://en.wikipedia.org/wiki/Gaussian_elimination
    /// Code adapted from https://www.codeproject.com/Tips/388179/Linear-Equation-Solver-Gaussian-Elimination-Csharp</summary>
    /// <param name="M">
    /// The system of linear equations as an augmented matrix[row, col] where (rows + 1 == cols).
    /// It will contain the solution in "row canonical form" if the function returns "true".
    /// </param>
    /// <returns>Returns whether the matrix has a unique solution or not.</returns>
    public static bool SolveGaußianElimination(double[,] M)
    {
        // input checks
        int rowCount = M.GetUpperBound(0) + 1;
        if (M == null || M.Length != rowCount * (rowCount + 1))
          throw new ArgumentException("The algorithm must be provided with a (n x n+1) matrix.");
        if (rowCount < 1)
          throw new ArgumentException("The matrix must at least have one row.");

        // pivoting
        for (int col = 0; col + 1 < rowCount; col++) if (M[col, col] == 0)
        // check for zero coefficients
        {
            // find non-zero coefficient
            int swapRow = col + 1;
            for (;swapRow < rowCount; swapRow++) if (M[swapRow, col] != 0) break;

            if (M[swapRow, col] != 0) // found a non-zero coefficient?
            {
                // yes, then swap it with the above
                double[] tmp = new double[rowCount + 1];
                for (int i = 0; i < rowCount + 1; i++)
                  { tmp[i] = M[swapRow, i]; M[swapRow, i] = M[col, i]; M[col, i] = tmp[i]; }
            }
            else return false; // no, then the matrix has no unique solution
        }

        // elimination
        for (int sourceRow = 0; sourceRow + 1 < rowCount; sourceRow++)
        {
            for (int destRow = sourceRow + 1; destRow < rowCount; destRow++)
            {
                double df = M[sourceRow, sourceRow];
                double sf = M[destRow, sourceRow];
                for (int i = 0; i < rowCount + 1; i++)
                  M[destRow, i] = M[destRow, i] * df - M[sourceRow, i] * sf;
            }
        }

        // back-insertion
        for (int row = rowCount - 1; row >= 0; row--)
        {
            double f = M[row,row];
            if (f == 0) return false;

            for (int i = 0; i < rowCount + 1; i++) M[row, i] /= f;
            for (int destRow = 0; destRow < row; destRow++)
              { M[destRow, rowCount] -= M[destRow, row] * M[row, rowCount]; M[destRow, row] = 0; }
        }
        return true;
    }
}