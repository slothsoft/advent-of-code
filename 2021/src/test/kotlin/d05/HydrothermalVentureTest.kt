package d05

import org.junit.Assert
import org.junit.Test

class HydrothermalVentureTest {

    private val input = arrayOf(
            "0,9 -> 5,9",
            "8,0 -> 0,8",
            "9,4 -> 3,4",
            "2,2 -> 2,1",
            "7,0 -> 7,4",
            "6,4 -> 2,0",
            "0,9 -> 2,9",
            "3,4 -> 1,4",
            "0,0 -> 8,8",
            "5,5 -> 8,2",
    )

    @Test
    fun testExample1() {
        val oceanFloor = HydrothermalVenture(10)
        oceanFloor.applyStraightLines(input)
        println(oceanFloor)
        Assert.assertEquals(5, oceanFloor.calculateHotSpots(2))
    }

    @Test
    fun testExample1Flipped() {
        val oceanFloor = HydrothermalVenture(10)
        oceanFloor.applyStraightLines(flip(input))
        println(oceanFloor)
        Assert.assertEquals(5, oceanFloor.calculateHotSpots(2))
    }

    private fun flip(input: Array<String>): Array<String> {
        val result = Array(input.size) { _ -> ""}

        for (i in result.indices) {
            val twoPoints = input[i].trim().split(" -> ")
            result[i] = twoPoints[1] + " -> " + twoPoints[0]
        }
        return result
    }

    @Test
    fun testPuzzle1() {
        val oceanFloor = HydrothermalVenture(1000)
        oceanFloor.applyStraightLines(HydrothermalVentureTest::class.java.getResource("input.txt")!!.readText().split("\n").toTypedArray())
        val result = oceanFloor.calculateHotSpots(2)
        println("Puzzle 1: $result")
        Assert.assertEquals(6005, result)
    }

    @Test
    fun testExample2() {
        val oceanFloor = HydrothermalVenture(10)
        oceanFloor.applyLines(input)
        println(oceanFloor)
        Assert.assertEquals(12, oceanFloor.calculateHotSpots(2))
    }

    @Test
    fun testExample2Flipped() {
        val oceanFloor = HydrothermalVenture(10)
        oceanFloor.applyLines(flip(input))
        println(oceanFloor)
        Assert.assertEquals(12, oceanFloor.calculateHotSpots(2))
    }

    @Test
    fun testPuzzle2() {
        val oceanFloor = HydrothermalVenture(1000)
        oceanFloor.applyLines(HydrothermalVentureTest::class.java.getResource("input.txt")!!.readText().split("\n").toTypedArray())
        val result = oceanFloor.calculateHotSpots(2)
        println("Puzzle 2: $result")
        Assert.assertEquals(23864, result)
    }
}