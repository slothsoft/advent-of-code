package d16;

public class LessThanOperator extends Operator {

    static final int TypeId = 6;

    public LessThanOperator(BinaryStream stream) {
        super(stream);
    }

    @Override
    public long calculateValue() {
        return getSubPackets()[0].calculateValue() < getSubPackets()[1].calculateValue() ? 1L : 0L;
    }
}
