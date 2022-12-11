package d16;

public class EqualsOperator extends Operator {

    static final int TypeId = 7;

    public EqualsOperator(BinaryStream stream) {
        super(stream);
    }

    @Override
    public long calculateValue() {
        return getSubPackets()[0].calculateValue() == getSubPackets()[1].calculateValue() ? 1L : 0L;
    }
}
