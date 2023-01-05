package d06;

import java.util.*;
import java.util.stream.IntStream;

/**
 * <a href="https://adventofcode.com/2021/day/6">Day 6: Lanternfish</a>:The sea
 * floor is getting steeper. Maybe the sleigh keys got carried this way?
 *
 * A massive school of glowing lanternfish swims past. They must spawn quickly to
 * reach such large numbers - maybe exponentially quickly? You should model their
 * growth rate to be sure.
 */
public final class Lanternfish {

    // every fish with value X produces the same children in the days simulated
    private static final Map<Integer, Long> cache = new HashMap<>();

    public static long calculateGrowth(int[] input, int daysSimulated) {
        // so now we only need to find the number of fish per X and add / multiply
        return IntStream.of(input).mapToLong(i -> calculateGrowthForSingleFish(i, daysSimulated)).sum();
    }


    private static long calculateGrowthForSingleFish(int input, int daysSimulated) {
        if (daysSimulated - input - 1 < 0) {
            // the simulation will not get to the point of this fish being a parent
            return 1;
        }
        int key = daysSimulated * 10 + input;
        if (cache.containsKey(key)) {
            // we use the cache because it is much, much faster than calculating this stuff
            // for every single fishie
            return cache.get(key);
        }
        // we "simulate" input days, then this fish duplicates, and we need to calculate 2
        // fishes with the remaining days
        long result =
                // parent fishie
                calculateGrowthForSingleFish(7, daysSimulated - input) +
                // baby fishie
                calculateGrowthForSingleFish(8, daysSimulated - input - 1);
        cache.put(key, result);
        return result;
    }

    private Lanternfish() {
        // hide this constructor
    }
}
