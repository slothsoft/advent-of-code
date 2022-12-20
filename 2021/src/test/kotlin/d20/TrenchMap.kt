package d20

/**
 * <a href="https://adventofcode.com/2021/day/20">Day 20: Trench Map</a>
 */
class TrenchMap(input: Array<String>) {

    private val _trueChar = '#'
    private val _falseChar: Char = '.'

    private val enhancementAlgorithm: String
    private var image: Array<BooleanArray>
    private var background: Char = _falseChar

    init {
        enhancementAlgorithm = input[0]
        image = Array(input.size - 2) { _ -> BooleanArray(input.size - 2) }
        for (y in 0 until image[0].size) {
            for (x in image.indices) {
                image[x][y] = input[y + 2][x] == _trueChar
            }
        }
    }

    fun stringify(): String {
        val result: StringBuilder = StringBuilder()
        for (y in 0 until image[0].size) {
            for (x in 0 until image.size) {
                if (image[x][y]) {
                    result.append(_trueChar)
                } else {
                    result.append(_falseChar)
                }
            }
            result.append("\n")
        }
        return result.trim().toString()
    }

    fun enhance(times: Int = 1) {
        for (y in 0 until times) {
            singleEnhance()
        }
    }

    private fun singleEnhance() {
        val newImage = Array(image.size + 2, { i -> BooleanArray(image[0].size + 2) })
        for (x in 0 until newImage.size) {
            for (y in 0 until newImage[x].size) {
                newImage[x][y] = enhancePixel(x - 1, y - 1)
            }
        }
        background = if (background == _trueChar) {
            enhancementAlgorithm[enhancementAlgorithm.length - 1]
        } else {
            enhancementAlgorithm[0]
        }
        image = newImage
    }

    fun enhancePixel(x: Int, y: Int): Boolean {
        val algorithmInput = calculateBinary(x, y)
        return enhancementAlgorithm[algorithmInput] == _trueChar
    }

    fun calculateBinary(middleX: Int, middleY: Int): Int {
        val result: StringBuilder = StringBuilder()
        for (y in -1 until 2) {
            for (x in -1 until 2) {
                result.append(getValue(middleX + x, middleY + y))
            }
        }
        return result.toString().toInt(2)
    }

    private fun getValue(x: Int, y: Int): Int {
        if (x < 0 || y < 0 || x >= image.size || y >= image.size) {
            if (background == _trueChar) {
                return 1
            }
            return 0
        }
        if (image[x][y]) {
            return 1
        }
        return 0
    }

    fun countTruePixels(): Int {
        var result = 0
        for (x in 0 until image.size) {
            for (y in 0 until image[x].size) {
                if (image[x][y])
                    result++
            }
        }
        return result
    }
}