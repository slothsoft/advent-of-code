package d22;

import org.junit.Assert;
import org.junit.Test;

import java.io.IOException;
import java.io.InputStream;
import java.nio.charset.StandardCharsets;
import java.util.Scanner;

public class ReactorRebootTest {

    @Test
    public void testExample1Line1() {
        ReactorReboot reactorReboot = new ReactorReboot("on x=10..12,y=10..12,z=10..12");
        ReactorReboot.Cube cube = reactorReboot.reboot(50);

        long result = cube.countCubes(true);
        Assert.assertEquals(27, result);
    }

    @Test
    public void testExample1Line2() {
        ReactorReboot reactorReboot = new ReactorReboot(
                "on x=10..12,y=10..12,z=10..12",
                "on x=11..13,y=11..13,z=11..13"
        );
        ReactorReboot.Cube cube = reactorReboot.reboot(50);

        long result = cube.countCubes(true);
        Assert.assertEquals(46, result);
    }

    @Test
    public void testExample1Line3() {
        ReactorReboot reactorReboot = new ReactorReboot(
                "on x=10..12,y=10..12,z=10..12",
                "on x=11..13,y=11..13,z=11..13",
                "off x=9..11,y=9..11,z=9..11"
        );
        ReactorReboot.Cube cube = reactorReboot.reboot(50);

        long result = cube.countCubes(true);
        Assert.assertEquals(38, result);
    }

    @Test
    public void testExample1A() throws IOException {
        ReactorReboot reactorReboot = new ReactorReboot(readInput("exampleA.txt"));
        ReactorReboot.Cube cube = reactorReboot.reboot(50);

        long result = cube.countCubes(true);
        Assert.assertEquals(39, result);
    }

    private String[] readInput(String fileName) throws IOException {
        try (InputStream input = getClass().getResourceAsStream(fileName);
             Scanner scanner = new Scanner(input, StandardCharsets.UTF_8)) {
            return scanner.useDelimiter("\\A").next().split("\\R");
        }
    }

    @Test
    public void testExample1B() throws IOException {
        ReactorReboot reactorReboot = new ReactorReboot(readInput("exampleB.txt"));
        ReactorReboot.Cube cube = reactorReboot.reboot(50);

        long result = cube.countCubes(true);
        Assert.assertEquals(590784, result);
    }

    @Test
    public void testPuzzle1() throws IOException {
        ReactorReboot reactorReboot = new ReactorReboot(readInput("input.txt"));
        ReactorReboot.Cube cube = reactorReboot.reboot(50);

        long result = cube.countCubes(true);
        System.out.println("Puzzle 1: " + result);
        Assert.assertEquals(596598, result);
    }

    @Test
    public void testExample2BoxTrue() {
        ReactorReboot.Box box = new ReactorReboot.Box(
                new int[]{1, 2, 3},
                new int[]{2, 4, 6},
                true
        );

        Assert.assertEquals(2 * 3 * 4, box.calculateArea());
        Assert.assertEquals(2 * 3 * 4, box.countWithValue(true));
        Assert.assertEquals(0, box.countWithValue(false));
    }

    @Test
    public void testExample2BoxFalse() {
        ReactorReboot.Box box = new ReactorReboot.Box(
                new int[]{1, 2, 3},
                new int[]{2, 4, 6},
                false
        );

        Assert.assertEquals(2 * 3 * 4, box.calculateArea());
        Assert.assertEquals(0, box.countWithValue(true));
        Assert.assertEquals(2 * 3 * 4, box.countWithValue(false));
    }

    @Test
    public void testExample2BoxTrueWithFalseChild() {
        ReactorReboot.Box box = new ReactorReboot.Box(
                new int[]{1, 1, 1},
                new int[]{10, 10, 10},
                true
        );
        Assert.assertEquals(1000, box.calculateArea());
        Assert.assertEquals(1000, box.countWithValue(true));
        Assert.assertEquals(0, box.countWithValue(false));

        box.children.add(new ReactorReboot.Box(
                new int[]{1, 2, 3},
                new int[]{2, 4, 6},
                false
        ));

        Assert.assertEquals(1000, box.calculateArea());
        Assert.assertEquals(1000 - 2 * 3 * 4, box.countWithValue(true));
        Assert.assertEquals(2 * 3 * 4, box.countWithValue(false));
    }

    @Test
    public void testExample2BoxTrueWithTrueChild() {
        ReactorReboot.Box box = new ReactorReboot.Box(
                new int[]{1, 1, 1},
                new int[]{10, 10, 10},
                true
        );
        Assert.assertEquals(1000, box.calculateArea());
        Assert.assertEquals(1000, box.countWithValue(true));
        Assert.assertEquals(0, box.countWithValue(false));

        box.children.add(new ReactorReboot.Box(
                new int[]{1, 2, 3},
                new int[]{2, 4, 6},
                true
        ));

        Assert.assertEquals(1000, box.calculateArea());
        Assert.assertEquals(1000, box.countWithValue(true));
        Assert.assertEquals(0, box.countWithValue(false));
    }

    @Test
    public void testExample2BoxFalseWithTrueChild() {
        ReactorReboot.Box box = new ReactorReboot.Box(
                new int[]{1, 1, 1},
                new int[]{10, 10, 10},
                false
        );
        Assert.assertEquals(1000, box.calculateArea());
        Assert.assertEquals(0, box.countWithValue(true));
        Assert.assertEquals(1000, box.countWithValue(false));

        box.children.add(new ReactorReboot.Box(
                new int[]{1, 2, 3},
                new int[]{2, 4, 6},
                true
        ));

        Assert.assertEquals(1000, box.calculateArea());
        Assert.assertEquals(2 * 3 * 4, box.countWithValue(true));
        Assert.assertEquals(1000 - 2 * 3 * 4, box.countWithValue(false));
    }

    @Test
    public void testExample2BoxFalseWithFalseChild() {
        ReactorReboot.Box box = new ReactorReboot.Box(
                new int[]{1, 1, 1},
                new int[]{10, 10, 10},
                false
        );
        Assert.assertEquals(1000, box.calculateArea());
        Assert.assertEquals(0, box.countWithValue(true));
        Assert.assertEquals(1000, box.countWithValue(false));

        box.children.add(new ReactorReboot.Box(
                new int[]{1, 2, 3},
                new int[]{2, 4, 6},
                false
        ));

        Assert.assertEquals(1000, box.calculateArea());
        Assert.assertEquals(0, box.countWithValue(true));
        Assert.assertEquals(1000, box.countWithValue(false));
    }

    @Test
    public void testExample2() throws IOException {
        ReactorReboot reactorReboot = new ReactorReboot(readInput("exampleC.txt"));

        long result = reactorReboot.countRebootCubes(Integer.MIN_VALUE, Integer.MAX_VALUE);
        Assert.assertEquals(2758514936282235L, result);
    }

    @Test
    public void testExample2For50A() throws IOException {
        ReactorReboot reactorReboot = new ReactorReboot(readInput("exampleA.txt"));
        long result = reactorReboot.countRebootCubes(-50, 50);

        Assert.assertEquals(39, result);
    }

    @Test
    public void testExample2For50B() throws IOException {
        ReactorReboot reactorReboot = new ReactorReboot(readInput("exampleB.txt"));
        long result = reactorReboot.countRebootCubes(-50, 50);

        Assert.assertEquals(590784, result);
    }

    @Test
    public void testExample2For50C() throws IOException {
        ReactorReboot reactorReboot = new ReactorReboot(readInput("exampleC.txt"));
        long result = reactorReboot.countRebootCubes(-50, 50);

        Assert.assertEquals(474140, result);
    }

    @Test
    public void testPuzzle2() throws IOException {
        ReactorReboot reactorReboot = new ReactorReboot(readInput("input.txt"));

        long result = reactorReboot.countRebootCubes(Integer.MIN_VALUE, Integer.MAX_VALUE);
        System.out.println("Puzzle 2: " + result);
        Assert.assertEquals(946529356520531L, result); // too low
    }
}
