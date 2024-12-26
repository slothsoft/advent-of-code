using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using NUnit.Framework;

namespace AoC.day24;

using System.Text.RegularExpressions;
public class CrossedWiresTest {
    [Test]
    public void Example1A_Values() {
        var example = new CrossedWires(File.ReadAllLines(@"24\example1.txt"));

        var values = example.CalculateOutputValues();
        Assert.AreEqual(false,  values["z00"]);       
        Assert.AreEqual(false,  values["z01"]);     
        Assert.AreEqual(true,  values["z02"]);         
    }
    
    [Test]
    public void Example1A_BinaryNumber() {
        var example = new CrossedWires(File.ReadAllLines(@"24\example1.txt"));

        Assert.AreEqual("100",  example.CalculateBinaryNumber());       
    }
    
    [Test]
    public void Example1A() {
        var example = new CrossedWires(File.ReadAllLines(@"24\example1.txt"));

        Assert.AreEqual(4L,  example.CalculateNumber());       
    }
    
    [Test]
    public void Example1B() {
        var example = new CrossedWires(File.ReadAllLines(@"24\example2.txt"));
        
        Assert.AreEqual(2024L,  example.CalculateNumber());   
    }

    [Test]
    public void Puzzle1() {
        var puzzle = new CrossedWires(File.ReadAllLines(@"24\input.txt"));
        
        Assert.AreEqual(51_410_244_478_064,  puzzle.CalculateNumber());  
    }

    // example 2 won't work because it's not an addition
    
    [Test]
    public void Example2A() {
        var example = new CrossedWires(File.ReadAllLines(@"24\example1.txt"));

        foreach (var checkResult in example.CheckGates()) {
            Console.WriteLine(checkResult);
        }
    }

    [Test]
    public void Example2B() {
        var example = new CrossedWires(File.ReadAllLines(@"24\example2.txt"));

        foreach (var checkResult in example.CheckGates()) {
            Console.WriteLine(checkResult);
        }
    }

    [Test]
    public void Puzzle2() {
        var puzzle = new CrossedWires(File.ReadAllLines(@"24\input.txt"));

        // NOT gst,pps,z12,z20,z21,z33,z44,z45
        Assert.AreEqual("gst,khg,nhn,tvb,vdc,z12,z21,z33",  puzzle.CheckGates());  
        
        /* BIT 11
         * ======
         *
         * x11 XOR y11 -> hhm    Sum
         * y11 AND x11 -> rvg    Carry
         *
         * spg XOR hhm -> z11    EntireSum
         * spg AND hhm -> wvv    CarryCarry
         * spg                   LastCarry
         *
         * rvg OR wvv -> wdg     NextCarry
         * 
         * BIT 12
         * ======
         * 
         * pps XOR wdg -> vdc X  EntireSum, "since x12 XOR y12 -> pps" should be z12
         * x12 AND y12 -> z12 X  only gate writing to z13, so this is the opposite of vdc
         *
         * BIT 20
         * ======
         *
         * x20 XOR y20 -> fht    Sum
         * x20 AND y20 -> fct    Carry
         *
         * fht XOR ddw -> z20    EntireSum
         * ddw AND fht -> qfn    CarryCarry
         * ddw                   LastCarry
         *
         * qfn OR fct -> rsc     NextCarry
         * 
         * BIT 21
         * ======
         * x21 XOR y21 -> bbn    Sum
         * x21 AND y21 -> stq    Carry
         *
         * rsc XOR bbn -> nhn X  EntireSum, should be z21
         * bbn AND rsc -> cdc    CarryCarry
         * rsc                   LastCarry
         *
         * cdc OR stq -> z21  X  NextCarry, should probably be nhn
         * 
         * BIT 22
         * ======
         * 
         * x22 XOR y22 -> jkq    Sum
         * y22 AND x22 -> nrq    Carry
         *
         * jkq XOR nhn -> z22    EntireSum
         * jkq AND nhn -> vkg    CarryCarry
         * nhn                   LastCarry
         * 
         * nrq OR vkg -> mmw     NextCarry, should probably be nhn
         * 
         * BIT 25
         * ======
         * 
         * x25 XOR y25 -> khg    Sum   <    
         * y25 AND x25 -> tvb    Carry < these two are mixed up
         *
         * tvb XOR jhd -> z25    EntireSum
         * tvb AND jhd -> rcb    CarryCarry
         * jhd                   LastCarry
         * 
         * rcb OR khg -> mfp     NextCarry
         *
         * BIT 32
         * ======
         *
         * x32 XOR y32 -> cpt    Sum
         * y32 AND x32 -> rjq    Carry
         *
         * cpt XOR fqc -> z32    EntireSum
         * fqc AND cpt -> nfh    CarryCarry
         * cpt                   LastCarry
         *
         * rjq OR nfh -> wcs     NextCarry
         * 
         * BIT 33
         * ======
         * 
         * x33 XOR y33 -> jbr    Sum
         * x33 AND y33 -> pvc    Carry
         *
         * jbr AND wcs -> z33 X  EntireSum    AND is wrong here, should be XOR
         * wcs XOR jbr -> gst X  CarryCarry   XOR is wrong here, should AND
         * wcs                   LastCarry
         *
         * pvc OR gst -> kmm     NextCarry    
         */
    }

    [Test]
    public void ASdasd() {
    }
}

static class  ASD {
    public static void GetAllValues(this Regex regex, string input) {
    }
}