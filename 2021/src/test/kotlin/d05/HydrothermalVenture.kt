package d05

class HydrothermalVenture {

    private val size: Int
    private val oceanFloor : Array<IntArray>

    constructor(initialSize: Int) {
        size = initialSize
        oceanFloor = Array(size) { IntArray(size) }
    }

    fun applyStraightLines(lines: Array<String>) {
        for (line in lines) {
            if (line.trim().isEmpty()) {
                continue
            }

            var twoPoints = line.trim().split(" -> ")
            var point1 = twoPoints[0].split(",").map { it.toInt() }.toTypedArray()
            var point2 = twoPoints[1].split(",").map { it.toInt() }.toTypedArray()

            if (point1[0] == point2[0] || point1[1] == point2[1]) {
                applyLine(point1, point2)
            }
        }
    }

    private fun applyLine(point1: Array<Int>, point2: Array<Int>) {
        var rowStart = Math.min(point1[0], point2[0])
        var rowEnd = Math.max(point1[0], point2[0]) + 1
        var colStart = Math.min(point1[1], point2[1])
        var colEnd = Math.max(point1[1], point2[1]) + 1

        for (row in rowStart until rowEnd) {
            for (col in colStart until colEnd) {
                oceanFloor[row][col]++
            }
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
        var result = 0;
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