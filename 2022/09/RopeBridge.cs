using System;
using System.Collections.Generic;

namespace AoC._09;

/// <summary>
/// <a href="https://adventofcode.com/2022/day/9">Day 9: Rope Bridge</a>: Consider a rope with a knot at each end;
/// these knots mark the head and the tail of the rope. If the head moves far enough away from the tail, the
/// tail is pulled toward the head.
/// </summary>
public class RopeBridge {

    public Point HeadPosition { get; } = new();
    public Point TailPosition => _tails[^1].Position;
    public int VisitedTailPositionsCount => _tails[^1].VisitedTailPositionsCount;
    
    private readonly List<Tail> _tails = new();

    public RopeBridge(int tailCount = 1) {
        var previousElement = HeadPosition;
        for (var i = 0; i < tailCount; i++) {
            var newElement = new Tail(previousElement);
            _tails.Add(newElement);
            previousElement = newElement.Position;
        }
    }
    
    public void MoveRight(int steps) {
        for (var i = 0; i < steps; i++) {
            MoveRight();
        }
    }
    
    public void MoveRight() {
        HeadPosition.X++;
        AdjustTail();
    }

    private void AdjustTail() {
        foreach (var tail in _tails) {
            tail.AdjustTail();
        }
    }


    public void MoveUp(int steps) {
        for (var i = 0; i < steps; i++) {
            MoveUp();
        }
    }
    
    public void MoveUp() {
        HeadPosition.Y--;
        AdjustTail();
    }

    public void MoveLeft(int steps) {
        for (var i = 0; i < steps; i++) {
            MoveLeft();
        }
    }
    
    public void MoveLeft() {
        HeadPosition.X--;
        AdjustTail();
    }
    
    public void MoveDown(int steps) {
        for (var i = 0; i < steps; i++) {
            MoveDown();
        }
    }

    public void MoveDown() {
        HeadPosition.Y++;
        AdjustTail();
    }

    public void ExecuteCommands(string[] lines) {
        for (var i = 0; i < lines.Length; i++) {
            var lineSplit = lines[i].Split(" ");
            switch (lineSplit[0][0]) {
                case 'R':
                    MoveRight(int.Parse(lineSplit[1]));
                    break;
                case 'U':
                    MoveUp(int.Parse(lineSplit[1]));
                    break;
                case 'D':
                    MoveDown(int.Parse(lineSplit[1]));
                    break;
                case 'L':
                    MoveLeft(int.Parse(lineSplit[1]));
                    break;
            }
        }
    }

    private class Tail {
        
        internal Point HeadPosition { get; }
        internal Point Position { get; }
        internal int VisitedTailPositionsCount => _visitedTailPositions.Count;
        
        private readonly HashSet<Point> _visitedTailPositions = new();

        public Tail(Point headPosition) {
            HeadPosition = headPosition;
            Position = new Point(headPosition.X, headPosition.Y);
        }

        internal void AdjustTail() {
            DoAdjustTail();
            _visitedTailPositions.Add(new Point(Position.X, Position.Y));
        }

        private void DoAdjustTail() {
            if (Math.Abs(HeadPosition.X - Position.X) <= 1 && Math.Abs(HeadPosition.Y - Position.Y) <= 1) {
                // head and tail are still close enough
                return;
            }
            if (HeadPosition.X == Position.X && HeadPosition.Y != Position.Y) {
                // head and tail above each other
                Position.Y += (HeadPosition.Y - Position.Y) / 2;
                return;
            }
            if (HeadPosition.Y == Position.Y && HeadPosition.X != Position.X) {
                // head and tail right next to each other
                Position.X += (HeadPosition.X - Position.X) / 2;
                return;
            }
            // head and tail are in "knight position"
            if (Math.Abs(HeadPosition.X - Position.X) <= 1) {
                Position.X = HeadPosition.X;
            } else {
                Position.X = (int) Math.Round((HeadPosition.X + Position.X) / 2.0);
            }
            if (Math.Abs(HeadPosition.Y - Position.Y) <= 1) {
                Position.Y = HeadPosition.Y;
            } else {
                Position.Y = (int) Math.Round((HeadPosition.Y + Position.Y) / 2.0);
            }
        }
    }
}

public record Point {
    public Point() : this(0, 0) {
    }
    
    public Point(int x, int y) {
        X = x;
        Y = y;
    }

    public int X { get; set; }
    public int Y { get; set; }
}