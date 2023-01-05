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
        Assert.assertEquals(27, reactorReboot.reboot(50));
    }

    @Test
    public void testExample1Line2() {
        ReactorReboot reactorReboot = new ReactorReboot(
                "on x=10..12,y=10..12,z=10..12",
                "on x=11..13,y=11..13,z=11..13"
        );
        Assert.assertEquals(46, reactorReboot.reboot(50));
    }

    @Test
    public void testExample1Line3() {
        ReactorReboot reactorReboot = new ReactorReboot(
                "on x=10..12,y=10..12,z=10..12",
                "on x=11..13,y=11..13,z=11..13",
                "off x=9..11,y=9..11,z=9..11"
        );
        Assert.assertEquals(38, reactorReboot.reboot(50));
    }

    @Test
    public void testExample1A() throws IOException {
        ReactorReboot reactorReboot = new ReactorReboot(readInput("exampleA.txt"));
        Assert.assertEquals(39, reactorReboot.reboot(50));
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
        Assert.assertEquals(590784, reactorReboot.reboot(50));
    }

    @Test
    public void testPuzzle1() throws IOException {
        ReactorReboot reactorReboot = new ReactorReboot(readInput("input.txt"));

        long result = reactorReboot.reboot(50);
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

        Assert.assertEquals(2 * 3 * 4, box.area);
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

        Assert.assertEquals(2 * 3 * 4, box.area);
        Assert.assertEquals(0, box.countWithValue(true));
        Assert.assertEquals(2 * 3 * 4, box.countWithValue(false));
    }

    @Test
    public void testExample2BoxTrueWithFalseChild() {
        ReactorReboot.BigBox box = new ReactorReboot.BigBox(
                new int[]{1, 1, 1},
                new int[]{10, 10, 10},
                true
        );
        Assert.assertEquals(1000, box.area);
        Assert.assertEquals(1000, box.countWithValue(true));
        Assert.assertEquals(0, box.countWithValue(false));

        box.addChild(new ReactorReboot.Box(
                new int[]{1, 2, 3},
                new int[]{2, 4, 6},
                false
        ));

        Assert.assertEquals(1000, box.area);
        Assert.assertEquals(1000 - 2 * 3 * 4, box.countWithValue(true));
        Assert.assertEquals(2 * 3 * 4, box.countWithValue(false));
    }

    @Test
    public void testExample2BoxTrueWithTrueChild() {
        ReactorReboot.BigBox box = new ReactorReboot.BigBox(
                new int[]{1, 1, 1},
                new int[]{10, 10, 10},
                true
        );
        Assert.assertEquals(1000, box.area);
        Assert.assertEquals(1000, box.countWithValue(true));
        Assert.assertEquals(0, box.countWithValue(false));

        box.addChild(new ReactorReboot.Box(
                new int[]{1, 2, 3},
                new int[]{2, 4, 6},
                true
        ));

        Assert.assertEquals(1000, box.area);
        Assert.assertEquals(1000, box.countWithValue(true));
        Assert.assertEquals(0, box.countWithValue(false));
    }

    @Test
    public void testExample2BoxFalseWithTrueChild() {
        ReactorReboot.BigBox box = new ReactorReboot.BigBox(
                new int[]{1, 1, 1},
                new int[]{10, 10, 10},
                false
        );
        Assert.assertEquals(1000, box.area);
        Assert.assertEquals(0, box.countWithValue(true));
        Assert.assertEquals(1000, box.countWithValue(false));

        box.addChild(new ReactorReboot.Box(
                new int[]{1, 2, 3},
                new int[]{2, 4, 6},
                true
        ));

        Assert.assertEquals(1000, box.area);
        Assert.assertEquals(2 * 3 * 4, box.countWithValue(true));
        Assert.assertEquals(1000 - 2 * 3 * 4, box.countWithValue(false));
    }

    @Test
    public void testExample2BoxFalseWithFalseChild() {
        ReactorReboot.BigBox box = new ReactorReboot.BigBox(
                new int[]{1, 1, 1},
                new int[]{10, 10, 10},
                false
        );
        Assert.assertEquals(1000, box.area);
        Assert.assertEquals(0, box.countWithValue(true));
        Assert.assertEquals(1000, box.countWithValue(false));

        box.addChild(new ReactorReboot.Box(
                new int[]{1, 2, 3},
                new int[]{2, 4, 6},
                false
        ));

        Assert.assertEquals(1000, box.area);
        Assert.assertEquals(0, box.countWithValue(true));
        Assert.assertEquals(1000, box.countWithValue(false));
    }

    @Test
    public void testExample2() throws IOException {
        ReactorReboot reactorReboot = new ReactorReboot(readInput("exampleC.txt"));

        long result = reactorReboot.rebootToMaxSize();
        Assert.assertEquals(2758514936282235L, result);
    }

    @Test
    public void testExample2For50A() throws IOException {
        // Visualization
        // https://www.matheretter.de/geoservant/de?draw=quader(11%7C11%7C11%203%203%203)%0Aquader(10%7C10%7C10%203%203%203)%0Aquader(13%7C11%7C11%201%203%203)%0Aquader(11%7C13%7C11%202%201%203)%0Aquader(11%7C11%7C13%202%202%201)
        ReactorReboot reactorReboot = new ReactorReboot(readInput("exampleA.txt"));
        long result = reactorReboot.reboot(50);

        Assert.assertEquals(39, result);
    }

    @Test
    public void testExample2For50B() throws IOException {
        ReactorReboot reactorReboot = new ReactorReboot(readInput("exampleB.txt"));
        long result = reactorReboot.reboot(50);

        Assert.assertEquals(590784, result);
    }

    @Test
    public void testExample2For50C() throws IOException {
        ReactorReboot reactorReboot = new ReactorReboot(readInput("exampleC.txt"));
        long result = reactorReboot.reboot(50);

        Assert.assertEquals(474140, result);
    }

    @Test
    public void testExample2For50D() throws IOException {
        // Visualization
        // https://www.matheretter.de/geoservant/de?draw=quader(11%7C11%7C11%203%203%203)%0Aquader(10%7C10%7C10%203%203%203)%0Aquader(13%7C11%7C11%201%203%203)%0Aquader(11%7C13%7C11%202%201%203)%0Aquader(11%7C11%7C13%202%202%201)

        // on x=2..4,y=0..3,z=0..3
        // - turns on 3x4x4 = 48 pixels
        // on x=0..2,y=2..5,z=1..4
        // - turns on 3x4x4 = 48 pixels
        // - BUT x=2, y=2..3, z=1..3 are already turned on (so minus 1x2x3 = 6)
        // - SUM = 48 + 48 - 6 = 90
        // on x=1..3,y=1..4,z=2..4
        // - turns on 3x4x3 = 36 pixels
        // - BUT x=2..3, y=1..3, z=2..3 are already turned on (so minus 2x3x2 = 12)
        // - BUT x=1..2, y=2..4, z=2..4 are already turned on (so minus 2x3x3 = 18)
        // - BUT x=2, y=2..3, z=2..3 were subtracted twice (so plus 1x2x2 = 4)
        // - SUM = 90 + 36 - 12 - 18 + 4 = 100

        ReactorReboot reactorReboot = new ReactorReboot(readInput("exampleD.txt"));
        long result = reactorReboot.reboot(50);

        Assert.assertEquals(100, result);
    }

    @Test
    public void testPuzzle2() throws IOException {
        ReactorReboot reactorReboot = new ReactorReboot(readInput("input.txt"));

        long result = reactorReboot.rebootToMaxSize();
        System.out.println("Puzzle 2: " + result);
        Assert.assertEquals(1199121349148621L, result);
    }
}
