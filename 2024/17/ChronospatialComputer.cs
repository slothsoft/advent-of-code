using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;

namespace AoC.day17;

/// <summary>
/// <a href="https://adventofcode.com/2024/day/17">Day 17: Chronospatial Computer</a>
/// </summary>
public class ChronospatialComputer {
    private const int A = 4;
    private const int B = 5;
    private const int C = 6;
    
    private const int MAX = 8;
    public class Register {
        private long[] _register = new long[3];
        private IList<long> _output = new List<long>();

        public long GetComboOperand(int operand) {
            if (operand >= 7 || operand < 0) throw new ArgumentException("Cannot handle operand " + operand);
            if (operand <= 3) return operand;
            return _register[operand - A];
        }
        
        public void SetComboOperand(int operand, long value) {
            if (operand >= 7 || operand < 3) throw new ArgumentException("Cannot handle operand " + operand);
            _register[operand - A] = value;
        }

        public void ChangeComboOperand(int operand, Func<long, long> work) {
            SetComboOperand(operand, work(GetComboOperand(operand)));
        }

        public void AddOutput(long value) => _output.Add(value);
        public void ClearOutput() => _output.Clear();
        public string Output => string.Join(",", _output);
        public int? OverrideInstructionPointer { get; set; }
        public override string ToString() => $"A: {GetComboOperand(A)}, B: {GetComboOperand(B)}, C: {GetComboOperand(C)}";
    }

    internal class Instruction(string name, int opCode, Action<Register, int> work) {
        public int OpCode { get; } = opCode;
        public string Name { get; set; } = name;
        public void Work(Register register, int operand) => work(register, operand);
        public override string ToString() => $"{Name}";
    }

    internal class DivInstruction(string name, int opCode, int targetRegister) : Instruction(name, opCode, (reg, operand) => {
        reg.SetComboOperand(targetRegister, (long) (reg.GetComboOperand(A) / Math.Pow(2, reg.GetComboOperand(operand))));
    });
    
    internal static readonly Instruction Adv = new DivInstruction(nameof(Adv), 0, A);
    internal static readonly Instruction Bxl = new(nameof(Bxl), 1, (reg, operand) => {
        reg.ChangeComboOperand(B, v => v ^ operand);
    });
    internal static readonly Instruction Bst = new(nameof(Bst), 2, (reg, operand) => {
        reg.SetComboOperand(B, reg.GetComboOperand(operand) % MAX);
    });
    internal static readonly Instruction Jnz = new(nameof(Jnz), 3, (reg, operand) => {
        if (reg.GetComboOperand(A) != 0) {
            reg.OverrideInstructionPointer = operand;
        }
    });
    internal static readonly Instruction Bxc = new(nameof(Bxc), 4, (reg, operand) => {
        reg.ChangeComboOperand(B, _ => reg.GetComboOperand(B) ^ reg.GetComboOperand(C));
    });
    internal static readonly Instruction Out = new(nameof(Out), 5, (reg, operand) => {
        reg.AddOutput(reg.GetComboOperand(operand) % MAX);
    });
    internal static readonly Instruction Bdv = new DivInstruction(nameof(Bdv), 6, B);
    internal static readonly Instruction Cdv = new DivInstruction(nameof(Cdv), 7, C);

    internal static readonly Instruction[] AllInstructions = [
        Adv, Bxl, Bst, Jnz, Bxc, Out, Bdv, Cdv,
    ];

    public ChronospatialComputer(IEnumerable<string> input) {
        Assert.AreEqual(MAX, AllInstructions.Length);
        Assert.AreEqual(MAX, AllInstructions.DistinctBy(i => i.OpCode).Count());
        Assert.AreEqual(MAX, AllInstructions.DistinctBy(i => i.Name).Count());

        (Instructions, Operands, InitialRegister) = ParseInput(input);
    }
    
    public ChronospatialComputer(string program, long a = 0, long b = 0, long c = 0) {
        Assert.AreEqual(MAX, AllInstructions.Length);
        Assert.AreEqual(MAX, AllInstructions.DistinctBy(i => i.OpCode).Count());
        Assert.AreEqual(MAX, AllInstructions.DistinctBy(i => i.Name).Count());
        
        (Instructions, Operands) = ParseProgramm(program);
        InitialRegister = [a, b, c];
    }

    internal Instruction[] Instructions { get; }
    internal int[] Operands { get; }
    internal long[] InitialRegister { get; }

    internal static (Instruction[], int[], long[]) ParseInput(IEnumerable<string> input) {
        var inputAsArray = input.ToArray();
        var initialRegister = inputAsArray.Take(3).Select(s => s.ExtractDigitsAsLong()).ToArray();
        var (instructions, operands) = ParseProgramm(inputAsArray[4]);
        return (instructions, operands, initialRegister);
    }
    
    private static (Instruction[], int[]) ParseProgramm(string input) {
        var instructions = new List<Instruction>();
        var operands = new List<int>();
        var instruction = true;
        
        foreach (var number in input.Split(",")) {
            if (instruction) {
                var numberAsInt = number.ExtractDigitsAsInt();
                instructions.Add(AllInstructions.Single(i => i.OpCode == numberAsInt));
            } else {
                operands.Add(number.ExtractDigitsAsInt());
            }
            instruction = !instruction;
        }

        Assert.AreEqual(operands.Count, instructions.Count);
        return (instructions.ToArray(), operands.ToArray());
    }

    public string RunProgramReturnOutput() {
        return RunProgram().Output;
    }

    public Register RunProgram() {
        var register = new Register();
        register.SetComboOperand(A, InitialRegister[0]);
        register.SetComboOperand(B, InitialRegister[1]);
        register.SetComboOperand(C, InitialRegister[2]);
        
        for (var instructionPointer = 0; instructionPointer < Instructions.Length; instructionPointer++) {
            Instructions[instructionPointer].Work(register, Operands[instructionPointer]);

            if (register.OverrideInstructionPointer != null) {
                instructionPointer = register.OverrideInstructionPointer.Value - 1; // loop increments this
                register.OverrideInstructionPointer = null;
            }
        }
        return register;
    }
    
    public long FindAForSolution(string expected) {
        var register = new Register();
        var startValue = 1L;
        
        do {
            register.SetComboOperand(A, startValue);
            register.SetComboOperand(B, 0);
            register.SetComboOperand(C, 0);
            register.ClearOutput();
            
            for (var instructionPointer = 0; instructionPointer < Instructions.Length; instructionPointer++) {
                var instruction = Instructions[instructionPointer];
                instruction.Work(register, Operands[instructionPointer]);

                if (register.OverrideInstructionPointer != null) {
                    instructionPointer = register.OverrideInstructionPointer.Value - 1; // loop increments this
                    register.OverrideInstructionPointer = null;
                }
                if (instruction == Out && !expected.StartsWith(register.Output)) {
                    // break early if the output will never work
                    break; 
                }
            }

            startValue++;
        } while (!register.Output.Equals(expected));
        
        
        return startValue - 1;
    }
}
