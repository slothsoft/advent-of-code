package d05

import org.junit.Assert
import org.junit.Test

class HydrothermalVentureTest {

    private val INPUT = arrayOf(
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
        oceanFloor.applyStraightLines(INPUT)
        println(oceanFloor)
        Assert.assertEquals(5, oceanFloor.calculateHotSpots(2))
    }

    @Test
    fun testPuzzle1() {
        val oceanFloor = HydrothermalVenture(1000)
        oceanFloor.applyStraightLines(HydrothermalVentureTest::class.java.getResource("input.txt").readText().split("\n").toTypedArray())
        var result = oceanFloor.calculateHotSpots(2)
        println("Puzzle 1: " + result)
    }
}