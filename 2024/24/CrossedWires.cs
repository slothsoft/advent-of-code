using System;
using System.Collections.Generic;
using System.Linq;

namespace AoC.day24;

/// <summary>
/// <a href="https://adventofcode.com/2024/day/24">Day 24: Crossed Wires</a>
/// </summary>
public class CrossedWires {
    public interface IComponent {
        public string OutputName { get; }
        public bool CalculateOutputValue(IDictionary<string, IComponent> context);
    }

    internal record Wire(string Name, bool Value) : IComponent {
        public string OutputName => Name;
        public bool CalculateOutputValue(IDictionary<string, IComponent> _) => Value;
    }

    internal record Gate(string Input1, string Input2, string OutputName, string OperationName, Func<bool, bool, bool> Operation) : IComponent {
        public static Gate And(string input1, string input2, string outputName) => new(input1, input2, outputName, "AND", (i1, i2) => i1 && i2);
        public static Gate Or(string input1, string input2, string outputName) => new(input1, input2, outputName, "OR", (i1, i2) => i1 || i2);
        public static Gate Xor(string input1, string input2, string outputName) => new(input1, input2, outputName, "XOR", (i1, i2) => i1 != i2);

        public bool CalculateOutputValue(IDictionary<string, IComponent> context) {
            var result = Operation(context[Input1].CalculateOutputValue(context), context[Input2].CalculateOutputValue(context));
            context[OutputName] = new Wire(OutputName, result);
            return result;
        }
    }

    public CrossedWires(IEnumerable<string> input) {
        Input = ParseInput(input);
    }

    internal IComponent[] Input { get; }

    private string[]? _xNames;

    internal string[] XNames {
        get {
            if (_xNames == null) {
                _xNames = Input.ToBitArray('x');
            }

            return _xNames;
        }
    }

    private string[]? _yNames;

    internal string[] YNames {
        get {
            if (_yNames == null) {
                _yNames = Input.ToBitArray('y');
            }

            return _yNames;
        }
    }

    private string[]? _zNames;

    internal string[] ZNames {
        get {
            if (_zNames == null) {
                _zNames = Input.ToBitArray('z');
            }

            return _zNames;
        }
    }

    internal static IComponent[] ParseInput(IEnumerable<string> input) {
        return input.Where(s => !string.IsNullOrWhiteSpace(s)).Select<string, IComponent>(s => {
            if (s.Contains(':')) {
                // this is a wire
                var wireSplit = s.Split(": ");
                return new Wire(wireSplit[0], wireSplit[1] == "1");
            }

            // this is a gate
            var gateSplit = s.Split(" ");
            return gateSplit[1] switch {
                "AND" => Gate.And(gateSplit[0], gateSplit[2], gateSplit[4]),
                "OR" => Gate.Or(gateSplit[0], gateSplit[2], gateSplit[4]),
                "XOR" => Gate.Xor(gateSplit[0], gateSplit[2], gateSplit[4]),
                _ => throw new Exception($"Could not parse operator {gateSplit[1]}")
            };
        }).ToArray();
    }

    public IDictionary<string, bool> CalculateOutputValues(char prefix = 'z') {
        var context = Input.ToDictionary(i => i.OutputName, i => i);
        return context.Where(kv => kv.Key.StartsWith(prefix)).ToDictionary(
            kv => kv.Key,
            kv => kv.Value.CalculateOutputValue(context));
    }

    public string CalculateBinaryNumber(char prefix = 'z') {
        return string.Join(
            string.Empty,
            CalculateOutputValues(prefix).OrderByDescending(kv => kv.Key).Select(kv => kv.Value ? "1" : "0")
        );
    }

    public long CalculateNumber(char prefix = 'z') {
        return Convert.ToInt64(CalculateBinaryNumber(prefix), 2);
    }

    /*
     * PART II - find broken gates
     * ===========================
     * See https://www.geeksforgeeks.org/binary-adder-with-logic-gates/
     *
     * Input: X, Y
     * Output: Sum, Carry (if both are 1, the next bit has to increase; Übertrag)
     * -> Carry itself becomes the 3rd input for the next bit
     *
     * This adds x and y
     * x XOR y = sum
     * x AND y = carry
     *
     * Then we need to add the resulting sum to the carry of the last bit, which uses the very same logic
     * lastCarry XOR sum = entireSum
     * lastCarry AND sum = carryCarry
     *
     * Now we need to add the potentially carryCarry to the carry to decide if the carry actually carries over to the next bit
     * (both can never be true at the same time, since carry is only true if x and y are 1, but then the sum is 0, so the carryCarry can never be null)
     * carry OR carryCarry = nextCarry
     *
     * if we cannot identify these gates, something is broken. If we can identify them, we can test the gate for all inputs of x, y, lastCarry
     */

    internal record AddGroup(int Bit, string XName, string YName) {
        internal string? SumName { get; set; } // x XOR y
        internal string? CarryName { get; set; } // x AND y 

        internal string? LastCarryName { get; set; } // input 
        internal string? EntireSumName { get; set; } // lastCarry XOR sum
        internal string? CarryCarryName { get; set; } // lastCarry AND sum

        internal string? NextCarryName { get; set; } // carry OR carryCarry

        public override string ToString() => $"{nameof(XName)} {XName} XOR {nameof(YName)} {YName} == {nameof(SumName)} {SumName}\n\t" +
                                             $"{nameof(XName)} {XName} AND {nameof(YName)} {YName} == {nameof(CarryName)} {CarryName}\n\t" +
                                             $"{nameof(LastCarryName)} {LastCarryName} XOR {nameof(SumName)} {SumName} == {nameof(EntireSumName)} {EntireSumName}\n\t" +
                                             $"{nameof(LastCarryName)} {LastCarryName} AND {nameof(SumName)} {SumName} == {nameof(CarryCarryName)} {CarryCarryName}\n\t" +
                                             $"{nameof(CarryName)} {CarryName} OR {nameof(CarryCarryName)} {CarryCarryName} == {nameof(NextCarryName)} {NextCarryName}";
    }

    internal IEnumerable<(AddGroup Group, string Gate, string Error)> CheckAddGroups() {
        if (XNames.Length != YNames.Length) throw new Exception("X and Y should have the same bits");

        string? lastCarryName = null;

        for (var bit = XNames.Length - 1; bit >= 0; bit--) {
            var result = new AddGroup(XNames.Length - 1 - bit, XNames[bit], YNames[bit]) {LastCarryName = lastCarryName, EntireSumName = ZNames[bit + 1],};
            // Console.WriteLine(XNames[bit] +"  + "+ YNames[bit] + " = " + ZNames[bit]);
            // only two gates have x and y as input - one XOR and one AND
            var allXYGates = Input.WhereGateHasOnlyInput(result.XName, result.YName).ToArray();
            result.SumName = allXYGates.WhereXorGate().Single().OutputName;
            result.CarryName = allXYGates.WhereAndGate().Single().OutputName;

            // bit 0 doesn't need the carry part  
            if (result.Bit == 0) {
                result.NextCarryName ??= result.CarryName;
                result.EntireSumName ??= result.SumName;
                lastCarryName = result.NextCarryName;
                continue;
            }

            // there should only be one gate leading to EntireSumName - it's inputs are LastCarry XOR Sum
            var entireSumGate = Input.OfType<Gate>().Single(g => g.OutputName == result.EntireSumName);
            if (entireSumGate.OperationName != "XOR") {
                yield return (result, result.EntireSumName, $"EntireSum gate has the wrong operation type: {entireSumGate.OperationName} (XOR expected)");
            }

            // the same two  inputs as for EntireSumName are used for lastCarry AND sum, and create CarryCarryName
            // try one: find correct AND gate
            // try two: try anything else that is just not the entireSumGate
            var carryCarryGate = Input.WhereAndGate()
                .WhereGateHasOnlyInput(entireSumGate.Input1, entireSumGate.Input2)
                .SingleOrDefault();
            carryCarryGate ??= Input
                .WhereGateHasOnlyInput(entireSumGate.Input1, entireSumGate.Input2)
                .SingleOrDefault(g => g.OutputName != entireSumGate.OutputName);
            if (carryCarryGate != null && carryCarryGate.OperationName != "AND") {
                yield return (result, carryCarryGate.OutputName, $"CarryCarry gate has the wrong operation type: {carryCarryGate.OperationName} (AND expected)");
            }
            result.CarryCarryName = carryCarryGate?.OutputName;

            // in the case the approach from the result back doesn't work, try it for the inputs forwards
            var allSumLastCarryGates = Input.WhereGateHasAnyInput(result.SumName ?? string.Empty, lastCarryName ?? string.Empty).ToArray();
            var otherEntireSumGate = allSumLastCarryGates.WhereXorGate().SingleOrDefault();
            var otherEntireSumName = otherEntireSumGate?.OutputName;
            if (result.EntireSumName == null || otherEntireSumName == null) {
                result.EntireSumName ??= otherEntireSumName;
            } else {
                if (result.EntireSumName != otherEntireSumName) {
                    yield return (result, otherEntireSumName, $"Found different definition for EntireSumName");
                }
            }

            result.LastCarryName ??= otherEntireSumGate?.Input1 == result.SumName ? otherEntireSumGate?.Input2 : otherEntireSumGate?.Input1;

            var otherCarryCarryName = allSumLastCarryGates.WhereAndGate().SingleOrDefault()?.OutputName;
            if (result.CarryCarryName == null || otherCarryCarryName == null) {
                result.CarryCarryName ??= otherCarryCarryName;
            } else {
                if (result.CarryCarryName != otherCarryCarryName) {
                    yield return (result, otherCarryCarryName, $"Found different definitions for CarryCarryName");
                }
            }

            if (otherEntireSumGate != null) {
                var lastCarryGates = allSumLastCarryGates.Where(g => g != otherEntireSumGate).ToArray();
                if (lastCarryGates.Length != 1) {
                    yield return (result, string.Join(",", lastCarryGates.Select(g => g.OutputName)), $"Could not find any gate for LastCarry");
                } else if (lastCarryGates[0].OperationName != "AND") {
                    result.LastCarryName = lastCarryGates[0].OutputName;
                    yield return (result, lastCarryGates[0].OutputName,
                        $"LastCarry gate has the wrong operation type: {lastCarryGates[0].OperationName} (AND expected)");
                } else {
                    result.LastCarryName = lastCarryGates[0].OutputName;
                }
            }

            var nextCarryGate = Input.WhereGateHasOnlyInput(result.CarryName ?? string.Empty, result.CarryCarryName ?? string.Empty).SingleOrDefault();
            if (nextCarryGate != null && nextCarryGate.OperationName != "OR") {
                result.NextCarryName = nextCarryGate.OutputName;
                yield return (result, nextCarryGate.OutputName,
                    $"NextCarryName gate has the wrong operation type: {nextCarryGate.OperationName} (OR expected)");
            } else {
                result.NextCarryName = nextCarryGate?.OutputName;
            }

            if (result.CarryCarryName == null) {
                yield return (result, "???", $"CarryCarry could not be found");
            }

            if (result.Bit == 0) {
                // bit 0 doesn't need the carry part
                result.NextCarryName ??= result.CarryName;
            }

            lastCarryName = result.NextCarryName;
            // yield return (result, "???", $"...");    
        }
    }

    internal string CheckGates() {
        var result = new List<string>();
        foreach (var group in CheckAddGroups().GroupBy(g => g.Group)) {
            foreach (var (_, gate, error) in group) {
                Console.WriteLine($"[{group.Key.Bit}.{gate}]: {error}");
                result.Add(gate);
            }

            Console.WriteLine($"\t{group.Key}");
        }

        return string.Join(",", result.Order()); // no idea why kcp is still there
    }

    private IEnumerable<string> SimulateAllInputsAndOutputs(AddGroup result) {
        var messages = new[] {
            SimulateInputAndOutput(result, x: false, y: false, carry: false, outputSum: false, outputCarry: false),
            SimulateInputAndOutput(result, x: false, y: false, carry: true, outputSum: true, outputCarry: false),
            SimulateInputAndOutput(result, x: false, y: true, carry: false, outputSum: true, outputCarry: false),
            SimulateInputAndOutput(result, x: false, y: true, carry: true, outputSum: false, outputCarry: true),
            SimulateInputAndOutput(result, x: true, y: false, carry: false, outputSum: true, outputCarry: false),
            SimulateInputAndOutput(result, x: true, y: false, carry: true, outputSum: false, outputCarry: true),
            SimulateInputAndOutput(result, x: true, y: true, carry: false, outputSum: false, outputCarry: true),
            SimulateInputAndOutput(result, x: true, y: true, carry: true, outputSum: true, outputCarry: true),
        };
        return messages.OfType<string>();
    }

    private string? SimulateInputAndOutput(AddGroup result, bool x, bool y, bool carry, bool outputSum, bool outputCarry) {
        if (carry && result.Bit == 0) {
            // the first bit doesn't have a carry
            return null;
        }

        if (!carry && result.LastCarryName == null) {
            // if LastCarryName couldn't be found we cannot use it
            return null;
        }

        var context = Input.ToDictionary(c => c.OutputName, c => c);
        context[result.XName] = new Wire(result.XName, x);
        context[result.YName] = new Wire(result.YName, y);
        if (result.LastCarryName != null) {
            context[result.LastCarryName] = new Wire(result.LastCarryName, carry);
        }

        bool actualSum;
        bool actualCarry;
        if (result.Bit > 0) {
            actualSum = context[result.EntireSumName!].CalculateOutputValue(context);
            actualCarry = context[result.NextCarryName!].CalculateOutputValue(context);
        } else {
            actualSum = context[result.SumName!].CalculateOutputValue(context);
            actualCarry = context[result.CarryName!].CalculateOutputValue(context);
        }

        if (actualSum != outputSum || actualCarry != outputCarry) {
            return
                $"Add didn't work correctly:\n\t\tInput x={x}, y={y}, carry={carry}\n\t\tOutput:\n\t\t\texpected sum={outputSum}, carry={outputCarry}\n\t\t\tactual sum={actualSum}, carry={actualCarry}";
        }

        return null;
    }
}

public static class CrossedWiresExtensions {
    public static string[] ToBitArray(this IEnumerable<CrossedWires.IComponent> components, char prefix) {
        return components.Select(i => i.OutputName).Where(n => n.StartsWith(prefix)).OrderDescending().ToArray();
    }

    internal static IEnumerable<CrossedWires.Gate> WhereAndGate(this IEnumerable<CrossedWires.IComponent> components) => components.WhereGate("AND");
    internal static IEnumerable<CrossedWires.Gate> WhereOrGate(this IEnumerable<CrossedWires.IComponent> components) => components.WhereGate("OR");
    internal static IEnumerable<CrossedWires.Gate> WhereXorGate(this IEnumerable<CrossedWires.IComponent> components) => components.WhereGate("XOR");

    internal static IEnumerable<CrossedWires.Gate> WhereGate(this IEnumerable<CrossedWires.IComponent> components, string operationName) {
        return components.OfType<CrossedWires.Gate>().Where(g => g.OperationName == operationName);
    }

    internal static IEnumerable<CrossedWires.Gate> WhereGateHasOnlyInput(this IEnumerable<CrossedWires.IComponent> components, params string[] inputNames) {
        return components.OfType<CrossedWires.Gate>().Where(g => inputNames.Contains(g.Input1) && inputNames.Contains(g.Input2));
    }

    internal static IEnumerable<CrossedWires.Gate> WhereGateHasAnyInput(this IEnumerable<CrossedWires.IComponent> components, params string[] inputNames) {
        return components.OfType<CrossedWires.Gate>().Where(g => inputNames.Contains(g.Input1) || inputNames.Contains(g.Input2));
    }
}