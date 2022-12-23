package d24;

import org.junit.Assert;
import org.junit.Test;

import java.io.IOException;
import java.io.InputStream;
import java.nio.charset.StandardCharsets;
import java.util.Map;
import java.util.Scanner;

public class ArithmeticLogicUnitTest {

    @Test
    public void testExample1A() throws IOException {
        ArithmeticLogicUnit arithmeticLogicUnit = new ArithmeticLogicUnit(readInput("exampleA.txt"));

        Map<Character, Long> result = arithmeticLogicUnit.execute(9).createVariablesMap();
        Assert.assertEquals(Long.valueOf(-9), result.get('x'));
    }

    private String[] readInput(String fileName) throws IOException {
        try (InputStream input = getClass().getResourceAsStream(fileName);
             Scanner scanner = new Scanner(input, StandardCharsets.UTF_8)) {
            return scanner.useDelimiter("\\A").next().split("\\R");
        }
    }

    @Test
    public void testExample1B() throws IOException {
        ArithmeticLogicUnit arithmeticLogicUnit = new ArithmeticLogicUnit(readInput("exampleB.txt"));

        Map<Character, Long> result = arithmeticLogicUnit.execute(2, 6).createVariablesMap();
        Assert.assertEquals(Long.valueOf(1L), result.get('z'));

        result = arithmeticLogicUnit.execute(2, 5).createVariablesMap();
        Assert.assertEquals(Long.valueOf(0L), result.get('z'));
    }

    @Test
    public void testExample1C() throws IOException {
        ArithmeticLogicUnit arithmeticLogicUnit = new ArithmeticLogicUnit(readInput("exampleC.txt"));

        Map<Character, Long> result = arithmeticLogicUnit.execute(4).createVariablesMap(); // 0100
        Assert.assertEquals(Long.valueOf(0), result.get('w'));
        Assert.assertEquals(Long.valueOf(1), result.get('x'));
        Assert.assertEquals(Long.valueOf(0), result.get('y'));
        Assert.assertEquals(Long.valueOf(0), result.get('z'));

        result = arithmeticLogicUnit.execute(7).createVariablesMap(); // 01111
        Assert.assertEquals(Long.valueOf(0), result.get('w'));
        Assert.assertEquals(Long.valueOf(1), result.get('x'));
        Assert.assertEquals(Long.valueOf(1), result.get('y'));
        Assert.assertEquals(Long.valueOf(1), result.get('z'));

        result = arithmeticLogicUnit.execute(0).createVariablesMap(); // 0000
        Assert.assertEquals(Long.valueOf(0), result.get('w'));
        Assert.assertEquals(Long.valueOf(0), result.get('x'));
        Assert.assertEquals(Long.valueOf(0), result.get('y'));
        Assert.assertEquals(Long.valueOf(0), result.get('z'));
    }

    @Test
    public void testPuzzle1() throws IOException {
        ArithmeticLogicUnit arithmeticLogicUnit = new ArithmeticLogicUnit(readInput("input.txt"));

        long result = arithmeticLogicUnit.findGreatestSerialNumber();
        System.out.println("Puzzle 1: " + result);
        Assert.assertEquals(946529356520531L, result);
    }

    @Test
    public void testPuzzle1WriteAsString() throws IOException {
        ArithmeticLogicUnit arithmeticLogicUnit = new ArithmeticLogicUnit(readInput("input.txt"));

        for (String line : arithmeticLogicUnit.createCodeLines()) {
            System.out.println(line);
        }
    }
}
