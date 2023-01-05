package d25;

/**
 * <a href="https://adventofcode.com/2021/day/25">Day 25: Sea Cucumber</a>
 */
public final class SeaCucumber {
    enum Cucumber {
        EAST('>', 1, 0),
        SOUTH('v', 0, 1),
        ;

        static final Cucumber[] ALL = values();

        final char character;
        final int xPlus;
        final int yPlus;

        Cucumber(char character, int xPlus, int yPlus) {
            this.character = character;
            this.xPlus = xPlus;
            this.yPlus = yPlus;
        }

        static Cucumber ofChar(char character) {
            for (Cucumber cucumber : ALL) {
                if (cucumber.character == character)
                    return cucumber;
            }
            return null;
        }

    }

    Cucumber[][] locations;

    public SeaCucumber(String... inputLines) {
        locations = new Cucumber[inputLines[0].length()][inputLines.length];
        for (var y = 0; y < inputLines.length; y++) {
            for (var x = 0; x < inputLines[y].length(); x++) {
                locations[x][y] = Cucumber.ofChar(inputLines[y].charAt(x));
            }
        }
    }

    public int calculateStepsUntilStagnant() {
        int result = 1;
        while (executeStep()) {
            result++;
        }
        return result;
    }

    public boolean executeSteps(int steps) {
        for (int i = 0; i < steps; i++) {
            if (!executeStep()) {
                return false;
            }
        }
        return true;
    }

    public boolean executeStep() {
        Cucumber[][] newLocations = new Cucumber[locations.length][locations[0].length];
        boolean somethingWasChanged = false;
        for (Cucumber cucumber : Cucumber.ALL) {
            for (var x = 0; x < locations.length; x++) {
                for (var y = 0; y < locations[x].length; y++) {
                    if (locations[x][y] == cucumber) {
                        // one of my cucumbers, so move it
                        int newX = (x + cucumber.xPlus) % locations.length;
                        int newY = (y + cucumber.yPlus) % locations[0].length;
                        if (
                                // east facing cucumbers can move if there is a space on the current locations
                                (cucumber == Cucumber.EAST && locations[newX][newY] == null) ||
                                // south facing cucumbers can move if there is space on the new locations (so the other
                                // cucumbers have moved already), and now other south facing cucumber is blocking them
                                (cucumber == Cucumber.SOUTH && newLocations[newX][newY] == null && locations[newX][newY] != Cucumber.SOUTH)) {
                            newLocations[newX][newY] = cucumber;
                            somethingWasChanged = true;
                        } else {
                            newLocations[x][y] = cucumber;
                        }
                    }
                }
            }
        }
        if (somethingWasChanged)
            locations = newLocations;
        return somethingWasChanged;
    }

    public String stringify() {
        var result = new StringBuilder();
        for (var y = 0; y < locations[0].length; y++) {
            for (var x = 0; x < locations.length; x++) {
                if (locations[x][y] == null) {
                    result.append('.');
                } else {
                    result.append(locations[x][y].character);
                }
            }
            result.append('\n');
        }
        return result.toString().trim();
    }
}
