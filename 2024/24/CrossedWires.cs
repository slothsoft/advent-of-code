using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;

namespace AoC.day24;

/// <summary>
/// <a href="https://adventofcode.com/2024/day/24">Day 24: Crossed Wires</a>
/// </summary>
public class CrossedWires {
    public interface IComponent {
        public string OutputName { get; }
        public bool CalculateOutputValue(IDictionary<string, IComponent> context, bool forceRecalculation = false);
    }

    internal record Wire(string Name, bool Value) : IComponent {
        public string OutputName => Name;
        public bool CalculateOutputValue(IDictionary<string, IComponent> _, bool __) => Value;
    }
    
    internal record Gate(string Input1, string Input2, string OutputName, string OperationName, Func<bool, bool, bool> Operation) : IComponent {
        public static Gate And(string input1, string input2, string outputName) => new Gate(input1, input2, outputName, "AND", (i1, i2) => i1 && i2);
        public static Gate Or(string input1, string input2, string outputName) => new Gate(input1, input2, outputName, "OR", (i1, i2) => i1 || i2);
        public static Gate Xor(string input1, string input2, string outputName) => new Gate(input1, input2, outputName, "XOR", (i1, i2) => i1 != i2);

        private bool? _value;

        public bool CalculateOutputValue(IDictionary<string, IComponent> context, bool forceRecalculation = false) {
            if (_value == null || forceRecalculation) {
                _value = Operation(context[Input1].CalculateOutputValue(context, forceRecalculation), context[Input2].CalculateOutputValue(context, forceRecalculation));
            }
            return _value.Value;
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
    
    internal record AddComponentsResult(int Bit, string XName, string YName) {
        internal string? SumName { get; set; } // x XOR y
        internal string? CarryName { get; set; } // x AND y 
        
        internal string? LastCarryName { get; set; } // input 
        internal string? EntireSumName { get; set; } // lastCarry XOR sum
        internal string? CarryCarryName { get; set; } // lastCarry AND sum
        
        internal string? NextCarryName { get; set; } // carry OR carryCarry
        
        internal string? BrokenMessage { get; set; }
    }

    internal IEnumerable<AddComponentsResult> CheckAndGroups() {
        if (XNames.Length != YNames.Length) throw new Exception("X and Y should have the same bits");

        string? lastCarryName = null;
        for (var bit = XNames.Length - 1; bit >= 0; bit--) {
            var result = new AddComponentsResult(XNames.Length - 1 - bit, XNames[bit], YNames[bit]) {
                LastCarryName = lastCarryName
            };
            try {
                var allXYGates = Input.WhereGateHasOnlyInput(result.XName, result.YName).ToArray();
                result.SumName = allXYGates.WhereXorGate().SingleOrDefault()?.OutputName;
                result.CarryName = allXYGates.WhereAndGate().SingleOrDefault()?.OutputName;

                IComponent[] allSumLastCarryGates;
                if (result.SumName != null && lastCarryName != null) {
                    allSumLastCarryGates = Input.WhereGateHasOnlyInput(result.SumName, lastCarryName).ToArray();
                } else if (result.SumName != null) {
                    // this is the first bit to check
                    allSumLastCarryGates = Input.WhereGateHasAnyInput(result.SumName).ToArray();
                } else if (lastCarryName != null) {
                    // something else went wrong
                    allSumLastCarryGates = Input.WhereGateHasAnyInput(lastCarryName).ToArray();
                } else {
                    // we couldn't find the sum so everything else won't work too
                    allSumLastCarryGates = [];
                }
                result.EntireSumName = allSumLastCarryGates.WhereXorGate().SingleOrDefault()?.OutputName;
                result.CarryCarryName = allSumLastCarryGates.WhereAndGate().SingleOrDefault()?.OutputName;
            
                if (result.CarryName != null && result.CarryCarryName != null) {
                    result.NextCarryName = Input.WhereGateHasOnlyInput(result.CarryName, result.CarryCarryName).SingleOrDefault()?.OutputName;
                }

                if (result.Bit == 0) {
                    // bit 0 doesn't need the carry part
                    result.NextCarryName ??= result.CarryName;
                }
                
                lastCarryName = result.NextCarryName;
            } catch (Exception e) {
                result.BrokenMessage = e.GetType().Name + ": " + e.Message;
            }

            yield return result;
        }
    }

    internal IEnumerable<(int bit, string errorMessage)> CheckGates() {
        return CheckAndGroups().Select(r => {
            var parts = new List<string>();
            if (r.BrokenMessage != null) {
                parts.Add(r.BrokenMessage);
            }

            if (r.SumName == null) {
                parts.Add("Could not find SumName");
            }

            if (r.CarryName == null) {
                parts.Add("Could not find CarryName");
            }

            if (r.Bit > 0) {
                if (r.EntireSumName == null) {
                    parts.Add("Could not find EntireSumName");
                }
                
                if (r.CarryCarryName == null) {
                    parts.Add("Could not find CarryCarryName");
                }
            } 
            
            if (r.NextCarryName == null) {
                parts.Add("Could not find NextCarry");
            }

            if (parts.Count == 0) {
                parts.AddRange(SimulateAllInputsAndOutputs(r));
            }

            // return
            if (parts.Count == 0) {
                return (r.Bit, null);
            }

            return (r.Bit, $"Components for {r.Bit} are not correct:\n\t" + string.Join("\n\t", parts));
        }).Where(r => r.Item2 != null).Select(r => (r.Bit, r.Item2!));
    }

    private IEnumerable<string> SimulateAllInputsAndOutputs(AddComponentsResult result) {
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
    
    private string? SimulateInputAndOutput(AddComponentsResult result, bool x, bool y, bool carry, bool outputSum, bool outputCarry) {
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
            actualSum = context[result.EntireSumName!].CalculateOutputValue(context, true);
            actualCarry = context[result.NextCarryName!].CalculateOutputValue(context, true);
        } else {
            actualSum = context[result.SumName!].CalculateOutputValue(context, true);
            actualCarry = context[result.CarryName!].CalculateOutputValue(context, true);
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

    public static IEnumerable<CrossedWires.IComponent> WhereAndGate(this IEnumerable<CrossedWires.IComponent> components) => components.WhereGate("AND");
    public static IEnumerable<CrossedWires.IComponent> WhereOrGate(this IEnumerable<CrossedWires.IComponent> components) => components.WhereGate("OR");
    public static IEnumerable<CrossedWires.IComponent> WhereXorGate(this IEnumerable<CrossedWires.IComponent> components) => components.WhereGate("XOR");
    public static IEnumerable<CrossedWires.IComponent> WhereGate(this IEnumerable<CrossedWires.IComponent> components, string operationName) {
        return components.OfType<CrossedWires.Gate>().Where(g => g.OperationName == operationName);
    }
    
    public static IEnumerable<CrossedWires.IComponent> WhereGateHasOnlyInput(this IEnumerable<CrossedWires.IComponent> components, params string[] inputNames) {
        return components.OfType<CrossedWires.Gate>().Where(g => inputNames.Contains(g.Input1) && inputNames.Contains(g.Input2));
    }
    
    public static IEnumerable<CrossedWires.IComponent> WhereGateHasAnyInput(this IEnumerable<CrossedWires.IComponent> components, params string[] inputNames) {
        return components.OfType<CrossedWires.Gate>().Where(g => inputNames.Contains(g.Input1) || inputNames.Contains(g.Input2));
    }
}
