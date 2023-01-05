package d19;

import org.junit.Assert;
import org.junit.Test;

import java.io.IOException;
import java.io.InputStream;
import java.nio.charset.StandardCharsets;
import java.util.Map;
import java.util.Scanner;

public class BeaconScannerTest {

    @Test
    public void testExample1Rotations() {
        BeaconScanner.Vector vector = new BeaconScanner.Vector(1, 2, 3);

        Assert.assertEquals("There should be 24 different rotations.", 24, BeaconScanner.Rotation.ALL_VALUES.length);

        for (int i = 0; i < BeaconScanner.Rotation.ALL_VALUES.length; i++) {
            BeaconScanner.Vector rotatedVector = BeaconScanner.Rotation.ALL_VALUES[i].transform(vector);

            for (int j = 0; j < BeaconScanner.Rotation.ALL_VALUES.length; j++) {
                if (i == j) {
                    continue;
                }
                BeaconScanner.Vector otherVector = BeaconScanner.Rotation.ALL_VALUES[j].transform(vector);
                Assert.assertNotEquals("Two rotations should not have the same result!\n\t" + i + " -> " +
                        rotatedVector + "\n\t" + j + " -> " + otherVector, rotatedVector, otherVector);
            }
        }
    }

    @Test
    public void testExample1AlignScanners() throws IOException {
        BeaconScanner beaconScanner = new BeaconScanner(readInput("example.txt"));
        Map<BeaconScanner.Scanner, BeaconScanner.Alignment> alignments = beaconScanner.alignScanners();

        // assert scanner 1 - 1 must be at 68,-1246,-43 (relative to scanner 0)
        BeaconScanner.Scanner scanner = alignments.keySet().stream().filter(s -> s.number == 1).findFirst().get();
        BeaconScanner.Alignment alignment = alignments.get(scanner);

        Assert.assertEquals(new BeaconScanner.Vector(68, -1246, -43), alignment.movement);

        // assert scanner 3 - 3 must be at -92,-2380,-20 (relative to scanner 0).
        scanner = alignments.keySet().stream().filter(s -> s.number == 3).findFirst().get();
        alignment = alignments.get(scanner);

        Assert.assertEquals(new BeaconScanner.Vector(-92, -2380, -20), alignment.movement);

        // assert scanner 4 - 4 is at -20,-1133,1061
        scanner = alignments.keySet().stream().filter(s -> s.number == 4).findFirst().get();
        alignment = alignments.get(scanner);

        Assert.assertEquals(new BeaconScanner.Vector(-20, -1133, 1061), alignment.movement);

        // assert scanner 2 - 2 must be at 1105,-1205,1229
        scanner = alignments.keySet().stream().filter(s -> s.number == 2).findFirst().get();
        alignment = alignments.get(scanner);

        Assert.assertEquals(new BeaconScanner.Vector(1105, -1205, 1229), alignment.movement);
    }

    private String[] readInput(String fileName) throws IOException {
        try (InputStream input = getClass().getResourceAsStream(fileName);
             Scanner scanner = new Scanner(input, StandardCharsets.UTF_8)) {
            return scanner.useDelimiter("\\A").next().split("\\R");
        }
    }

    @Test
    public void testExample1() throws IOException {
        BeaconScanner beaconScanner = new BeaconScanner(readInput("example.txt"));
        int result = beaconScanner.mergeIntoOneCoordinateSystem().length;
        Assert.assertEquals(79, result);
    }

    @Test
    public void testPuzzle1() throws IOException {
        BeaconScanner beaconScanner = new BeaconScanner(readInput("input.txt"));
        int result = beaconScanner.mergeIntoOneCoordinateSystem().length;
        System.out.println("Puzzle 1: " + result);
        Assert.assertEquals(449, result);
    }

    @Test
    public void testExample2() throws IOException {
        BeaconScanner beaconScanner = new BeaconScanner(readInput("example.txt"));
        int result = beaconScanner.calculateManhattenDistance();
        Assert.assertEquals(3621, result);
    }

    @Test
    public void testPuzzle2() throws IOException {
        BeaconScanner beaconScanner = new BeaconScanner(readInput("input.txt"));
        int result = beaconScanner.calculateManhattenDistance();
        System.out.println("Puzzle 2: " + result);
        Assert.assertEquals(449, result);
    }
}
