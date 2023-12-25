using System;
using System.Collections.Generic;
using System.Linq;

namespace AoC.day24;

/// <summary>
/// <a href="https://adventofcode.com/2023/day/24">Day 24: Never Tell Me The Odds</a>
///
/// Source for the line longersections is https://emedia.rmit.edu.au/learninglab/content/v7-longersecting-lines-3d
/// </summary>
public class NeverTellMeTheOdds {
    // x=x0+at; y=y0+bt; z=z0+ct (the first part is Zero; the second part that multiplies with t is Velocity)
    internal record Line3D(Point3D ZeroCoordinates, Point3D VelocityCoordinates) {
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
    }

    public readonly struct Point3D {
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

        public Point3D Copy() {
            return new Point3D(X, Y, Z);
        }

        public Point3D With(int coordinate, double value) {
            return new Point3D(X, Y, Z) {_coordinates = {[coordinate] = value}};
        }

        public override string ToString() => $"({X}|{Y}|{Z})";

        public bool Equals(Point3D other) => _coordinates.SequenceEqual(other._coordinates);
        public override bool Equals(object? obj) => obj is Point3D other && Equals(other);
        public override int GetHashCode() => _coordinates.Select(n => n.GetHashCode()).Aggregate((a, b) => a * (13 + b));
    }

    private bool _ignoreZ;
    
    public NeverTellMeTheOdds(IEnumerable<string> input, bool ignoreZ = false) {
        _ignoreZ = ignoreZ;
        Input = input
            .Select(ParseLine)
            .Select(line => ignoreZ ? new Line3D(line.ZeroCoordinates with {Z = 0}, line.VelocityCoordinates with {Z = 0}) : line)
            .ToArray();
    }

    internal Line3D[] Input { get; }

    internal static Line3D ParseLine(string input) {
        var split = input.Split(" @ ");
        return new Line3D(ParsePoint(split[0]), ParsePoint(split[1]));
    }

    internal static Point3D ParsePoint(string input) {
        return new Point3D(input.Split(", ").Select(s => double.Parse(s.Trim())).ToArray());
    }

    public long CalculateIntersections(double min, double max) {
        var result = 0L;
        foreach (var input in Input) {
            foreach (var other in Input) {
                if (input != other) {
                    var intersection = input.CalculateIntersection(other);
                    if (intersection == null) {
                        continue; // they don't intersect at all
                    }

                    if (intersection.Value.X >= min && intersection.Value.X <= max
                                                    && intersection.Value.Y >= min && intersection.Value.Y <= max
                                                    && (_ignoreZ || (intersection.Value.Z >= min && intersection.Value.Z <= max))) {
                        // in range!
                        Console.WriteLine($"Found intersection between {Array.IndexOf(Input, input)} and {Array.IndexOf(Input, other)}: {intersection}");
                        result++;
                    }
                }
            }
        }

        return result;
    }
}