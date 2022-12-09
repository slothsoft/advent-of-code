package d13

import org.junit.Assert
import org.junit.Test
import java.awt.Point

class TransparentOrigamiTest {

    private val inputAsPoints = arrayOf(
        Point(6, 10),
        Point(0, 14),
        Point(9, 10),
        Point(0, 3),
        Point(10, 4),
        Point(4, 11),
        Point(6, 0),
        Point(6, 12),
        Point(4, 1),
        Point(0, 13),
        Point(10, 12),
        Point(3, 4),
        Point(3, 0),
        Point(8, 4),
        Point(1, 10),
        Point(2, 14),
        Point(8, 10),
        Point(9, 0),
    )

    @Test
    fun testExample1Programmatically() {
        val transparentOrigami = TransparentOrigami()
        inputAsPoints.forEach {
            p -> transparentOrigami.addPoint(p.x, p.y)
        }
        Assert.assertEquals("""...#..#..#.
....#......
...........
#..........
...#....#.#
...........
...........
...........
...........
...........
.#....#.##.
....#......
......#...#
#..........
#.#........""", transparentOrigami.toString())
        Assert.assertEquals(inputAsPoints.size, transparentOrigami.getPointCount())

        transparentOrigami.foldAlongY(7)

        Assert.assertEquals("""#.##..#..#.
#...#......
......#...#
#...#......
.#.#..#.###""", transparentOrigami.toString())
        Assert.assertEquals(17, transparentOrigami.getPointCount())

        transparentOrigami.foldAlongX(5)

        Assert.assertEquals("""#####
#...#
#...#
#...#
#####""", transparentOrigami.toString())
        Assert.assertEquals(16, transparentOrigami.getPointCount())
    }

    @Test
    fun testExample1a() {
        val transparentOrigami = TransparentOrigami()
        transparentOrigami.execute(readLines("example.txt"), 1)
        Assert.assertEquals(17, transparentOrigami.getPointCount())
    }

    @Test
    fun testExample1b() {
        val transparentOrigami = TransparentOrigami()
        transparentOrigami.execute(readLines("example.txt"), 2)
        Assert.assertEquals(16, transparentOrigami.getPointCount())
    }

    private fun readLines(fileName: String) : Array<String> {
        return TransparentOrigamiTest::class.java.getResource(fileName)!!.readText().split(Regex("\\R")).toTypedArray()
    }

    @Test
    fun testPuzzle1() {
        val transparentOrigami = TransparentOrigami()
        transparentOrigami.execute(readLines("input.txt"), 1)
        val result = transparentOrigami.getPointCount()
        println("Puzzle 1: $result")
        Assert.assertEquals(724, result)
    }

    @Test
    fun testPuzzle2() {
        val transparentOrigami = TransparentOrigami()
        transparentOrigami.execute(readLines("input.txt"), 12)
        println("Puzzle 2:")
        print(transparentOrigami)
    }
}