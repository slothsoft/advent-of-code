using System;
using System.Collections.Generic;
using System.Linq;

namespace AoC.day20;

/// <summary>
/// <a href="https://adventofcode.com/2023/day/20">Day 20: Pulse Propagation</a>
/// </summary>
public class PulsePropagation {
    public interface IModule {
        string DisplayName { get; }
        string[] TargetModules { get; }
        Signal LastSignal { get; }

        void ReceiveSignal(ModuleContext context, Signal inputSignal);
    }

    public record BroadcasterModule(string DisplayName, params string[] TargetModules) : IModule {
        public Signal LastSignal { get; private set; } = Signal.Low;

        public void ReceiveSignal(ModuleContext context, Signal inputSignal) {
            LastSignal = inputSignal;
            context.SendSignal(TargetModules, LastSignal);
        }
    }

    public record FlipFlopModule(string DisplayName, params string[] TargetModules) : IModule {
        public Signal LastSignal { get; private set; } = Signal.Low;

        public void ReceiveSignal(ModuleContext context, Signal inputSignal) {
            if (inputSignal == Signal.Low) {
                LastSignal = LastSignal == Signal.High ? Signal.Low : Signal.High;
                context.SendSignal(TargetModules, LastSignal);
            }
        }
    }

    public record ConjunctionModule(string DisplayName, params string[] TargetModules) : IModule {
        public Signal LastSignal { get; set; } = Signal.Low;
        public bool WasEverHigh { get; private set; }

        public void ReceiveSignal(ModuleContext context, Signal inputSignal) {
            LastSignal = Signal.Low;
            foreach (var model in context.FetchInputFor(DisplayName)) {
                if (model.LastSignal == Signal.Low) {
                    LastSignal = Signal.High;
                    WasEverHigh = true;
                    break;
                }
            }

            context.SendSignal(TargetModules, LastSignal);
        }
    }

    internal record RxModule : IModule {
        public string DisplayName => MODULE_RX;
        public Signal LastSignal { get; private set; } = Signal.None;
        public string[] TargetModules { get; } = Array.Empty<string>();

        public void ReceiveSignal(ModuleContext context, Signal inputSignal) {
            if (inputSignal == Signal.Low) {
                LastSignal = inputSignal;
            }
        }
    }
    
    public enum Signal {
        None,
        Low,
        High,
    }

    public record ModuleContext(IDictionary<string, IModule> Modules) {
        private readonly IList<Action> _signalPipeline = new List<Action>();

        public long LowSignalsSend { get; private set; }
        public long HighSignalsSend { get; private set; }

        public void SendSignal(string[] targetModules, Signal inputSignal) {
            foreach (var targetModule in targetModules) {
                SendSignal(targetModule, inputSignal);
            }
        }

        public void SendSignal(string targetModule, Signal inputSignal) {
            _signalPipeline.Add(() => {
                Modules[targetModule].ReceiveSignal(this, inputSignal);
            });

            if (inputSignal == Signal.Low) {
                LowSignalsSend++;
            }

            if (inputSignal == Signal.High) {
                HighSignalsSend++;
            }
        }

        public IEnumerable<IModule> FetchInputFor(string displayName) {
            return Modules.Select(m => m.Value).Where(m => m.TargetModules.Contains(displayName));
        }

        public void ExecuteSignalPipeline() {
            while (_signalPipeline.Count > 0) {
                var signalSender = _signalPipeline.First();
                _signalPipeline.RemoveAt(0);
                signalSender();
            }
        }
    }

    private const string MODULE_BROADCASTER = "broadcaster";
    private const string MODULE_RX = "rx";

    public PulsePropagation(IEnumerable<string> input) {
        Modules = ParseInput(input);
        Modules.Add(MODULE_RX, new RxModule());
    }

    internal IDictionary<string, IModule> Modules { get; }

    internal static IDictionary<string, IModule> ParseInput(IEnumerable<string> input) {
        return input.Select(ParseInputLine).ToDictionary(m => m.DisplayName, m => m);
    }

    private static IModule ParseInputLine(string input) {
        var split = input.Split(" -> ");
        var targetModules = split[1].Split(", ");

        if (split[0].Equals(MODULE_BROADCASTER)) {
            return new BroadcasterModule(split[0], targetModules);
        }

        switch (split[0][0]) {
            case '%':
                return new FlipFlopModule(split[0][1..], targetModules);
            case '&':
                return new ConjunctionModule(split[0][1..], targetModules);
            default:
                throw new ArgumentException($"Cannot handle parsing of module {split[0]}");
        }
    }

    public (long LowSignalsSend, long HighSignalsSend) PushButton() {
        var context = new ModuleContext(Modules);
        context.SendSignal(MODULE_BROADCASTER, Signal.Low);
        context.ExecuteSignalPipeline();
        return (context.LowSignalsSend, context.HighSignalsSend);
    }

    public long PushButtonOften(int times) {
        var (lowSignalsSend, highSignalsSend) = (0L, 0L);
        for (var i = 0; i < times; i++) {
            var result = PushButton();
            lowSignalsSend += result.LowSignalsSend;
            highSignalsSend += result.HighSignalsSend;
        }

        return lowSignalsSend * highSignalsSend;
    }

    public long PushButtonUntilSand() {
        var context = new ModuleContext(Modules);
        
        // only one module signals to the RX module
        var feedingIntoRx = context.FetchInputFor(MODULE_RX).OfType<ConjunctionModule>().Single();

        // multiple modules signal to the above one
        var signaler = context.FetchInputFor(feedingIntoRx.DisplayName).OfType<ConjunctionModule>().ToArray();
        var signalerWithHighLength = new Dictionary<string, long>();
        var buttonsPressed = 0L;
        
        do {
            // push the button
            context.SendSignal(MODULE_BROADCASTER, Signal.Low);
            context.ExecuteSignalPipeline();
            buttonsPressed++;
            
            // now check if any signaler is now High
            foreach (var conjunctionModule in signaler.Where(s => !signalerWithHighLength.ContainsKey(s.DisplayName))) {
                if (conjunctionModule.WasEverHigh) {
                    signalerWithHighLength.Add(conjunctionModule.DisplayName, buttonsPressed);
                }
            }
            
        } while (signalerWithHighLength.Count != signaler.Length);
            
        // now just do the naive thing and calculate the product and hope for the best
        return signalerWithHighLength.Values.Aggregate((a, b) => a * b);
    }
}