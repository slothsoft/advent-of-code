package d05

import kotlin.math.abs

class HydrothermalVenture(initialSize: Int) {

    private val size: Int = initialSize
    private val oceanFloor : Array<IntArray> = Array(size) { IntArray(size) }

    fun applyStraightLines(lines: Array<String>) {
        for (line in lines) {
            if (line.trim().isEmpty()) {
                continue
            }

            val twoPoints = line.trim().split(" -> ")
            val point1 = twoPoints[0].split(",").map { it.toInt() }.toTypedArray()
            val point2 = twoPoints[1].split(",").map { it.toInt() }.toTypedArray()

            if (point1[0] == point2[0] || point1[1] == point2[1]) {
                applyLine(point1, point2)
            }
        }
    }

    private fun applyLine(point1: Array<Int>, point2: Array<Int>) {
        if (point1[0] == point2[0] || point1[1] == point2[1]) {
            // horizontal or vertical lines
            val rowStart = point1[0].coerceAtMost(point2[0])
            val rowEnd = point1[0].coerceAtLeast(point2[0]) + 1
            val colStart = point1[1].coerceAtMost(point2[1])
            val colEnd = point1[1].coerceAtLeast(point2[1]) + 1

            for (row in rowStart until rowEnd) {
                for (col in colStart until colEnd) {
                    oceanFloor[row][col]++
                }
            }
            return
        }
        // diagonal lines
        val length = abs(point1[0] - point2[0]) + 1
        val increment0 = if (point1[0] < point2[0]) 1 else -1
        val increment1 = if (point1[1] < point2[1]) 1 else -1
        var value0 = point1[0]
        var value1 = point1[1]

        for (i in 0 until length) {
            oceanFloor[value0][value1]++
            value0 += increment0
            value1 += increment1
        }
    }

    fun applyLines(lines: Array<String>) {
        for (line in lines) {
            if (line.trim().isEmpty()) {
                continue
            }

            val twoPoints = line.trim().split(" -> ")
            val point1 = twoPoints[0].split(",").map { it.toInt() }.toTypedArray()
            val point2 = twoPoints[1].split(",").map { it.toInt() }.toTypedArray()
            applyLine(point1, point2)
        }
    }

    override fun toString(): String {
        val sb = StringBuilder()
        for (col in 0 until size) {
            for (row in 0 until size) {
                sb.append(oceanFloor[row][col])
            }
            sb.append("\n")
        }
        return sb.toString()
    }

    fun calculateHotSpots(overlap: Int): Int {
        var result = 0
        for (row in 0 until size) {
            for (col in 0 until size) {
                if (oceanFloor[row][col] >= overlap) {
                    result++
                }
            }
        }
        return result
    }
}