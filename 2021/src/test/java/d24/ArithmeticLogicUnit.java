package d24;

import java.text.MessageFormat;
import java.util.Arrays;
import java.util.HashMap;
import java.util.Map;
import java.util.stream.Collectors;

/**
 * <a href="https://adventofcode.com/2021/day/24">Day 24: Arithmetic Logic Unit</a>
 */
public final class ArithmeticLogicUnit {

    static class AluContext {
        private final int[] variables = new int[4];

        private final String input;
        int inputIndex;

        public AluContext(String input) {
            this.input = input;
        }

        public final void setVariable(char variable, int value) {
            variables[variable - 'w'] = value;
        }

        public final int getVariable(char variable) {
            return variables[variable - 'w'];
        }

        public final int getNextInput() {
            return this.input.charAt(inputIndex++) - '0';
        }

        public final Map<Character, Long> createVariablesMap() {
            Map<Character, Long> result = new HashMap<>();
            for (char i = 'w'; i <= 'z'; i++) {
                result.put(i, Long.valueOf(getVariable(i)));
            }
            return result;
        }
    }

    private interface Command {
        void execute(AluContext context);
        String stringify(AluContext context);
    }

    private static class InputCommand implements Command {

        private final char variable;

        public InputCommand(char variable) {
            this.variable = variable;
        }

        public final void execute(AluContext context) {
            context.setVariable(variable, context.getNextInput());
        }

        @Override
        public String stringify(AluContext context) {
            return variable + " = input[" + context.inputIndex + "]; // is " + context.getNextInput() + " on default";
        }
    }

    private enum Operator {
        ADD(" + {0}") {
            @Override
            final int apply(int firstOperand, int secondOperand) {
                return firstOperand + secondOperand;
            }
        },
        MULTIPLY(" * {0}") {
            @Override
            final int apply(int firstOperand, int secondOperand) {
                return firstOperand * secondOperand;
            }
        },
        DIVIDE(" / {0}") {
            @Override
            final int apply(int firstOperand, int secondOperand) {
                return firstOperand / secondOperand;
            }
        },
        MODULO(" % {0}") {
            @Override
            final int apply(int firstOperand, int secondOperand) {
                return firstOperand % secondOperand;
            }
        },
        EQUALS( " == {0} ? 1 : 0") {
            @Override
            final int apply(int firstOperand, int secondOperand) {
                return firstOperand == secondOperand ? 1 : 0;
            }
        },
        ;

        final String stringifyPattern;

        Operator(String stringifyPattern) {
            this.stringifyPattern = stringifyPattern;
        }

        abstract int apply(int firstOperand, int secondOperand);
    }

    private static class OperatorCommand implements Command {

        private final char firstOperand;
        private final String secondOperand;
        private final Operator operator;

        public OperatorCommand(char firstOperand, String secondOperand, Operator operator) {
            this.firstOperand = firstOperand;
            this.secondOperand = secondOperand;
            this.operator = operator;
        }

        public void execute(AluContext context) {
            int firstOperandValue = context.getVariable(firstOperand);
            int secondOperandValue = Character.isAlphabetic(secondOperand.charAt(0))
                    ? context.getVariable(secondOperand.charAt(0))
                    : Integer.parseInt(secondOperand);
            context.setVariable(firstOperand, operator.apply(firstOperandValue, secondOperandValue));
        }

        @Override
        public String stringify(AluContext context) {
            return firstOperand + " = " + firstOperand + MessageFormat.format(operator.stringifyPattern, secondOperand) + ";";
        }
    }

    private final Command[] commands;

    public ArithmeticLogicUnit(String... inputLines) {
        commands = new Command[inputLines.length];
        for (int i = 0; i < inputLines.length; i++) {
            String[] split = inputLines[i].split(" ");
            switch (split[0]) {
                case "inp":
                    commands[i] = new InputCommand(split[1].charAt(0));
                    break;
                case "add":
                    commands[i] = new OperatorCommand(split[1].charAt(0), split[2], Operator.ADD);
                    break;
                case "mul":
                    commands[i] = new OperatorCommand(split[1].charAt(0), split[2], Operator.MULTIPLY);
                    break;
                case "div":
                    commands[i] = new OperatorCommand(split[1].charAt(0), split[2], Operator.DIVIDE);
                    break;
                case "mod":
                    commands[i] = new OperatorCommand(split[1].charAt(0), split[2], Operator.MODULO);
                    break;
                case "eql":
                    commands[i] = new OperatorCommand(split[1].charAt(0), split[2], Operator.EQUALS);
                    break;

                default:
                    throw new IllegalArgumentException("Do not know command " + split[0]);
            }
        }
    }

    AluContext execute(int... params) {
        return execute(Arrays.stream(params).mapToObj(p -> "" + p).collect(Collectors.joining()));
    }

    private AluContext execute(String input) {
        AluContext context = new AluContext(input);
        for (Command command : commands) {
            command.execute(context);
        }
        return context;
    }

    public long findGreatestSerialNumber() {
        for (long i = 99_999_999_999_999L; i >= 10_000_000_000_000L; i--) {
            String serialNumber = String.valueOf(i);
            if (serialNumber.contains("0")) {
                continue;
            }
            AluContext context = execute(serialNumber);
            if (context.getVariable('z') == 0L) {
                return i;
            }
        }
        return -1L;
    }

    public String[] createCodeLines() {
        String[] result = new String[commands.length];
        AluContext context = new AluContext("13579246899999");
        for (int i = 0; i < commands.length; i++) {
            result[i] = commands[i].stringify(context);
        }
        return result;
    }
}
