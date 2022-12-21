package d21

import java.util.function.IntSupplier

/**
 * <a href="https://adventofcode.com/2021/day/21">Day 21: Dirac Dice</a>
 */
class DeterministicDiracDice(vararg playerPositions: Int) {

    internal val players: Array<Player>
    private var playerTurn: Int = 0

    private var nextDieValue: Int = 1
    internal var dieRollsCount: Int = 0
    val die: IntSupplier = IntSupplier { nextDieValue++ }

    init {
        players = Array(playerPositions.size) { i -> Player(i + 1, playerPositions[i]) }
    }

    fun calculateLosingPlayerProduct(): Int {
        do {
            executePlayerRound()
        } while (players.count { p -> p.HasWon(1000) } == 0)

        return players[playerTurn].score * dieRollsCount
    }

    fun executePlayerRound() {
        players[playerTurn].rollDie(die)
        dieRollsCount += Constants.DIE_ROLLS
        playerTurn = (playerTurn + 1) % players.size
    }
}

data class Player(val id: Int, var initialPosition: Int, var initialScore: Int = 0) {

    var position: Int = initialPosition
    var score: Int = initialScore

    fun rollDie(die: IntSupplier) {
        var dieSum = 0
        for (d in 0 until Constants.DIE_ROLLS) {
            dieSum += die.asInt
        }
        moveSteps(dieSum)
    }

    private fun moveSteps(dieSum: Int) {
        position = ((position - 1 + dieSum) % Constants.BOARD_SIZE) + 1
        score += position
    }

    fun HasWon(wantedScore: Int): Boolean {
        return score >= wantedScore
    }
}

class QuantumDiracDice(vararg playerPositions: Int) {

    internal val rollsToCount = mapOf(3 to 1, 4 to 3, 5 to 6, 6 to 7, 7 to 6, 8 to 3, 9 to 1)

    private val playerPosition1: Int
    private val playerPosition2: Int
    private val targetScore = 21

    init {
        playerPosition1 = playerPositions[0]
        playerPosition2 = playerPositions[1]
    }

    fun calculateWinCounts(): Pair<Long, Long> {
        return calculateWinCounts(playerPosition1, 0, playerPosition2, 0)
    }

    private fun calculateWinCounts(position1: Int, score1: Int, position2: Int, score2: Int): Pair<Long, Long> {
        // the first player of the arguments is always moving here - the recursive call swaps the player to let the other one move

        var result1 = 0L
        var result2 = 0L

        for (rollToCount in rollsToCount.entries.iterator()) {
            val newPosition1 = ((position1 - 1 + rollToCount.key) % Constants.BOARD_SIZE) + 1
            val newScore1 = score1 + newPosition1

            if (newScore1 >= targetScore) {
                // if the player has won, just add the count of rolls that brought it here to the result
                result1 += rollToCount.value
            } else {
                // swap the players so the other one moves
                val (subResult2, subResult1) = calculateWinCounts(position2, score2, newPosition1, newScore1)

                result1 += rollToCount.value * subResult1
                result2 += rollToCount.value * subResult2
            }
        }

        return Pair(result1, result2)
    }
}

object Constants {
    const val BOARD_SIZE = 10
    const val DIE_ROLLS = 3
}