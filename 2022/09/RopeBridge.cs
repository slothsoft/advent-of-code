using System;
using System.Collections.Generic;

namespace AoC._09;

/// <summary>
/// <a href="https://adventofcode.com/2022/day/9">Day 9: Rope Bridge</a>: Consider a rope with a knot at each end;
/// these knots mark the head and the tail of the rope. If the head moves far enough away from the tail, the
/// tail is pulled toward the head.
/// </summary>
public class RopeBridge {

    public Point HeadPosition { get; private set; } = new();
    public Point TailPosition { get; private set; } = new();
    public int VisitedTailPositionsCount => _visitedTailPositions.Count;

    private HashSet<Point> _visitedTailPositions = new();

    public RopeBridge() {
        _visitedTailPositions.Add(new Point(TailPosition.X, TailPosition.Y));
    }
    
    public void MoveRight(int steps) {
        for (var i = 0; i < steps; i++) {
            MoveRight();
        }
    }
    
    public void MoveRight() {
        HeadPosition.X++;
        AdjustTail();
        _visitedTailPositions.Add(new Point(TailPosition.X, TailPosition.Y));
    }

    private void AdjustTail() {
        if (Math.Abs(HeadPosition.X - TailPosition.X) <= 1 && Math.Abs(HeadPosition.Y - TailPosition.Y) <= 1) {
            // head and tail are still close enough
            return;
        }
        if (HeadPosition.X == TailPosition.X && HeadPosition.Y != TailPosition.Y) {
            // head and tail above each other
            TailPosition.Y += (HeadPosition.Y - TailPosition.Y) / 2;
            return;
        }
        if (HeadPosition.Y == TailPosition.Y && HeadPosition.X != TailPosition.X) {
            // head and tail right next to each other
            TailPosition.X += (HeadPosition.X - TailPosition.X) / 2;
            return;
        }
        // head and tail are in "knight position"
        if (Math.Abs(HeadPosition.X - TailPosition.X) <= 1) {
            TailPosition.X = HeadPosition.X;
        } else {
            TailPosition.X = (int) Math.Round((HeadPosition.X + TailPosition.X) / 2.0);
        }
        if (Math.Abs(HeadPosition.Y - TailPosition.Y) <= 1) {
            TailPosition.Y = HeadPosition.Y;
        } else {
            TailPosition.Y = (int) Math.Round((HeadPosition.Y + TailPosition.Y) / 2.0);
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
        _visitedTailPositions.Add(new Point(TailPosition.X, TailPosition.Y));
    }

    public void MoveLeft(int steps) {
        for (var i = 0; i < steps; i++) {
            MoveLeft();
        }
    }
    
    public void MoveLeft() {
        HeadPosition.X--;
        AdjustTail();
        _visitedTailPositions.Add(new Point(TailPosition.X, TailPosition.Y));
    }
    
    public void MoveDown(int steps) {
        for (var i = 0; i < steps; i++) {
            MoveDown();
        }
    }

    public void MoveDown() {
        HeadPosition.Y++;
        AdjustTail();
        _visitedTailPositions.Add(new Point(TailPosition.X, TailPosition.Y));
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