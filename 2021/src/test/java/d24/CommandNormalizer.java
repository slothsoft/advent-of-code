package d24;

import d24.ArithmeticLogicUnit.AluContext;
import d24.ArithmeticLogicUnit.Command;
import d24.ArithmeticLogicUnit.Operator;
import d24.ArithmeticLogicUnit.OperatorCommand;
import java.text.MessageFormat;
import java.util.List;

class CommandNormalizer {

    public static void normalize(List<Command> commands) {
        int index = 0;
        while (index < commands.size() - 1) {
            if (commands.get(index) instanceof OperatorCommand && commands.get(index + 1) instanceof OperatorCommand) {
                OperatorCommand operator1 = (OperatorCommand) commands.get(index);
                OperatorCommand operator2 = (OperatorCommand) commands.get(index + 1);

                if (operator1.operator == Operator.MULTIPLY && operator1.secondOperand.equals("0")
                    && operator2.operator == Operator.ADD && operator1.firstOperand == operator2.firstOperand) {
                    // a * 0 + b => a + b
                    commands.remove(index);
                    commands.remove(index);
                    commands.add(index, new SetVariableCommand(operator1.firstOperand, operator2.secondOperand));
                } else if (operator1.operator == Operator.EQUALS && operator2.operator == Operator.EQUALS
                    && operator2.secondOperand.equals("0") && operator1.firstOperand == operator2.firstOperand) {
                    // y = y == 8 ? 1 : 0; y = y == 0 ? 1 : 0; => y = y == 8 ? 0 : 1;
                    commands.remove(index);
                    commands.remove(index);
                    commands.add(index, new OperatorCommand(operator1.firstOperand, operator1.secondOperand, Operator.NOT_EQUALS));
                } else {
                    index++;
                }
            } else if (commands.get(index) instanceof SetVariableCommand && commands.get(index + 1) instanceof OperatorCommand) {
                SetVariableCommand operator1 = (SetVariableCommand) commands.get(index);
                OperatorCommand operator2 = (OperatorCommand) commands.get(index + 1);

                if (operator1.variable == operator2.firstOperand) {
                    // y = a; y = y + 8; => y = a + 8;
                    commands.remove(index);
                    commands.remove(index);
                    commands.add(index, new BetterOperatorCommand(operator1.variable, operator1.value, operator2.secondOperand, operator2.operator));
                } else {
                    index++;
                }
            } else {
                index++;
            }
        }
    }

    static class SetVariableCommand implements Command {

        final char variable;
        final String value;

        public SetVariableCommand(char variable, String value) {
            this.variable = variable;
            this.value = value;
        }

        public void execute(AluContext context) {
            context.setVariable(variable, context.getVariableOrValue(value));
        }

        @Override
        public String stringify(AluContext context) {
            return variable + " = " + value + ";";
        }

        @Override
        public String toString() {
            return "SetVariableCommand{" + stringify(new AluContext(7)) + '}';
        }
    }

    static class BetterOperatorCommand implements Command {

        final char variable;
        final String firstOperand;
        final String secondOperand;
        final Operator operator;

        public BetterOperatorCommand(char variable, String firstOperand, String secondOperand, Operator operator) {
            this.variable = variable;
            this.firstOperand = firstOperand;
            this.secondOperand = secondOperand;
            this.operator = operator;
        }

        public void execute(AluContext context) {
            long firstOperandValue = context.getVariableOrValue(firstOperand);
            long secondOperandValue = context.getVariableOrValue(secondOperand);
            context.setVariable(variable, operator.apply(firstOperandValue, secondOperandValue));
        }

        @Override
        public String stringify(AluContext context) {
            return variable + " = " + firstOperand + MessageFormat.format(operator.stringifyPattern, secondOperand) + ";";
        }

        @Override
        public String toString() {
            return "OperatorCommand{" + stringify(new AluContext(7)) + '}';
        }
    }
}
