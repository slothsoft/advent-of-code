package d16;

public class LiteralValue implements Packet {
    static final int TypeId = 4;

    private final int version;
    private final long value;

    public LiteralValue(BinaryStream stream) {
        // 110100101111111000101000
        // VVVTTTAAAAABBBBBCCCCC
        version = Integer.parseInt(stream.read(3), BINARY);
        stream.read(3); // typeId
        StringBuilder sb = new StringBuilder();
        boolean readNext;

        do {
            readNext = stream.readBoolean();
            sb.append(stream.read(4));
        } while (readNext);
        value = Long.parseLong(sb.toString(), BINARY);
    }

    @Override
    public int getVersion() {
        return version;
    }
    public int getTypeId() {
        return TypeId;
    }
    public long getValue() {
        return value;
    }
    @Override
    public int calculateVersion() { return version; }
    @Override
    public long calculateValue() { return value; }
}