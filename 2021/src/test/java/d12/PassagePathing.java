package d12;

import java.util.*;
import java.util.stream.Collectors;

/**
 * <a href="https://adventofcode.com/2021/day/12">Day 12: Passage Pathing</a>: With your submarine's subterranean
 * subsystems subsisting suboptimally, the only way you're getting out of this cave anytime soon is by finding a path
 * yourself. Not just a path - the only way to know if you've found the best path is to find all of them.
 */
public final class PassagePathing {

    private static final String START = "start";
    private static final String END = "end";

    private final Map<String, Set<String>> paths = new HashMap<>();

    public PassagePathing(String[] input) {
        for (String line : input) {
            String[] fromAndTo = line.split("-");
            addPath(fromAndTo[0], fromAndTo[1]);
            addPath(fromAndTo[1], fromAndTo[0]);
        }
    }

    private void addPath(String from, String to) {
        if (!paths.containsKey(from)) {
            paths.put(from, new HashSet<>());
        }
        paths.get(from).add(to);
    }

    public int calculatePathCombinations() {
        return createAllPaths().size();
    }

    Set<String[]> createAllPaths() {
        Map<String, Integer> entriesAllowed = createCaveEntriesAllowed();
        Set<String[]> createdPaths = new TreeSet<>(Comparator.comparing(Arrays::toString));
        createPaths(START, createdPaths, entriesAllowed, new ArrayList<>(List.of(START)));
        return createdPaths;
    }

    private Map<String, Integer> createCaveEntriesAllowed() {
        Map<String, Integer> result = new HashMap<>();
        for (String cave : paths.keySet()) {
            if (cave.equals(START)) {
                // we are not allowed to go back to the start
                result.put(cave, 0);
            } else if (Character.isLowerCase(cave.charAt(0))) {
                // small cave or end
                result.put(cave, 1);
            } else {
                // big cave
                result.put(cave, Integer.MAX_VALUE);
            }
        }
        return result;
    }

    private void createPaths(String start, Set<String[]> createdPaths, Map<String, Integer> entriesAllowed, List<String> currentPath) {

        if (start.equals(END)) {
            // we are at the end of the cave! add to list and return early
            createdPaths.add(currentPath.toArray(new String[0]));
            return;
        }

        String[] possiblePaths = paths.get(start).stream().filter(p -> entriesAllowed.get(p) > 0).toArray(String[]::new);

        if (possiblePaths.length == 0) {
            // we do not get to the exit from here
        } else if (possiblePaths.length == 1) {
            // there is only one way to go just call the next method
            currentPath.add(possiblePaths[0]);
            entriesAllowed.put(possiblePaths[0], entriesAllowed.get(possiblePaths[0]) - 1);
            createPaths(possiblePaths[0], createdPaths, entriesAllowed, currentPath);
        } else {
            // we have multiple ways to go, so we need to copy the data
            for (String possiblePath : possiblePaths) {
                List<String> newCurrentPath = new ArrayList<>(currentPath);
                newCurrentPath.add(possiblePath);

                Map<String, Integer> newEntriesAllowed = new HashMap<>(entriesAllowed);
                newEntriesAllowed.put(possiblePath, entriesAllowed.get(possiblePath) - 1);
                createPaths(possiblePath, createdPaths, newEntriesAllowed, newCurrentPath);
            }
        }
    }


    public int calculatePathCombinations2() {
        return createAllPaths2().size();
    }

    Set<String[]> createAllPaths2() {
        Map<String, Integer> entriesAllowed = createCaveEntriesAllowed();
        Set<String[]> createdPaths = new TreeSet<>(Comparator.comparing(Arrays::toString));
        List<String> smallCaves = entriesAllowed.entrySet()
                .stream()
                .filter(e -> e.getValue() == 1)
                .map(Map.Entry::getKey)
                .filter(k -> !START.equals(k) && !END.equals(k))
                .collect(Collectors.toList());

        for (String smallCave : smallCaves) {
            Map<String, Integer> newEntriesAllowed = new HashMap<>(entriesAllowed);
            newEntriesAllowed.put(smallCave, 2);
            createPaths(START, createdPaths, newEntriesAllowed, new ArrayList<>(List.of(START)));
        }
        return createdPaths;
    }
}
