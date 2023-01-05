package d14

import org.junit.Assert
import org.junit.Test

class ExtendedPolymerizationTest {

    @Test
    fun testExample1A() {
        val extendedPolymerization = ExtendedPolymerization(readLines("example.txt"))

        Assert.assertEquals("NNCB", extendedPolymerization.getTemplate())

        extendedPolymerization.executeStep()

        Assert.assertEquals("NCNBCHB", extendedPolymerization.getTemplate())

        extendedPolymerization.executeStep()

        Assert.assertEquals("NBCCNBBBCBHCB", extendedPolymerization.getTemplate())

        extendedPolymerization.executeStep()

        Assert.assertEquals("NBBBCNCCNBBNBNBBCHBHHBCHB", extendedPolymerization.getTemplate())

        extendedPolymerization.executeStep()

        Assert.assertEquals("NBBNBNBBCCNBCNCCNBBNBBNBBBNBBNBBCBHCBHHNHCBBCBHCB", extendedPolymerization.getTemplate())

        extendedPolymerization.executeStep()

        Assert.assertEquals(97, extendedPolymerization.getTemplate().length)

        extendedPolymerization.executeSteps(5)

        Assert.assertEquals(1588, extendedPolymerization.getQuantityDifference())
    }

    private fun readLines(fileName: String) : Array<String> {
        return ExtendedPolymerizationTest::class.java.getResource(fileName)!!.readText().split(Regex("\\R")).toTypedArray()
    }

    @Test
    fun testExample1B() {
        val extendedPolymerization = ExtendedPolymerization(readLines("example.txt"))

        Assert.assertEquals("NNCB", extendedPolymerization.getTemplate())

        extendedPolymerization.executeSteps(10)

        Assert.assertEquals(1588, extendedPolymerization.getQuantityDifference())
    }

    @Test
    fun testPuzzle1() {
        val extendedPolymerization = ExtendedPolymerization(readLines("input.txt"))
        extendedPolymerization.executeSteps(10)

        val result = extendedPolymerization.getQuantityDifference()
        println("Puzzle 1: $result")
        Assert.assertEquals(2590, result)
    }

    @Test
    fun testExample2() {
        val extendedPolymerization = ExtendedPolymerization(readLines("example.txt"))

        Assert.assertEquals(getQuantityDifference("NNCB"), extendedPolymerization.calculateQuantityDifference(0))
        Assert.assertEquals(getQuantityDifference("NCNBCHB"), extendedPolymerization.calculateQuantityDifference(1))
        Assert.assertEquals(getQuantityDifference("NBCCNBBBCBHCB"), extendedPolymerization.calculateQuantityDifference(2))
        Assert.assertEquals(getQuantityDifference("NBBBCNCCNBBNBNBBCHBHHBCHB"), extendedPolymerization.calculateQuantityDifference(3))
        Assert.assertEquals(getQuantityDifference("NBBNBNBBCCNBCNCCNBBNBBNBBBNBBNBBCBHCBHHNHCBBCBHCB"), extendedPolymerization.calculateQuantityDifference(4))
        Assert.assertEquals(1588L, extendedPolymerization.calculateQuantityDifference(10))
        Assert.assertEquals(2188189693529L, extendedPolymerization.calculateQuantityDifference(40))
    }

    private fun getQuantityDifference(template: String): Long {
        val extendedPolymerization = ExtendedPolymerization(template)
        return extendedPolymerization.getQuantityDifference().toLong()
    }

    @Test
    fun testPuzzle2() {
        val extendedPolymerization = ExtendedPolymerization(readLines("input.txt"))
        val result = extendedPolymerization.calculateQuantityDifference(40)
        println("Puzzle 2: $result")
        Assert.assertEquals(2875665202438L, result)
    }
}