package d24;

import java.text.MessageFormat;
import java.util.ArrayList;
import java.util.Arrays;
import java.util.HashMap;
import java.util.List;
import java.util.Map;
import java.util.stream.Collectors;
/**
 * <a href="https://adventofcode.com/2021/day/24">Day 24: Arithmetic Logic Unit</a>
 */
public final class ArithmeticLogicUnit {

    static class AluContext {

        private final long[] variables = new long[4];

        private final int[] serialNumber;
        private int inputIndex;

        public AluContext(int... serialNumber) {
            this.serialNumber = serialNumber;
        }

        public void setVariable(char variable, long value) {
            variables[variable - 'w'] = value;
        }


        public long getVariableOrValue(String string) {
            return Character.isAlphabetic(string.charAt(0))
                ? getVariable(string.charAt(0))
                : Long.parseLong(string);
        }

        public long getVariable(char variable) {
            return variables[variable - 'w'];
        }

        public int getNextInput() {
            return serialNumber[inputIndex++];
        }

        public Map<Character, Long> createVariablesMap() {
            Map<Character, Long> result = new HashMap<>();
            for (char i = 'w'; i <= 'z'; i++) {
                result.put(i, getVariable(i));
            }
            return result;
        }
    }

    interface Command {

        void execute(AluContext context);

        String stringify(AluContext context);

    }

    static class InputCommand implements Command {

        private final char variable;

        public InputCommand(char variable) {
            this.variable = variable;
        }

        public void execute(AluContext context) {
            context.setVariable(variable, context.getNextInput());
        }

        @Override
        public String stringify(AluContext context) {
            return variable + " = input[" + context.inputIndex + "]; // is " + context.getNextInput() + " on default";
        }

        @Override
        public String toString() {
            return "InputCommand{" + stringify(new AluContext(7)) + '}';
        }
    }

    enum Operator {
        ADD(" + {0}") {
            @Override
            final long apply(long firstOperand, long secondOperand) {
                return firstOperand + secondOperand;
            }
        },
        MULTIPLY(" * {0}") {
            @Override
            final long apply(long firstOperand, long secondOperand) {
                return firstOperand * secondOperand;
            }
        },
        DIVIDE(" / {0}") {
            @Override
            final long apply(long firstOperand, long secondOperand) {
                return firstOperand / secondOperand;
            }
        },
        MODULO(" % {0}") {
            @Override
            final long apply(long firstOperand, long secondOperand) {
                return firstOperand % secondOperand;
            }
        },
        EQUALS(" == {0} ? 1 : 0") {
            @Override
            final long apply(long firstOperand, long secondOperand) {
                return firstOperand == secondOperand ? 1 : 0;
            }
        },
        NOT_EQUALS(" == {0} ? 0 : 1") {
            @Override
            final long apply(long firstOperand, long secondOperand) {
                return firstOperand == secondOperand ? 0 : 1;
            }
        },
        ;

        final String stringifyPattern;

        Operator(String stringifyPattern) {
            this.stringifyPattern = stringifyPattern;
        }

        abstract long apply(long firstOperand, long secondOperand);
    }

    static class OperatorCommand implements Command {

        final char firstOperand;
        final String secondOperand;
        final Operator operator;

        public OperatorCommand(char firstOperand, String secondOperand, Operator operator) {
            this.firstOperand = firstOperand;
            this.secondOperand = secondOperand;
            this.operator = operator;
        }

        public void execute(AluContext context) {
            long firstOperandValue = context.getVariable(firstOperand);
            long secondOperandValue = context.getVariableOrValue(secondOperand);
            context.setVariable(firstOperand, operator.apply(firstOperandValue, secondOperandValue));
        }

        @Override
        public String stringify(AluContext context) {
            return firstOperand + " = " + firstOperand + MessageFormat.format(operator.stringifyPattern, secondOperand) + ";";
        }

        @Override
        public String toString() {
            return "OperatorCommand{" + stringify(new AluContext(7)) + '}';
        }
    }

    private static final int DIGIT_COUNT = 14;

    final List<Command> commands;

    public ArithmeticLogicUnit(String... inputLines) {
        commands = new ArrayList(inputLines.length);
        for (int i = 0; i < inputLines.length; i++) {
            String[] split = inputLines[i].split(" ");
            switch (split[0]) {
                case "inp":
                    commands.add(new InputCommand(split[1].charAt(0)));
                    break;
                case "add":
                    commands.add(new OperatorCommand(split[1].charAt(0), split[2], Operator.ADD));
                    break;
                case "mul":
                    commands.add(new OperatorCommand(split[1].charAt(0), split[2], Operator.MULTIPLY));
                    break;
                case "div":
                    commands.add(new OperatorCommand(split[1].charAt(0), split[2], Operator.DIVIDE));
                    break;
                case "mod":
                    commands.add(new OperatorCommand(split[1].charAt(0), split[2], Operator.MODULO));
                    break;
                case "eql":
                    commands.add(new OperatorCommand(split[1].charAt(0), split[2], Operator.EQUALS));
                    break;

                default:
                    throw new IllegalArgumentException("Do not know command " + split[0]);
            }
        }
    }

    public ArithmeticLogicUnit(Command... commands) {
        this.commands = new ArrayList<>(Arrays.asList(commands));
    }

    AluContext execute(int... input) {
        AluContext context = new AluContext(input);
        for (Command command : commands) {
            command.execute(context);
        }
        return context;
    }

    public String findMyGreatestSerialNumber() {
        // see the calculations on paper (math.png)
        // there are 7 numbers that increase the resulting z, and 7 that decrease it

        int[] serialNumber = new int[DIGIT_COUNT];
        for (serialNumber[0] = 9; serialNumber[0] >= 8; serialNumber[0]--) {
            serialNumber[13] = serialNumber[0] - 7;

            for (serialNumber[1] = 2; serialNumber[1] >= 1; serialNumber[1]--) {
                serialNumber[12] = serialNumber[1] + 7;

                for (serialNumber[2] = 9; serialNumber[2] >= 5; serialNumber[2]--) {
                    serialNumber[5] = serialNumber[2] - 4;

                    for (serialNumber[3] = 6; serialNumber[3] >= 1; serialNumber[3]--) {
                        serialNumber[4] = serialNumber[3] + 3;

                        for (serialNumber[6] = 9; serialNumber[6] >= 7; serialNumber[6]--) {
                            serialNumber[7] = serialNumber[6] - 6;

                            for (serialNumber[8] = 4; serialNumber[8] >= 1; serialNumber[8]--) {
                                serialNumber[9] = serialNumber[8] + 5;

                                for (serialNumber[10] = 7; serialNumber[10] >= 1; serialNumber[10]--) {
                                    serialNumber[11] = serialNumber[10] + 2;

                                    AluContext context = execute(serialNumber);
                                    if (context.getVariable('z') == 0) {
                                        return Arrays.stream(serialNumber).mapToObj(String::valueOf).collect(Collectors.joining());
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }
        return null;
    }

    public String findMySmallestSerialNumber() {
        // see the calculations on paper (math.png)
        // there are 7 numbers that increase the resulting z, and 7 that decrease it

        int[] serialNumber = new int[DIGIT_COUNT];
        for (serialNumber[0] = 8; serialNumber[0] <= 9; serialNumber[0]++) {
            serialNumber[13] = serialNumber[0] - 7;

            for (serialNumber[1] = 1; serialNumber[1] <= 2; serialNumber[1]++) {
                serialNumber[12] = serialNumber[1] + 7;

                for (serialNumber[2] = 5; serialNumber[2] <= 9; serialNumber[2]++) {
                    serialNumber[5] = serialNumber[2] - 4;

                    for (serialNumber[3] = 1; serialNumber[3] <= 6; serialNumber[3]++) {
                        serialNumber[4] = serialNumber[3] + 3;

                        for (serialNumber[6] = 7; serialNumber[6] <= 9; serialNumber[6]++) {
                            serialNumber[7] = serialNumber[6] - 6;

                            for (serialNumber[8] = 1; serialNumber[8] <= 4; serialNumber[8]++) {
                                serialNumber[9] = serialNumber[8] + 5;

                                for (serialNumber[10] = 1; serialNumber[10] <= 7; serialNumber[10]++) {
                                    serialNumber[11] = serialNumber[10] + 2;

                                    AluContext context = execute(serialNumber);
                                    if (context.getVariable('z') == 0) {
                                        return Arrays.stream(serialNumber).mapToObj(String::valueOf).collect(Collectors.joining());
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }
        return null;
    }


    static int[] convertSerialNumberToArray(long serialNumber) {
        int[] result = new int[DIGIT_COUNT];
        for (int i = 0; i < result.length; i++) {
            result[result.length - i - 1] = (int) (serialNumber % 10);
            serialNumber /= 10;
        }
        return result;
    }

    public void normalize() {
        CommandNormalizer.normalize(commands);
    }

    public String[] createCodeLines() {
        String[] result = new String[commands.size()];
        int index = 0;
        AluContext context = new AluContext(convertSerialNumberToArray(13579246899999L));
        for (Command command : commands) {
            result[index++] = command.stringify(context);
        }
        return result;
    }
}
