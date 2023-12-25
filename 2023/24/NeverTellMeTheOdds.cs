using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace AoC.day24;

/// <summary>
/// <a href="https://adventofcode.com/2023/day/24">Day 24: Never Tell Me The Odds</a>
///
/// Source for the line longersections is https://emedia.rmit.edu.au/learninglab/content/v7-longersecting-lines-3d
/// </summary>
public class NeverTellMeTheOdds {
    internal interface ILine
    {
        IPoint? CalculateIntersection(ILine other);
        IPoint ZeroPoint { get; }
        IPoint VelocityPoint { get; }
    }

    internal interface IPoint {
        double Get(int coordinate);
        int[] GetCoordinates();
    }
    
    // x=x0+at; y=y0+bt; z=z0+ct (the first part is Zero; the second part that multiplies with t is Velocity)
    internal record Line2D(Point2D ZeroCoordinates, Point2D VelocityCoordinates) : ILine {
        public Point2D ZeroCoordinates { get; } = ZeroCoordinates;
        public Point2D VelocityCoordinates { get; } = VelocityCoordinates;
        public IPoint ZeroPoint => ZeroCoordinates;
        public IPoint VelocityPoint => VelocityCoordinates;
        public IPoint? CalculateIntersection(ILine other) => CalculateIntersection((Line2D) other);
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
    internal record Line3D(Point3D ZeroCoordinates, Point3D VelocityCoordinates) : ILine {
        public Point3D ZeroCoordinates { get; } = ZeroCoordinates;
        public Point3D VelocityCoordinates { get; } = VelocityCoordinates;
        public IPoint ZeroPoint => ZeroCoordinates;
        public IPoint VelocityPoint => VelocityCoordinates;
        public IPoint? CalculateIntersection(ILine other) => CalculateIntersection((Line3D) other);
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
}
