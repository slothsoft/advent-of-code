using System.Collections.Generic;
using System.IO;
using NUnit.Framework;
using Broadcaster = AoC.day20.PulsePropagation.BroadcasterModule;
using Conjunction = AoC.day20.PulsePropagation.ConjunctionModule;
using FlipFlop = AoC.day20.PulsePropagation.FlipFlopModule;
using IModule = AoC.day20.PulsePropagation.IModule;
using Signal = AoC.day20.PulsePropagation.Signal;

namespace AoC.day20;

public class PulsePropagationTest {
    internal record TestModule(string DisplayName, params string[] TargetModules) : IModule {
        public Signal LastSignal { get; set; } = Signal.Low;
        public string[] TargetModules { get; set; } = TargetModules;

        public void ReceiveSignal(PulsePropagation.ModuleContext context, Signal inputSignal) {
            LastSignal = inputSignal;
        }
    }

    private const string MODULE_A = "a";
    private const string MODULE_B = "b";
    private const string MODULE_C = "c";

    private IDictionary<string, IModule> _models = new Dictionary<string, IModule>();
    private PulsePropagation.ModuleContext _context = new(new Dictionary<string, IModule>());

    [SetUp]
    public void SetUp() {
        _models = new Dictionary<string, IModule>();
        _context = new PulsePropagation.ModuleContext(_models);
        
        _models.Add(MODULE_A, new TestModule(MODULE_A));
        _models.Add(MODULE_B, new TestModule(MODULE_B));
        _models.Add(MODULE_C, new TestModule(MODULE_C));
    }

    [Test]
    [TestCase(Signal.Low, Signal.Low)]
    [TestCase(Signal.High, Signal.High)]
    public void Example1_Broadcaster(Signal inputSignal, Signal outputSignal) {
        var module = new Broadcaster("BREADCASTER", MODULE_A, MODULE_B);
        module.ReceiveSignal(_context, inputSignal);

        Assert.AreEqual(outputSignal, module.LastSignal);
        
        // this module moves the signal to the targets
        _context.ExecuteSignalPipeline();
        Assert.AreEqual(outputSignal, _models[MODULE_A].LastSignal);
        Assert.AreEqual(outputSignal, _models[MODULE_B].LastSignal);
    }

    [Test]
    [TestCase(new Signal[0], Signal.Low)]
    [TestCase(new[] {Signal.Low}, Signal.High)]
    [TestCase(new[] {Signal.High}, Signal.Low)]
    [TestCase(new[] {Signal.Low, Signal.Low}, Signal.Low)]
    [TestCase(new[] {Signal.Low, Signal.High}, Signal.High)]
    [TestCase(new[] {Signal.High, Signal.Low}, Signal.High)]
    [TestCase(new[] {Signal.High, Signal.High}, Signal.Low)]
    public void Example1_FlipFlop(Signal[] inputSignals, Signal outputSignal) {
        var module = new FlipFlop("CROC", MODULE_C);
        foreach (var inputSignal in inputSignals) {
            module.ReceiveSignal(_context, inputSignal);
        }

        Assert.AreEqual(outputSignal, module.LastSignal);
        
        // this module moves the signal to the targets
        _context.ExecuteSignalPipeline();
        Assert.AreEqual(outputSignal, _models[MODULE_C].LastSignal);
    }

    [Test]
    [TestCase(Signal.None, Signal.None, Signal.High)]
    [TestCase(Signal.Low, Signal.Low, Signal.High)]
    [TestCase(Signal.High, Signal.Low, Signal.High)]
    [TestCase(Signal.Low, Signal.High, Signal.High)]
    [TestCase(Signal.High, Signal.High, Signal.Low)]
    public void Example1_Conjunction(Signal inputSignalA, Signal inputSignalB, Signal outputSignal) {
        var moduleName = "NAND";

        var testModelA = GetTestModel(MODULE_A);
        testModelA.TargetModules = new[] {moduleName};
        if (inputSignalA != Signal.None) {
            testModelA.LastSignal = inputSignalA;
        }

        var testModelB = GetTestModel(MODULE_B);
        testModelB.TargetModules = new[] {moduleName};
        if (inputSignalB != Signal.None) {
            testModelB.LastSignal = inputSignalB;
        }

        var module = new Conjunction(moduleName, MODULE_C);
        module.ReceiveSignal(_context, Signal.None);

        Assert.AreEqual(outputSignal, module.LastSignal);
        
        // this module moves the signal to the targets
        _context.ExecuteSignalPipeline();
        Assert.AreEqual(outputSignal, _models[MODULE_C].LastSignal);
    }

    private TestModule GetTestModel(string name) {
        return (TestModule)_models[name];
    }

    [Test]
    public void Example1A_PushButton() {
        var example = new PulsePropagation(File.ReadAllLines(@"20\example1.txt"));

        var result = example.PushButton();
        Assert.AreEqual(8, result.LowSignalsSend);
        Assert.AreEqual(4, result.HighSignalsSend);
    }
    
    [Test]
    public void Example1A_Often1() {
        var example = new PulsePropagation(File.ReadAllLines(@"20\example1.txt"));

        Assert.AreEqual(32, example.PushButtonOften(1));
    }
    
    [Test]
    public void Example1A() {
        var example = new PulsePropagation(File.ReadAllLines(@"20\example1.txt"));

        Assert.AreEqual(32_000_000, example.PushButtonOften(1000));
    }
    
    [Test]
    public void Example1B() {
        var example = new PulsePropagation(File.ReadAllLines(@"20\example2.txt"));
        example.Modules.Add("output", new TestModule("output"));

        Assert.AreEqual(11_687_500, example.PushButtonOften(1000));
    }

    [Test]
    public void Puzzle1() {
        var puzzle = new PulsePropagation(File.ReadAllLines(@"20\input.txt"));

        Assert.AreEqual(788_081_152, puzzle.PushButtonOften(1000));
    }

    [Test]
    public void Puzzle2() {
        var puzzle = new PulsePropagation(File.ReadAllLines(@"20\input.txt"));

        Assert.AreEqual(788_081_152, puzzle.PushButtonUntilSand());
    }
}