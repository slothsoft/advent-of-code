using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace AoC._05;

/// <summary>
/// <a href="https://adventofcode.com/2022/day/5">Day 5: Supply Stacks</a>: The expedition can depart as soon as the
/// final supplies have been unloaded from the ships. Supplies are stored in stacks of marked crates, but because the
/// needed supplies are buried under many other crates, the crates need to be rearranged.
/// </summary>
public class SupplyStacks
{
    private readonly Stack<char>[] _stacks = new Stack<char>[10];

    public SupplyStacks()
    {
        for (var i = 0; i < _stacks.Length; i++)
        {
            _stacks[i] = new Stack<char>();
        }
    }
    
    public bool CraneMover9001 { get; init; }
    
    public string ParseAndExecute(string[] lines)
    {
        var emptyLineIndex = Array.IndexOf(lines, "");
        
        var stacksOfBoxes = new string[emptyLineIndex - 1];
        Array.Copy(lines, 0, stacksOfBoxes, 0, stacksOfBoxes.Length);
        InitStacks(stacksOfBoxes);

        var movementInstructions = new string[lines.Length - emptyLineIndex - 1];
        Array.Copy(lines, emptyLineIndex + 1, movementInstructions, 0, movementInstructions.Length);
        MoveAll(movementInstructions);
        
        return PeekAtTopBoxes();
    }
    
    private void InitStacks(string[] stacksOfBoxes)
    {
        for (int stack = 1; stack <= _stacks.Length; stack++)
        {
            var charIndex = (stack - 1) * 4 + 1;
            if (charIndex >= stacksOfBoxes[0].Length)
                break;
            var boxes = stacksOfBoxes.Take(stacksOfBoxes.Length).Select(s => s[charIndex]).Where(c => c != ' ').ToArray();
            InitStack(stack, boxes);
        }
    }
    
    public void InitStack(int stack,params char[] boxes)
    {
        _stacks[stack].Clear();
        for (var i = boxes.Length - 1; i >= 0; i--)
        {
            _stacks[stack].Push(boxes[i]);
        }
    }

    public void MoveAll(string[] instructions) // move 1 from 2 to 1
    {
        var  regex = new Regex("( from | to )");    
        foreach (var instruction in instructions)
        {
            var numberSplit = regex.Split(instruction[5..]);
            Move(int.Parse(numberSplit[0]), int.Parse(numberSplit[2]), int.Parse(numberSplit[4]));
        }
    }

    public void Move(int steps, int fromStack, int toStack)
    {
        var boxes = new char[steps];
        for (var i = 0; i < steps; i++)
        {
            var boxIndex = CraneMover9001 ? steps - i - 1 : i;
            boxes[boxIndex] = _stacks[fromStack].Pop();
        }

        for (var i = 0; i < steps; i++)
        {
            _stacks[toStack].Push(boxes[i]);
        }
        
    }

    public char[] PeekAtStack(int stack)
    {
        return _stacks[stack].ToArray();
    }

    public string PeekAtTopBoxes()
    {
        var result = "";
        for (var i = 0; i < _stacks.Length; i++)
        {
            if (_stacks[i].Count > 0)
            {
                result += _stacks[i].Peek();
            }
        }
        return result;
    }
}