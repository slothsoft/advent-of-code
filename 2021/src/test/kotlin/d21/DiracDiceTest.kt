package d21

import org.junit.Assert
import org.junit.Test

class DiracDiceTest {

    @Test
    fun testExample1Steps() {
        val diracDice = DeterministicDiracDice(7)

        diracDice.players[0].rollDie { 2 }

        Assert.assertEquals(3, diracDice.players[0].position)
        Assert.assertEquals(3, diracDice.players[0].score)

        diracDice.players[0].rollDie { 1 }

        Assert.assertEquals(6, diracDice.players[0].position)
        Assert.assertEquals(9, diracDice.players[0].score)
    }

    @Test
    fun testExample1MoreSteps() {
        val diracDice = DeterministicDiracDice(4, 8)

        // Player 1 rolls 1+2+3 and moves to space 10 for a total score of 10.
        diracDice.executePlayerRound()

        Assert.assertEquals(10, diracDice.players[0].position)
        Assert.assertEquals(10, diracDice.players[0].score)

        // Player 2 rolls 4+5+6 and moves to space 3 for a total score of 3.
        diracDice.executePlayerRound()

        Assert.assertEquals(10, diracDice.players[0].position)
        Assert.assertEquals(10, diracDice.players[0].score)
        Assert.assertEquals(3, diracDice.players[1].position)
        Assert.assertEquals(3, diracDice.players[1].score)

        // Player 1 rolls 7+8+9 and moves to space 4 for a total score of 14.
        diracDice.executePlayerRound()

        Assert.assertEquals(4, diracDice.players[0].position)
        Assert.assertEquals(14, diracDice.players[0].score)
        Assert.assertEquals(3, diracDice.players[1].position)
        Assert.assertEquals(3, diracDice.players[1].score)

        // Player 2 rolls 10+11+12 and moves to space 6 for a total score of 9.
        diracDice.executePlayerRound()

        Assert.assertEquals(4, diracDice.players[0].position)
        Assert.assertEquals(14, diracDice.players[0].score)
        Assert.assertEquals(6, diracDice.players[1].position)
        Assert.assertEquals(9, diracDice.players[1].score)

        // Player 1 rolls 13+14+15 and moves to space 6 for a total score of 20.
        diracDice.executePlayerRound()

        Assert.assertEquals(6, diracDice.players[0].position)
        Assert.assertEquals(20, diracDice.players[0].score)
        Assert.assertEquals(6, diracDice.players[1].position)
        Assert.assertEquals(9, diracDice.players[1].score)

        // Player 2 rolls 16+17+18 and moves to space 7 for a total score of 16.
        diracDice.executePlayerRound()

        Assert.assertEquals(6, diracDice.players[0].position)
        Assert.assertEquals(20, diracDice.players[0].score)
        Assert.assertEquals(7, diracDice.players[1].position)
        Assert.assertEquals(16, diracDice.players[1].score)

        // Player 1 rolls 19+20+21 and moves to space 6 for a total score of 26.
        diracDice.executePlayerRound()

        Assert.assertEquals(6, diracDice.players[0].position)
        Assert.assertEquals(26, diracDice.players[0].score)
        Assert.assertEquals(7, diracDice.players[1].position)
        Assert.assertEquals(16, diracDice.players[1].score)

        // Player 2 rolls 22+23+24 and moves to space 6 for a total score of 22.
        diracDice.executePlayerRound()

        Assert.assertEquals(6, diracDice.players[0].position)
        Assert.assertEquals(26, diracDice.players[0].score)
        Assert.assertEquals(6, diracDice.players[1].position)
        Assert.assertEquals(22, diracDice.players[1].score)

        // ...after many turns...
        while (diracDice.players[0].score < 986) {
            diracDice.executePlayerRound()
        }

        // Player 2 rolls 82+83+84 and moves to space 6 for a total score of 742.
        diracDice.executePlayerRound()

        Assert.assertEquals(6, diracDice.players[0].position)
        Assert.assertEquals(986, diracDice.players[0].score)
        Assert.assertEquals(6, diracDice.players[1].position)
        Assert.assertEquals(742, diracDice.players[1].score)

        // Player 1 rolls 85+86+87 and moves to space 4 for a total score of 990.
        diracDice.executePlayerRound()

        Assert.assertEquals(4, diracDice.players[0].position)
        Assert.assertEquals(990, diracDice.players[0].score)
        Assert.assertEquals(6, diracDice.players[1].position)
        Assert.assertEquals(742, diracDice.players[1].score)

        // Player 2 rolls 88+89+90 and moves to space 3 for a total score of 745.
        diracDice.executePlayerRound()

        Assert.assertEquals(4, diracDice.players[0].position)
        Assert.assertEquals(990, diracDice.players[0].score)
        Assert.assertEquals(3, diracDice.players[1].position)
        Assert.assertEquals(745, diracDice.players[1].score)

        Assert.assertFalse(diracDice.players[0].HasWon(1000))

        // Player 1 rolls 91+92+93 and moves to space 10 for a final score, 1000.
        diracDice.executePlayerRound()

        Assert.assertEquals(10, diracDice.players[0].position)
        Assert.assertEquals(1000, diracDice.players[0].score)
        Assert.assertEquals(3, diracDice.players[1].position)
        Assert.assertEquals(745, diracDice.players[1].score)

        Assert.assertTrue(diracDice.players[0].HasWon(1000))
    }

    @Test
    fun testExample1() {
        val diracDice = DeterministicDiracDice(4, 8)

        val result = diracDice.calculateLosingPlayerProduct()

        Assert.assertEquals(745, diracDice.players[1].score)
        Assert.assertEquals(993, diracDice.dieRollsCount)

        Assert.assertEquals(739_785, result)
    }

    @Test
    fun testPuzzle1() {
        val diracDice = DeterministicDiracDice(10, 2)

        val result = diracDice.calculateLosingPlayerProduct()
        println("Puzzle 1: $result")
        Assert.assertEquals(916083, result)
    }

    @Test
    fun testExample2RollToCount() {
        val diracDice = QuantumDiracDice(4, 8)

        var result = 0
        for (rollToCount in diracDice.rollsToCount.values) {
            result += rollToCount
        }

        Assert.assertEquals(27, result)
    }

    @Test
    fun testExample2() {
        val diracDice = QuantumDiracDice(4, 8)

        val (result1, result2) = diracDice.calculateWinCounts()

        if (result1 < result2) {
            Assert.assertEquals(341960390180808, result1)
            Assert.assertEquals(444356092776315, result2)
        } else {
            Assert.assertEquals(444356092776315, result1)
            Assert.assertEquals(341960390180808, result2)
        }
    }

    @Test
    fun testPuzzle2() {
        val diracDice = QuantumDiracDice(10, 2)

        val (result1, result2) = diracDice.calculateWinCounts()
        println("Puzzle 2: \n$result1\n$result2")
        Assert.assertEquals(49982165861983, result1)
        Assert.assertEquals(36086577212020, result2)
    }
}