package d25;

import org.junit.Assert;
import org.junit.Test;

import java.io.IOException;
import java.io.InputStream;
import java.nio.charset.StandardCharsets;
import java.util.Scanner;

public class SeaCucumberTest {

    @Test
    public void testExample1A() {
        SeaCucumber seaCucumber = new SeaCucumber("...>>>>>...");

        Assert.assertEquals("...>>>>>...", seaCucumber.stringify());

        Assert.assertTrue(seaCucumber.executeStep());
        Assert.assertEquals("...>>>>.>..", seaCucumber.stringify());

        Assert.assertTrue(seaCucumber.executeStep());
        Assert.assertEquals("...>>>.>.>.", seaCucumber.stringify());
    }

    @Test
    public void testExample1B() {
        SeaCucumber seaCucumber = new SeaCucumber("..........",
                ".>v....v..",
                ".......>..",
                "..........");

        Assert.assertEquals("..........\n" +
                ".>v....v..\n" +
                ".......>..\n" +
                "..........", seaCucumber.stringify());

        Assert.assertTrue(seaCucumber.executeStep());
        Assert.assertEquals("..........\n" +
                ".>........\n" +
                "..v....v>.\n" +
                "..........", seaCucumber.stringify());
    }

    @Test
    public void testExample1CSteps() throws IOException {
        SeaCucumber seaCucumber = new SeaCucumber(readInput("exampleC.txt"));
        Assert.assertEquals("...>...\n" +
                ".......\n" +
                "......>\n" +
                "v.....>\n" +
                "......>\n" +
                ".......\n" +
                "..vvv..", seaCucumber.stringify());

        Assert.assertTrue(seaCucumber.executeStep());
        Assert.assertEquals("..vv>..\n" +
                ".......\n" +
                ">......\n" +
                "v.....>\n" +
                ">......\n" +
                ".......\n" +
                "....v..", seaCucumber.stringify());

        Assert.assertTrue(seaCucumber.executeStep());
        Assert.assertEquals("....v>.\n" +
                "..vv...\n" +
                ".>.....\n" +
                "......>\n" +
                "v>.....\n" +
                ".......\n" +
                ".......", seaCucumber.stringify());

        Assert.assertTrue(seaCucumber.executeStep());
        Assert.assertEquals("......>\n" +
                "..v.v..\n" +
                "..>v...\n" +
                ">......\n" +
                "..>....\n" +
                "v......\n" +
                ".......", seaCucumber.stringify());

        Assert.assertTrue(seaCucumber.executeStep());
        Assert.assertEquals(">......\n" +
                "..v....\n" +
                "..>.v..\n" +
                ".>.v...\n" +
                "...>...\n" +
                ".......\n" +
                "v......", seaCucumber.stringify());
    }

    @Test
    public void testExample1C() throws IOException {
        SeaCucumber seaCucumber = new SeaCucumber(readInput("exampleC.txt"));
        Assert.assertTrue(seaCucumber.executeSteps(4));
        Assert.assertEquals(">......\n" +
                "..v....\n" +
                "..>.v..\n" +
                ".>.v...\n" +
                "...>...\n" +
                ".......\n" +
                "v......", seaCucumber.stringify());
    }

    private String[] readInput(String fileName) throws IOException {
        try (InputStream input = getClass().getResourceAsStream(fileName);
             Scanner scanner = new Scanner(input, StandardCharsets.UTF_8)) {
            return scanner.useDelimiter("\\A").next().split("\\R");
        }
    }

    @Test
    public void testExample1D() throws IOException {
        SeaCucumber seaCucumber = new SeaCucumber(readInput("exampleD.txt"));
        Assert.assertEquals(58, seaCucumber.calculateStepsUntilStagnant());
        Assert.assertFalse(seaCucumber.executeStep());
    }

    @Test
    public void testPuzzle1() throws IOException {
        SeaCucumber seaCucumber = new SeaCucumber(readInput("input.txt"));
        int result = seaCucumber.calculateStepsUntilStagnant();
        System.out.println("Puzzle 1: " + result);
        Assert.assertEquals(400, result);
    }
}
