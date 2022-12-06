package d07;

import java.util.stream.IntStream;

/**
 * <a href="https://adventofcode.com/2021/day/7">Day 7: The Treachery of Whales</a>:A giant whale has decided your
 * submarine is its next meal, and it's much faster than you are. There's nowhere to run!
 *
 * Suddenly, a swarm of crabs (each in its own tiny submarine - it's too deep for them otherwise) zooms in to
 * rescue you! They seem to be preparing to blast a hole in the ocean floor; sensors indicate a massive
 * underground cave system just beyond where they're aiming!
 */
public final class TreacheryOfWhales {

    public static int calculateMinimumFuel(int[] input, boolean incrementingFuelConsumption) {
        int min = IntStream.of(input).min().getAsInt();
        int max = IntStream.of(input).max().getAsInt();

        int minimumFuel = Integer.MAX_VALUE;

        for (int i = min; i <=max ; i++) {
            int currentFuel = calculateFuel(input, i, incrementingFuelConsumption);
            minimumFuel = Math.min(currentFuel, minimumFuel);
        }
        return minimumFuel;
    }

    private static int calculateFuel(int[] input, int index, boolean incrementingFuelConsumption) {
        return IntStream.of(input).map(value -> {
            int diff = Math.abs(value - index);
            if (incrementingFuelConsumption)
                return diff * (diff + 1) / 2;
            return diff;
        }).sum();
    }

    private TreacheryOfWhales() {
        // hide this constructor
    }
}
