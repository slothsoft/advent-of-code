package d24;

import java.util.HashMap;
import java.util.Map;
import java.util.Objects;
import java.util.function.ToLongBiFunction;

/**
 * <a href="https://adventofcode.com/2021/day/24">Day 24: Arithmetic Logic Unit</a>
 */
public final class ArithmeticLogicUnit {

    static class AluContext {
        private final long[] variables = new long[4];

        private final long[] inputs;
        private int inputIndex;

        public AluContext(long[] inputs) {
            this.inputs = inputs;
        }

        public void setVariable(char variable, long value) {
            variables[variable - 'w'] = value;
        }

        public long getVariable(char variable) {
            return variables[variable - 'w'];
        }

        public long getNextInput() {
            return inputs[inputIndex++];
        }

        public Map<Character, Long> createVariablesMap() {
            Map<Character, Long> result = new HashMap<>();
            for (char i = 'w'; i <= 'z'; i++) {
                result.put(i, getVariable(i));
            }
            return result;
        }
    }

    private interface Command {
        void execute(AluContext context);
    }

    private static class InputCommand implements Command {

        private final char variable;

        public InputCommand(char variable) {
            this.variable = variable;
        }

        public void execute(AluContext context) {
            context.setVariable(variable, context.getNextInput());
        }
    }

    private static class OperatorCommand implements Command {

        private final char firstOperand;
        private final String secondOperand;
        private final ToLongBiFunction<Long, Long> operator;

        public OperatorCommand(char firstOperand, String secondOperand, ToLongBiFunction<Long, Long> operator) {
            this.firstOperand = firstOperand;
            this.secondOperand = secondOperand;
            this.operator = operator;
        }

        public void execute(AluContext context) {
            long firstOperandValue = context.getVariable(firstOperand);
            long secondOperandValue = Character.isAlphabetic(secondOperand.charAt(0))
                    ? context.getVariable(secondOperand.charAt(0))
                    : Long.parseLong(secondOperand);
            context.setVariable(firstOperand, operator.applyAsLong(firstOperandValue, secondOperandValue));
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
                    commands[i] = new OperatorCommand(split[1].charAt(0), split[2], Long::sum);
                    break;
                case "mul":
                    commands[i] = new OperatorCommand(split[1].charAt(0), split[2], (a,b) -> a * b);
                    break;
                case "div":
                    commands[i] = new OperatorCommand(split[1].charAt(0), split[2], (a,b) -> a / b);
                    break;
                case "mod":
                    commands[i] = new OperatorCommand(split[1].charAt(0), split[2], (a,b) -> a % b);
                    break;
                case "eql":
                    commands[i] = new OperatorCommand(split[1].charAt(0), split[2], (a,b) -> Objects.equals(a, b) ? 1L : 0L);
                    break;

                default:
                    throw new IllegalArgumentException("Do not know command " + split[0]);
            }
        }
    }

    AluContext execute(long... params) {
        AluContext context = new AluContext(params);
        for (Command command : commands) {
            command.execute(context);
        }
        return context;
    }

    public long findGreatestSerialNumber() {
        for (long i = 99_999_999_999_999L; i >= 10_000_000_000_000L; i--) {
            if (String.valueOf(i).contains("0")) {
                continue;
            }
            long[] serialNumber = convertSerialNumberToArray(i);
            AluContext context = execute(serialNumber);
            if (context.getVariable('z') == 0L) {
                return i;
            }
        }
        return -1L;
    }

    static long[] convertSerialNumberToArray(long serialNumber) {
        long[] result = new long[14];
        for (int i = 0; i < result.length; i++) {
            result[result.length - i - 1] = serialNumber % 10;
            serialNumber /= 10;
        }
        return result;
    }


}
