package d13

import java.awt.Point

/**
 * <a href="https://adventofcode.com/2021/day/13">Day 13: Transparent Origami</a>: You reach another volcanically
 * active part of the cave. It would be nice if you could do some kind of thermal imaging so you could tell
 * ahead of time which caves are too hot to safely enter.
 */
class TransparentOrigami {

    private val points: MutableSet<Point> = mutableSetOf()

    fun execute(commands: Array<String>, folds: Int) {
        val indexOfBreaker = commands.indexOf("")
        for (i in 0 until indexOfBreaker) {
            addPoint(commands[i])
        }

        val lastFold = indexOfBreaker + 1 + folds
        for (i in indexOfBreaker + 1 until lastFold) {
            foldAlong(commands[i])
        }
    }

    private fun addPoint(xAndY: String) { // 9,10
        val xAndYSplit = xAndY.split(",")
        addPoint(xAndYSplit[0].toInt(), xAndYSplit[1].toInt())
    }

    fun addPoint(x: Int, y: Int) {
        points.add(Point(x, y))
    }

    private fun foldAlong(command: String) { // fold along y=7
        val coordAndValue = command.split(" ")[2].split("=")
        if (coordAndValue[0] == "y") {
            foldAlongY(coordAndValue[1].toInt())
        } else {
            foldAlongX(coordAndValue[1].toInt())
        }
    }

    fun foldAlongX(x: Int) {
        val foldedPoints = points.filter { it.x > x }.toList()
        points.removeAll(foldedPoints.toSet())
        foldedPoints.forEach{ it.x = 2 * x - it.x}
        points.addAll(foldedPoints)
    }

    fun foldAlongY(y: Int) {
        val foldedPoints = points.filter { it.y > y }.toList()
        points.removeAll(foldedPoints.toSet())
        foldedPoints.forEach{ it.y = (y - (it.y - y))}
        points.addAll(foldedPoints)
    }

    fun getPointCount(): Int {
        return points.size
    }

    override fun toString(): String {
        val xMax: Int = points.maxOf { it.x }
        val yMax: Int = points.maxOf { it.y }
        val point = Point()

        val result: StringBuilder = StringBuilder()
        for (y in 0 until yMax + 1) {
            for (x in 0 until xMax + 1) {
                point.x = x
                point.y = y
                if (points.contains(point)) {
                    result.append('#')
                } else {
                    result.append('.')
                }
            }
            result.append('\n')
        }
        return result.toString().trim()
    }

}