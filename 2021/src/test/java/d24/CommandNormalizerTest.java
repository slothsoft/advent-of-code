package d24;

import d24.ArithmeticLogicUnit.Command;
import d24.ArithmeticLogicUnit.Operator;
import d24.ArithmeticLogicUnit.OperatorCommand;
import d24.CommandNormalizer.BetterOperatorCommand;
import d24.CommandNormalizer.SetVariableCommand;
import java.io.IOException;
import java.io.InputStream;
import java.nio.charset.StandardCharsets;
import java.util.Arrays;
import java.util.List;
import java.util.Scanner;
import org.junit.Test;

public class CommandNormalizerTest {

    @Test
    public void testPuzzle1Normalize() throws IOException {
        ArithmeticLogicUnit arithmeticLogicUnit = new ArithmeticLogicUnit(readInput("input.txt"));
        arithmeticLogicUnit.normalize();

        for (String line : arithmeticLogicUnit.createCodeLines()) {
            System.out.println(line);
        }
    }

    private String[] readInput(String fileName) throws IOException {
        try (InputStream input = getClass().getResourceAsStream(fileName);
            Scanner scanner = new Scanner(input, StandardCharsets.UTF_8)) {
            return scanner.useDelimiter("\\A").next().split("\\R");
        }
    }

    @Test
    public void testNormalizeSetVariable() {
        ArithmeticLogicUnit arithmeticLogicUnit = new ArithmeticLogicUnit(
            new OperatorCommand('a', "0", Operator.MULTIPLY),
            new OperatorCommand('a', "27", Operator.ADD)
        );
        arithmeticLogicUnit.normalize();

        Arrays.asList(Arrays.asList(new SetVariableCommand('a', "27")), arithmeticLogicUnit.commands);
    }

    @Test
    public void testNormalizeSetVariableNot() {
        List<Command> commands = Arrays.asList(
            new OperatorCommand('a', "0", Operator.MULTIPLY),
            new OperatorCommand('a', "27", Operator.ADD)
        );
        ArithmeticLogicUnit arithmeticLogicUnit = new ArithmeticLogicUnit(commands.toArray(new Command[commands.size()]));
        arithmeticLogicUnit.normalize();

        Arrays.asList(commands, arithmeticLogicUnit.commands);
    }

    @Test
    public void testNormalizeSetThenOperatorVariable() {
        ArithmeticLogicUnit arithmeticLogicUnit = new ArithmeticLogicUnit(
            new SetVariableCommand('a', "14"),
            new OperatorCommand('a', "27", Operator.ADD)
        );
        arithmeticLogicUnit.normalize();

        Arrays.asList(Arrays.asList(new BetterOperatorCommand('a', "14", "27", Operator.ADD)), arithmeticLogicUnit.commands);
    }

    @Test
    public void testNormalizeSetThenOperatorVariableNot() {
        List<Command> commands = Arrays.asList(
            new SetVariableCommand('a', "14"),
            new OperatorCommand('a', "27", Operator.ADD)
        );
        ArithmeticLogicUnit arithmeticLogicUnit = new ArithmeticLogicUnit(commands.toArray(new Command[commands.size()]));
        arithmeticLogicUnit.normalize();

        Arrays.asList(commands, arithmeticLogicUnit.commands);
    }

    @Test
    public void testNormalizeNotEquals() {
        ArithmeticLogicUnit arithmeticLogicUnit = new ArithmeticLogicUnit(
            new OperatorCommand('a', "14", Operator.EQUALS),
            new OperatorCommand('a', "0", Operator.EQUALS)
        );
        arithmeticLogicUnit.normalize();

        Arrays.asList(Arrays.asList(new OperatorCommand('a', "14", Operator.NOT_EQUALS)), arithmeticLogicUnit.commands);
    }

    @Test
    public void testNormalizNotEqualsNot() {
        List<Command> commands = Arrays.asList(
            new OperatorCommand('a', "14", Operator.EQUALS),
            new OperatorCommand('b', "0", Operator.EQUALS)
        );
        ArithmeticLogicUnit arithmeticLogicUnit = new ArithmeticLogicUnit(commands.toArray(new Command[commands.size()]));
        arithmeticLogicUnit.normalize();

        Arrays.asList(commands, arithmeticLogicUnit.commands);
    }

    @Test
    public void testNumberFive() {
        int z = 26 + 6;
        for (int i = 1; i <= 9; i++) {
            int w = i - 5;
            int x = z % 26;
            z = z / 26;
            x = x == w ? 1 : 0;
            x = x == 0 ? 1 : 0;
            int y = 0;
            y = y + 25;
            y = y * x;
            y = y + 1;
            z = z * y;
            y = y * 0;
            y = y + w;
            y = y + 9;
            y = y * x;
            z = z + y;
            System.out.println(i + "\t->\t" + z);
        }
    }
}
