package d20

import org.junit.Assert
import org.junit.Test

class TrenchMapTest {

    @Test
    fun testExample1Steps() {
        val trenchMap = TrenchMap(readLines("example.txt"))

        Assert.assertEquals(
            """#..#.
#....
##..#
..#..
..###""", trenchMap.stringify()
        )

        trenchMap.enhance()

        Assert.assertEquals(
            """.##.##.
#..#.#.
##.#..#
####..#
.#..##.
..##..#
...#.#.""", trenchMap.stringify()
        )

        trenchMap.enhance()

        Assert.assertEquals(
            """.......#.
.#..#.#..
#.#...###
#...##.#.
#.....#.#
.#.#####.
..#.#####
...##.##.
....###..""", trenchMap.stringify()
        )
    }

    private fun readLines(fileName: String): Array<String> {
        return TrenchMapTest::class.java.getResource(fileName)!!.readText().split(Regex("\\R")).toTypedArray()
    }

    @Test
    fun testExample1CalculateBinary() {
        val trenchMap = TrenchMap(readLines("example.txt"))

        Assert.assertEquals(34, trenchMap.calculateBinary(2, 2))
        Assert.assertEquals(true, trenchMap.enhancePixel(2, 2))
    }

    @Test
    fun testExample1() {
        val trenchMap = TrenchMap(readLines("example.txt"))

        trenchMap.enhance(2)

        Assert.assertEquals(
            """.......#.
.#..#.#..
#.#...###
#...##.#.
#.....#.#
.#.#####.
..#.#####
...##.##.
....###..""", trenchMap.stringify()
        )

        Assert.assertEquals(35, trenchMap.countTruePixels())
    }

    @Test
    fun testPuzzle1() {
        val trenchMap = TrenchMap(readLines("input.txt"))
        trenchMap.enhance(2)

        val result = trenchMap.countTruePixels()
        println("Puzzle 1: $result")
        Assert.assertEquals(5682, result)
    }

    @Test
    fun testExample2() {
        val trenchMap = TrenchMap(readLines("example.txt"))

        trenchMap.enhance(50)

        Assert.assertEquals(3351, trenchMap.countTruePixels())
    }

    @Test
    fun testPuzzle2() {
        val trenchMap = TrenchMap(readLines("input.txt"))
        trenchMap.enhance(50)

        val result = trenchMap.countTruePixels()
        println("Puzzle 2: $result")
        Assert.assertEquals(17628, result)
    }
}