package d14

/**
 * <a href="https://adventofcode.com/2021/day/14">Day 14: Extended Polymerization</a>: The incredible pressures at this
 * depth are starting to put a strain on your submarine. The submarine has polymerization equipment that would produce
 * suitable materials to reinforce the submarine, and the nearby volcanically-active caves should even have the
 * necessary input elements in sufficient quantities.
 *
 * The submarine manual contains instructions for finding the optimal polymer formula; specifically, it offers a polymer
 * template and a list of pair insertion rules (your puzzle input). You just need to work out what polymer would result
 * after repeating the pair insertion process a few times.
 */
class ExtendedPolymerization {

    private val template: MutableList<Char> = mutableListOf()
    private val pairInsertions: MutableMap<String, Char> = mutableMapOf()

    constructor(templateString: String) {
        templateString.toCharArray().forEach { template.add(it) }
    }

    constructor(lines: Array<String>) {
        lines[0].toCharArray().forEach { template.add(it) }

        for (i in 2 until lines.size) {
            val insertion = lines[i].split(" -> ")
            pairInsertions[insertion[0]] = insertion[1][0]
        }
    }

    fun executeSteps(steps: Int) {
        for (s in 0 until steps) {
            executeStep()
        }
    }

    fun executeStep() {
        val templateAsString = getTemplate()
        for (i in template.size - 1 downTo 1) {
            val insertion = pairInsertions[templateAsString.substring(i - 1, i + 1)]
            if (insertion != null) {
                template.add(i, insertion)
            }
        }
    }

    fun getTemplate(): String {
        return template.joinToString("")
    }

    fun getQuantityDifference(): Int {
        val charWithCount = template.groupBy { t -> t }.mapValues { v -> v.value.count() }
        val max = charWithCount.values.max()
        val min = charWithCount.values.min()
        return max - min
    }

    fun calculateQuantityDifference(steps: Int): Long {
        val combinations = createCombinations()

        for (s in 0 until steps) {
            calculateQuantityDifference(combinations)
        }

        val charWithCount = calculateCharWithCount(combinations)
        val max = charWithCount.values.max()
        val min = charWithCount.values.min()
        return max - min
    }

    private fun createCombinations(): MutableMap<String, Long> {
        val result: MutableMap<String, Long> = mutableMapOf()
        val templateAsString = getTemplate()
        for (i in 0 until template.size - 1) {
            result[templateAsString.substring(i, i + 2)] = 1
        }
        return result
    }

    private fun calculateQuantityDifference(combinations: MutableMap<String, Long>) {
        val newCombinations: MutableMap<String, Long> = mutableMapOf()

        for (key in combinations.keys) {
            val insertion = pairInsertions[key]
            if (insertion != null) {
                val value = combinations[key]
                increment(newCombinations, "" + key[0] + insertion, value)
                increment(newCombinations, "" + insertion + key[1] , value)
            }
        }
        combinations.clear()
        combinations.putAll(newCombinations)
    }

    private fun <TKey> increment(combinations: MutableMap<TKey, Long>, key: TKey, value: Long?) {
        if (value == null)
            return
        if (combinations.contains(key)) {
            combinations[key] = combinations[key]!! + value
        } else {
            combinations[key] = value
        }
    }

    private fun calculateCharWithCount(combinations: MutableMap<String, Long>): Map<Char, Long> {
        val result: MutableMap<Char, Long> = mutableMapOf()

        for (key in combinations.keys) {
            for (c in key) {
                increment(result, c, combinations[key])
            }
        }
        // every character except the very first is duplicate - so
        increment(result, template[0], 1)
        increment(result, template[template.size - 1], 1)

        // now we can half all the values and get the correct value
        for (key in result.keys) {
            result[key] = result[key]!! / 2
        }
        return result
    }
}